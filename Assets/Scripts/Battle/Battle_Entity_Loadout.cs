using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class Battle_Entity_Loadout
{
    public enum HandSlots {
        Weapon,
        Weapon_Both,
        Shield,
        NULL
    }

    public enum ArmorSlots {
        Head,
        Chest,
        Legs,
        Boots,
        NULL
    }

    public enum AccessorySlots {
        Ring,
        Necklace,
        SpiritAura,
        NULL
    }

    private List<Weapon> weaponSlots;
    private Shield shieldSlot;
    private List<Armor> armorSlots;
    private List<Accessory> accessorySlots;

    public Battle_Entity_Loadout() {
        weaponSlots = new List<Weapon>(2);
        shieldSlot = new No_Shield();
        armorSlots = new List<Armor>(4);
        accessorySlots = new List<Accessory>(5);

        weaponSlots.Add(new Bare_Handed());
        weaponSlots.Add(new No_Weapon());

        for (int i = 0; i < armorSlots.Capacity; i++) {
            armorSlots.Add(new No_Armor());
        }

        for (int i = 0; i < accessorySlots.Capacity; i++) {
            accessorySlots.Add(new No_Accessory());
        }
    }

    public Battle_Entity_Loadout(Battle_Entity_Loadout other) {
        weaponSlots    = new List<Weapon>(other.weaponSlots);
        shieldSlot     = new Shield(other.shieldSlot);
        armorSlots     = new List<Armor>(other.armorSlots);
        accessorySlots = new List<Accessory>(other.accessorySlots);
    }

    public List<Weapon> GetWeapons() {
        return weaponSlots;
    }

    public Shield GetShield() {
        return shieldSlot;
    }

    public List<Armor> GetArmorPieces() {
        return armorSlots;
    }

    public List<Accessory> GetAccessories() {
        return accessorySlots;
    }
}
