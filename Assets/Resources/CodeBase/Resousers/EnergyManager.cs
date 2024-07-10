using System;
using UnityEngine;

public class EnergyManager : ResourceManagerBase
{
    private static EnergyManager instance;
    private static readonly object lockObject = new object ();

    public static EnergyManager Instance
    {
        get
        {
            lock ( lockObject )
            {
                if ( instance == null )
                {
                    instance = FindObjectOfType<EnergyManager> ();
                    if ( instance == null )
                    {
                        GameObject singletonObject = new GameObject ( "EnergyManager" );
                        instance = singletonObject.AddComponent<EnergyManager> ();
                    }
                }
                return instance;
            }
        }
    }

    private int currentEnergy;
    private int maxEnergy = 6;
    private float energyIncreaseInterval = 300.0f; // Interval for energy increase in seconds (5 minutes)
    private DateTime lastUpdateTime; // Last energy update time

    private void Start( )
    {
        LoadData (); // Load saved data at startup
        UpdateEnergyOverTime (); // Update energy considering elapsed time
        InvokeRepeating ( "UpdateEnergyOverTime" , 0.0f , 1.0f ); // Call the method every second to track time
    }

    private void Update( )
    {
        if ( Input.GetKeyDown ( KeyCode.Space ) )
        {
            Spend ( 1 ); // Spend energy on space key press
        }
    }

    private void UpdateEnergyOverTime( )
    {
        TimeSpan elapsedTime = DateTime.UtcNow - lastUpdateTime;

        // Calculate the number of full intervals passed since the last update
        int intervalsPassed = Mathf.FloorToInt ( ( float ) ( elapsedTime.TotalSeconds / energyIncreaseInterval ) );

        if ( intervalsPassed > 0 )
        {
            // Increase energy by the number of full intervals
            currentEnergy = Mathf.Min ( currentEnergy + intervalsPassed , maxEnergy );
            Debug.Log ( "Energy Increased: " + currentEnergy );

            // Update the last update time
            lastUpdateTime = DateTime.UtcNow;

            // Save data
            SaveData ();
        }
    }

    public void AddEnergy( int amount )
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp ( currentEnergy , 0 , maxEnergy );
        Debug.Log ( "Energy Added: " + amount );
        SaveData ();
    }

    public override void GetCurrent( )
    {
        Debug.Log ( "Current Energy: " + currentEnergy );
    }
    public bool PurchaseEnergy( int amount , int cost )
    {
        // Assuming you have a ScoreManager instance to check if the player has enough score
        ScoreManager scoreManager = FindObjectOfType<ScoreManager> ();
        if ( scoreManager != null && scoreManager.Spend ( cost ) )
        {
            AddEnergy ( amount );
            return true;
        }
        return false;
    }
    public override int GetCurrentAmount( )
    {
        return currentEnergy;
    }

    public override void LoadData( )
    {
        if ( PlayerPrefs.HasKey ( "CurrentEnergy" ) )
        {
            currentEnergy = PlayerPrefs.GetInt ( "CurrentEnergy" );
        }
        else
        {
            currentEnergy = 0;
        }

        long storedTicks;
        if ( PlayerPrefs.HasKey ( "LastUpdateTimeTicks" ) && long.TryParse ( PlayerPrefs.GetString ( "LastUpdateTimeTicks" ) , out storedTicks ) )
        {
            lastUpdateTime = new DateTime ( storedTicks );
        }
        else
        {
            lastUpdateTime = DateTime.UtcNow; // Use the current UTC time if no saved time
        }
    }

    public override void SaveData( )
    {
        PlayerPrefs.SetInt ( "CurrentEnergy" , currentEnergy );
        PlayerPrefs.SetString ( "LastUpdateTimeTicks" , lastUpdateTime.Ticks.ToString () );
        PlayerPrefs.Save ();
    }

    public override bool Spend( int amount )
    {
        if ( currentEnergy >= amount )
        {
            currentEnergy -= amount;
            Debug.Log ( "Energy Spent: " + amount );
            SaveData ();
            return true;
        }
        else
        {
            Debug.Log ( "Not enough energy!" );
            return false;
        }
    }

    public override void Add( int amount )
    {
        throw new NotImplementedException ();
    }

    public DateTime NextEnergyIncreaseTime
    {
        get
        {
            if ( currentEnergy < maxEnergy )
            {
                return lastUpdateTime.AddSeconds ( energyIncreaseInterval );
            }
            else
            {
                return DateTime.MaxValue; // Если энергия максимальная, следующее пополнение не требуется
            }
        }
    }
}
