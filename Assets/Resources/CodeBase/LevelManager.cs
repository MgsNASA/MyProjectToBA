using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance
    {
        get; private set;
    } // Синглтон-экземпляр

    public LoadingScreen loadingScreen;
    public CreateCircle createCircle;
    [SerializeField]
    private int backgroundIndex = 0;
    private bool isLevelTransitioning = false; // Флаг, указывающий на процесс перехода между уровнями
    public bool IsLevelTransitioning => isLevelTransitioning;

    private void Awake( )
    {
        if ( Instance == null )
        {
            Instance = this;
            DontDestroyOnLoad ( gameObject ); // Сохраняем объект между сценами
        }
        else
        {
            Destroy ( gameObject ); // Уничтожаем дубликаты
        }
    }

    public void Load( )
    {
        // Загружаем сохраненный индекс фона
        backgroundIndex = PlayerPrefs.GetInt ( "BackgroundIndex" , 0 );

        // Проверка на null для ChangeBackGround.Instance
        if ( ChangeBackGround.Instance != null )
        {
            // Устанавливаем фон по загруженному индексу
            ChangeBackGround.Instance.ChangeBackground ( backgroundIndex );
        }
        else
        {
            Debug.LogError ( "ChangeBackGround.Instance is null" );
        }

        // Проверка на null для createCircle
        if ( createCircle != null )
        {
            // Загружаем состояние кругов
            createCircle.InitializeAndLoadGame ();

            // Устанавливаем активный круг, если есть созданные круги
            if ( createCircle.CreatedCircles.Count > 0 )
            {
                createCircle.SetActiveCircle ( 0 );
            }
        }
        else
        {
            Debug.LogError ( "createCircle is null" );
        }
    }

    public void CheckAndAdvanceLevel( )
    {
        if ( createCircle.AreAllIndicatorsFilled () )
        {
            loadingScreen.enabled = true;
            // Показать экран загрузки
            loadingScreen.Show ();
        }
    }

    public void StartLevelChanger( )
    {
        if ( !isLevelTransitioning )
        {
            StartCoroutine ( AdvanceToNextLevel () );
        }
    }

    public IEnumerator AdvanceToNextLevel( )
    {
        // Устанавливаем флаг, что уровень находится в процессе перехода
        isLevelTransitioning = true;

        backgroundIndex = ( backgroundIndex + 1 ) % ChangeBackGround.Instance.BackGrounds.Count; // Обновляем индекс фона с учетом количества фонов
        ChangeBackGround.Instance.ChangeBackground ( backgroundIndex );

        // Уничтожить все старые индикаторы
        createCircle.DestroyAllCircles ();

        // Сбросить индексы доступных кругов
        createCircle.ResetAvailableIndices ();

        // Создать новые индикаторы
        for ( int i = 0; i < 3; i++ )
        {
            createCircle.CreateRandomCircle (); // Создаем новые круги с начальным прогрессом 0%
        }

        // Устанавливаем первый шарик активным
        if ( createCircle.CreatedCircles.Count > 0 )
        {
            createCircle.SetActiveCircle ( 0 );
        }

        // Ждем некоторое время для имитации загрузки
        yield return new WaitForSeconds ( 5.0f );

        // Сохраняем индекс фона перед завершением уровня
        PlayerPrefs.SetInt ( "BackgroundIndex" , backgroundIndex );
        PlayerPrefs.Save ();

        // Сброс флага, когда уровень завершен
        isLevelTransitioning = false;
    }


}
