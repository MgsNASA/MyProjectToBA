using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    public string shapeName;  // Делаем публичным
    public Sprite sprite;     // Делаем публичным
    public Color colorSprite; // Делаем публичным
    public Vector2 size;      // Делаем публичным

    // Абстрактный метод для отрисовки, который должен быть реализован в производных классах
    public abstract void Draw( );
}
