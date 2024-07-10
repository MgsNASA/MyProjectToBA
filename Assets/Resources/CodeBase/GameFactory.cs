using UnityEngine;

public static class GameFactory
{
    public static T CreateObject<T>( Color colorSprite , Vector2 size , Vector3 position , Sprite sprite = null , string shapeName = null ) where T : Shape
    {
        GameObject shapeObject = new GameObject ( shapeName );
        shapeObject.transform.position = position;

        T shape = shapeObject.AddComponent<T> ();
        shape.sprite = sprite;
        shape.shapeName = shapeName;
        shape.colorSprite = colorSprite;
        shape.size = size;

        return shape;
    }

    public static T CreateObject<T>( Vector2 size , Vector3 position , Sprite sprite = null , string shapeName = null ) where T : Shape
    {
        GameObject shapeObject = new GameObject ( shapeName );
        shapeObject.transform.position = position;

        T shape = shapeObject.AddComponent<T> ();
        shape.sprite = sprite;
        shape.shapeName = shapeName;
        shape.size = size;

        // Add a SpriteRenderer component if not already present
        SpriteRenderer spriteRenderer = shapeObject.GetComponent<SpriteRenderer> ();
        if ( spriteRenderer == null )
        {
            spriteRenderer = shapeObject.AddComponent<SpriteRenderer> ();
        }

        // Set the sprite and color
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = Color.white; // White color with full opacity

        return shape;
    }

}
