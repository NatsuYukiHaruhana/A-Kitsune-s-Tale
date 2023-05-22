using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Battle_Handler : MonoBehaviour
{
    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private GameObject drawBoardParent;

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

    private void Awake() {
        allowedTargets = Battle_Entity.Faction.NULL;
        currentAction = TurnAction.NULL;
        
        arrows = new List<ArrowMovement>();
        units = new List<Battle_Entity>();
        unitButtons = new List<Button_Functionality>();
        targets = new List<Battle_Entity>();
        
        unitTurn = 0;
        canSelectTarget = false;

        Utils.LoadGameData();
        for (int i = 0; i < Team_Data.names.Count; i++) {
            units.Add(Instantiate(unitPrefab).GetComponent<Battle_Entity>());
            unitButtons.Add(units[i].gameObject.GetComponent<Button_Functionality>());

            units[i].SetName(Team_Data.names[i]);
            units[i].SetStats(Team_Data.stats[i]);
            units[i].SetLoadout(Team_Data.loadouts[i]);
            units[i].SetSpells(Team_Data.spells[i]);
            units[i].SetItems(Team_Data.items[i]);

            units[i].gameObject.transform.position = unitPositions[i];

            arrows.Add(Instantiate(arrowPrefab).GetComponent<ArrowMovement>());
            arrows[i].SetTarget(units[i].gameObject);
            arrows[i].SetVisible(false);
        }

        for (int i = Team_Data.names.Count; i < Team_Data.names.Count + 3; i++) {
            units.Add(Instantiate(unitPrefab).GetComponent<Battle_Entity>());
            unitButtons.Add(units[i].gameObject.GetComponent<Button_Functionality>());

            units[i].SetName("Unit " + (i + 1));
            units[i].SetStats(new Battle_Entity_Stats(i + 1,           // level
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
                                                        10));          // resistance
            units[i].SetFaction(Battle_Entity.Faction.Enemy);

            units[i].gameObject.transform.position = unitPositions[i + (3 - Team_Data.names.Count)];

            arrows.Add(Instantiate(arrowPrefab).GetComponent<ArrowMovement>());
            arrows[i].SetOffsetX(-1.3f);
            arrows[i].SetTarget(units[i].gameObject);
            arrows[i].RotateArrow(0f);
            arrows[i].SetVisible(false);
        }
    }

    void Update()
    {
        if (canSelectTarget) {
            DoTargetSelection();
        }
    }

    public void SetCurrentAction(string action) {
        switch (action) {
            case "Attack": {
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
                currentAction = TurnAction.Items;
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

    public void SelectTargets() {
        if (currentAction == TurnAction.Attack) { // Attack button pressed
            allowedTargets = Battle_Entity.Faction.Enemy;
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

        drawBoardParent.SetActive(false);
        canSelectTarget = false;
        targets.Clear();
        foreach(ArrowMovement arrow in arrows) {
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

                DrawController.TakeScreenshot();
                if (TesseractHandler.GetIsDone()) {
                    TesseractHandler.ResetIsDone();
                    DoAttack();
                }
                break;
            }
            case TurnAction.Guard: {
                DrawController.TakeScreenshot();
                if (TesseractHandler.GetIsDone()) {
                    TesseractHandler.ResetIsDone();
                    DoGuard();
                }
                break;
            }
            case TurnAction.Run: {
                DoRun();
                break;
            }
        }
    }
    private void DoAttack() {
        char wantedChar = Utils.GetRandomCharacter(true, false, false);
        Debug.Log("Wanted: " + wantedChar);
        if (TesseractHandler.GetRecognizedText()[0] == wantedChar) {
            units[unitTurn].BasicAttack(targets);
            NextTurn();
        } else {
            NextTurn();
        }
    }

    private void DoGuard() {
        char wantedChar = Utils.GetRandomCharacter(true, false, false);
        Debug.Log("Wanted: " + wantedChar);
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
