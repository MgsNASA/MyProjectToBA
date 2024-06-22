using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostManager : MonoBehaviour
{
    private static BoostManager _instance;
    public static BoostManager Instance => _instance;

    public TextMeshProUGUI popupText;
    public TextMeshProUGUI costText;
    public Button confirmButton;
    public Button cancelButton;
    public GameObject popupPanel;

    public Button BoostMoney;
    public Button AutoClick;
    public Button ProgressBonus;

    private Action onConfirmAction;
    private MoneyManager moneyManager;

    private int coinMultiplierBoostDuration = 50; // Продолжительность буста в кликах
    private int coinMultiplierBoostCost;
    private int coinMultiplierClicksRemaining = 0;
    private int coinsPerClickMultiplier = 5; // Множитель монет за клик

    private int autoClickBoostDuration;
    private int autoClickBoostCost;
    private int autoClickClicksRemaining = 0;
    private int autoClickInitialDuration = 50; // Начальная продолжительность автоклика

    private int progressBonusBoostDuration = 100;
    private int progressBonusBoostCost;
    private int progressBonusClicksRemaining = 0;
    public float progressPerClickMultiplier = 0.02f; // Множитель прогресса

    private const int initialBoostCost = 250; // Начальная стоимость бустов
    private const int costIncrement = 250; // Увеличение стоимости после каждой покупки

    public bool IsCoinMultiplierActive( ) => coinMultiplierClicksRemaining > 0;
    public void DecrementCoinMultiplierClicks( ) => coinMultiplierClicksRemaining--;

    public bool IsAutoClickActive( ) => autoClickClicksRemaining > 0;
    public void DecrementAutoClickClicks( ) => autoClickClicksRemaining--;

    public bool IsProgressBonusActive( ) => progressBonusClicksRemaining > 0;
    public void DecrementProgressBonusClicks( ) => progressBonusClicksRemaining--;

    private void Awake( )
    {
        InitializeSingleton ();
        moneyManager = FindObjectOfType<MoneyManager> ();

        // Загрузка сохраненных данных
        coinMultiplierBoostCost = PlayerPrefs.GetInt ( "CoinMultiplierBoostCost" , initialBoostCost );
        autoClickBoostCost = PlayerPrefs.GetInt ( "AutoClickBoostCost" , initialBoostCost );
        autoClickBoostDuration = PlayerPrefs.GetInt ( "AutoClickBoostDuration" , autoClickInitialDuration );
        progressBonusBoostCost = PlayerPrefs.GetInt ( "ProgressBonusBoostCost" , initialBoostCost );
    }

    private void Start( )
    {
        BoostMoney.onClick.AddListener ( OnBoostMoneyClicked );
        AutoClick.onClick.AddListener ( OnAutoClickClicked );
        ProgressBonus.onClick.AddListener ( OnProgressBonusClicked );

        UpdateAllButtonsState ();
    }

    private void InitializeSingleton( )
    {
        if ( _instance != null && _instance != this )
        {
            Destroy ( gameObject );
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad ( gameObject );
        }
    }

    public void ShowPopup( string message , int cost , Action onConfirm )
    {
        popupText.text = message;
        costText.text = $"Cost: {cost}";
        onConfirmAction = onConfirm;

        confirmButton.onClick.RemoveAllListeners ();
        confirmButton.onClick.AddListener ( ConfirmPurchase );

        cancelButton.onClick.RemoveAllListeners ();
        cancelButton.onClick.AddListener ( HidePopup );

        popupPanel.SetActive ( true );
        Time.timeScale = 0; // Ставим игру на паузу
    }

    private void ConfirmPurchase( )
    {
        onConfirmAction?.Invoke ();
        HidePopup ();
    }

    private void HidePopup( )
    {
        popupPanel.SetActive ( false );
        Time.timeScale = 1; // Снимаем игру с паузы
    }

    private void OnBoostMoneyClicked( )
    {
        ShowPopup ( $"Buy Coin Multiplier Boost for {coinMultiplierBoostCost} coins?" , coinMultiplierBoostCost , ( ) =>
        {
            if ( moneyManager.Coins >= coinMultiplierBoostCost && !IsCoinMultiplierActive () )
            {
                moneyManager.SpendCoins ( coinMultiplierBoostCost );
                coinMultiplierClicksRemaining = coinMultiplierBoostDuration;
                UpdateButtonState ( BoostMoney , false ); // Делаем кнопку полупрозрачной

                // Увеличение стоимости
                coinMultiplierBoostCost += costIncrement;
                PlayerPrefs.SetInt ( "CoinMultiplierBoostCost" , coinMultiplierBoostCost );
            }
        } );
    }

    private void OnAutoClickClicked( )
    {
        ShowPopup ( $"Buy Auto Click Boost for {autoClickBoostCost} coins?" , autoClickBoostCost , ( ) =>
        {
            if ( moneyManager.Coins >= autoClickBoostCost && !IsAutoClickActive () )
            {
                moneyManager.SpendCoins ( autoClickBoostCost );
                autoClickClicksRemaining = autoClickBoostDuration;
                StartCoroutine ( StartAutoClickBoost ( autoClickBoostDuration ) );

                // Увеличение стоимости и продолжительности
                autoClickBoostCost += costIncrement;
                autoClickBoostDuration += 10;
                PlayerPrefs.SetInt ( "AutoClickBoostCost" , autoClickBoostCost );
                PlayerPrefs.SetInt ( "AutoClickBoostDuration" , autoClickBoostDuration );
            }
        } );
    }

    private void OnProgressBonusClicked( )
    {
        ShowPopup ( $"Buy Progress Bonus for {progressBonusBoostCost} coins?" , progressBonusBoostCost , ( ) =>
        {

            if ( moneyManager.Coins >= progressBonusBoostCost && !IsProgressBonusActive () )
            {
               
                moneyManager.SpendCoins ( progressBonusBoostCost );
                progressBonusClicksRemaining = progressBonusBoostDuration;

                ApplyProgressBoostMultiplier (); // Применяем множитель прогресса
                progressPerClickMultiplier = 2f;
                UpdateButtonState ( ProgressBonus , false ); // Делаем кнопку полупрозрачной

                // Увеличение стоимости
                progressBonusBoostCost += costIncrement;
                Debug.Log ( "Boost" );
                PlayerPrefs.SetInt ( "ProgressBonusBoostCost" , progressBonusBoostCost );
            }
        } );
    }


    private void ApplyProgressBoostMultiplier( )
    {
        // Добавляем множитель прогресса
        progressPerClickMultiplier += 0.2f;
    }

    private void RemoveProgressBoostMultiplier( )
    {
        // Удаляем множитель прогресса
        progressPerClickMultiplier -= 0.2f;
    }

    private void UpdateAllButtonsState( )
    {
        UpdateButtonState ( BoostMoney , !IsCoinMultiplierActive () );
        UpdateButtonState ( AutoClick , !IsAutoClickActive () );
        UpdateButtonState ( ProgressBonus , !IsProgressBonusActive () );
    }

    private void UpdateButtonState( Button button , bool isEnabled )
    {
        Color color = button.image.color;
        color.a = isEnabled ? 1.0f : 0.5f;
        button.image.color = color;
        button.interactable = isEnabled;
    }

    private IEnumerator StartAutoClickBoost( int duration )
    {
        autoClickClicksRemaining = duration;
        UpdateButtonState ( AutoClick , false ); // Делаем кнопку полупрозрачной

        while ( autoClickClicksRemaining > 0 )
        {
            yield return new WaitForSeconds ( 0.1f ); // интервал между автокликами

            // Находим активный шарик
            CircleFunctionality activeCircleFunctionality = FindActiveCircle ();

            if ( activeCircleFunctionality != null )
            {
                activeCircleFunctionality.OnClick (); // Вызываем метод OnClick у активного шарика
                autoClickClicksRemaining--;
            }
            else
            {
                Debug.LogError ( "Active CircleFunctionality not found in scene." );
                yield break;
            }
        }

        UpdateButtonState ( AutoClick , true ); // Возвращаем кнопку в исходное состояние
    }

    private CircleFunctionality FindActiveCircle( )
    {
        CircleFunctionality [ ] circles = FindObjectsOfType<CircleFunctionality> ();
        foreach ( CircleFunctionality circle in circles )
        {
            if ( circle.gameObject.activeSelf )
            {
                return circle;
            }
        }
        return null;
    }

    public int GetCoinBoostMultiplier( )
    {
        return IsCoinMultiplierActive () ? coinsPerClickMultiplier : 1;
    }

    public float GetProgressBoostMultiplier( )
    {
        return IsProgressBonusActive () ? progressPerClickMultiplier : 1;
    }

    public void PerformClick( )
    {
        int coinsToAdd = GetSum ();
        float progressToAdd = GetSumProgress ();

        moneyManager.AddCoins ( coinsToAdd );
        moneyManager.AddProgress ( progressToAdd );

        if ( IsCoinMultiplierActive () )
        {
            DecrementCoinMultiplierClicks ();
            if ( !IsCoinMultiplierActive () )
            {
                UpdateButtonState ( BoostMoney , true ); // Возвращаем кнопку в исходное состояние
            }
        }

        if ( IsProgressBonusActive () )
        {
            DecrementProgressBonusClicks ();
            if ( !IsProgressBonusActive () )
            {
                UpdateButtonState ( ProgressBonus , true ); // Возвращаем кнопку в исходное состояние
                RemoveProgressBoostMultiplier (); // Устанавливаем множитель прогресса обратно в исходное значение
            }
        }
    }

    public float GetSumProgress( )
    {
        return moneyManager.ProgressPerClick * GetProgressBoostMultiplier ();
    }

    public int GetSum( )
    {
        return moneyManager.CoinsPerClick * GetCoinBoostMultiplier ();
    }
}
