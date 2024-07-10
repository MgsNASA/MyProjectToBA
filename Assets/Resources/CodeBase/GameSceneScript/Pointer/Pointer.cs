using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    private const string CircleTag = "Circle";
    private const string UpgradeLevelKey = "UpgradeLevel";
    private const string DoubleStrengthKey = "DoubleStrength";

    public Sprite currentSprite;
    [SerializeField] private Vector2 circleSize = new Vector2 ( 1.0f , 1.0f );
    public float shootForce = 10.0f;

    private Circle currentCircle;
    private Aim aim;

    private int energyCost = 1;
    private int upgradeLevel = 0;
    private float doubleStrengthMaxUpgradeLevel = 2; // Maximum upgrade level for double strength
    private int doubleStrengthExtraBalls = 0; // Extra balls per shot with double strength

    [SerializeField] private Transform shootPosition;

    private bool isCharging = false;
    private float chargeTime = 0.0f;
    private float maxChargeTime = 2.0f; // Maximum charge time to create multiple balls

    [SerializeField] private LayerMask nonUiLayerMask; // Layer mask for non-UI elements

    private void Start( )
    {
        aim = FindObjectOfType<Aim> ();
        if ( aim == null )
        {
            Debug.LogError ( "Aim component not found in the scene." );
        }

        upgradeLevel = PlayerPrefs.GetInt ( UpgradeLevelKey , 0 );
        UpdateEnergyCost ( upgradeLevel );

        // Load double strength upgrade level
        doubleStrengthExtraBalls = PlayerPrefs.GetInt ( DoubleStrengthKey , 0 );
    }

    private void FixedUpdate( )
    {
        if ( IsPointerOverNonUI ( nonUiLayerMask ) )
        {
            if ( Input.GetMouseButtonDown ( 0 ) )
            {
                isCharging = true;
                chargeTime = 0.0f;
                CreateCircleForPointer ( circleSize );
            }

            if ( Input.GetMouseButton ( 0 ) )
            {
                chargeTime += Time.deltaTime;
                if ( chargeTime >= maxChargeTime )
                {
                    chargeTime = maxChargeTime;
                }
            }

            if ( Input.GetMouseButtonUp ( 0 ) )
            {
                isCharging = false;
                ShootCircle ();
            }
        }
    }

    private void OnValidate( )
    {
        circleSize.x = Mathf.Max ( 0 , circleSize.x );
        circleSize.y = Mathf.Max ( 0 , circleSize.y );
    }

    private void CreateCircleForPointer( Vector2 size )
    {
        if ( currentCircle != null )
        {
            Destroy ( currentCircle.gameObject );
        }

        string shapeName = $"Circle_{Time.time}";
        Vector3 position = shootPosition != null ? shootPosition.position : transform.position;
        currentCircle = GameFactory.CreateObject<Circle> ( size , position , currentSprite , shapeName );

        if ( currentCircle != null )
        {
            currentCircle.transform.parent = transform;
            currentCircle.Draw ();
            currentCircle.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
            currentCircle.gameObject.tag = CircleTag;

            if ( aim != null )
            {
                aim.UpdateCurrentSprite ( currentSprite );
            }
            Debug.Log ( "Circle created successfully." );
        }
        else
        {
            Debug.LogError ( "Failed to create Circle object." );
        }
    }

    public void UpdateDoubleBallChance( float chance )
    {
        doubleStrengthExtraBalls += Mathf.RoundToInt ( chance * 3 ); // Example calculation based on chance
    }

    public void UpdateCurrentSprite( Sprite newSprite )
    {
        currentSprite = newSprite;
    }

    public void UpdateEnergyCost( int level )
    {
        upgradeLevel = level;
        energyCost = upgradeLevel == 0 ? 1 : 2;
        PlayerPrefs.SetInt ( UpgradeLevelKey , upgradeLevel );
    }

    public void UpdateDoubleStrengthUpgrade( int level )
    {
        doubleStrengthExtraBalls = Mathf.Clamp ( level , 0 , ( int ) doubleStrengthMaxUpgradeLevel );
        PlayerPrefs.SetInt ( DoubleStrengthKey , doubleStrengthExtraBalls );
    }

    private void ShootCircle( )
    {
        if ( currentCircle != null )
        {
            int totalBalls = 1 + Mathf.RoundToInt ( ( chargeTime / maxChargeTime ) * doubleStrengthExtraBalls );
            Debug.Log ( $"Total balls to shoot: {totalBalls}" );

            for ( int i = 0; i < totalBalls; i++ )
            {
                if ( EnergyManager.Instance.Spend ( energyCost ) )
                {
                    Debug.Log ( $"Shooting ball {i + 1} of {totalBalls}" );
                    ShootSingleCircle ( currentCircle );
                    if ( i < totalBalls - 1 )
                    {
                        CreateCircleForPointer ( circleSize );
                    }
                }
                else
                {
                    Debug.Log ( "Not enough energy to shoot." );
                    break;
                }
            }
        }
        else
        {
            Debug.Log ( "Current circle is null, cannot shoot." );
        }
    }

    private void ShootSingleCircle( Circle circle )
    {
        Rigidbody2D rb = circle.GetComponent<Rigidbody2D> ();

        if ( rb == null )
        {
            rb = circle.gameObject.AddComponent<Rigidbody2D> ();
        }
        rb.bodyType = RigidbodyType2D.Dynamic;

        Vector2 shootDirection = aim.GetAimDirection ();
        rb.AddForce ( shootDirection * shootForce , ForceMode2D.Impulse );

        circle.transform.parent = null;
        Debug.Log ( "Circle shot with force." );
    }

    private bool IsPointerOverNonUI( LayerMask layerMask )
    {
        // Check if the mouse is over any collider in the specified layer mask
        RaycastHit2D hit = Physics2D.Raycast ( Camera.main.ScreenToWorldPoint ( Input.mousePosition ) , Vector2.zero , Mathf.Infinity , layerMask );
        return hit.collider != null;
    }
}
