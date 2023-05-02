using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection.Emit;

public class Utils
{
    public static Vector3 GetMouseWorldPosition() {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        return worldPos;
    }

    public static bool ApproximatelyEqual(float a, float b, float errorAllowed) {
        return (a - errorAllowed <= b && b <= a + errorAllowed);
    }

    public static Vector2 GetTouchWorldPosition(Touch touch) {
        return Camera.main.ScreenToWorldPoint(touch.position);
    }

    public static bool CheckPointIsWithinRadius(Vector2 centerPoint, Vector2 pointToCheck, double radius) {
        return radius >= Math.Sqrt((Math.Pow(centerPoint.x - pointToCheck.x, 2) + 
                                    Math.Pow(centerPoint.y - pointToCheck.y, 2)));
    }

    public static void InitGameData() {
        string dirPath = Application.persistentDataPath + "item_data";
        if (!Directory.Exists(dirPath)) {
            Directory.CreateDirectory(dirPath);
        }

        InitAccessoryData();
        //InitArmorData();
        //InitShieldData();
        //InitWeaponData();
    }

    public static void SaveGameData() {
        SaveTeamData();
    }

    public static void LoadGameData() {
        LoadTeamData();
    }

    private static void SaveTeamData() {
        string filePath = Application.persistentDataPath + "/team_data.dat";
        FileStream file;

        if (File.Exists(filePath) == true) {
            file = File.OpenWrite(filePath);
        } else {
            file = File.Create(filePath);
        }

        ArrayList data = new ArrayList(5);
        data.Add(Team_Data.names);
        data.Add(Team_Data.stats);
        data.Add(Team_Data.loadouts);
        data.Add(Team_Data.spells);
        data.Add(Team_Data.items);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    private static void LoadTeamData() {
        string filePath = Application.persistentDataPath + "/team_data.dat";
        FileStream file;

        if (File.Exists(filePath) == false) {
            Debug.LogError("No file found! Function: LoadTeamData");
            return;
        }

        file = File.OpenRead(filePath);
        
        BinaryFormatter bf = new BinaryFormatter();
        ArrayList data = (ArrayList)bf.Deserialize(file);
        file.Close();

        Team_Data.names    = (List<string>)                     data[0];
        Team_Data.stats    = (List<Battle_Entity_Stats>)        data[1];
        Team_Data.loadouts = (List<Battle_Entity_Loadout>)      data[2];
        Team_Data.spells   = (List<List<Battle_Entity_Spells>>) data[3];
        Team_Data.items    = (List<List<Item>>)                 data[4];
    }
    
    public static void InitAccessoryData() {
        string filePath = Application.persistentDataPath + "/item_data/accessory_data.dat";
        if (File.Exists(filePath) == true) {
            return;
        }

        using (FileStream file = new FileStream(filePath, FileMode.Append, FileAccess.Write))

        using (StreamWriter sw = new StreamWriter(file)) {
            Accessory accessory_to_write = new No_Accessory();

            sw.WriteLine("[Acc_Name]=" + accessory_to_write.GetItemName());
            sw.WriteLine("[Acc_Desc]=" + accessory_to_write.GetItemDesc());
            sw.WriteLine("[Acc_Equippable]=" + accessory_to_write.GetIsEquippable());
            sw.WriteLine("[Acc_Type_Slot]=" + accessory_to_write.GetAccessorySlots());
        }
    }

    public static void InitArmorData() {
        string filePath = Application.persistentDataPath + "/item_data/armor_data.dat";
        if (File.Exists(filePath) == true) {
            return;
        }

        using (FileStream file = new FileStream(filePath, FileMode.Append, FileAccess.Write))

        using (StreamWriter sw = new StreamWriter(file)) {
            Accessory accessory_to_write = new No_Accessory();

            sw.WriteLine("[Acc_Name]=" + accessory_to_write.GetItemName());
            sw.WriteLine("[Acc_Desc]=" + accessory_to_write.GetItemDesc());
            sw.WriteLine("[Acc_Equippable]=" + accessory_to_write.GetIsEquippable());
            sw.WriteLine("[Acc_Type_Slot]=" + accessory_to_write.GetAccessorySlots());
        }
    }

    public static void InitShieldData() {
        string filePath = Application.persistentDataPath + "/item_data/shield_data.dat";
        if (File.Exists(filePath) == true) {
            return;
        }

        using (FileStream file = new FileStream(filePath, FileMode.Append, FileAccess.Write))

        using (StreamWriter sw = new StreamWriter(file)) {
            Accessory accessory_to_write = new No_Accessory();

            sw.WriteLine("[Acc_Name]=" + accessory_to_write.GetItemName());
            sw.WriteLine("[Acc_Desc]=" + accessory_to_write.GetItemDesc());
            sw.WriteLine("[Acc_Equippable]=" + accessory_to_write.GetIsEquippable());
            sw.WriteLine("[Acc_Type_Slot]=" + accessory_to_write.GetAccessorySlots());
        }
    }

    public static void InitWeaponData() {
        string filePath = Application.persistentDataPath + "/item_data/weapon_data.dat";
        if (File.Exists(filePath) == true) {
            return;
        }

        using (FileStream file = new FileStream(filePath, FileMode.Append, FileAccess.Write))

        using (StreamWriter sw = new StreamWriter(file)) {
            Accessory accessory_to_write = new No_Accessory();

            sw.WriteLine("[Acc_Name]=" + accessory_to_write.GetItemName());
            sw.WriteLine("[Acc_Desc]=" + accessory_to_write.GetItemDesc());
            sw.WriteLine("[Acc_Equippable]=" + accessory_to_write.GetIsEquippable());
            sw.WriteLine("[Acc_Type_Slot]=" + accessory_to_write.GetAccessorySlots());
        }
    }

}
