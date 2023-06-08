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
    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private GameObject drawBoardParent;
    [SerializeField]
    private TextMeshProUGUI wantedCharText;
    [SerializeField]
    private ScrollViewBehaviour scrollViewBehaviour;

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

        Utils.LoadGameData();
        for (int i = 0; i < Team_Data.names.Count; i++) {
            LoadUnit(Team_Data.names[i], 
                Team_Data.stats[i], 
                Team_Data.loadouts[i], 
                Team_Data.spells[i], 
                Team_Data.items,
                Battle_Entity.Faction.Ally);
        }

        for (int i = Team_Data.names.Count; i < Team_Data.names.Count + 3; i++) {
            LoadUnit("Enemy " + (i - Team_Data.names.Count + 1),
                new Battle_Entity_Stats(i + 1,           // level
                                        0,             // currXP
                                        100 * (i + 1), // maxXP
                                        100,           // currHP
                                        100,           // maxHP
                                        100,           // currMana
                                        100,           // maxMana
                                        20,            // strength
                                        10,            // magic
                                        10,            // speed
                                        10,            // defense
                                        10),          // resistance);
                new Battle_Entity_Loadout(),
                new List<Battle_Entity_Spells>(),
                new List<Item>(),
                Battle_Entity.Faction.Enemy);
        }

        // THIS IS FOR TESTING POTIONS, TO REMOVE LATER
        units[0].TakeDamage(50, DamageType.Physical);
    }

    private void Update()
    {
        if (canSelectTarget) {
            DoTargetSelection();
        }

        GetLastGameObjectSelected();
    }

    private void GetLastGameObjectSelected() {
        if (eventSystem.currentSelectedGameObject != currentSelectedGameObject_Recent) {
            lastSelectedGameObject = currentSelectedGameObject_Recent;
            currentSelectedGameObject_Recent = eventSystem.currentSelectedGameObject;
        }
    }

    public void SetCurrentAction(string action) {
        switch (action) {
            case "Attack": {
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
                break;
            }
            case "Guard": {
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
                scrollViewBehaviour.gameObject.SetActive(true);
                break;
            }
            case "Spells": {
                currentAction = TurnAction.Spells;
                break;
            }
            case "Run": {
                currentAction = TurnAction.Run;
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
        List<Item> items,
        Battle_Entity.Faction faction) {
        units.Add(Instantiate(unitPrefab).GetComponent<Battle_Entity>());
        int currentUnit = units.Count - 1;
        unitButtons.Add(units[currentUnit].gameObject.GetComponent<Button_Functionality>());

        units[currentUnit].SetFaction(faction);

        units[currentUnit].SetName(name);
        units[currentUnit].SetStats(stats);
        units[currentUnit].SetLoadout(loadout);
        units[currentUnit].SetSpells(spells);
        units[currentUnit].SetItems(items);

        if (faction == Battle_Entity.Faction.Enemy) {
            units[currentUnit].gameObject.transform.position = unitPositions[currentUnit + (3 - Team_Data.names.Count)];
        } else if (faction == Battle_Entity.Faction.Ally) {
            units[currentUnit].gameObject.transform.position = unitPositions[currentUnit];
        }

        arrows.Add(Instantiate(arrowPrefab).GetComponent<ArrowMovement>());
        if (faction == Battle_Entity.Faction.Enemy) {
            arrows[currentUnit].SetOffsetX(-1.3f);
            arrows[currentUnit].RotateArrow(0f);
        } else if (faction == Battle_Entity.Faction.Ally) {
            foreach(Item item in items) {
                scrollViewBehaviour.CreateNewContent(item.GetItemName());
            }

            if (currentUnit + 1 == Team_Data.names.Count) {
                scrollViewBehaviour.gameObject.SetActive(false);
            }
        }
        arrows[currentUnit].SetTarget(units[currentUnit].gameObject);
        arrows[currentUnit].SetVisible(false);
    }

    public void SelectTargets() {
        if (currentAction == TurnAction.Attack) { // Attack button pressed
            allowedTargets = Battle_Entity.Faction.Enemy;
        }

        if (currentAction == TurnAction.Items) {
            allowedTargets = Battle_Entity.Faction.Ally;
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

        scrollViewBehaviour.gameObject.SetActive(false);
        drawBoardParent.SetActive(false);
        canSelectTarget = false;
        targets.Clear();
        wantedCharText.gameObject.SetActive(false);
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
            case TurnAction.Run: {
                DoRun();
                break;
            }
        }
    }
    
    public void UseItem() {
        if (targets.Count == 0) {
            Debug.Log("Please select targets!");
            return;
        }

        GameObject buttonObject = lastSelectedGameObject;
        if (buttonObject != null) {
            string itemName = buttonObject.GetComponentInChildren<TextMeshProUGUI>().text;
            Item itemToRemove = null;

            foreach (Item item in Team_Data.items) {
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
                Team_Data.items.Remove(itemToRemove);

                scrollViewBehaviour.RemoveContent(itemName);
            }
        }
        NextTurn();
    }

    private void PrepareWantedChar() {
        wantedChar = Utils.GetRandomCharacter(true, false, false);
        wantedCharText.text = "Please draw " + wantedChar;
        wantedCharText.gameObject.SetActive(true);
    }

    IEnumerator WaitForTesseract(Action doAction) {
        DrawController.TakeScreenshot();

        while (TesseractHandler.GetIsDone() == false) {
            yield return null;
        }
        TesseractHandler.ResetIsDone(); 

        doAction();
    }

    private void DoAttack() {
        if (TesseractHandler.GetRecognizedText()[0] == wantedChar) {
            units[unitTurn].BasicAttack(targets);
            NextTurn();
        } else {
            NextTurn();
        }
    }

    private void DoGuard() {
        if (TesseractHandler.GetRecognizedText()[0] == wantedChar) {
            units[unitTurn].RaiseGuard();
            NextTurn();
        } else {
            NextTurn();
        }
    }

    private void DoRun() {
        SceneManager.LoadScene("SampleScene");
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
        currentAction = TurnAction.NULL;
        canSelectTarget = false;
        targets.Clear();
        unitTurn = unitTurn < units.Count ? unitTurn++ : 0;

        if (unitTurn == 0) { // new turn
            foreach (Battle_Entity unit in units) {
                unit.LowerGuard();
                unit.CheckStatChanges();
            }
        }

        GoBack();
    }
}
