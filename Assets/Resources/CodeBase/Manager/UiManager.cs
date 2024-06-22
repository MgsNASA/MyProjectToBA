using UnityEngine;

public class UIManager : MonoBehaviour, IInilization
{
    public enum UIState
    {
        MainMenu,
        GameHud,
        SettingsPanel
        // Добавьте другие состояния по мере необходимости
    }

    public GameObject mainMenuUI;
    public GameObject gameHud;
    public GameObject settingsPanel;
    // Добавьте другие объекты UI по мере необходимости

    private UIState currentState = UIState.MainMenu;
    private bool isPaused = false;

    private static UIManager _instance;
    public static UIManager Instance => _instance;

    private void Awake( )
    {
        if ( _instance != null && _instance != this )
        {
            Destroy ( this.gameObject );
        }
        else
        {
            _instance = this;
        }
    }

    public void Inilization( )
    {
        SwitchUIState ( UIState.MainMenu );
    }

    private void SwitchUIState( UIState newState )
    {
        Time.timeScale = 0f; 

        // Отключаем все UI окна
        mainMenuUI.SetActive ( false );
        gameHud.SetActive ( false );
        settingsPanel.SetActive ( false );

        // Включаем UI окно в соответствии с новым состоянием
        switch ( newState )
        {
            case UIState.MainMenu:
                mainMenuUI.SetActive ( true );
                break;
            case UIState.GameHud:
                gameHud.SetActive ( true );
                Time.timeScale = 1f; // Возобновляем время
                // При возвращении к GameHud снимаем состояние паузы
                isPaused = false;
                break;
            case UIState.SettingsPanel:
                settingsPanel.SetActive ( true );
                break;
                // Добавляйте другие случаи по мере добавления состояний
        }

        // Обновляем текущее состояние
        currentState = newState;
    }

    public void SetUIStateMainMenu( )
    {
        SwitchUIState ( UIState.MainMenu );
    }

    public void SetUIStateGameHud( )
    {
        SwitchUIState ( UIState.GameHud );
    }

    public void SetUIStateSettingsPanel( )
    {
        SwitchUIState ( UIState.SettingsPanel );
    }

    public void StartGame( )
    {
        SetUIStateGameHud ();
        // Дополнительные действия для старта игры
    }

    public void Restart( )
    {
        SwitchUIState ( UIState.GameHud );
        // Дополнительные действия для перезапуска игры
    }


    public void ShowSettingsPanel( )
    {
        settingsPanel.SetActive ( true );
    }

    public void HideSettingsPanel( )
    {
        settingsPanel.SetActive ( false );
    }
}
