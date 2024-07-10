using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    public Sprite circleSprite;
    public float spacingX = 1.5f;
    public float spacingY = 1.5f;
    public int rows;
    public int [ ] ballsPerRow = { 4 , 5 , 4 , 5 , 4 , 5 , 4 , 5 , 4 , 5 };
    public Vector2 ballSize = new Vector2 ( 1.0f , 1.0f );

    private List<Circle> circles = new List<Circle> ();

    private const float InitialYPosition = 0f;
    private const float StartXOffset = 0f;
    private const float StartZPosition = 0f;

    void Start( )
    {
        CreateCircle ();
    }

    private void CreateCircle( )
    {
        Vector3 startPos = transform.position + new Vector3 ( StartXOffset , InitialYPosition , StartZPosition );

        for ( int i = 0; i < rows; i++ )
        {
            int count = ballsPerRow [ i ];
            float startX = startPos.x - ( count - 1 ) * spacingX / 2f;

            for ( int j = 0; j < count; j++ )
            {
                Vector3 ballPosition = new Vector3 ( startX + j * spacingX , startPos.y - i * spacingY , startPos.z );
                Color randomColor = GetRandomColor ();
                Circle circle = GameFactory.CreateObject<Circle> ( randomColor , ballSize , ballPosition , circleSprite , $"Circle_{i}_{j}" );
                circle.Draw ();

                circle.transform.SetParent ( transform );

                circles.Add ( circle );
            }

            startPos.y -= spacingY;
        }
    }

    private Color GetRandomColor( )
    {
        List<Color> colors = new List<Color>
        {
            new Color(1f, 0.92f, 0.016f), // Yellow
            new Color(0.255f, 0.788f, 0.031f), // Green
            new Color(0.016f, 0.255f, 1f), // Blue
            new Color(0.576f, 0.031f, 0.788f), // Purple
            new Color(1f, 0.031f, 0.016f), // Red
            new Color(1f, 0.486f, 0.486f), // Pink
            new Color(1f, 0.486f, 0f) // Orange
        };

        int totalWeight = 100;
        int randomNumber = Random.Range ( 0 , totalWeight );

        int cumulativeWeight = 0;
        for ( int i = 0; i < colors.Count; i++ )
        {
            cumulativeWeight += GetWeight ( i );
            if ( randomNumber < cumulativeWeight )
            {
                return colors [ i ];
            }
        }

        return colors [ Random.Range ( 0 , colors.Count ) ];
    }

    private int GetWeight( int index )
    {
        switch ( index )
        {
            case 0:
                return 75; // Yellow
            default:
                return 5; // Other colors
        }
    }

    public void ApplyFewerBarriers( int reductionPercent )
    {
        // Уменьшаем количество барьеров на заданный процент
        foreach ( Circle circle in circles )
        {
            circle.ReduceBarrierCount ( reductionPercent );
        }
    }

    public void ApplyDoubleStrength( )
    {
        // Удваиваем силу фигур
        foreach ( Circle circle in circles )
        {
            circle.DoubleStrength ();
        }
    }

    public void ApplyMoreReward( float rewardIncreasePercent )
    {
        // Увеличиваем награду за взаимодействие с фигурами
        foreach ( Circle circle in circles )
        {
            circle.IncreaseReward ( rewardIncreasePercent );
        }
    }

    public void Restart( )
    {
        foreach ( Transform child in transform )
        {
            Destroy ( child.gameObject );
        }

        circles.Clear ();

        CreateCircle ();
    }
}
