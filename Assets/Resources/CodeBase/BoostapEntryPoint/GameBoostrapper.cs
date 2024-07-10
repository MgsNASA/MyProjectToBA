using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBoostrapper : MonoBehaviour
{
    private UiManager uiManager;
    private Pointer pointer;
    private List<ResourceManagerBase> resourceManagerBase;
    [SerializeField]
    private BoostrapLoadingPanel loadingPanel;

    private void Awake( )
    {
        // Автоматически находим и назначаем компоненты
        uiManager = UiManager.Instance;
        pointer = FindObjectOfType<Pointer> ();
        resourceManagerBase = new List<ResourceManagerBase> ( FindObjectsOfType<ResourceManagerBase> () );


        // Проверяем, что все компоненты найдены
        if ( uiManager == null )
        {
            Debug.LogError ( "UiManager not found!" );
        }

        if ( pointer == null )
        {
            Debug.LogError ( "Pointer not found!" );
        }

        if ( resourceManagerBase == null || resourceManagerBase.Count == 0 )
        {
            Debug.LogError ( "No ResourceManagerBase instances found!" );
        }

        if ( loadingPanel == null )
        {
            Debug.LogError ( "BoostrapLoadingPanel not found!" );
        }
        else
        {
            // Запускаем корутину для симуляции процесса загрузки
            StartCoroutine ( SimulateLoading () );
        }
    }

    private IEnumerator SimulateLoading( )
    {
        loadingPanel.Show ();

        float duration = 10f; // Продолжительность загрузки в секундах
        float elapsedTime = 0f;

        while ( elapsedTime < duration )
        {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01 ( elapsedTime / duration );
            loadingPanel.SetProgress ( progress );
            yield return null;
        }

        loadingPanel.SetProgress ( 1f );
        loadingPanel.Hide ();
    }
}
