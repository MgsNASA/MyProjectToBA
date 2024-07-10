using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardMap : MonoBehaviour
{
    public enum CardState
    {
        NotPurchased,
        Purchased,
        Selected
    }

    [SerializeField] private CardState state = CardState.NotPurchased;
    [SerializeField] private int price = 100;
    [SerializeField] private Sprite spriteMap; // Sprite for the map

    public Sprite visitSprite;
    public Sprite purchasedSprite;
    public Sprite notPurchasedSprite;

    private Image buttonImage;
    private Button button;
    private TextMeshProUGUI buttonText;

    private Achievement buy1MapAchievement; // Reference to Buy1Map achievement

    public CardState State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            UpdateButtonState ();
            SaveState ();
        }
    }

    private void Awake( )
    {
        button = GetComponentInChildren<Button> ();
        if ( button == null )
        {
            Debug.LogError ( "Button component not found on CardMap." );
            return;
        }

        buttonImage = button.image;
        buttonText = button.GetComponentInChildren<TextMeshProUGUI> ();

        button.onClick.AddListener ( OnButtonClick );
        LoadState (); // Load the state when the card is initialized
        UpdateButtonState ();

        // Initialize Buy1Map achievement
        buy1MapAchievement = new Achievement ( Achievement.AchievementType.Buy1Map , 5000 , 1 );
    }

    private void OnButtonClick( )
    {
        if ( state == CardState.NotPurchased )
        {
            if ( MapManager.Instance.MoneyManager.Spend ( price ) )
            {
                State = CardState.Purchased;
                Debug.Log ( "Map purchased" );

                // Check and achieve Buy1Map achievement
                if ( !buy1MapAchievement.isAchieved && buy1MapAchievement.CheckCondition () )
                {
                    buy1MapAchievement.Achieve ();
                    Debug.Log ( "Achievement Buy1Map achieved!" );
                }
            }
            else
            {
                Debug.Log ( "Not enough money to purchase the map." );
            }
        }
        else if ( state == CardState.Purchased )
        {
            State = CardState.Selected;
            Debug.Log ( "Map selected" );
            MapManager.Instance.UpdateCurrentMap ( spriteMap );
            MapManager.Instance.UpdateCurrentMap ( spriteMap ); // Update the current map display
        }
        else if ( state == CardState.Selected )
        {
            State = CardState.Purchased;
            Debug.Log ( "Map deselected" );
            MapManager.Instance.UpdateCurrentMap ( null );
        }
        else
        {
            Debug.LogError ( "Unhandled state." );
        }
    }

    public void UpdateButtonState( )
    {
        if ( buttonImage == null )
        {
            Debug.LogError ( "ButtonImage not initialized." );
            return;
        }

        switch ( state )
        {
            case CardState.NotPurchased:
                buttonImage.sprite = notPurchasedSprite;
                buttonText.text = $"Price: {price}";
                button.interactable = true;
                break;
            case CardState.Purchased:
                buttonImage.sprite = purchasedSprite;
                buttonText.text = "Purchased";
                button.interactable = true;
                break;
            case CardState.Selected:
                buttonImage.sprite = visitSprite;
                buttonText.text = "Selected";
                button.interactable = true;
                break;
            default:
                Debug.LogError ( "Unhandled state." );
                break;
        }
    }

    private void SaveState( )
    {
        PlayerPrefs.SetInt ( gameObject.name + "_state" , ( int ) state );
        PlayerPrefs.Save ();
    }

    private void LoadState( )
    {
        if ( PlayerPrefs.HasKey ( gameObject.name + "_state" ) )
        {
            state = ( CardState ) PlayerPrefs.GetInt ( gameObject.name + "_state" );
        }
    }
}
