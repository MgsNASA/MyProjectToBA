using UnityEngine;

// Производный класс Circle
[RequireComponent ( typeof ( CircleCollider2D ) , typeof ( SpriteRenderer ) , typeof ( Rigidbody2D ) )]
public class Circle : Shape
{
    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private int fewerBarriersMaxUpgradeLevel;
    private float fewerBarriersReduction;

    private int doubleStrengthMaxUpgradeLevel;
    private int doubleStrengthExtraBalls;

    private int moreRewardMaxUpgradeLevel;
    private float moreRewardIncreasePercent;

    // Конструктор класса Circle
    public Circle( Sprite sprite , string shapeName , Vector2 size , Transform parentTransform , Color colorSprite )
    {
        // Создаем игровой объект и назначаем его родителем текущему объекту
        GameObject shapeObject = GameFactory.CreateObject<Circle> ( colorSprite , size , parentTransform.position , sprite , shapeName ).gameObject;
        shapeObject.transform.SetParent ( parentTransform );

        // Перемещаем текущий объект в созданный игровой объект
        this.transform.SetParent ( shapeObject.transform );
    }

    // Метод для отрисовки Circle
    public override void Draw( )
    {
        // Получаем компоненты после того, как объект был создан и настроен
        circleCollider = GetComponent<CircleCollider2D> ();
        spriteRenderer = GetComponent<SpriteRenderer> ();
        rb = GetComponent<Rigidbody2D> ();
        spriteRenderer.sortingOrder = 10;
        if ( spriteRenderer != null && circleCollider != null )
        {
            // Назначаем спрайт
            spriteRenderer.sprite = sprite;

            // Назначаем цвет
            spriteRenderer.color = colorSprite;

            // Устанавливаем радиус коллайдера
            circleCollider.radius = sprite.bounds.extents.x;
            rb.bodyType = RigidbodyType2D.Static;
            // Устанавливаем размер объекта
            transform.localScale = new Vector2 ( size.x , size.y );
        }
        else
        {
            Debug.LogError ( "SpriteRenderer or CircleCollider2D components are missing." );
        }
    }

    public void SetFewerBarriersUpgrade( int maxLevel , float reduction )
    {
        fewerBarriersMaxUpgradeLevel = maxLevel;
        fewerBarriersReduction = reduction;
    }

    public void SetDoubleStrengthUpgrade( int maxLevel , int extraBalls )
    {
        doubleStrengthMaxUpgradeLevel = maxLevel;
        doubleStrengthExtraBalls = extraBalls;
    }

    public void SetMoreRewardUpgrade( int maxLevel , float increasePercent )
    {
        moreRewardMaxUpgradeLevel = maxLevel;
        moreRewardIncreasePercent = increasePercent;
    }

    public void ReduceBarrierCount( int reductionPercent )
    {
        // Логика для уменьшения барьеров
        Debug.Log ( $"Reducing barriers by {reductionPercent}%" );
        // Ваша логика здесь
    }

    public void DoubleStrength( )
    {
        // Логика для удвоения силы
        Debug.Log ( "Doubling strength" );
        // Ваша логика здесь
    }

    public void IncreaseReward( float rewardIncreasePercent )
    {
        // Логика для увеличения награды
        Debug.Log ( $"Increasing reward by {rewardIncreasePercent}%" );
        // Ваша логика здесь
    }
}
