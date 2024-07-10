using UnityEngine;
using static CardUpgrade;

[System.Serializable]
public class Upgrade
{
    public UpgradeType upgradeType; // Тип улучшения
    public string upgradeName;
    public int currentLevel;
    public int maxLevel;
    public int baseCost; // Начальная стоимость
    public float [ ] effects; // Эффект улучшения на каждом уровне

    public Upgrade( string name , UpgradeType type )
    {
        upgradeName = name;
        upgradeType = type;

        // Инициализация эффектов и стоимости в зависимости от типа улучшения
        switch ( upgradeType )
        {
            case UpgradeType.SlotHitChance:
                effects = new float [ ] { 0.1f , 0.2f , 0.3f , 0.4f , 0.5f , 0.6f , 0.7f , 0.8f , 0.9f , 1.0f };
                maxLevel = 10;
                baseCost = 100; // Пример базовой стоимости
                break;
            case UpgradeType.IncreaseEnergy:
                effects = new float [ ] { 1 , 2 , 3 };
                maxLevel = 3;
                baseCost = 200; // Пример базовой стоимости
                break;
            case UpgradeType.DoubleBall:
                effects = new float [ ] { 0.1f , 0.2f , 0.3f , 0.4f , 0.5f };
                maxLevel = 5;
                baseCost = 150; // Пример базовой стоимости
                break;
            case UpgradeType.FewerBarriers:
                effects = new float [ ] { 0.5f , 0.4f , 0.3f };
                maxLevel = 3;
                baseCost = 250; // Пример базовой стоимости
                break;
            case UpgradeType.DoubleStrength:
                effects = new float [ ] { 2 , 4 };
                maxLevel = 2;
                baseCost = 300; // Пример базовой стоимости
                break;
            case UpgradeType.MoreReward:
                effects = new float [ ] { 0.05f , 0.1f , 0.15f , 0.2f , 0.25f };
                maxLevel = 5;
                baseCost = 120; // Пример базовой стоимости
                break;
            default:
                Debug.LogError ( "Unhandled upgrade type." );
                break;
        }
    }

    public bool CanUpgrade( )
    {
        return currentLevel < maxLevel;
    }

    public void UpgradeLevel( )
    {
        if ( CanUpgrade () )
        {
            currentLevel++;
        }
    }

    public int GetCurrentCost( )
    {
        return CanUpgrade () ? Mathf.RoundToInt ( baseCost * Mathf.Pow ( 1.5f , currentLevel ) ) : 0;
    }

    public float GetCurrentEffect( )
    {
        // Проверяем, что currentLevel находится в допустимом диапазоне
        int index = Mathf.Clamp ( currentLevel , 0 , effects.Length - 1 );
        return effects [ index ];
    }
}
