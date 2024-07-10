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

    private float moveDistance = 0.1f; // Расстояние, на которое ячейка будет двигаться вниз
    private float moveDuration = 0.3f; // Продолжительность движения вниз

    private bool isMoving = false; // Флаг для отслеживания движения

    private string baseScoreKey;

    void Start( )
    {
        // Define a unique key for this cell's base score based on its ID and color
        baseScoreKey = $"CellBaseScore_{GetInstanceID ()}_{cellColor}";

        // Устанавливаем базовые очки в зависимости от цвета ячейки
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

        // Получаем ссылку на ScoreManager
       scoreManager = FindObjectOfType<ScoreManager> ();
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        // Проверяем, что объект, с которым произошло столкновение, является мячиком
        if ( collision.gameObject.CompareTag ( "Circle" ) )
        {
            int score = baseScore;

            // Применяем множитель в зависимости от цвета штыря
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

            // Добавляем очки через ScoreManager
            if ( scoreManager != null )
            {
                scoreManager.Add ( score );
            }

            // Выводим очки в консоль (можно заменить на другую логику)
            Debug.Log ( "Score: " + score );

            // Двигаем ячейку вниз, если она не двигается уже
            if ( !isMoving )
            {
                StartCoroutine ( MoveDown () );
            }

            // Удаляем ячейку из игры
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
