#define ALLOW_HIRAGANA
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection.Emit;

public class Utils
{
    public static int saveFile = 0;
    public static string username = "";
    public static string currentLanguage = "null";

    public static string enemyToBattle = "null";

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
        string dirPath = Application.persistentDataPath + "/item_data";
        if (!Directory.Exists(dirPath)) {
            Directory.CreateDirectory(dirPath);
        }

        InitTextRecognitionData();
        InitUnitData();
        InitEnemyData();
    }

    public static void SaveGameData() {
        SaveTeamData();
    }

    public static void LoadGameData() {
        LoadTeamData();
    }

    public static char GetRandomCharacter(bool allowHiragana, bool allowKatakana) {
        List<string> allowedFiles = new List<string>();
        if (allowHiragana) {
            string filePath = Application.persistentDataPath + "/hiragana.dat";

            if (!File.Exists(filePath)) {
                Debug.LogError("hiragana.dat was not found in " + Application.persistentDataPath + "!");
            }
            allowedFiles.Add(filePath);
        }
        if (allowKatakana) {
            string filePath = Application.persistentDataPath + "/katakana.dat";

            if (!File.Exists(filePath)) {
                Debug.LogError("katakana.dat was not found in " + Application.persistentDataPath + "!");
            }
            allowedFiles.Add(filePath);
        }

        if (allowedFiles.Count == 0) {
            return '\0';
        }

        int fileToUse;

        switch (currentLanguage) {
            case "hiragana":
            case "hiragana2": {
                fileToUse = 0;
                break;
            }
            case "katakana": {
                fileToUse = 1;
                break;
            }
            default: {
                fileToUse = -1;
                break;
            }
        }

        if (fileToUse == -1) {
            return '\0';
        }

        using (FileStream file = File.OpenRead(allowedFiles[fileToUse])) {
            using (StreamReader sr = new StreamReader(file, System.Text.Encoding.UTF8)) {
                string line = sr.ReadLine();
                return line[UnityEngine.Random.Range(0, 10)];
            }
        }
    }

    private static void SaveTeamData() {
        string filePath = Application.persistentDataPath + "/team_data_" + saveFile + ".dat";
        FileStream file;

        if (File.Exists(filePath) == true) {
            file = File.OpenWrite(filePath);
        } else {
            file = File.Create(filePath);
        }

        ArrayList data = Team_Data.GetData();

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    private static void LoadTeamData() {
        string filePath = Application.persistentDataPath + "/team_data_" + saveFile + ".dat";
        FileStream file;

        if (File.Exists(filePath) == false) {
            Debug.LogError("No file found! Function: LoadTeamData");
            return;
        }

        file = File.OpenRead(filePath);
        
        BinaryFormatter bf = new BinaryFormatter();
        ArrayList data = (ArrayList)bf.Deserialize(file);
        file.Close();

        Team_Data.InitData((List<string>) data[0], 
                        (List<Battle_Entity_Stats>) data[1], 
                        (List<Battle_Entity_Loadout>) data[2], 
                        (List<List<Battle_Entity_Spells>>) data[3],
                        (List<Item>) data[4]);
    }

    private static void InitTextRecognitionData() {
        string filePath = Application.persistentDataPath + "/hiragana.dat";
        if (!File.Exists(filePath)) {
            WriteOneLineToFile(filePath,
                            "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん" +
                            "がぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽ");
        }

        filePath = Application.persistentDataPath + "/katakana.dat";
        if (!File.Exists(filePath)) {
            WriteOneLineToFile(filePath,
                            "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲン" +
                            "ガギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポ");
        }

        filePath = Application.persistentDataPath + "/kanji.dat";
        if (!File.Exists(filePath)) {
            WriteOneLineToFile(filePath,
                            ""); // TODO: Add kanji characters
        }
    }

    private static void WriteOneLineToFile(string filePath, string line) {
        using (FileStream file = new FileStream(filePath, FileMode.Append, FileAccess.Write))

        using (StreamWriter sw = new StreamWriter(file)) {
            sw.WriteLine(line);
        }
    }

    private static void InitUnitData() {
        string filePath = Application.persistentDataPath + "/team_data_" + saveFile + ".dat";
        FileStream file;

        if (File.Exists(filePath) == true) {
            LoadTeamData();
            return;
        }

        Battle_Entity_Stats unitStats = new Battle_Entity_Stats(1,   // level
                                                0,    // currXP
                                                100,  // maxXP
                                                100,  // currHP
                                                100,  // maxHP
                                                100,  // currMana
                                                100,  // maxMana
                                                20,   // strength
                                                10,   // magic
                                                10,   // speed
                                                10,   // defense
                                                10); // resistance);

        List<Battle_Entity_Spells> spells = new List<Battle_Entity_Spells> {
            new Fireball(),
            new Frostbite(),
            new Lightning(),
            new Shadow(),
            new HolyLight()
        };

        Team_Data.AddNewEntry(username, unitStats, new Battle_Entity_Loadout(), spells);

        for (int i = 0; i < 3; i++) {
            Team_Data.AddItem(new Potion());
            Team_Data.AddItem(new Mana_Potion());
            Team_Data.AddItem(new Scroll_of_Strength());
            Team_Data.AddItem(new Scroll_of_Magic());
            Team_Data.AddItem(new Scroll_of_Protection());
        }

        SaveTeamData();
    }

    private static void InitEnemyData() {
        string filePath = Application.persistentDataPath + "/enemy_data.dat";
        FileStream file;

        if (File.Exists(filePath) == true) {
            file = File.OpenWrite(filePath);
        } else {
            file = File.Create(filePath);
        }

        // Format: "Enemy_Name: sizeX sizeY offsetX offsetY"
        string dataString = "Flower: +0.66 +1.23 +0.00 -0.39\n" +
                            "Masked Doctor: +0.94 +1.26 -0.28 -0.37\n" +
                            "Cavern Slime: +0.57 +0.60 -0.12 -0.20" +
                            "Serpent: +0.50 +1.67 -0.34 -0.14";

        ArrayList data = new ArrayList() { dataString };

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public static List<float> LoadEnemyData(string enemyName) {
        string filePath = Application.persistentDataPath + "/enemy_data.dat";
        FileStream file;

        if (File.Exists(filePath) == true) {
            file = File.OpenRead(filePath);
        } else {
            Debug.LogError("No enemy_data.dat file found!");
            return null;
        }


        BinaryFormatter bf = new BinaryFormatter();

        ArrayList data = (ArrayList)bf.Deserialize(file);
        file.Close();

        string dataString = (string)data[0];

        if (!dataString.Contains(enemyName)) {
            Debug.LogError("No enemy named " + enemyName + " found!");
            return null;
        }

        List<float> list = new List<float>(4);

        for (int i = 0; i < 4; i++) { 
            list.Add(float.Parse(dataString.Substring(dataString.IndexOf(enemyName) + enemyName.Length + 2 + i * 6, 5)));
        }

        return list;
    }
}
