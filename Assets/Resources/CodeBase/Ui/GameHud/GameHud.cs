using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHud : MonoBehaviour
{
    public Button restartButton;
    public Button popButton;
    public TextMeshProUGUI scoreText;
    public ScoreManager scoreManager;
    public EnergyManager energyManager; // Added EnergyManager reference
    public Image popPanel;
    public Image popPanel_Without_Panel;

    void Start( )
    {
        // Find ScoreManager on the scene
        scoreManager = FindObjectOfType<ScoreManager> ();
        if ( scoreManager == null )
        {
            Debug.LogError ( "ScoreManager not found on the scene." );
            return;
        }

        // Find EnergyManager on the scene
        energyManager = EnergyManager.Instance;
        if ( energyManager == null )
        {
            Debug.LogError ( "EnergyManager not found on the scene." );
            return;
        }

        // Bind functions to buttons
        restartButton.onClick.AddListener ( RestartGame );
        popButton.onClick.AddListener ( HandlePopButton );

        // Initialize score text
        UpdateScoreText ();
    }

    void RestartGame( )
    {
        scoreManager.Restart ();
        UpdateScoreText ();
    }

    void HandlePopButton( )
    {
        if ( energyManager.GetCurrentAmount () > 0 )
        {
            // Show panel with energy
            ShowPopPanel ();
        }
        else
        {
            // Show panel without energy
            ShowPopPanelWithoutEnergy ();
        }
    }

    void ShowPopPanel( )
    {
        popPanel.gameObject.SetActive ( true );
        popPanel_Without_Panel.gameObject.SetActive ( false );
    }

    void ShowPopPanelWithoutEnergy( )
    {
        popPanel.gameObject.SetActive ( false );
        popPanel_Without_Panel.gameObject.SetActive ( true );
    }

    public void UpdateScoreText( )
    {
        scoreText.text = "Score: " + scoreManager.GetCurrentAmount ().ToString ();
    }
}
