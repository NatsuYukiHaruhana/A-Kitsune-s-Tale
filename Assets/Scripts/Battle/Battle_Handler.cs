using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class Battle_Handler : MonoBehaviour
{
    public static bool playerStrikeFirst = false;
    public static bool enemyStrikeFirst = false;

    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private GameObject drawBoardParent;
    [SerializeField]
    private TextMeshProUGUI wantedCharText;
    [SerializeField]
    private ScrollViewBehaviour itemMenuBehaviour;
    [SerializeField]
    private ScrollViewBehaviour spellMenuBehaviour;

    [SerializeField]
    private List<GameObject> buttons;

    public enum TurnAction {
        Attack,
        Guard,
        Items,
        Spells,
        Run,
        NULL
    }

    private static readonly ReadOnlyCollection<Vector3> unitPositions = 
        new ReadOnlyCollection<Vector3>(new List<Vector3>{new Vector3(-2.6f, 2f, 0f),     // leader unit (ally)
                                                          new Vector3(-5.3f, 3.3f, 0f),   // top-most unit (ally)
                                                          new Vector3(-5.3f, 0.7f, 0f),   // bottom-most unit (ally)
                                                          new Vector3(2.6f, 2f, 0f),      // leader unit (enemy)
                                                          new Vector3(5.3f, 3.3f, 0f),    // top-most unit (enemy)
                                                          new Vector3(5.3f, 0.7f, 0f)});  // bottom-most unit (enemy)

    private Battle_Entity.Faction allowedTargets;
    private TurnAction currentAction;
    private List<ArrowMovement> arrows;
    private List<Battle_Entity> units;
    private List<Button_Functionality> unitButtons;
    private List<Battle_Entity> targets;
    private int unitTurn;
    private bool canSelectTarget;
    private char wantedChar;

    private EventSystem eventSystem;
    private GameObject lastSelectedGameObject;
    private GameObject currentSelectedGameObject_Recent;

    private List<Battle_Entity_Spells> loadedSpells;
    private Battle_Entity_Spells selectedSpell;

    private bool enemyTurn;

    private void Awake() {
        allowedTargets = Battle_Entity.Faction.NULL;
        currentAction = TurnAction.NULL;
        
        arrows = new List<ArrowMovement>();
        units = new List<Battle_Entity>();
        unitButtons = new List<Button_Functionality>();
        targets = new List<Battle_Entity>();
        
        unitTurn = 0;
        canSelectTarget = false;

        eventSystem = EventSystem.current;

        loadedSpells = new List<Battle_Entity_Spells>();

        selectedSpell = null;

        enemyTurn = false;

        Utils.LoadGameData();
        for (int i = 0; i < Team_Data.count; i++) {
            ArrayList data = Team_Data.GetUnitData(i); // 0 - name, 1 - stats, 2 - loadout, 3 - spells

            if (data != null) {
                LoadUnit((string)data[0], (Battle_Entity_Stats)data[1], (Battle_Entity_Loadout)data[2], (List<Battle_Entity_Spells>)data[3], Battle_Entity.Faction.Ally);
            }
        }

        LoadItems(Team_Data.GetItems());

        for (int i = Team_Data.count; i < Team_Data.count + 1; i++) {
            LoadUnit("Flower",
                new Battle_Entity_Stats(i + 1,         // level
                                        50,            // currXP
                                        100,           // maxXP
                                        100,           // currHP
                                        100,           // maxHP
                                        100,           // currMana
                                        100,           // maxMana
                                        35,            // strength
                                        35,            // magic
                                        10,            // speed
                                        10,            // defense
                                        10),           // resistance);
                new Battle_Entity_Loadout(),
                new List<Battle_Entity_Spells>(),
                Battle_Entity.Faction.Enemy);
        }

        spellMenuBehaviour.gameObject.SetActive(false);
    }

    private void Start() {
        for (int i = Team_Data.count; i < Team_Data.count + 1; i++) {
            if (playerStrikeFirst) {
                units[i].TakeDamage(units[0].GetStats().str / 2, DamageType.Physical);
            }

            if (enemyStrikeFirst) {
                for (int j = 0; j < Team_Data.count; j++) {
                    units[j].TakeDamage(units[i].GetStats().str / 2, DamageType.Physical);
                }
            }
        }

        playerStrikeFirst = false;
        enemyStrikeFirst = false;
    }

    private void Update()
    {
        if (canSelectTarget) {
            DoTargetSelection();
        } else {
            DoUnitDetailShow();
        }

        GetLastGameObjectSelected();

        if (units[unitTurn].GetFaction() == Battle_Entity.Faction.Enemy) { // AI's turn
            StartCoroutine(EnemyAIWait(DoAITurn));
        } else {
            enemyTurn = false;
        }
    }

    private void GetLastGameObjectSelected() {
        if (eventSystem.currentSelectedGameObject != currentSelectedGameObject_Recent && eventSystem.currentSelectedGameObject != null) {
            lastSelectedGameObject = currentSelectedGameObject_Recent;
            currentSelectedGameObject_Recent = eventSystem.currentSelectedGameObject;

            if (!enemyTurn) {
                if (currentAction == TurnAction.Items) {
                    GameObject buttonObject = currentSelectedGameObject_Recent;
                    string itemName = buttonObject.GetComponentInChildren<TextMeshProUGUI>().text;
                    itemName = itemName.Remove(itemName.LastIndexOf("X") - 1);

                    foreach (Item item in Team_Data.GetItems()) {
                        if (itemName == item.GetItemName()) {
                            if (item.GetType().IsSubclassOf(typeof(Item_Not_Equippable)) == true) {
                                wantedCharText.text = item.GetItemDesc();

                                break;
                            }

                            break;
                        }
                    }
                } else if (currentAction == TurnAction.Spells) {
                    GameObject buttonObject = currentSelectedGameObject_Recent;
                    string spellName = buttonObject.GetComponentInChildren<TextMeshProUGUI>().text;
                    
                    foreach (Battle_Entity_Spells spell in loadedSpells) {
                        if (spellName == spell.GetSpellName()) {
                            wantedCharText.text = spell.GetSpellDesc();

                            break;
                        }
                    }
                }
            }
        }
    }

    public void SetCurrentAction(string action) {
        switch (action) {
            case "Attack": {
                Utils.currentLanguage = "hiragana2";
                PrepareWantedChar();

                drawBoardParent.SetActive(true);

                foreach (GameObject buttonMenu in buttons) {
                    if (buttonMenu.name == "Battle_Menu_1") {
                        buttonMenu.SetActive(false);
                    } else if (buttonMenu.name == "Battle_Menu_2") {
                        buttonMenu.SetActive(true);
                    }
                }

                currentAction = TurnAction.Attack;
                SelectTargets();
                break;
            }
            case "Guard": {
                Utils.currentLanguage = "hiragana2";
                PrepareWantedChar();

                drawBoardParent.SetActive(true);

                foreach (GameObject buttonMenu in buttons) {
                    if (buttonMenu.name == "Battle_Menu_1") {
                        buttonMenu.SetActive(false);
                    } else if (buttonMenu.name == "Battle_Menu_2") {
                        buttonMenu.SetActive(true);
                    }
                }

                currentAction = TurnAction.Guard;
                break;
            }
            case "Items": {
                foreach (GameObject buttonMenu in buttons) {
                    if (buttonMenu.name == "Battle_Menu_1") {
                        buttonMenu.SetActive(false);
                    } else if (buttonMenu.name == "Battle_Menu_2") {
                        buttonMenu.SetActive(true);
                    }
                }

                currentAction = TurnAction.Items;
                itemMenuBehaviour.gameObject.SetActive(true);
                SelectTargets();
                break;
            }
            case "Spells": {
                Utils.currentLanguage = "kanji";

                foreach (GameObject buttonMenu in buttons) {
                    if (buttonMenu.name == "Battle_Menu_3") {
                        buttonMenu.SetActive(true);
                    } else {
                        buttonMenu.SetActive(false);
                    }
                }

                currentAction = TurnAction.Spells;
                LoadSpells();
                break;
            }
            case "Run": {
                currentAction = TurnAction.Run;
                DoAction();
                break;
            }
            default: {
                currentAction = TurnAction.NULL;
                break;
            }
        }
    }

    private void LoadUnit(string name, 
        Battle_Entity_Stats stats, 
        Battle_Entity_Loadout loadout, 
        List<Battle_Entity_Spells> spells,
        Battle_Entity.Faction faction) {
        units.Add(Instantiate(unitPrefab).GetComponent<Battle_Entity>());
        int currentUnit = units.Count - 1;
        unitButtons.Add(units[currentUnit].gameObject.GetComponent<Button_Functionality>());

        units[currentUnit].SetFaction(faction);

        units[currentUnit].SetName(name);
        units[currentUnit].SetStats(stats);
        units[currentUnit].SetLoadout(loadout);
        units[currentUnit].SetSpells(spells);
        units[currentUnit].LoadSprites();

        if (faction == Battle_Entity.Faction.Enemy) {
            units[currentUnit].gameObject.transform.position = unitPositions[currentUnit + (3 - Team_Data.count)];
        } else if (faction == Battle_Entity.Faction.Ally) {
            units[currentUnit].gameObject.transform.position = unitPositions[currentUnit];
        }

        arrows.Add(Instantiate(arrowPrefab).GetComponent<ArrowMovement>());
        arrows[currentUnit].gameObject.name = name + " Arrow";
        if (faction == Battle_Entity.Faction.Enemy) {
            arrows[currentUnit].SetOffsetX(-1.3f);
            arrows[currentUnit].RotateArrow(0f);
        }

        arrows[currentUnit].SetTarget(units[currentUnit].gameObject);
        arrows[currentUnit].SetVisible(false);
    }

    private void LoadItems(List<Item> items) {
        for (int i = 0; i < Team_Data.count; i++) {
            units[i].SetItems(items);
        }

        foreach (Item item in items) {
            int count = itemMenuBehaviour.GetContentCount(item.GetItemName());
            
            string text = item.GetItemName() + " X " + (count + 1);
            string removeText = item.GetItemName() + " X " + count;

            if (count == 0) {
                itemMenuBehaviour.CreateNewContent(text);
            } else { 
                itemMenuBehaviour.ReplaceContent(removeText, text);
            }
        }

        itemMenuBehaviour.gameObject.SetActive(false);
    }

    private void SelectTargets() {
        if (currentAction == TurnAction.Attack) { // Attack button pressed
            allowedTargets = Battle_Entity.Faction.Enemy;
        }

        if (currentAction == TurnAction.Items) {
            allowedTargets = Battle_Entity.Faction.Ally;
        }

        if (currentAction == TurnAction.Spells) {
            allowedTargets = selectedSpell.GetAllowedTargets();
        }

        canSelectTarget = true;
    }

    public void GoBack() {
        foreach(GameObject buttonMenu in buttons) {
            if (buttonMenu.name == "Battle_Menu_1") {
                buttonMenu.SetActive(true);
            } else {
                buttonMenu.SetActive(false);
            }
        }

        itemMenuBehaviour.gameObject.SetActive(false);
        spellMenuBehaviour.gameObject.SetActive(false);
        drawBoardParent.SetActive(false);
        canSelectTarget = false;
        targets.Clear();
        wantedCharText.text = "";
        foreach (ArrowMovement arrow in arrows) {
            arrow.SetVisible(false);
        }
    }

    public void DoAction() {
        switch (currentAction) {
            case TurnAction.Attack: {
                if (targets.Count == 0) {
                    Debug.Log("Please select targets!");
                    return;
                }

                StartCoroutine(WaitForTesseract(DoAttack));
                break;
            }
            case TurnAction.Guard: {
                StartCoroutine(WaitForTesseract(DoGuard));
                break;
            }
            case TurnAction.Items: {
                UseItem();
                break;
            }
            case TurnAction.Spells: {
                StartCoroutine(WaitForTesseract(CastSpell));
                break;
            }
            case TurnAction.Run: {
                DoRun();
                break;
            }
        }
    }
    
    private void UseItem() {
        if (targets.Count == 0) {
            Debug.Log("Please select targets!");
            return;
        }

        GameObject buttonObject = lastSelectedGameObject;
        if (buttonObject != null) {
            string itemName = buttonObject.GetComponentInChildren<TextMeshProUGUI>().text;
            itemName = itemName.Remove(itemName.LastIndexOf("X") - 1);
            Item itemToRemove = null;

            foreach (Item item in Team_Data.GetItems()) {
                if (itemName == item.GetItemName()) {
                    if (item.GetType().IsSubclassOf(typeof(Item_Not_Equippable)) == true) {
                        ((Item_Not_Equippable)item).UseItem(targets[0]);

                        itemToRemove = item;
                        break;
                    }
                    
                    break;
                }
            }

            if (itemToRemove != null) {
                Team_Data.RemoveItem(itemToRemove);

                itemMenuBehaviour.ContentCountDown(itemName);
            }
        }
        NextTurn();
    }

    public void LoadSpells() {
        foreach (Battle_Entity_Spells spell in loadedSpells) {
            spellMenuBehaviour.RemoveContent(spell.GetSpellName());
        }
        loadedSpells = units[unitTurn].GetSpells();

        spellMenuBehaviour.gameObject.SetActive(true);

        foreach (Battle_Entity_Spells spell in loadedSpells) {
            spellMenuBehaviour.CreateNewContent(spell.GetSpellName());
        }
    }

    public void ConfirmSpell() {
        GameObject buttonObject = lastSelectedGameObject;
        if (buttonObject != null) {
            string spellName = buttonObject.GetComponentInChildren<TextMeshProUGUI>().text;

            foreach (Battle_Entity_Spells spell in loadedSpells) {
                if (spellName == spell.GetSpellName()) {
                    selectedSpell = spell;

                    break;
                }
            }
        }

        spellMenuBehaviour.gameObject.SetActive(false);

        foreach (GameObject buttonMenu in buttons) {
            if (buttonMenu.name == "Battle_Menu_2") {
                buttonMenu.SetActive(true);
            } else {
                buttonMenu.SetActive(false);
            }
        }

        SelectTargets();

        drawBoardParent.SetActive(true);
    }

    private void CastSpell() {
        if (targets.Count == 0) {
            Debug.Log("Please select targets!");
            return;
        }

        //if (TesseractHandler.GetRecognizedText() == selectedSpell.GetSpellChars()) {
            selectedSpell.CastSpell(targets, units[unitTurn]);
        //}

        NextTurn();
    }

    private void PrepareWantedChar() {
        wantedChar = Utils.GetRandomCharacter(true, true);
        wantedCharText.text = "Please draw " + wantedChar;
    }

    IEnumerator WaitForTesseract(Action doAction) {
        DrawController.TakeScreenshot();

        while (TesseractHandler.GetIsDone() == false) {
            yield return null;
        }
        TesseractHandler.ResetIsDone(); 

        doAction();
    }

    IEnumerator EnemyAIWait(Action doAction) {
        yield return new WaitForSeconds(1);

        if (units[unitTurn].GetFaction() == Battle_Entity.Faction.Enemy) {
            doAction();
        }
    }

    private void DoAttack() {
        if (enemyTurn) {
            units[unitTurn].BasicAttack(targets);
            NextTurn();
            return;
        }

        //if (TesseractHandler.GetRecognizedText()[0] == wantedChar) {
            units[unitTurn].BasicAttack(targets);
        //}

        NextTurn();
    }

    private void DoGuard() {
        if (enemyTurn) {
            units[unitTurn].RaiseGuard();
            NextTurn();
            return;
        }

        //if (TesseractHandler.GetRecognizedText()[0] == wantedChar) {
            units[unitTurn].RaiseGuard();
        //}

        NextTurn();
    }

    private void DoRun() {
        SceneManager.LoadScene("Platforming Scene");
    }

    private void DoUnitDetailShow() {
        for (int i = 0; i < unitButtons.Count; i++) {
            if (unitButtons[i].IsButtonPressed()) {
                wantedCharText.text = units[i].GetName() + "\n" + units[i].GetStats().ToString();
            }
        }
    }

    private void DoTargetSelection() {
        for (int i = 0; i < unitButtons.Count; i++) {
            if (unitButtons[i].IsButtonPressed()) {
                if (units[i].GetFaction() == allowedTargets) {
                    if (targets.Contains(units[i])) {
                        targets.Remove(units[i]);
                        arrows[i].SetVisible(false);
                    } else {
                        targets.Add(units[i]);
                        arrows[i].SetVisible(true, true);
                    }
                }
            }
        }
    }

    private void NextTurn() {
        if (currentAction == TurnAction.Attack || currentAction == TurnAction.Spells) {
            foreach (Battle_Entity target in targets) {
                if (target.GetStats().currHP <= 0f) {
                    if (target.GetFaction() == Battle_Entity.Faction.Ally) {
                        units.Remove(target);

                        bool foundAlliedUnit = false;
                        foreach (Battle_Entity unit in units) { 
                            if (unit.GetFaction() == Battle_Entity.Faction.Ally) {
                                foundAlliedUnit = true;
                                break;
                            }
                        }

                        if (!foundAlliedUnit) {
                            SceneManager.LoadScene("Main Menu");
                        }
                    }

                    if (target.GetFaction() == Battle_Entity.Faction.Enemy) {
                        foreach (Battle_Entity unit in units) {
                            if (unit.GetFaction() == Battle_Entity.Faction.Ally) {
                                unit.GrantExperience(target.GetStats().currXP);

                                Team_Data.ModifyEntry(unit.GetName(), unit.GetStats(), unit.GetLoadout(), unit.GetSpells());
                            }
                        }

                        Destroy(arrows[units.IndexOf(target)].gameObject);
                        arrows.RemoveAt(units.IndexOf(target));
                        units.Remove(target);
                        Destroy(target.gameObject);

                        bool foundEnemyUnit = false;
                        foreach (Battle_Entity unit in units) {
                            if (unit.GetFaction() == Battle_Entity.Faction.Enemy) {
                                foundEnemyUnit = true;
                                break;
                            }
                        }

                        if (!foundEnemyUnit) {
                            Utils.SaveGameData();
                            SceneManager.LoadScene("Platforming Scene");
                        }
                    }
                }
            }
        }

        currentAction = TurnAction.NULL;
        canSelectTarget = false;
        targets.Clear();
        unitTurn = unitTurn < units.Count - 1 ? unitTurn + 1 : 0;
        wantedCharText.text = "";

        if (unitTurn == 0) { // new turn
            foreach (Battle_Entity unit in units) {
                unit.CheckStatChanges();
            }
        }

        GoBack();
    }

    private void DoAITurn() {
        enemyTurn = true;
        int action = UnityEngine.Random.Range(0, 2);

        switch(action) {
            case 0: {
                currentAction = TurnAction.Attack;

                targets.Add(units[0]);
                DoAttack();

                break;
            }
            case 1: {
                currentAction = TurnAction.Guard;

                DoGuard();

                break;
            }
        }
    }
}
