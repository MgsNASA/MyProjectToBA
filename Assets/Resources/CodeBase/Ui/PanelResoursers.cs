using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelResources : MonoBehaviour
{
    public Button buttonPlus;
    public TextMeshProUGUI energyText; // Текст для отображения энергии и таймера
    public TextMeshProUGUI moneyText; // Текст для отображения денег
    [SerializeField]
    private List<Image> energyImages = new List<Image> (); // Список для хранения созданных изображений энергии
    private EnergyManager energyManager; // Ссылка на менеджер энергии
    private MoneyManager moneyManager; // Ссылка на менеджер счета

    private void Start( )
    {
        // Найти и сохранить ссылки на EnergyManager и ScoreManager
        energyManager = FindObjectOfType<EnergyManager> ();
        moneyManager = FindObjectOfType<MoneyManager> ();

        // Обновить интерфейс при запуске
        UpdateUI ();
    }

    private void Update( )
    {
        // Обновлять интерфейс каждую секунду или реже для улучшения производительности
        UpdateUI ();
    }

    // Метод для обновления текстовых полей и изображений энергии
    public void UpdateUI( )
    {
        int currentEnergy = energyManager.GetCurrentAmount ();
        moneyText.text = $"{moneyManager.GetCurrentAmount ()}";

        if ( currentEnergy > 0 )
        {
            buttonPlus.gameObject.SetActive ( false );
            energyText.enabled = false;
            energyText.gameObject.SetActive ( true ); // Показываем текст энергии
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

        // Обновляем состояния изображений энергии
        UpdateEnergyImages ( currentEnergy );
    }

    // Метод для обновления состояний изображений энергии
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

    // Пример метода для использования энергии через интерфейс
    public void OnUseEnergyButtonPressed( )
    {
        energyManager.Spend ( 1 ); // Пример использования энергии
        UpdateUI ();
    }

    // Пример метода для покупки энергии через интерфейс
    public void OnBuyEnergyButtonPressed( )
    {
        // Пример покупки энергии, если есть такая функциональность в EnergyManager
        // energyManager.BuyEnergy(1); 
        UpdateUI ();
    }

    // Пример метода для добавления денег через интерфейс
    public void OnAddMoneyButtonPressed( int amount )
    {
        moneyManager.Add ( amount ); // Пример добавления денег
        UpdateUI ();
    }

    // Пример метода для трат денег через интерфейс
    public void OnSpendMoneyButtonPressed( int amount )
    {
        moneyManager.Spend ( amount ); // Пример траты денег
        UpdateUI ();
    }
}
