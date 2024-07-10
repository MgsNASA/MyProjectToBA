using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public PlayerResources playerResources;

    private const int maxEnergy = 6;
    private const float rechargeRate = 1.0f;
    private const int initialMoney = 100;
    private const int initialScore = 0;

    public PanelResources panelResources;
    [SerializeField]
    private EnergyManager energyManager;
    [SerializeField]
    private MoneyManager moneyManager;
    [SerializeField]
    private ScoreManager scoreManager;

    private void Awake( )
    {
        playerResources = new PlayerResources ( maxEnergy , rechargeRate , initialMoney , initialScore );
       // CreateManagers ();
    }



    private void Update( )
    {
     //   energyManager.RechargeEnergy ( Time.deltaTime );
        panelResources.UpdateUI (); // Обновляем UI каждый кадр
    }




    public void BuyEnergy( int amount )
    {
        energyManager.Add ( amount );
    }

    public void AddMoney( int amount )
    {
        moneyManager.Add ( amount );
    }

    public bool SpendMoney( int amount )
    {
        return moneyManager.Spend ( amount );
    }
    public void ConsumeEnergy(int amount)
    {

    }
  

    public void AddScore( int amount )
    {
        scoreManager.Add ( amount );
    }

    public bool SpendScore( int amount )
    {
        return scoreManager.Spend ( amount );
    }

 

    public ScoreManager GetScoreManager( )
    {
        return scoreManager;
    }

  

    private void OnApplicationQuit( )
    {
        energyManager.SaveData ();
        moneyManager.SaveData ();
        scoreManager.SaveData ();
    }
}
