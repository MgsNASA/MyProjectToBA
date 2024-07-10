public struct EnergyData
{
    public int maxEnergy;       // Максимальное количество энергии
    public int currentEnergy;   // Текущее количество энергии
    public float rechargeRate;  // Скорость восстановления энергии

    public EnergyData( int maxEnergy , int currentEnergy , float rechargeRate )
    {
        this.maxEnergy = maxEnergy;
        this.currentEnergy = currentEnergy;
        this.rechargeRate = rechargeRate;
    }
}
