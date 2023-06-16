using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

public static class Team_Data
{
    private static List<string> names                      = new List<string>();
    private static List<Battle_Entity_Stats> stats         = new List<Battle_Entity_Stats>();
    private static List<Battle_Entity_Loadout> loadouts    = new List<Battle_Entity_Loadout>();
    private static List<List<Battle_Entity_Spells>> spells = new List<List<Battle_Entity_Spells>>();
    private static List<Item> items                        = new List<Item>();

    public static int count;

    public static void AddNewEntry(string name, Battle_Entity_Stats unitStats, Battle_Entity_Loadout loadout, List<Battle_Entity_Spells> unitSpells) {
        names.Add(name);
        stats.Add(unitStats);
        loadouts.Add(loadout);
        spells.Add(unitSpells);

        count++;
    }

    public static void ModifyEntry(string name, Battle_Entity_Stats unitStats, Battle_Entity_Loadout loadout, List<Battle_Entity_Spells> unitSpells) {
        int index = names.IndexOf(name);
        if (index != -1) {
            stats[index] = unitStats;
            loadouts[index] = loadout;
            spells[index] = unitSpells;
        }
    }

    public static void RemoveEntry(string name) {
        int index = names.IndexOf(name);
        if (index != -1) {
            names.RemoveAt(index);
            stats.RemoveAt(index);
            loadouts.RemoveAt(index);
            spells.RemoveAt(index);

            count--;
        }
    }

    public static void AddItem(Item item) {
        items.Add(item);
    }

    public static void RemoveItem(string itemName) {
        for (int i = 0; i < items.Count; i++) {
            if (items[i].GetItemName() == itemName) {
                items.RemoveAt(i);
                break;
            }
        }
    }

    public static void ModifyItems(List<Item> newItems) {
        items = newItems;
    }

    public static void InitData(List<string> newNames, List<Battle_Entity_Stats> newStats, List<Battle_Entity_Loadout> newLoadouts, List<List<Battle_Entity_Spells>> newSpells, List<Item> newItems) {
        names = newNames;
        stats = newStats;
        loadouts = newLoadouts;
        spells = newSpells;
        items = newItems;

        count = names.Count;
    }

    public static ArrayList GetData() {
        ArrayList data = new ArrayList(5) { names, stats, loadouts, spells, items };
        return data;
    }

    public static ArrayList GetUnitData(int index) {
        ArrayList data = null;
        
        if (names.ElementAt(index) != null) {
            data = new ArrayList(4) { names[index], stats[index], loadouts[index], spells[index] };
        }

        return data;
    }

    public static List<Item> GetItems() {
        return items;
    }
}
