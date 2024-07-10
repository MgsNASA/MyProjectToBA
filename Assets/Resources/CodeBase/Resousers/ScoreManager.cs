using System;
using UnityEngine;

public class ScoreManager : ResourceManagerBase
{
    [SerializeField]
    private int currentScore;
    private int bestScore;  // Add this line
    [SerializeField]
    private LeaderboardManager leaderboardManager;

    private void Awake( )
    {
        currentScore = PlayerPrefs.GetInt ( "Score" , 0 );
        bestScore = PlayerPrefs.GetInt ( "BestScore" , 0 );  // Add this line
    }

    public override void Add( int amount )
    {
        currentScore += amount;
        SaveData ();
        Debug.Log ( $"Score added: {amount}. Current score: {currentScore}" );
    }

    public override bool Spend( int amount )
    {
        if ( currentScore >= amount )
        {
            currentScore -= amount;
            SaveData ();
            Debug.Log ( $"Score spent: {amount}. Current score: {currentScore}" );
            return true;
        }
        else
        {
            Debug.Log ( "Not enough score." );
            return false;
        }
    }

    public override int GetCurrentAmount( )
    {
        return currentScore;
    }

    public int GetBestScore( )  // Add this method
    {
        return bestScore;
    }

    public override void SaveData( )
    {
        PlayerPrefs.SetInt ( "Score" , currentScore );
        if ( currentScore > bestScore )
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt ( "BestScore" , bestScore );
        }
        PlayerPrefs.Save ();
    }

    public override void LoadData( )
    {
        currentScore = PlayerPrefs.GetInt ( "Score" , 0 );
        bestScore = PlayerPrefs.GetInt ( "BestScore" , 0 );  // Add this line
    }

    public void Restart( )
    {
        leaderboardManager?.AddEntry ( currentScore );
        leaderboardManager?.SaveLeaderboard ();
        currentScore = 0;
        SaveData ();
        Debug.Log ( "Score has been reset." );
    }

    public override void GetCurrent( )
    {
        throw new NotImplementedException ();
    }
}
