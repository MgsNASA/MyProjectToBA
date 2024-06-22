using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float moveSpeed = 100f;
    public float fadeDuration = 3f;
    private Color originalColor;
    private Vector3 direction;

    private float elapsed = 0f;

    public void Initialize( string text , Color color , Vector3 dir )
    {
        originalColor = color;
        textMesh.text = text;
        textMesh.color = color;
        direction = dir.normalized; // Нормализуем направление, чтобы скорость была постоянной
        Debug.Log ( "Inis" );
    }

    private void FixedUpdate( )
    {
        // Перемещаем текст в заданном направлении с постоянной скоростью
        transform.Translate ( direction * moveSpeed * Time.fixedDeltaTime );

        // Обновляем альфа-канал цвета текста для плавного затухания
        elapsed += Time.fixedDeltaTime;
        float t = Mathf.Clamp01 ( elapsed / fadeDuration );
        textMesh.color = Color.Lerp ( originalColor , new Color ( originalColor.r , originalColor.g , originalColor.b , 0 ) , t );

        // Если прошло время fadeDuration, уничтожаем объект
        if ( elapsed >= fadeDuration )
        {
            Destroy ( gameObject );
        }
    }

    private void OnDisable( )
    {
        Destroy ( gameObject );
    }

}
