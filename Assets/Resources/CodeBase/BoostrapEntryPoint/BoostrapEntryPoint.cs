using System.Collections;
using UnityEngine;
public class BoostrapEntryPoint : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    private IEnumerator Start( )
    {
        // ���������� ����������� �����
        yield return new WaitForSecondsRealtime ( 0 );
        // ��� ��� �������� ��� �������������
        Debug.Log ( "��������..." );
        // _gameManager.StartGame ();
        _gameManager.Inilization ();
        // ���������� ��������
        Debug.Log ( "������������� � �������� �������� ��������� !" );

    }
}

