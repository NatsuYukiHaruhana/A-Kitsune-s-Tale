public class Battle_Entity_Stat_Change
{
    public enum StatChangeType {
        Buff,
        Debuff,
        NULL
    }

    private Battle_Entity_Stats stats;
    private StatChangeType statChangeType;
    private int turnCount;
    private Battle_Entity target;
    private bool readyToRemove;

    public Battle_Entity_Stat_Change() {
        stats = new Battle_Entity_Stats();
        statChangeType = StatChangeType.NULL;
        turnCount = 0;
        target = null;
        readyToRemove = false;
    }

    public Battle_Entity_Stat_Change(Battle_Entity_Stats stats, StatChangeType statChangeType, int turnCount, Battle_Entity target) {
        this.stats = stats;
        this.statChangeType = statChangeType;
        this.turnCount = turnCount + 1;
        this.target = target;
        readyToRemove = false;
    }

    public void LowerTurnCount() {
        turnCount--;

        if (turnCount == 0) {
            RemoveStatChanges();
            readyToRemove = true;
        }
    }

    public void ApplyStatChanges() {
        Battle_Entity_Stats newStats = target.GetStats();

        if (statChangeType == StatChangeType.Buff) {
            newStats += stats;
            target.SetStats(newStats);
        } else if (statChangeType == StatChangeType.Debuff) {
            newStats -= stats;
            target.SetStats(newStats);
        }
    }
    
    public void RemoveStatChanges() {
        Battle_Entity_Stats newStats = target.GetStats();

        if (statChangeType == StatChangeType.Buff) {
            newStats -= stats;
            target.SetStats(newStats);
        } else if (statChangeType == StatChangeType.Debuff) {
            newStats += stats;
            target.SetStats(newStats);
        }
    }

    public bool GetReadyToRemove() {
        return readyToRemove;
    }
}
