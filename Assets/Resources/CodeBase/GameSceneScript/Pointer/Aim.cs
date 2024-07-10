using UnityEngine;

public class Aim : MonoBehaviour
{
    // Переменная для хранения позиции цели
    private Vector3 targetPosition;

    // Ссылка на компонент LineRenderer
    private LineRenderer lineRenderer;

    // Начальная точка линии
    [SerializeField]
    private Transform lineStartPoint;

    // Конечная точка линии (может быть необязательно)
    [SerializeField]
    private Transform lineEndPoint;

    // Обновление вызывается один раз за кадр
    void Update( )
    {
        // Получаем позицию указателя мыши в мировых координатах при нажатии левой кнопки мыши
        if ( Input.GetMouseButton ( 0 ) )
        {
            targetPosition = Camera.main.ScreenToWorldPoint ( Input.mousePosition );
            targetPosition.z = transform.position.z; // Устанавливаем Z координату такую же, как у прицела
            // Поворачиваем прицел в сторону указателя мыши
            RotateTowardsTarget ( targetPosition );
            // Обновляем линию
            UpdateLine ( targetPosition );
        }
        else
        {
            // Если кнопка мыши не нажата, скрываем линию
            lineRenderer.enabled = false;
        }
    }

    // Метод для поворота прицела в сторону цели
    private void RotateTowardsTarget( Vector3 target )
    {
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2 ( direction.y , direction.x ) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis ( angle , Vector3.forward );
    }

    // Метод для обновления текущего спрайта прицела
    public void UpdateCurrentSprite( Sprite newSprite )
    {
        // Здесь можно добавить логику обновления спрайта прицела
        // Например:
        // GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    // Метод для получения направления прицеливания
    public Vector2 GetAimDirection( )
    {
        // Если targetPosition не инициализирована (например, если никакая кнопка мыши не нажата), возвращаем нулевой вектор
        if ( targetPosition == Vector3.zero )
        {
            return Vector2.zero;
        }
        // Возвращаем направление от прицела к цели
        return ( targetPosition - transform.position ).normalized;
    }

    // Метод для обновления линии
    private void UpdateLine( Vector3 target )
    {
        if ( lineRenderer == null )
            return;

        // Если не хотите отключать линию полностью, можете просто не обновлять её
        // lineRenderer.enabled = true; // Включаем LineRenderer
        // lineRenderer.positionCount = 2; // Устанавливаем количество точек линии

        // Устанавливаем начальную и конечную точки линии
        lineRenderer.SetPosition ( 0 , lineStartPoint.position );
        lineRenderer.SetPosition ( 1 , target );
    }

    // Метод для инициализации LineRenderer
    private void Awake( )
    {
        lineRenderer = GetComponent<LineRenderer> ();

        if ( lineRenderer == null )
        {
            Debug.LogError ( "LineRenderer component not found." );
        }
        else
        {
            // Отключаем LineRenderer, если не хотим использовать его
            lineRenderer.enabled = false;
        }
    }
}
