using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Handler : MonoBehaviour
{
    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private List<GameObject> buttons;

    private static readonly ReadOnlyCollection<Vector3> unitPositions = 
        new ReadOnlyCollection<Vector3>(new List<Vector3>{new Vector3(-5.3f, 3.3f, 0f),   // top-most unit (ally)
                                                          new Vector3(-2.6f, 2f, 0f),     // leader unit (ally)
                                                          new Vector3(-5.3f, 0.7f, 0f),   // bottom-most unit (ally)
                                                          new Vector3(5.3f, 3.3f, 0f),    // top-most unit (enemy)
                                                          new Vector3(2.6f, 2f, 0f),      // leader unit (enemy)
                                                          new Vector3(5.3f, 0.7f, 0f)});  // bottom-most unit (enemy)

    private List<ArrowMovement> arrows;
    private List<Battle_Entity> units;
    private List<Button_Functionality> unitButtons;
    private List<Battle_Entity> targets;
    private Battle_Entity.Faction allowedTargets = Battle_Entity.Faction.NULL;
    private int unitTurn;
    private bool canSelectTarget;

    private void Awake() {
        arrows = new List<ArrowMovement>();
        units = new List<Battle_Entity>();
        unitButtons = new List<Button_Functionality>();
        targets = new List<Battle_Entity>();
        unitTurn = 0;
        canSelectTarget = false;

        // TODO: Move this stuff when you start a new game, after you've named your character.
        for (int i = 0; i < 3; i++) {
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
            units[i].SetFaction(Battle_Entity.Faction.Ally);
            units[i].gameObject.transform.position = unitPositions[i];
            arrows.Add(Instantiate(arrowPrefab).GetComponent<ArrowMovement>());
            arrows[i].SetTarget(units[i].gameObject);
            arrows[i].SetVisible(false);

            Team_Data.names.Add(units[i].GetName());
            Team_Data.stats.Add(units[i].GetStats());
            Team_Data.loadouts.Add(units[i].GetLoadout());
            Team_Data.spells.Add(units[i].GetSpells());
            Team_Data.items.Add(units[i].GetItems());
        }
        //Utils.SaveGameData();
        for (int i = 3; i < 6; i++) {
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
            units[i].gameObject.transform.position = unitPositions[i];
            arrows.Add(Instantiate(arrowPrefab).GetComponent<ArrowMovement>());
            arrows[i].SetOffsetX(-1.3f);
            arrows[i].SetTarget(units[i].gameObject);
            arrows[i].RotateArrow(0f);
            arrows[i].SetVisible(false);

            Team_Data.names.Add(units[i].GetName());
            Team_Data.stats.Add(units[i].GetStats());
            Team_Data.loadouts.Add(units[i].GetLoadout());
            Team_Data.spells.Add(units[i].GetSpells());
            Team_Data.items.Add(units[i].GetItems());
        }

        //Utils.LoadGameData();
        for (int i = 0; i < Team_Data.names.Count; i++) {
            //units.Add(Instantiate(unitPrefab).GetComponent<Battle_Entity>());
            units[i].SetName(Team_Data.names[i]);
            units[i].SetStats(Team_Data.stats[i]);
            units[i].SetLoadout(Team_Data.loadouts[i]);
            units[i].SetSpells(Team_Data.spells[i]);
            units[i].SetItems(Team_Data.items[i]);
            units[i].gameObject.transform.position = unitPositions[i];
        }
    }

    void Update()
    {
        if (canSelectTarget) {
            DoTargetSelection();
        }
    }

    public void SelectTargets(string menuToOpen) {
        if (menuToOpen == "Battle_Menu_2") { // Attack button pressed
            allowedTargets = Battle_Entity.Faction.Enemy;
        }

        foreach(GameObject buttonMenu in buttons) {
            if (buttonMenu.name == "Battle_Menu_1") {
                buttonMenu.SetActive(false);
            } else if (buttonMenu.name == menuToOpen) {
                buttonMenu.SetActive(true);
            }
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

        canSelectTarget = false;
        targets.Clear();
        foreach(ArrowMovement arrow in arrows) {
            arrow.SetVisible(false);
        }
    }

    public void DoAttack() {
        if (targets.Count == 0) {
            Debug.Log("Please select targets!");
            return;
        }

        units[unitTurn].BasicAttack(targets);
        NextTurn();
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
                        arrows[i].SetVisible(true);
                    }
                }
            }
        }
    }

    private void NextTurn() {
        canSelectTarget = false;
        targets.Clear();
        unitTurn = unitTurn < units.Count ? unitTurn++ : 0;
        GoBack();
    }
}
