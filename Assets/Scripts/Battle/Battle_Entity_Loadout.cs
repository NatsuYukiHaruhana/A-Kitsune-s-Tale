using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class Battle_Entity_Loadout : ISerializable
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

    public Battle_Entity_Loadout(SerializationInfo info, StreamingContext ctxt) {
        weaponSlots    = (List<Weapon>)   info.GetValue("Weapons",   typeof(List<Weapon>));
        shieldSlot     = (Shield)         info.GetValue("Shield",    typeof(Shield));
        armorSlots     = (List<Armor>)    info.GetValue("Armor",     typeof(List<Armor>));
        accessorySlots = (List<Accessory>)info.GetValue("Accessory", typeof(List<Accessory>));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
        info.AddValue("Weapons",   weaponSlots);
        info.AddValue("Shield",    shieldSlot);
        info.AddValue("Armor",     armorSlots);
        info.AddValue("Accessory", accessorySlots);

        for (int i = 0; i < weaponSlots.Capacity; i++) {
            weaponSlots[i].GetObjectData(info, ctxt);
        }

        shieldSlot.GetObjectData(info, ctxt);
        
        for (int i = 0; i < armorSlots.Capacity; i++) {
            armorSlots[i].GetObjectData(info, ctxt);
        }

        for (int i = 0; i < accessorySlots.Capacity; i++) {
            accessorySlots[i].GetObjectData(info, ctxt);
        }
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
