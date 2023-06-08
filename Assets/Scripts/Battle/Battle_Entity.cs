using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Entity : MonoBehaviour {
    public enum Faction {
        Ally,
        Enemy,
        Ally_Spirit,
        Enemy_Spirit,
        Neutral,
        NULL
    };

    private string unitName = "\n";
    private Faction unitFaction = Faction.NULL;
    private Battle_Entity_Stats battleStats;
    private List<Battle_Entity_Stat_Change> statChanges;
    private bool isGuarding;
    private Battle_Entity_Loadout loadout;
    private List<Battle_Entity_Spells> spells;
    private List<Item> items;

    [SerializeField]
    private GameObject barPrefab;

    private Battle_Entity_Bar hpBar;
    private Battle_Entity_Bar manaBar;

    private void Awake() {
        hpBar = Instantiate(barPrefab, this.transform).GetComponent<Battle_Entity_Bar>();
        hpBar.SetColor(new Color(0, 255, 0));
        hpBar.transform.localPosition = new Vector3(0f, -0.88f, 0f);

        manaBar = Instantiate(barPrefab, this.transform).GetComponent<Battle_Entity_Bar>();
        manaBar.SetColor(new Color(0, 0, 255));
        manaBar.transform.localPosition = new Vector3(0f, 0.88f, 0f);

        statChanges = new List<Battle_Entity_Stat_Change>();
        isGuarding = false;

        loadout = new Battle_Entity_Loadout();

        spells = new List<Battle_Entity_Spells>();

        items = new List<Item>();
    }

    void Update() {

    }

    public void BasicAttack(List<Battle_Entity> targets) {
        foreach (Weapon weapon in loadout.GetWeapons()) {
            if (weapon is No_Weapon) {
                continue;
            }

            float premitigationDamage;
            DamageType damageType = weapon.GetDamageType();

            switch(damageType) {
                case DamageType.Physical: {
                    premitigationDamage = battleStats.str;
                    break;
                }
                case DamageType.Magical: {
                    premitigationDamage = battleStats.mag;
                    break;
                }
                case DamageType.NULL: 
                default: {
                    premitigationDamage = 0f;
                    break;
                }
            }

            foreach (Battle_Entity target in targets) {
                target.TakeDamage(premitigationDamage, damageType);
            }
        }
    }

    public void RaiseGuard() {
        isGuarding = true;
    }

    public void LowerGuard() {
        isGuarding = false;
    }

    public void Heal(float amount) {
        if (battleStats.currHP > 0.0f) {
            battleStats.currHP = Mathf.Min(battleStats.currHP + amount, battleStats.maxHP);

            hpBar.SetTargetPercentage(battleStats.currHP / battleStats.maxHP);
            hpBar.gameObject.SetActive(true);
        }
    }

    public void TakeDamage(float damage, DamageType damageType) {
        float damageReduction;
        switch (damageType) {
            case DamageType.Physical: {
                damageReduction = battleStats.def;
                break;
            }
            case DamageType.Magical: {
                damageReduction = battleStats.res;
                break;
            }
            case DamageType.NULL:
            default: {
                damageReduction = 0f;
                break;
            }
        }

        float damageDealt = damage - damageReduction;
        if (damageDealt < 0f) {
            return;
        }

        if (isGuarding) {
            damageDealt /= 2f;
        }

        if (damageDealt < battleStats.currHP) {
            battleStats.currHP -= damageDealt;
        } else {
            battleStats.currHP = 0;
        }

        hpBar.SetTargetPercentage(battleStats.currHP / battleStats.maxHP);
        hpBar.gameObject.SetActive(true);
    }

    public void AddStatChange(Battle_Entity_Stat_Change newStatChange) {
        statChanges.Add(newStatChange);
    }

    public void CheckStatChanges() {
        foreach (Battle_Entity_Stat_Change statChange in statChanges) {
            statChange.LowerTurnCount();
        }

        statChanges.RemoveAll(statChange => statChange.GetReadyToRemove() == true);
    }

    public Battle_Entity_Stats GetStats() {
        return battleStats;
    }

    public Battle_Entity_Loadout GetLoadout() {
        return loadout;
    }

    public List<Battle_Entity_Spells> GetSpells() {
        return spells;
    }

    public List<Item> GetItems() {
        return items;
    }

    public string GetName() {
        return unitName;
    }

    public Faction GetFaction() {
        return unitFaction;
    }

    public void SetStats(Battle_Entity_Stats newStats) {
        battleStats = new Battle_Entity_Stats(newStats);
    }

    public void SetLoadout(Battle_Entity_Loadout newLoadout) {
        loadout = new Battle_Entity_Loadout(newLoadout);
    }

    public void SetSpells(List<Battle_Entity_Spells> newSpells) {
        spells = new List<Battle_Entity_Spells>(newSpells);
    }

    public void SetItems(List<Item> newItems) {
        items = new List<Item>(newItems);
    }

    public void SetName(string newName) {
        if (unitName == "\n") {
            unitName = newName;
            name = newName;
        }
    }

    public void SetFaction(Faction newFaction) {
        unitFaction = newFaction;
    }

}
