using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopWithoutEnergyPanel : MonoBehaviour
{
    public Button _mainMenuPanel;
    public Button _PursharseButton;
    public TextMeshProUGUI BestScore;
    public TextMeshProUGUI CurentScore;
    public TextMeshProUGUI EnergyDisplay;  // Text to display the current energy
    [SerializeField]
    private List<Image> energyImages = new List<Image> (); // List to store energy images
    private EnergyManager energyManager; // Reference to EnergyManager
    private ScoreManager scoreManager;  // Reference to ScoreManager

    void Start( )
    {
        // Find and assign the ScoreManager
        scoreManager = FindObjectOfType<ScoreManager> ();
        if ( scoreManager == null )
        {
            Debug.LogError ( "ScoreManager not found on the scene." );
            return;
        }

        // Find and assign the EnergyManager
        energyManager = EnergyManager.Instance;
        if ( energyManager == null )
        {
            Debug.LogError ( "EnergyManager not found on the scene." );
            return;
        }

        // Bind the purchase button click event
        _PursharseButton.onClick.AddListener ( PurchaseEnergy );
        _mainMenuPanel.onClick.AddListener (UiManager.Instance.ShowMainSceneWithAdditionalPanels);
        // Update the scores and energy on the UI
        UpdateUI ();
    }

    void Update( )
    {
        // Update the UI every frame
        UpdateUI ();
    }

    void PurchaseEnergy( )
    {
        int energyAmount = 1;  // Amount of energy to purchase
        int energyCost = 2000;  // Cost of energy in score points

        if ( energyManager.PurchaseEnergy ( energyAmount , energyCost ) )
        {
            Debug.Log ( "Energy purchased successfully." );
            UpdateUI ();
        }
        else
        {
            Debug.Log ( "Not enough score to purchase energy." );
        }
    }

    void UpdateUI( )
    {
        if ( scoreManager != null )
        {
            CurentScore.text = "Current Score: " + scoreManager.GetCurrentAmount ().ToString ();
            BestScore.text = "Best Score: " + scoreManager.GetBestScore ().ToString ();
        }

        if ( energyManager != null )
        {
            int currentEnergy = energyManager.GetCurrentAmount ();
            EnergyDisplay.text = "Energy: " + currentEnergy.ToString ();

            if ( currentEnergy > 0 )
            {
 
                EnergyDisplay.enabled = false;
            }
            else
            {
             
                TimeSpan timeLeft = energyManager.NextEnergyIncreaseTime - DateTime.UtcNow;
                if ( timeLeft.TotalSeconds > 0 )
                {
                    string formattedTime = string.Format ( "{0:D2}:{1:D2}" , timeLeft.Minutes , timeLeft.Seconds );
                    EnergyDisplay.text = $"{formattedTime}";
                }
                else
                {
                    EnergyDisplay.text = "00:00";
                }
            }

            UpdateEnergyImages ( currentEnergy );
        }
    }

    void UpdateEnergyImages( int currentEnergy )
    {
        for ( int i = 0; i < energyImages.Count; i++ )
        {
            if ( i < currentEnergy )
            {
                energyImages [ i ].gameObject.SetActive ( true );
            }
            else
            {
                energyImages [ i ].gameObject.SetActive ( false );
            }
        }
    }
}
