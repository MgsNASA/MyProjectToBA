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
        // ������������� �� ������� ������� ������
        _firstPanelButton.onClick.AddListener ( OnLeaderBoardButtonClicked );
        _secondPanelButton.onClick.AddListener ( OnAchievementButtonClicked );
        OnLeaderBoardButtonClicked ();
    }

    private void OnLeaderBoardButtonClicked( )
    {
        // ��������� ����� ������ ������ (achievementText)
        _secondPanelText.gameObject.SetActive ( false );

        // ��������� ��������, ��������� � ������� LeaderBoard (��������, ���������� ����� LeaderBoard)
        _firstPanelText.gameObject.SetActive ( true );
        AudioManager.Instance.Play ( "ClickButton" ); // ������������� ���� ��� ������� ������
    }

    private void OnAchievementButtonClicked( )
    {
        // ��������� ����� ������ ������ (leaderBoardText)
        _firstPanelText.gameObject.SetActive ( false );

        // ��������� ��������, ��������� � ������� Achievement (��������, ���������� ����� Achievement)
        _secondPanelText.gameObject.SetActive ( true );
        AudioManager.Instance.Play ( "ClickButton" ); // ������������� ���� ��� ������� ������
        Debug.Log ( "OKkok" );
    }
}
