using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChangeBackGround : MonoBehaviour
{
    public static ChangeBackGround Instance
    {
        get; private set;
    }

    public List<Sprite> BackGrounds; // ������ �������� ����
    public Image targetImage; // Image ���������, ������� ����� ���������� ���

    private void Start( )
    {
        if ( Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy ( gameObject );
        }
        LevelManager.Instance.Load ();
    }

    // ����� ��� ����� ����
    public void ChangeBackground( int index )
    {
        if ( index >= 0 && index < BackGrounds.Count )
        {
            // ������������� ��������� ������
            targetImage.sprite = BackGrounds [ index ];
        }
        else
        {
            Debug.LogError ( "Background index is out of range" );
        }
    }
}
