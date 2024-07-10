using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelResources : MonoBehaviour
{
    public Button buttonPlus;
    public TextMeshProUGUI energyText; // ����� ��� ����������� ������� � �������
    public TextMeshProUGUI moneyText; // ����� ��� ����������� �����
    [SerializeField]
    private List<Image> energyImages = new List<Image> (); // ������ ��� �������� ��������� ����������� �������
    private EnergyManager energyManager; // ������ �� �������� �������
    private MoneyManager moneyManager; // ������ �� �������� �����

    private void Start( )
    {
        // ����� � ��������� ������ �� EnergyManager � ScoreManager
        energyManager = FindObjectOfType<EnergyManager> ();
        moneyManager = FindObjectOfType<MoneyManager> ();

        // �������� ��������� ��� �������
        UpdateUI ();
    }

    private void Update( )
    {
        // ��������� ��������� ������ ������� ��� ���� ��� ��������� ������������������
        UpdateUI ();
    }

    // ����� ��� ���������� ��������� ����� � ����������� �������
    public void UpdateUI( )
    {
        int currentEnergy = energyManager.GetCurrentAmount ();
        moneyText.text = $"{moneyManager.GetCurrentAmount ()}";

        if ( currentEnergy > 0 )
        {
            buttonPlus.gameObject.SetActive ( false );
            energyText.enabled = false;
            energyText.gameObject.SetActive ( true ); // ���������� ����� �������
        }
        else
        {
            energyText.enabled = true;
            buttonPlus.gameObject.SetActive ( true );

            TimeSpan timeLeft = energyManager.NextEnergyIncreaseTime - DateTime.UtcNow;
            if ( timeLeft.TotalSeconds > 0 )
            {
                string formattedTime = string.Format ( "{0:D2}:{1:D2}" , timeLeft.Minutes , timeLeft.Seconds );
                energyText.text = $"{formattedTime}";
            }
            else
            {
                energyText.text = "00:00";
            }
        }

        // ��������� ��������� ����������� �������
        UpdateEnergyImages ( currentEnergy );
    }

    // ����� ��� ���������� ��������� ����������� �������
    public void UpdateEnergyImages( int currentEnergy )
    {
        for ( int i = 0; i < energyImages.Count; i++ )
        {
            if ( i < currentEnergy )
            {
                energyImages [ i ].gameObject.SetActive ( true );
            }
            else
            {
                energyImages [ i ].gameObject.SetActive ( false );
            }
        }
    }

    // ������ ������ ��� ������������� ������� ����� ���������
    public void OnUseEnergyButtonPressed( )
    {
        energyManager.Spend ( 1 ); // ������ ������������� �������
        UpdateUI ();
    }

    // ������ ������ ��� ������� ������� ����� ���������
    public void OnBuyEnergyButtonPressed( )
    {
        // ������ ������� �������, ���� ���� ����� ���������������� � EnergyManager
        // energyManager.BuyEnergy(1); 
        UpdateUI ();
    }

    // ������ ������ ��� ���������� ����� ����� ���������
    public void OnAddMoneyButtonPressed( int amount )
    {
        moneyManager.Add ( amount ); // ������ ���������� �����
        UpdateUI ();
    }

    // ������ ������ ��� ���� ����� ����� ���������
    public void OnSpendMoneyButtonPressed( int amount )
    {
        moneyManager.Spend ( amount ); // ������ ����� �����
        UpdateUI ();
    }
}
