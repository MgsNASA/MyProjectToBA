using System;
using UnityEngine;

public class GameManager : MonoBehaviour, IInilization
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public int coinsPerClick = 5;
    public float progressPerClick = 0.04f; // 1% per 25 clicks

    [SerializeField]
    private GameObject [ ] _scriptsObjects;
    [SerializeField]
    private MoneyManager moneyManager;
    [SerializeField]
    private BoostManager boostManager;
    private bool isClicking = false;

    private void Awake( )
    {
        InitializeSingleton ();
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
            DontDestroyOnLoad ( gameObject ); // Keep this instance across different scenes
        }
    }

  


    public bool IsClicking( ) => isClicking;

    public void Inilization( )
    {
        foreach ( var scriptsObject in _scriptsObjects )
        {
            InitializeScripts ( scriptsObject );
        }
    }

    private void InitializeScripts( GameObject scriptsObject )
    {
        var scripts = scriptsObject.GetComponents<MonoBehaviour> ();
        foreach ( var script in scripts )
        {
            if ( script is IInilization initializationScript )
            {
                initializationScript.Inilization ();
            }
        }
    }

    public void Restart( )
    {
        foreach ( var scriptsObject in _scriptsObjects )
        {
            RestartScripts ( scriptsObject );
        }
    }

    private void RestartScripts( GameObject scriptsObject )
    {
        var scripts = scriptsObject.GetComponents<MonoBehaviour> ();
        foreach ( var script in scripts )
        {
            if ( script is IInilization initializationScript )
            {
                initializationScript.Restart ();
            }
        }
    }

    public void StartGame( )
    {
        foreach ( var scriptsObject in _scriptsObjects )
        {
            StartScripts ( scriptsObject );
        }
    }

    private void StartScripts( GameObject scriptsObject )
    {
        var scripts = scriptsObject.GetComponents<MonoBehaviour> ();
        foreach ( var script in scripts )
        {
            if ( script is IInilization initializationScript )
            {
                initializationScript.StartGame ();
            }
        }
    }
}
