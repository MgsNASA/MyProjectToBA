using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _firstPanelText;
    [SerializeField] private Button _firstPanelButton;
    [SerializeField] private TextMeshProUGUI _secondPanelText;
    [SerializeField] private Button _secondPanelButton;

    private void Start( )
    {
        // Подписываемся на события нажатия кнопок
        _firstPanelButton.onClick.AddListener ( OnLeaderBoardButtonClicked );
        _secondPanelButton.onClick.AddListener ( OnAchievementButtonClicked );
        OnLeaderBoardButtonClicked ();
    }

    private void OnLeaderBoardButtonClicked( )
    {
        // Выключаем текст другой панели (achievementText)
        _secondPanelText.gameObject.SetActive ( false );

        // Выполняем действия, связанные с кнопкой LeaderBoard (например, показываем текст LeaderBoard)
        _firstPanelText.gameObject.SetActive ( true );
        AudioManager.Instance.Play ( "ClickButton" ); // Воспроизводим звук при нажатии кнопки
    }

    private void OnAchievementButtonClicked( )
    {
        // Выключаем текст другой панели (leaderBoardText)
        _firstPanelText.gameObject.SetActive ( false );

        // Выполняем действия, связанные с кнопкой Achievement (например, показываем текст Achievement)
        _secondPanelText.gameObject.SetActive ( true );
        AudioManager.Instance.Play ( "ClickButton" ); // Воспроизводим звук при нажатии кнопки
        Debug.Log ( "OKkok" );
    }
}
