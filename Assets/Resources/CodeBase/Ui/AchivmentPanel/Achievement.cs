using System;
using UnityEngine;

[System.Serializable]
public class Achievement
{
    public enum AchievementType
    {
        BuyAllUpgrades,
        Score100000Points,
        Collect5xInOneGame,
        Buy1Ball,
        Buy1Map,
        MaxOf3Upgrades
    }

    public AchievementType achievementType;
    public int rewardAmount;
    public bool isAchieved;
    public float progress;
    public float targetProgress;

    private string stateKey;
    private string progressKey;

    public Achievement( AchievementType type , int rewardAmount , float targetProgress )
    {
        this.achievementType = type;
        this.rewardAmount = rewardAmount;
        this.targetProgress = targetProgress;
        this.stateKey = $"AchievementState_{type}";
        this.progressKey = $"AchievementProgress_{type}";
        LoadState ();
    }

    public void Achieve( )
    {
        isAchieved = true;
        SaveState ();
    }

    public void UpdateProgress( float amount )
    {
        if ( !isAchieved )
        {
            progress += amount;
            if ( progress >= targetProgress )
            {
                progress = targetProgress;
                Achieve ();
            }
            SaveState ();
        }
    }

    public void SaveState( )
    {
        PlayerPrefs.SetInt ( stateKey , isAchieved ? 1 : 0 );
        PlayerPrefs.SetFloat ( progressKey , progress );
        PlayerPrefs.Save ();
    }

    public void LoadState( )
    {
        isAchieved = PlayerPrefs.GetInt ( stateKey , 0 ) == 1;
        progress = PlayerPrefs.GetFloat ( progressKey , 0 );
    }

    public bool CheckCondition( )
    {
        switch ( achievementType )
        {
            case AchievementType.BuyAllUpgrades:
                return UpgradeManager.Instance.CheckBuyAllUpgradesAchievement ();
            case AchievementType.Score100000Points:
                // Add logic for checking this condition
                return false;
            // Add other cases as needed
            default:
                return false;
        }
    }

    public void Clear( )
    {
        progress = 0;
        isAchieved = false;
        SaveState ();
    }
}
