using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostPanel : MonoBehaviour
{
    [SerializeField]
    private float _price;
    [SerializeField]
    private EnergyManager energyManager;
    [SerializeField]
    private MoneyManager moneyManager; // Добавлено поле для MoneyManager
    [SerializeField]
    private Button _button;
    [SerializeField]
    private TextMeshProUGUI _textPrice;

    private void Awake( )
    {

        _textPrice = _button.GetComponentInChildren<TextMeshProUGUI> ();
        _textPrice.text = _price.ToString (); // Установить цену на кнопку
        _button.onClick.AddListener ( PurshargeButton );
    }

    private void PurshargeButton( )
    {
        if ( moneyManager.Spend ( ( int ) _price ) ) // Проверить, хватает ли денег, и снять их
        {
            energyManager.AddEnergy ( 1 ); // Добавить энергию, предположим, что добавляется 1 единица энергии
            Debug.Log ( "Energy purchased successfully!" );
        }
        else
        {
            Debug.Log ( "Not enough money to purchase energy." );
        }
    }
}
