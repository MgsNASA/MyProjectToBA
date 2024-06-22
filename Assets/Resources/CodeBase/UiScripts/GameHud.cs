using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHud : MonoBehaviour
{
    public Button HomePanel;
    public Button LeftButton;
    public Button RightButton;
    public TextMeshProUGUI textMeshProUGUI;
    [SerializeField]
    private CreateCircle createCircle;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private BoostManager boostManager;
    [SerializeField]
    private MoneyManager moneyManager;

    private void Start( )
    {
        gameManager = GameManager.Instance;

        // Убедитесь, что компоненты BoostManager и MoneyManager существуют в GameManager
        if ( boostManager == null || moneyManager == null )
        {
            Debug.LogError ( "BoostManager or MoneyManager component not found in GameManager!" );
            return;
        }

        // Assign button click listeners
        LeftButton.onClick.AddListener ( OnLeftButtonClicked );
        RightButton.onClick.AddListener ( OnRightButtonClicked );
    }

    private void OnLeftButtonClicked( )
    {
        createCircle.ShowPreviousCircle ();
    }

    private void OnRightButtonClicked( )
    {
        createCircle.ShowNextCircle ();
    }

    public void UpdateCoinText( int coins )
    {
        textMeshProUGUI.text = $"{coins}";
    }
}
