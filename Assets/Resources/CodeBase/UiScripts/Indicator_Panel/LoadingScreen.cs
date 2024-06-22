using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Image backgroundImage;
    private bool reachedCenter = false; // Флаг, который показывает, достигло ли изображение центра
    [SerializeField]
    private RectTransform rectTransform; // Изменил тип переменной на RectTransform
    private float speed = 800f; // Скорость движения вниз
    private float delayBeforeStart = 3f; // Задержка перед началом движения
    private float delayBetweenMoves = 3f; // Задержка между каждым шагом движения
    private CreateCircle circle;

    private void Awake( )
    {
    
        if ( rectTransform == null )
        {
            Debug.LogError ( "LoadingScreen: RectTransform component not found!" );
        }

        circle = FindObjectOfType<CreateCircle> (); // Поиск объекта circle на сцене
    }

    public void Show( )
    {
        if ( rectTransform == null )
        {
            Debug.LogError ( "LoadingScreen: RectTransform component not assigned!" );
            return;
        }

        ResetScreen ();
        gameObject.SetActive ( true );
        Invoke ( "StartMovingImage" , delayBeforeStart );
    }

    public void Hide( )
    {
        StopAllCoroutines (); // Остановить все запущенные корутины
        gameObject.SetActive ( false );
        ResetScreen ();
    }

    private void StartMovingImage( )
    {
        if ( !gameObject.activeSelf )
            return;

        StartCoroutine ( MoveImageDown () );
    }

    private IEnumerator MoveImageDown( )
    {
        // Перемещение до середины экрана
        while ( !reachedCenter )
        {
            Vector3 newPos = rectTransform.localPosition;
            newPos.y -= speed * Time.deltaTime;

            if ( newPos.y <= 0 )
            {
                newPos.y = 0; // Устанавливаем точно в середину экрана
                reachedCenter = true; // Устанавливаем флаг в true
            }

            rectTransform.localPosition = newPos;
            yield return null;
        }

        // Задержка перед продолжением движения вниз
        yield return new WaitForSeconds ( delayBetweenMoves );
        LevelManager.Instance.StartLevelChanger ();

        // Продолжение движения вниз
        while ( true )
        {
            Vector3 newPos = rectTransform.localPosition;
            newPos.y -= speed * Time.deltaTime;

            if ( newPos.y <= -Screen.height * 2f )
            {
                newPos.y = -Screen.height * 2f; // Устанавливаем ниже нижнего края экрана
                rectTransform.localPosition = newPos;
                break; // Выходим из цикла, когда достигаем конца
            }

            rectTransform.localPosition = newPos;

            yield return null;
        }
    }

    private void ResetScreen( )
    {
        if ( rectTransform == null )
        {
            Debug.LogError ( "LoadingScreen: RectTransform component not assigned!" );
            return;
        }

        // Сбросить позицию изображения обратно в самую верхнюю часть экрана
        Vector3 startPos = rectTransform.localPosition;
        startPos.y = Screen.height * 2f; // Расположить изображение за пределами верхней части экрана
        rectTransform.localPosition = startPos;

        reachedCenter = false; // Сброс флага reachedCenter
    }
}
