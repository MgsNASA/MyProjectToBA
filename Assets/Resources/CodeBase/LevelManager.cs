using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance
    {
        get; private set;
    } // ��������-���������

    public LoadingScreen loadingScreen;
    public CreateCircle createCircle;
    [SerializeField]
    private int backgroundIndex = 0;
    private bool isLevelTransitioning = false; // ����, ����������� �� ������� �������� ����� ��������
    public bool IsLevelTransitioning => isLevelTransitioning;

    private void Awake( )
    {
        if ( Instance == null )
        {
            Instance = this;
            DontDestroyOnLoad ( gameObject ); // ��������� ������ ����� �������
        }
        else
        {
            Destroy ( gameObject ); // ���������� ���������
        }
    }

    public void Load( )
    {
        // ��������� ����������� ������ ����
        backgroundIndex = PlayerPrefs.GetInt ( "BackgroundIndex" , 0 );

        // �������� �� null ��� ChangeBackGround.Instance
        if ( ChangeBackGround.Instance != null )
        {
            // ������������� ��� �� ������������ �������
            ChangeBackGround.Instance.ChangeBackground ( backgroundIndex );
        }
        else
        {
            Debug.LogError ( "ChangeBackGround.Instance is null" );
        }

        // �������� �� null ��� createCircle
        if ( createCircle != null )
        {
            // ��������� ��������� ������
            createCircle.InitializeAndLoadGame ();

            // ������������� �������� ����, ���� ���� ��������� �����
            if ( createCircle.CreatedCircles.Count > 0 )
            {
                createCircle.SetActiveCircle ( 0 );
            }
        }
        else
        {
            Debug.LogError ( "createCircle is null" );
        }
    }

    public void CheckAndAdvanceLevel( )
    {
        if ( createCircle.AreAllIndicatorsFilled () )
        {
            loadingScreen.enabled = true;
            // �������� ����� ��������
            loadingScreen.Show ();
        }
    }

    public void StartLevelChanger( )
    {
        if ( !isLevelTransitioning )
        {
            StartCoroutine ( AdvanceToNextLevel () );
        }
    }

    public IEnumerator AdvanceToNextLevel( )
    {
        // ������������� ����, ��� ������� ��������� � �������� ��������
        isLevelTransitioning = true;

        backgroundIndex = ( backgroundIndex + 1 ) % ChangeBackGround.Instance.BackGrounds.Count; // ��������� ������ ���� � ������ ���������� �����
        ChangeBackGround.Instance.ChangeBackground ( backgroundIndex );

        // ���������� ��� ������ ����������
        createCircle.DestroyAllCircles ();

        // �������� ������� ��������� ������
        createCircle.ResetAvailableIndices ();

        // ������� ����� ����������
        for ( int i = 0; i < 3; i++ )
        {
            createCircle.CreateRandomCircle (); // ������� ����� ����� � ��������� ���������� 0%
        }

        // ������������� ������ ����� ��������
        if ( createCircle.CreatedCircles.Count > 0 )
        {
            createCircle.SetActiveCircle ( 0 );
        }

        // ���� ��������� ����� ��� �������� ��������
        yield return new WaitForSeconds ( 5.0f );

        // ��������� ������ ���� ����� ����������� ������
        PlayerPrefs.SetInt ( "BackgroundIndex" , backgroundIndex );
        PlayerPrefs.Save ();

        // ����� �����, ����� ������� ��������
        isLevelTransitioning = false;
    }


}
