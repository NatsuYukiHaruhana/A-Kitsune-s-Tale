using System;
using System.Runtime.Serialization;

[Serializable()]
public struct Battle_Entity_Stats : ISerializable
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

    public Battle_Entity_Stats(SerializationInfo info, StreamingContext ctxt) {
        level    = (int)  info.GetValue("Level",       typeof(int));
        currXP   = (float)info.GetValue("CurrentXP",   typeof(float));
        maxXP    = (float)info.GetValue("MaxXP",       typeof(float));
        currHP   = (float)info.GetValue("CurrentHP",   typeof(float));
        maxHP    = (float)info.GetValue("MaxHP",       typeof(float));
        currMana = (float)info.GetValue("CurrentMana", typeof(float));
        maxMana  = (float)info.GetValue("MaxMana",     typeof(float));
        str      = (float)info.GetValue("Strength",    typeof(float));
        mag      = (float)info.GetValue("Magic",       typeof(float));
        spd      = (float)info.GetValue("Speed",       typeof(float));
        def      = (float)info.GetValue("Defense",     typeof(float));
        res      = (float)info.GetValue("Resistance",  typeof(float));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
        info.AddValue("Level",       level);
        info.AddValue("CurrentXP",   currXP);
        info.AddValue("MaxXP",       maxXP);
        info.AddValue("CurrentHP",   currHP);
        info.AddValue("MaxHP",       maxHP);
        info.AddValue("CurrentMana", currMana);
        info.AddValue("MaxMana",     maxMana);
        info.AddValue("Strength",    str);
        info.AddValue("Magic",       mag);
        info.AddValue("Speed",       spd);
        info.AddValue("Defense",     def);
        info.AddValue("Resistance",  res);
    }
}
