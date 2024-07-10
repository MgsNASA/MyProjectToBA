using System.Collections;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public enum CellColor
    {
        Red,
        Yellow,
        Green
    }

    public enum PinColor
    {
        None,
        Green,
        Red,
        Purple
    }

    public CellColor cellColor;
    public PinColor pinColor;

    private int baseScore;
    private ScoreManager scoreManager;

    private float moveDistance = 0.1f; // ����������, �� ������� ������ ����� ��������� ����
    private float moveDuration = 0.3f; // ����������������� �������� ����

    private bool isMoving = false; // ���� ��� ������������ ��������

    private string baseScoreKey;

    void Start( )
    {
        // Define a unique key for this cell's base score based on its ID and color
        baseScoreKey = $"CellBaseScore_{GetInstanceID ()}_{cellColor}";

        // ������������� ������� ���� � ����������� �� ����� ������
        switch ( cellColor )
        {
            case CellColor.Red:
                baseScore = 1000;
                break;
            case CellColor.Yellow:
                baseScore = 500;
                break;
            case CellColor.Green:
                baseScore = 100;
                break;
        }

        // Load the base score if it exists
        if ( PlayerPrefs.HasKey ( baseScoreKey ) )
        {
            baseScore = PlayerPrefs.GetInt ( baseScoreKey );
        }

        // �������� ������ �� ScoreManager
       scoreManager = FindObjectOfType<ScoreManager> ();
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        // ���������, ��� ������, � ������� ��������� ������������, �������� �������
        if ( collision.gameObject.CompareTag ( "Circle" ) )
        {
            int score = baseScore;

            // ��������� ��������� � ����������� �� ����� �����
            switch ( pinColor )
            {
                case PinColor.Green:
                    score *= 2;
                    break;
                case PinColor.Red:
                    score *= 5;
                    break;
                case PinColor.Purple:
                    score *= 3;
                    break;
            }

            // ��������� ���� ����� ScoreManager
            if ( scoreManager != null )
            {
                scoreManager.Add ( score );
            }

            // ������� ���� � ������� (����� �������� �� ������ ������)
            Debug.Log ( "Score: " + score );

            // ������� ������ ����, ���� ��� �� ��������� ���
            if ( !isMoving )
            {
                StartCoroutine ( MoveDown () );
            }

            // ������� ������ �� ����
            Destroy ( collision.gameObject );
        }
    }

    private IEnumerator MoveDown( )
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos - new Vector3 ( 0 , moveDistance , 0 );
        float elapsedTime = 0f;

        while ( elapsedTime < moveDuration )
        {
            transform.position = Vector3.Lerp ( startPos , endPos , elapsedTime / moveDuration );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        isMoving = false;
    }

    public void IncreaseBaseScore( int amount )
    {
        baseScore += amount;
        PlayerPrefs.SetInt ( baseScoreKey , baseScore );
        PlayerPrefs.Save ();
    }
}
