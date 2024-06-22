using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    public Button Settings;
    public Button Play;

    void Start( )
    {
        // Добавляем обработчики событий для кнопок
        Settings.onClick.AddListener ( OnSettingsButtonClicked );
        Play.onClick.AddListener ( OnPlayButtonClicked );
    }

    void OnSettingsButtonClicked( )
    {
        // Обработчик события для кнопки Settings
        Debug.Log ( "Settings button clicked" );
        // Здесь можно вызвать метод для открытия панели настроек
        UIManager.Instance.SetUIStateSettingsPanel ();
    }

    void OnPlayButtonClicked( )
    {
        // Обработчик события для кнопки Play
        Debug.Log ( "Play button clicked" );
        // Здесь можно вызвать метод для начала игры
        GameManager.Instance.StartGame ();
        UIManager.Instance.StartGame ();
    }
}
