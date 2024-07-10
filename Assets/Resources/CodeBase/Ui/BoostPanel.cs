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
    private MoneyManager moneyManager; // ��������� ���� ��� MoneyManager
    [SerializeField]
    private Button _button;
    [SerializeField]
    private TextMeshProUGUI _textPrice;

    private void Awake( )
    {

        _textPrice = _button.GetComponentInChildren<TextMeshProUGUI> ();
        _textPrice.text = _price.ToString (); // ���������� ���� �� ������
        _button.onClick.AddListener ( PurshargeButton );
    }

    private void PurshargeButton( )
    {
        if ( moneyManager.Spend ( ( int ) _price ) ) // ���������, ������� �� �����, � ����� ��
        {
            energyManager.AddEnergy ( 1 ); // �������� �������, �����������, ��� ����������� 1 ������� �������
            Debug.Log ( "Energy purchased successfully!" );
        }
        else
        {
            Debug.Log ( "Not enough money to purchase energy." );
        }
    }
}
