using System;

[Serializable()]
public struct Battle_Entity_Stats
{
    public int level;
    public float currXP;
    public float maxXP;
    public float currHP;
    public float maxHP;
    public float currMana;
    public float maxMana;
    public float str;
    public float mag;
    public float spd;
    public float def;
    public float res;

    public Battle_Entity_Stats(int newLevel, float newCurrXP, float newMaxXP,
                                float newCurrHP, float newMaxHP, float newCurrMana, float newMaxMana,
                                float newStr, float newMag, float newSpd, float newDef, float newRes) {
        level    = newLevel;
        currXP   = newCurrXP;
        maxXP    = newMaxXP;
        currHP   = newCurrHP;
        maxHP    = newMaxHP;
        currMana = newCurrMana;
        maxMana  = newMaxMana;
        str      = newStr;
        mag      = newMag;
        spd      = newSpd;
        def      = newDef;
        res      = newRes;
    }

    public Battle_Entity_Stats(Battle_Entity_Stats other) {
        level    = other.level;
        currXP   = other.currXP;
        maxXP    = other.maxXP;
        currHP   = other.currHP;
        maxHP    = other.maxHP;
        currMana = other.currMana;
        maxMana  = other.maxMana;
        str      = other.str;
        mag      = other.mag;
        spd      = other.spd;
        def      = other.def;
        res      = other.res;
    }

    public bool Equals(Battle_Entity_Stats other) {
        if (level   == other.level   &&
            currXP  == other.currXP  &&
            maxXP   == other.maxXP   &&
            maxHP   == other.maxHP   &&
            maxMana == other.maxMana &&
            str     == other.str     &&
            mag     == other.mag     &&
            spd     == other.spd     &&
            def     == other.def     &&
            res     == other.res) {
            return true;
        }

        return false;
    }

    public static Battle_Entity_Stats operator+(Battle_Entity_Stats left, Battle_Entity_Stats right) {
        Battle_Entity_Stats retStats = left;

        retStats.currHP += right.maxHP;
        retStats.maxHP += right.maxHP;
        retStats.currMana += right.currMana;
        retStats.maxMana += right.maxMana;
        retStats.str += right.str;
        retStats.mag += right.mag;
        retStats.spd += right.spd;
        retStats.def += right.def;
        retStats.res += right.res;

        return retStats;
    }

    public static Battle_Entity_Stats operator-(Battle_Entity_Stats left, Battle_Entity_Stats right) {
        Battle_Entity_Stats retStats = left;

        retStats.maxHP -= right.maxHP;
        if (retStats.currHP > retStats.maxHP) {
            retStats.currHP = retStats.maxHP;
        }
        retStats.maxMana -= right.maxMana;
        if (retStats.currMana > retStats.maxMana) {
            retStats.currMana = retStats.maxMana;
        }
        retStats.str -= right.str;
        retStats.mag -= right.mag;
        retStats.spd -= right.spd;
        retStats.def -= right.def;
        retStats.res -= right.res;

        return retStats;
    }

    override public string ToString() {
        return "Level: " + level + "\n" +
                "XP: " + currXP + "/" + maxXP + "\n" +
                "HP: " + currHP + "/" + maxHP + "\n" +
                "Mana: " + currMana + "/" + maxMana + "\n" +
                "Str: " + str + "\n" +
                "Mag: " + mag + "\n" +
                "Spd: " + spd + "\n" +
                "Def: " + def + "\n" +
                "Res: " + res;
    }
}
