using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels; // List of all panels
    [SerializeField] private AnimatedLoadingPanel loadingPanel; // Reference to AnimatedLoadingPanel
    private static Dictionary<string , GameObject> panelDictionary = new Dictionary<string , GameObject> ();

    public const string MainScenePanel = "MainScene_Panel";
    public const string BoostPanel = "Boost_Panel";
    public const string GameBootstrapPointLevel = "Game_BoostrapPoint_Level";
    public const string SettingsPanel = "Settings_Panel";
    public const string BottomField = "BottomField";
    public const string ResourcePanel = "Resource_Panel";
    public const string GameHud = "GameHud";
    public const string ShopPanel = "Shop_Panel";
    public const string UpgradePanel = "UpgradePanel";
    public const string AnimateLoadingPanel = "AnimateLoadingPanel";
    public const string AchievementPanel = "LeaderBoard_Panel";
    public const string PopPanel = "Pop_Panel"; // New constant for PopPanel
    public const string PopWithoutEnergyPanel = "PopWithoutEnergy_Panel"; // New constant for PopWithoutEnergyPanel

    private static UiManager instance;

    public static UiManager Instance
    {
        get
        {
            if ( instance == null )
            {
                instance = FindObjectOfType<UiManager> ();
                if ( instance == null )
                {
                    GameObject uiManagerObject = new GameObject ( "UiManager" );
                    instance = uiManagerObject.AddComponent<UiManager> ();
                }
            }
            return instance;
        }
    }

    private string currentPanel;

    private void Awake( )
    {
        if ( instance == null )
        {
            instance = this;
            DontDestroyOnLoad ( gameObject );
        }
        else if ( instance != this )
        {
            Destroy ( gameObject );
            return;
        }

        if ( panelDictionary.Count == 0 )
        {
            foreach ( GameObject panel in panels )
            {
                if ( panel != null )
                {
                    panelDictionary.Add ( panel.name , panel );
                    panel.SetActive ( false ); // All panels are disabled initially
                }
            }
        }

        ShowGameBootstrapPointLevel ();
    }

    public static void ShowPanel( string panelName )
    {
        if ( panelDictionary.TryGetValue ( panelName , out GameObject panel ) )
        {
            // Check if the panel is already active
            if ( Instance.currentPanel == panelName )
            {
                // If active, switch to MainScenePanel
                panelName = MainScenePanel;
                panel = panelDictionary [ MainScenePanel ];
            }

            // Hide all panels before showing a new one if showing GameHud
            if ( panelName == GameHud )
            {
                HideAllPanels ();
            }

            panel.SetActive ( true );

            // Freeze the game if showing something other than GameHud
            if ( panelName != GameHud )
            {
                // Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }

            Instance.currentPanel = panelName;
        }
        else
        {
            Debug.LogError ( "Panel not found: " + panelName );
        }
    }

    public static void HidePanel( string panelName )
    {
        if ( panelDictionary.TryGetValue ( panelName , out GameObject panel ) )
        {
            panel.SetActive ( false );
            // Unfreeze the game if hiding GameHud
            if ( panelName == GameHud )
            {
                Time.timeScale = 1f;
            }
        }
        else
        {
            Debug.LogError ( "Panel not found: " + panelName );
        }
    }

    public static void HideAllPanels( )
    {
        foreach ( GameObject panel in panelDictionary.Values )
        {
            if ( panel.name != ResourcePanel && panel.name != BottomField ) // Disable all panels except ResourcePanel and BottomField
            {
                panel.SetActive ( false );
            }
        }
        // Unfreeze the game if hiding all panels
        Time.timeScale = 1f;
    }

    public void HideAllPanelsInstance( )
    {
        foreach ( GameObject panel in panelDictionary.Values )
        {
            if ( panel.name != ResourcePanel && panel.name != BottomField ) // Disable all panels except ResourcePanel and BottomField
            {
                panel.SetActive ( false );
            }
        }
        // Unfreeze the game if hiding all panels
        Time.timeScale = 1f;
    }

    public static bool IsPanelActive( string panelName )
    {
        if ( panelDictionary.TryGetValue ( panelName , out GameObject panel ) )
        {
            return panel.activeSelf;
        }
        else
        {
            Debug.LogError ( "Panel not found: " + panelName );
            return false;
        }
    }

    // Inspector wrappers
    public void ShowMainScenePanel( ) => ShowPanelWithLoading ( MainScenePanel );
    public void ShowBoostPanel( ) => ShowPanelWithLoading ( BoostPanel );
    public void ShowGameBootstrapPointLevel( ) => ShowPanel ( GameBootstrapPointLevel );
    public void ShowSettingsPanel( ) => ShowPanelWithLoading ( SettingsPanel );
    public void ShowBottomField( ) => ShowPanelWithLoading ( BottomField );
    public void ShowResourcePanel( ) => ShowPanelWithLoading ( ResourcePanel );
    public void ShowGameHud( ) => ShowPanelWithLoading ( GameHud );
    public void ShowShopPanel( ) => ShowPanelWithLoading ( ShopPanel );
    public void ShowUpgradePanel( ) => ShowPanelWithLoading ( UpgradePanel );
    public void ShowAchievementPanel( ) => ShowPanelWithLoading ( AchievementPanel ); // Wrapper for AchievementPanel
    public void ShowPopPanel( ) => ShowPanelWithLoading ( PopPanel ); // Wrapper for PopPanel
    public void ShowPopWithoutEnergyPanel( ) => ShowPanelWithLoading ( PopWithoutEnergyPanel ); // Wrapper for PopWithoutEnergyPanel

    public void HideMainScenePanel( ) => HidePanel ( MainScenePanel );
    public void HideBoostPanel( ) => HidePanel ( BoostPanel );
    public void HideGameBootstrapPointLevel( ) => HidePanel ( GameBootstrapPointLevel );
    public void HideSettingsPanel( ) => HidePanel ( SettingsPanel );
    public void HideBottomField( ) => HidePanel ( BottomField );
    public void HideResourcePanel( ) => HidePanel ( ResourcePanel );
    public void HideGameHud( ) => HidePanel ( GameHud );
    public void HideShopPanel( ) => HidePanel ( ShopPanel );
    public void HideUpgradePanel( ) => HidePanel ( UpgradePanel );
    public void HideAchievementPanel( ) => HidePanel ( AchievementPanel ); // Wrapper for AchievementPanel
    public void HidePopPanel( ) => HidePanel ( PopPanel ); // Wrapper for PopPanel
    public void HidePopWithoutEnergyPanel( ) => HidePanel ( PopWithoutEnergyPanel ); // Wrapper for PopWithoutEnergyPanel

    // Methods for showing/hiding multiple panels

    // New method for showing MainScenePanel, ResourcePanel, and BottomField together
    public void ShowMainSceneWithAdditionalPanels( )
    {
        ShowPanel ( MainScenePanel );
        ShowPanel ( ResourcePanel );
        ShowPanel ( BottomField );
    }

    // Method for showing the loading panel, performing operations, and switching panels
    public void ShowPanelWithLoading( string targetPanel )
    {
        if ( targetPanel != AnimateLoadingPanel )
        {
            loadingPanel.Show ();
            AudioManager.Instance.Play ( "ClickButton" ); // Play sound when showing the panel
            loadingPanel.SetTargetPanel ( targetPanel );
        }
        else
        {
            ShowPanel ( targetPanel );
        }
    }
}
