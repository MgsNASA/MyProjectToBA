using System;

[Serializable]
public struct PlayerResources
{
    public EnergyData energyData;
    public int money;
    public int score;

    public PlayerResources( int maxEnergy , float rechargeRate , int initialMoney , int initialScore )
    {
        energyData = new EnergyData ( maxEnergy , maxEnergy , rechargeRate );
        money = initialMoney;
        score = initialScore;
    }
}
