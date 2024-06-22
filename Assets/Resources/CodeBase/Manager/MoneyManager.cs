using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int Coins { get; private set; } = 0;
    public int CoinsPerClick { get; private set; } = 5; // ��������� ���������� ����� �� ����
    public float Progress { get; private set; } = 0f;
    public float ProgressPerClick { get; private set; } = 0.01f; // ��������� ���������� ��������� �� ����
    public GameHud gameHud;

    private void Awake( )
    {
        LoadCoins ();
        gameHud.UpdateCoinText ( Coins );
    }

    private void Update( )
    {
        // �������� ������� �������
        if ( Input.GetKeyDown ( KeyCode.Space ) )
        {
            AddCoins ( 20000 ); // ��������� �������� �� 2000% (20 � �������)
        }
    }

    public void AddCoins( int amount )
    {
        Coins += amount;
        gameHud.UpdateCoinText ( Coins );
        Debug.Log ( $"Added {amount} coins. Total coins: {Coins}" );
        SaveCoins ();
    }

    public void SpendCoins( int amount )
    {
        if ( Coins >= amount )
        {
            Coins -= amount;
            Debug.Log ( $"Spent {amount} coins. Total coins: {Coins}" );
        }
        else
        {
            Debug.Log ( "Not enough coins." );
        }
        gameHud.UpdateCoinText ( Coins );
        SaveCoins ();
    }

    public void AddProgress( float amount )
    {
        Progress += amount;
        Debug.Log ( $"Added {amount * 100}% progress" );
    }

    public void SaveCoins( )
    {
        PlayerPrefs.SetInt ( "Coins" , Coins );
    }

    public void LoadCoins( )
    {
        Coins = PlayerPrefs.GetInt ( "Coins" , 0 );
        gameHud.UpdateCoinText ( Coins );
    }
}
