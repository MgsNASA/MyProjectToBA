using System;
using UnityEngine;

public class MoneyManager : ResourceManagerBase
{
    public static MoneyManager Instance
    {
        get; private set;
    }

    [SerializeField]
    private int currentMoney;
    [SerializeField]
    private PanelResources panelResources;

    private void Awake( )
    {
        if ( Instance == null )
        {
            Instance = this;
            DontDestroyOnLoad ( gameObject );
        }
        else
        {
            Destroy ( gameObject );
        }

        LoadData ();
    }

    public override void Add( int amount )
    {
        currentMoney += amount;
        SaveData ();
        Debug.Log ( $"Money added: {amount}. Current money: {currentMoney}" );
        panelResources?.UpdateUI (); // Обновляем UI после добавления денег
    }

    public override bool Spend( int amount )
    {
        if ( currentMoney >= amount )
        {
            currentMoney -= amount;
            SaveData ();
            Debug.Log ( $"Money spent: {amount}. Current money: {currentMoney}" );
            panelResources?.UpdateUI (); // Обновляем UI после траты денег
            return true;
        }
        else
        {
            Debug.Log ( "Not enough money." );
            return false;
        }
    }

    public override int GetCurrentAmount( )
    {
        return currentMoney;
    }

    public override void SaveData( )
    {
        PlayerPrefs.SetInt ( "Money" , currentMoney );
        PlayerPrefs.Save ();
    }

    public override void LoadData( )
    {
        currentMoney = PlayerPrefs.GetInt ( "Money" , 0 ); // Загружаем сохраненные деньги или устанавливаем ноль
    }

    public override void GetCurrent( )
    {
        throw new NotImplementedException ();
    }
}
