using UnityEngine;

public interface IShapeInitializer
{
    void Initialize( string shapeName , Sprite sprite , Color colorSprite , Vector2 size , Transform parentTransform );
}
