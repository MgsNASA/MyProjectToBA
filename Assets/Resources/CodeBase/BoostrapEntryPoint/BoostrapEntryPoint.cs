using System.Collections;
using UnityEngine;
public class BoostrapEntryPoint : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    private IEnumerator Start( )
    {
        // Показываем загрузочный экран
        yield return new WaitForSecondsRealtime ( 0 );
        // Ваш код загрузки или инициализации
        Debug.Log ( "Загрузка..." );
        // _gameManager.StartGame ();
        _gameManager.Inilization ();
        // Завершение загрузки
        Debug.Log ( "Инициализация и Загрузка объектов завершена !" );

    }
}

