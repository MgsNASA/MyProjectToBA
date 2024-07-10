using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardCircle : CardBase
{
    public enum State
    {
        Choose,
        Available,
        Unavailable,
        Purchased
    }

    [SerializeField] private State currentState = State.Unavailable;
    [SerializeField] private int price = 100;
    [SerializeField] private Sprite circleSprite;

    public Sprite chooseSprite;
    public Sprite availableSprite;
    public Sprite unavailableSprite;
    public Sprite purchasedSprite;
    public TextMeshProUGUI textMeshProUGUI;
    private Image buttonImage;
    private Button button;
    [SerializeField]
    private TextMeshProUGUI priceText;
    private MoneyManager moneyManager;
    private Pointer pointer;

    private string stateKey;

    public State CurrentState
    {
        get => currentState;
        set
        {
            currentState = value;
            UpdateButtonState ();
            SaveState ();
        }
    }

    void Awake( )
    {
        stateKey = $"CardState_{GetInstanceID ()}";

        button = GetComponentInChildren<Button> ();
        if ( button == null )
        {
            Debug.LogError ( "Button component not found." );
        }
        else
        {
            buttonImage = button.image;
        }

        pointer = FindObjectOfType<Pointer> ();
        if ( pointer == null )
        {
            Debug.LogError ( "Pointer not found." );
        }

        button.onClick.AddListener ( OnButtonClick );
        base.Start ();
        UpdateButtonState ();
    }

    private void OnEnable( )
    {
        base.OnEnable ();
        moneyManager = FindObjectOfType<MoneyManager> ();
        if ( moneyManager == null )
        {
            Debug.LogError ( "MoneyManager not found." );
        }

        LoadState ();
        CheckAvailability ();
    }

    protected override void OnButtonClick( )
    {
        switch ( currentState )
        {
            case State.Choose:
                pointer.currentSprite = circleSprite;
                Debug.Log ( "Circle chosen" );
                CircleManager.Instance.UpdateCircleSelection ( this );
 
                break;

            case State.Available:
                if ( moneyManager.Spend ( price ) )
                {
                    Debug.Log ( "Circle purchased" );
                    CurrentState = State.Purchased;
                    pointer.UpdateCurrentSprite ( chooseSprite );
                    CircleManager.Instance.UpdateCircleSelection ( this );
                }
                else
                {
                    Debug.Log ( "Not enough money" );
                }
                break;

            case State.Purchased:
                CurrentState = State.Choose;
                pointer.UpdateCurrentSprite ( chooseSprite );
                CircleManager.Instance.UpdateCircleSelection ( this );
                Debug.Log ( "Circle chosen" );
                break;

            case State.Unavailable:
                Debug.Log ( "Not enough money to purchase" );
                break;
        }
    }

    protected override void UpdateButtonState( )
    {
        if ( buttonImage == null )
        {
            Debug.LogError ( "ButtonImage not initialized." );
            return;
        }

        switch ( currentState )
        {
            case State.Choose:
                buttonImage.sprite = chooseSprite;
                if ( priceText != null )
                {
                    priceText.text = "Choose";
                }
                if ( button != null )
                {
                    button.interactable = true;
                }
                break;

            case State.Available:
                buttonImage.sprite = availableSprite;
                if ( priceText != null )
                {
                    priceText.text = price.ToString ();
                }
                if ( button != null )
                {
                    button.interactable = true;
                }
                break;

            case State.Purchased:
                buttonImage.sprite = purchasedSprite;
                if ( priceText != null )
                {
                    priceText.text = "Purchased";
                }
                if ( button != null )
                {
                    button.interactable = true;
                }
                break;

            case State.Unavailable:
                buttonImage.sprite = unavailableSprite;
                if ( priceText != null )
                {
                    priceText.text = price.ToString ();
                }
                if ( button != null )
                {
                    button.interactable = false;
                }
                break;
        }
    }

    public override void CheckAvailability( )
    {
        if ( currentState == State.Choose || currentState == State.Purchased )
        {
            UpdateButtonState ();
            return;
        }

        if ( moneyManager != null && moneyManager.GetCurrentAmount () >= price )
        {
            CurrentState = State.Available;
        }
        else
        {
            CurrentState = State.Unavailable;
        }

        UpdateButtonState ();
    }

    protected override void SaveState( )
    {
        PlayerPrefs.SetInt ( stateKey , ( int ) currentState );
        PlayerPrefs.Save ();
    }

    protected override void LoadState( )
    {
        if ( PlayerPrefs.HasKey ( stateKey ) )
        {
            currentState = ( State ) PlayerPrefs.GetInt ( stateKey );
        }
        else
        {
            currentState = State.Choose;
        }
    }
}
