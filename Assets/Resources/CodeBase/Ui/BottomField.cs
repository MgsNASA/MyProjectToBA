using UnityEngine;
using UnityEngine.UI;

public class BottomField : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button achivementButton;

    private void Awake( )
    {
        if ( settingsButton != null )
        {
            settingsButton.onClick.AddListener ( ShowSettingsPanel );
        }
        if ( shopButton != null )
        {
            shopButton.onClick.AddListener ( ShowShopPanel );
        }
        if ( upgradeButton != null )
        {
            upgradeButton.onClick.AddListener ( ShowUpgradePanel );
        }
        if ( achivementButton != null )
        {
            achivementButton.onClick.AddListener ( ShowAchievementPanel );
        }
    }

    private void ShowSettingsPanel( )
    {


        UiManager.Instance.ShowSettingsPanel ();
    }

    private void ShowShopPanel( )
    {

        UiManager.Instance.ShowShopPanel ();
    }

    private void ShowUpgradePanel( )
    {

        UiManager.Instance.ShowUpgradePanel ();
    }

    private void ShowAchievementPanel( )
    {

        UiManager.Instance.ShowAchievementPanel ();
    }
}
