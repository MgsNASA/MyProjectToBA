using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager instance;
    [SerializeField]
    private List<CardUpgrade> upgradeCards = new List<CardUpgrade> ();

    // Булевая переменная для отслеживания выполнения достижения
    private bool isBuyAllUpgradesAchieved = false;

    public static UpgradeManager Instance
    {
        get
        {
            if ( instance == null )
            {
                instance = FindObjectOfType<UpgradeManager> ();
                if ( instance == null )
                {
                    GameObject managerObject = new GameObject ( "UpgradeManager" );
                    instance = managerObject.AddComponent<UpgradeManager> ();
                }
            }
            return instance;
        }
    }

    private void Awake( )
    {
        if ( instance == null )
        {
            instance = this;
            DontDestroyOnLoad ( gameObject );
        }
        else if ( instance != this )
        {
            Destroy ( gameObject );
            return;
        }
    }

    // Метод для проверки выполнения достижения BuyAllUpgrades
    public bool CheckBuyAllUpgradesAchievement( )
    {
        foreach ( var card in upgradeCards )
        {
            // Используем свойство IsPurchasedOrUpgraded
            if ( !card.IsPurchasedOrUpgraded )
            {
                return false; // Если хотя бы один не выполнен, возвращаем false
            }
        }
        return true; // Если все выполнены, возвращаем true
    }

    // Дополнительные методы для управления upgradeCards
    public void AddUpgradeCard( CardUpgrade card )
    {
        if ( !upgradeCards.Contains ( card ) )
        {
            upgradeCards.Add ( card );
        }
    }

    public void RemoveUpgradeCard( CardUpgrade card )
    {
        upgradeCards.Remove ( card );
    }
}
