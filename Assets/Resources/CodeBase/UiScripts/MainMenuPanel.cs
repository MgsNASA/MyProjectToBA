using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    public Button Settings;
    public Button Play;

    void Start( )
    {
        // ��������� ����������� ������� ��� ������
        Settings.onClick.AddListener ( OnSettingsButtonClicked );
        Play.onClick.AddListener ( OnPlayButtonClicked );
    }

    void OnSettingsButtonClicked( )
    {
        // ���������� ������� ��� ������ Settings
        Debug.Log ( "Settings button clicked" );
        // ����� ����� ������� ����� ��� �������� ������ ��������
        UIManager.Instance.SetUIStateSettingsPanel ();
    }

    void OnPlayButtonClicked( )
    {
        // ���������� ������� ��� ������ Play
        Debug.Log ( "Play button clicked" );
        // ����� ����� ������� ����� ��� ������ ����
        GameManager.Instance.StartGame ();
        UIManager.Instance.StartGame ();
    }
}
