using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CircleFunctionality : MonoBehaviour
{
    [SerializeField] private IndicatorPanel indicatorPanelPrefab;
    [SerializeField] private ScaleAnimation scaleAnimation;
    [SerializeField] private FloatingText floatingTextPrefab;

    private const string ParentTransformName = "Indicators_Panel";
    private IndicatorPanel indicatorPanelInstance;
    private int clickCount = 0;
    private float progress = 0.01f; // Приватное поле для хранения прогресса

    public float Progress
    {
        get
        {
            return progress;
        }
        set
        {
            progress = value; // Устанавливаем новое значение прогресса
            indicatorPanelInstance.IncreaseProgress ( progress ); // Обновляем индикатор прогресса
            SaveProgress (); // Сохраняем прогресс
        }
    }

    public int CircleID
    {
        get; set;
    }
    public int PrefabIndex
    {
        get; set;
    }

    private void Awake( )
    {
        InitializeIndicatorPanel ();
        InitializeScaleAnimation ();
    }

    private void InitializeIndicatorPanel( )
    {
        Transform parentTransform = GameObject.Find ( ParentTransformName )?.transform;
        if ( parentTransform == null )
        {
            Debug.LogError ( "CircleFunctionality: ParentTransform not found!" );
            return;
        }

        if ( indicatorPanelPrefab == null )
        {
            Debug.LogError ( "CircleFunctionality: IndicatorPanelPrefab is not assigned!" );
            return;
        }

        indicatorPanelInstance = Instantiate ( indicatorPanelPrefab , parentTransform );
        indicatorPanelInstance.transform.SetParent ( parentTransform , false );

        LoadProgress ();
    }

    private void InitializeScaleAnimation( )
    {
        scaleAnimation = GetComponent<ScaleAnimation> ();
        if ( scaleAnimation == null )
        {
            Debug.LogError ( "CircleFunctionality: ScaleAnimation component not found!" );
        }
    }

    public void OnClick( )
    {
        clickCount++;
        Debug.Log ( $"CircleFunctionality: OnClick called, clickCount = {clickCount}" );

        if ( indicatorPanelInstance == null )
        {
            Debug.LogError ( "CircleFunctionality: IndicatorPanelInstance is null" );
            return;
        }

        HandleClickAction ();
        CreateFloatingTextForCoins ();
        PerformBoostManagerClick ();
        PlayScaleAnimation ();
        CheckAndAdvanceLevelIfIndicatorsFilled ();
    }

    private void HandleClickAction( )
    {
        if ( clickCount % 25 == 0 )
        {
            float newFillAmount = Mathf.Clamp01 ( BoostManager.Instance.GetSumProgress () );
            indicatorPanelInstance.IncreaseProgress ( newFillAmount );
            clickCount = 0;
            Debug.Log ( $"CircleFunctionality: Progress increased, newFillAmount = {newFillAmount}" );

           // int roundedValue = Mathf.CeilToInt ( newFillAmount );
            CreateFloatingText ( $"+{newFillAmount *100}%" , indicatorPanelInstance.ImageColor , TextAlignment.Right );
            
            SaveProgress ();
        }
    }

    private void CreateFloatingTextForCoins( )
    {
        CreateFloatingText ( $"+{BoostManager.Instance.GetSum ()}" , Color.white , TextAlignment.Left );
    }

    private void PerformBoostManagerClick( )
    {
        if ( BoostManager.Instance != null )
        {
            BoostManager.Instance.PerformClick ();
        }
        else
        {
            Debug.LogError ( "CircleFunctionality: BoostManager.Instance is null" );
        }
    }

    private void PlayScaleAnimation( )
    {
        if ( scaleAnimation != null )
        {
            scaleAnimation.PlayAnimation ();
        }
        else
        {
            Debug.LogError ( "CircleFunctionality: ScaleAnimation is null" );
        }
    }

    private void CheckAndAdvanceLevelIfIndicatorsFilled( )
    {
        if ( CreateCircle.Instance.AreAllIndicatorsFilled () )
        {
            if ( !LevelManager.Instance.IsLevelTransitioning )
            {
                LevelManager.Instance.CheckAndAdvanceLevel ();
                Debug.Log ( "CircleFunctionality: All indicators are filled!" );
            }
        }
    }

    private void CreateFloatingText( string text , Color color , TextAlignment alignment )
    {
        if ( floatingTextPrefab == null )
        {
            Debug.LogError ( "CircleFunctionality: FloatingTextPrefab is not assigned!" );
            return;
        }

        Vector3 position = transform.position;
        Vector3 offset = GetOffsetBasedOnAlignment ( alignment );

        position += offset;

        FloatingText floatingTextInstance = Instantiate ( floatingTextPrefab , position , Quaternion.identity , transform );
        floatingTextInstance.Initialize ( text , color , Vector3.up );
    }

    private Vector3 GetOffsetBasedOnAlignment( TextAlignment alignment )
    {
        RectTransform circleRectTransform = GetComponent<RectTransform> ();
        switch ( alignment )
        {
            case TextAlignment.Left:
                return new Vector3 ( -circleRectTransform.rect.width / 2f , -circleRectTransform.rect.height / 2f , 0f );
            case TextAlignment.Right:
                return new Vector3 ( circleRectTransform.rect.width / 2f , -circleRectTransform.rect.height / 2f , 0f );
            default:
                return Vector3.zero;
        }
    }

    public float IndicatorFillAmount => indicatorPanelInstance?.GetFillAmount () ?? 0f;

    public void DestroyCircle( )
    {
        if ( indicatorPanelInstance != null )
        {
            Destroy ( indicatorPanelInstance.gameObject );
        }

        foreach ( Transform child in transform )
        {
            if ( child.CompareTag ( "FloatingText" ) )
            {
                Destroy ( child.gameObject );
            }
        }

        Destroy ( gameObject );
    }

    private void OnDestroy( )
    {
        DestroyCircle ();
    }

    public void UpdateProgress( float newProgress )
    {
        Progress = newProgress; // Обновляем переменную прогресса через свойство
    }

    private void SaveProgress( )
    {
        PlayerPrefs.SetFloat ( $"CircleProgress_{CircleID}" , progress ); // Сохраняем прогресс по CircleID
        PlayerPrefs.SetInt ( $"CirclePrefabIndex_{CircleID}" , PrefabIndex ); // Сохраняем индекс префаба по CircleID
        PlayerPrefs.Save (); // Сохраняем изменения в PlayerPrefs
    }

    private void LoadProgress( )
    {
        progress = PlayerPrefs.GetFloat ( $"CircleProgress_{CircleID}" , 0.01f ); // Загружаем прогресс по CircleID, если есть, иначе 0.00f
        PrefabIndex = PlayerPrefs.GetInt ( $"CirclePrefabIndex_{CircleID}" , -1 ); // Загружаем индекс префаба по CircleID, если есть, иначе -1
    }


}
