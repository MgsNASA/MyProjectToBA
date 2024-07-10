using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUpgrade : CardBase
{
    public enum UpgradeType
    {
        SlotHitChance,
        IncreaseEnergy,
        DoubleBall,
        FewerBarriers,
        DoubleStrength,
        MoreReward
    }

    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private Upgrade upgrade;

    public Sprite notPurchasedSprite;
    public Sprite purchasedSprite;
    public Sprite upgradedSprite;
    private Image background;
    public Sprite spriteBackground;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    private Image buttonImage;
    private Button button;
    private TextMeshProUGUI buttonText;
    [SerializeField]
    private MoneyManager moneyManager;
    private string stateKey;

    public bool IsPurchasedOrUpgraded { get; private set; } = false;
    public UpgradeType UpgradeTypeEnum
    {
        get
        {
            return upgradeType;
        }
        set
        {
            upgradeType = value;
        }
    }

    private void Awake( )
    {
        stateKey = $"CardUpgradeState_{GetInstanceID ()}_{upgradeType}";

        button = GetComponentInChildren<Button> ();
        if ( button == null )
        {
            Debug.LogError ( "Button component not found." );
            return;
        }

        buttonImage = button.image;
        buttonText = button.GetComponentInChildren<TextMeshProUGUI> ();
        button.onClick.AddListener ( OnButtonClick );
        background = GetComponent<Image> ();
        moneyManager = FindObjectOfType<MoneyManager> ();
        if ( moneyManager == null )
        {
            Debug.LogError ( "MoneyManager not found." );
        }

        upgrade = new Upgrade ( upgradeType.ToString () , upgradeType );

        LoadState ();
        UpdateButtonState ();
        ApplyUpgradeEffect ();
    }

    protected override void OnButtonClick( )
    {
        if ( upgrade.CanUpgrade () && moneyManager.Spend ( upgrade.GetCurrentCost () ) )
        {
            upgrade.UpgradeLevel ();
            IsPurchasedOrUpgraded = true;
            UpdateButtonState ();
            SaveState ();
            ApplyUpgradeEffect ();
            Debug.Log ( $"{upgradeType} upgraded to level {upgrade.currentLevel}" );
        }
        else
        {
            Debug.Log ( "Not enough money to upgrade or already at max level." );
        }
    }

    private void ApplyUpgradeEffect( )
    {
        switch ( upgradeType )
        {
            case UpgradeType.SlotHitChance:
                ApplySlotHitChanceUpgrade ();
                break;
            case UpgradeType.IncreaseEnergy:
                ApplyIncreaseEnergyUpgrade ();
                break;
            case UpgradeType.DoubleBall:
                ApplyDoubleBallUpgrade ();
                break;
            case UpgradeType.FewerBarriers:
                ApplyFewerBarriersUpgrade ();
                break;
            case UpgradeType.DoubleStrength:
                ApplyDoubleStrengthUpgrade ();
                break;
            case UpgradeType.MoreReward:
                ApplyMoreRewardUpgrade ();
                break;
            default:
                Debug.LogError ( "Unhandled upgrade type." );
                break;
        }
    }

    private void ApplySlotHitChanceUpgrade( )
    {
        int increaseAmount = Mathf.RoundToInt ( upgrade.GetCurrentEffect () );
        Cell [ ] allCells = FindObjectsOfType<Cell> ();
        foreach ( var cell in allCells )
        {
            cell.IncreaseBaseScore ( increaseAmount );
        }
    }

    private void ApplyIncreaseEnergyUpgrade( )
    {
        Pointer pointer = FindObjectOfType<Pointer> ();
        if ( pointer != null )
        {
            pointer.UpdateEnergyCost ( upgrade.currentLevel );
        }
    }

    private void ApplyDoubleBallUpgrade( )
    {
        Pointer pointer = FindObjectOfType<Pointer> ();
        if ( pointer != null )
        {
            float doubleBallChance = upgrade.GetCurrentEffect () / 100f;
            pointer.UpdateDoubleBallChance ( doubleBallChance );
        }
    }

    private void ApplyFewerBarriersUpgrade( )
    {
        ShapeManager shapeManager = FindObjectOfType<ShapeManager> ();
        if ( shapeManager != null )
        {
            shapeManager.ApplyFewerBarriers ( upgrade.currentLevel * 10 ); // Example: Reduce barriers by 10% per level
        }
    }

    private void ApplyDoubleStrengthUpgrade( )
    {
        ShapeManager shapeManager = FindObjectOfType<ShapeManager> ();
        if ( shapeManager != null )
        {
            shapeManager.ApplyDoubleStrength ();
        }
    }

    private void ApplyMoreRewardUpgrade( )
    {
        ShapeManager shapeManager = FindObjectOfType<ShapeManager> ();
        if ( shapeManager != null )
        {
            shapeManager.ApplyMoreReward ( upgrade.currentLevel * 5 ); // Example: Increase reward by 5% per level
        }
    }

    protected override void UpdateButtonState( )
    {
        if ( buttonImage == null )
        {
            Debug.LogError ( "ButtonImage not initialized." );
            return;
        }

        if ( upgrade.currentLevel == 0 )
        {
            buttonImage.sprite = notPurchasedSprite;
            buttonText.text = $"Price: {upgrade.GetCurrentCost ()}";
        }
        else if ( upgrade.CanUpgrade () )
        {
            buttonImage.sprite = purchasedSprite;
            buttonText.text = $"Upgrade: {upgrade.GetCurrentCost ()}";
        }
        else
        {
            buttonImage.sprite = upgradedSprite;
            background.sprite = spriteBackground;
            buttonText.enabled = false;
        }

        button.interactable = upgrade.CanUpgrade () && moneyManager.GetCurrentAmount () >= upgrade.GetCurrentCost ();

        currentLevelText.text = $"{upgrade.currentLevel}/{upgrade.maxLevel}";

        if ( upgrade.currentLevel == upgrade.maxLevel )
        {
            IsPurchasedOrUpgraded = true;
        }
    }

    public override void CheckAvailability( )
    {
        UpdateButtonState ();
    }

    protected override void SaveState( )
    {
        PlayerPrefs.SetInt ( stateKey , upgrade.currentLevel );
        PlayerPrefs.SetInt ( $"{stateKey}_purchasedOrUpgraded" , IsPurchasedOrUpgraded ? 1 : 0 );
        PlayerPrefs.Save ();
    }

    protected override void LoadState( )
    {
        if ( PlayerPrefs.HasKey ( stateKey ) )
        {
            upgrade.currentLevel = PlayerPrefs.GetInt ( stateKey );
            IsPurchasedOrUpgraded = PlayerPrefs.GetInt ( $"{stateKey}_purchasedOrUpgraded" ) == 1;
        }
        else
        {
            upgrade.currentLevel = 0;
            IsPurchasedOrUpgraded = false;
        }
    }
}
