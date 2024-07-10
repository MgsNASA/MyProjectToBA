using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardAchievement : CardBase
{
    [SerializeField] private Achievement.AchievementType achievementType;
    [SerializeField] private Image progressSlider;
    [SerializeField] private Button claimButton;
    [SerializeField] private TextMeshProUGUI achievementText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI progressUnderText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private Image completedImage;

    private Achievement achievement;

    private void Start( )
    {
        InitializeAchievement ();
        UpdateUI ();
    }

    private void OnDestroy( )
    {
        if ( AchievementManager.Instance != null )
        {
            AchievementManager.Instance.UnregisterCard ( this );
        }
    }

    public Achievement.AchievementType GetAchievementType( )
    {
        return achievementType;
    }

    public void UpdateUI( )
    {
        if ( achievement != null )
        {
            // Установка текста для achievementText и rewardText
            achievementText.text = achievement.achievementType.ToString ();
            rewardText.text = $"Reward: {achievement.rewardAmount}";

            // Обновление прогресса в UI
            UpdateProgressUI ();
            UpdateUnderAchievementText ();
            UpdateButtonState ();
            completedImage.gameObject.SetActive ( achievement.isAchieved );
        }
        else
        {
            achievementText.text = "Unknown";
            rewardText.text = "Reward: 0";
            progressSlider.fillAmount = 0;
            progressText.text = "0%";
            progressUnderText.text = "0%";
            claimButton.interactable = false;
            completedImage.gameObject.SetActive ( false );
        }
    }

    private void UpdateProgressUI( )
    {
        if ( achievement != null )
        {
            progressSlider.fillAmount = achievement.progress / achievement.targetProgress;
        }
    }

    private void UpdateUnderAchievementText( )
    {
        if ( achievement != null )
        {
            float progressPercent = ( achievement.progress / achievement.targetProgress ) * 100f;
            progressText.text = $"{progressPercent:F0}%";
            progressUnderText.text = $"{progressPercent:F0}%";
        }
    }

    private void InitializeAchievement( )
    {
        achievement = AchievementManager.Instance.GetAchievement ( achievementType );
        if ( achievement != null )
        {
            LoadState ();
        }
    }

    public override void CheckAvailability( )
    {
        if ( achievement != null && !achievement.isAchieved && achievement.CheckCondition () )
        {
            achievement.UpdateProgress ( 1 );
            UpdateProgressUI ();
            if ( achievement.isAchieved )
            {
                OnAchievementCompleted ();
            }
        }
    }

    private void OnAchievementCompleted( )
    {
        completedImage.gameObject.SetActive ( true );
        claimButton.interactable = true;
    }

    protected override void LoadState( )
    {
        if ( achievement != null )
        {
            achievement.LoadState ();
            UpdateProgressUI ();
            UpdateButtonState ();
            completedImage.gameObject.SetActive ( achievement.isAchieved );
        }
    }

    protected override void SaveState( )
    {
        if ( achievement != null )
        {
            achievement.SaveState ();
            SaveCompletedImageState ( true );
        }
    }

    protected override void UpdateButtonState( )
    {
        if ( claimButton != null )
        {
            claimButton.interactable = achievement != null && !achievement.isAchieved;
        }
    }

    public void OnClaimButtonClicked( )
    {
        if ( achievement != null && achievement.isAchieved )
        {
            MoneyManager.Instance.Add ( achievement.rewardAmount );
            achievement.Achieve ();
            UpdateUI ();
            SaveCompletedImageState ( true );
            SaveState ();
        }
        else
        {
            Debug.LogWarning ( "Achievement is not achieved yet." );
        }
    }

    private void SaveCompletedImageState( bool isActive )
    {
        PlayerPrefs.SetInt ( GetCompletedImageKey () , isActive ? 1 : 0 );
        PlayerPrefs.Save ();
    }

    private string GetCompletedImageKey( )
    {
        return $"CompletedImageActive_{achievementType}";
    }
}
