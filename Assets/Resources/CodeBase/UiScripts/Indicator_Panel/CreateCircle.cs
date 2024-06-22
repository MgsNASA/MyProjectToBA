using UnityEngine;
using System.Collections.Generic;

public class CreateCircle : MonoBehaviour
{
    public static CreateCircle Instance
    {
        get; private set;
    }

    public CircleFunctionality [ ] circlePrefabs;
    public Transform canvasTransform;
    public List<CircleFunctionality> CreatedCircles { get; private set; } = new List<CircleFunctionality> ();
    private List<int> availableIndices;
    private int currentCircleIndex = 0;
    [SerializeField]
    private bool isFirstLaunch = true;

    private void Awake( )
    {
        if ( Instance == null )
        {
            Instance = this;
            DontDestroyOnLoad ( gameObject );
        }
        else
        {
            Destroy ( gameObject );
        }
    }

    public void InitializeAndLoadGame( )
    {
        availableIndices = new List<int> ( circlePrefabs.Length );
        for ( int i = 0; i < circlePrefabs.Length; i++ )
        {
            availableIndices.Add ( i );
        }

        isFirstLaunch = PlayerPrefs.GetInt ( "IsFirstLaunch" , 1 ) == 1;

        if ( isFirstLaunch )
        {
            CreateInitialCircles ();
            isFirstLaunch = false;
            SaveGame ();
        }
        else
        {
            LoadGame ();
        }
    }

    private void ClearPlayerPrefs( )
    {
        PlayerPrefs.DeleteAll ();
        PlayerPrefs.Save ();
    }

    public void CreateRandomCircle( )
    {
        if ( circlePrefabs.Length == 0 )
        {
            Debug.LogError ( "Circle prefabs array is empty!" );
            return;
        }

        if ( availableIndices.Count == 0 )
        {
            Debug.LogError ( "No more unique prefabs available!" );
            return;
        }

        int randomIndex = Random.Range ( 0 , availableIndices.Count );
        int prefabIndex = availableIndices [ randomIndex ];
        availableIndices.RemoveAt ( randomIndex );

        CircleFunctionality circleInstance = Instantiate ( circlePrefabs [ prefabIndex ] );
        circleInstance.CircleID = CreatedCircles.Count;
        circleInstance.UpdateProgress ( 0.0f );
        circleInstance.PrefabIndex = prefabIndex;
        circleInstance.transform.SetParent ( canvasTransform , false );
        CreatedCircles.Add ( circleInstance );
    }

    public void DestroyAllCircles( )
    {
        for ( int i = CreatedCircles.Count - 1; i >= 0; i-- )
        {
            if ( CreatedCircles [ i ] != null )
            {
                Destroy ( CreatedCircles [ i ].gameObject );
            }
        }

        CreatedCircles.Clear ();
        ResetAvailableIndices ();
    }

    public void SetActiveCircle( int index )
    {
        if ( index >= 0 && index < CreatedCircles.Count )
        {
            currentCircleIndex = index;
            for ( int i = 0; i < CreatedCircles.Count; i++ )
            {
                CreatedCircles [ i ].gameObject.SetActive ( i == currentCircleIndex );
            }
        }
    }

    public void ShowNextCircle( )
    {
        if ( CreatedCircles.Count > 0 )
        {
            int nextIndex = ( currentCircleIndex + 1 ) % CreatedCircles.Count;
            SetActiveCircle ( nextIndex );
        }
    }

    public void ShowPreviousCircle( )
    {
        if ( CreatedCircles.Count > 0 )
        {
            int prevIndex = ( currentCircleIndex - 1 + CreatedCircles.Count ) % CreatedCircles.Count;
            SetActiveCircle ( prevIndex );
        }
    }

    public bool AreAllIndicatorsFilled( )
    {
        foreach ( var circle in CreatedCircles )
        {
            if ( !circle || circle.IndicatorFillAmount < 1.0f )
            {
                return false;
            }
        }
        return true;
    }

    private void SaveGame( )
    {
        PlayerPrefs.SetInt ( "IsFirstLaunch" , isFirstLaunch ? 1 : 0 );
        PlayerPrefs.SetInt ( "CircleCount" , CreatedCircles.Count );
        for ( int i = 0; i < CreatedCircles.Count; i++ )
        {
            PlayerPrefs.SetInt ( $"CircleIndex_{i}" , CreatedCircles [ i ].CircleID );
            PlayerPrefs.SetFloat ( $"CircleProgress_{CreatedCircles [ i ].CircleID}" , CreatedCircles [ i ].IndicatorFillAmount );
            PlayerPrefs.SetInt ( $"CirclePrefabIndex_{CreatedCircles [ i ].CircleID}" , CreatedCircles [ i ].PrefabIndex );
        }
        PlayerPrefs.Save ();
    }

    public void LoadGame( )
    {
        int circleCount = PlayerPrefs.GetInt ( "CircleCount" , 0 );
        for ( int i = 0; i < circleCount; i++ )
        {
            int circleID = PlayerPrefs.GetInt ( $"CircleIndex_{i}" );
            CreateRandomCircleWithID ( circleID );
        }
    }

    private void CreateRandomCircleWithID( int circleID )
    {
        if ( circlePrefabs.Length == 0 )
        {
            Debug.LogError ( "Circle prefabs array is empty!" );
            return;
        }

        int prefabIndex = PlayerPrefs.GetInt ( $"CirclePrefabIndex_{circleID}" , -1 );
        if ( prefabIndex == -1 )
        {
            Debug.LogError ( $"PrefabIndex for circle with ID {circleID} not found in PlayerPrefs!" );
            return;
        }

        if ( availableIndices.Contains ( prefabIndex ) )
        {
            availableIndices.Remove ( prefabIndex );
        }

        CircleFunctionality circleInstance = Instantiate ( circlePrefabs [ prefabIndex ] );

        circleInstance.CircleID = circleID;
        circleInstance.PrefabIndex = prefabIndex;
        circleInstance.transform.SetParent ( canvasTransform , false );
        CreatedCircles.Add ( circleInstance );

        circleInstance.UpdateProgress ( PlayerPrefs.GetFloat ( $"CircleProgress_{circleID}" , 0.00f ) );
    }

    private void CreateInitialCircles( )
    {
        for ( int i = 0; i < 3; i++ )
        {
            CreateRandomCircle ();
        }
    }

    public void ResetAvailableIndices( )
    {
        availableIndices.Clear ();
        for ( int i = 0; i < circlePrefabs.Length; i++ )
        {
            availableIndices.Add ( i );
        }
    }

    private void OnApplicationQuit( )
    {
        SaveGame ();
    }

    private void OnDestroy( )
    {
        SaveGame ();
    }

    // Метод для проверки наличия круга с заданным CircleID
    public bool ContainsCircleWithID( int circleID )
    {
        CircleFunctionality circleToFind = new GameObject ().AddComponent<CircleFunctionality> ();
        circleToFind.CircleID = circleID;

        foreach ( var circle in CreatedCircles )
        {
            if ( circle.Equals ( circleToFind ) )
            {
                Destroy ( circleToFind.gameObject ); // Удаление временного объекта
                return true;
            }
        }
        Destroy ( circleToFind.gameObject ); // Удаление временного объекта
        return false;
    }
}
