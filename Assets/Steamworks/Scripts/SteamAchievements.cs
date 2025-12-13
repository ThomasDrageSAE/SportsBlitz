using UnityEngine;
using Helper.Blake;

public class SteamAchievementsManager : Singleton<SteamAchievementsManager>
{
    [SerializeField] private bool debug = false;

    public bool IsThisAchievementUnlocked(Achievements achievementID)
    {
        var ach = new Steamworks.Data.Achievement(achievementID.ToString());
        if (debug) Debug.Log($"Achievement {ach} status: {ach.State}");
        return ach.State;
    }

    public void UnlockAchievement(Achievements achievementID)
    {
        var ach = new Steamworks.Data.Achievement(achievementID.ToString());
        ach.Trigger();

        if (debug) Debug.Log($"Achievement Unlocked: {achievementID}");
    }

    public void ClearAchievement(Achievements achievementID)
    {
        var ach = new Steamworks.Data.Achievement(achievementID.ToString());
        ach.Clear();

        if (debug) Debug.Log($"Achievement Cleared: {achievementID}");
    }
}

public enum Achievements
{
    ACH_FIRST_WIN,

}