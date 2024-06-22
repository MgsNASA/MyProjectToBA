using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChangeBackGround : MonoBehaviour
{
    public static ChangeBackGround Instance
    {
        get; private set;
    }

    public List<Sprite> BackGrounds; // Список спрайтов фона
    public Image targetImage; // Image компонент, который будет отображать фон

    private void Start( )
    {
        if ( Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy ( gameObject );
        }
        LevelManager.Instance.Load ();
    }

    // Метод для смены фона
    public void ChangeBackground( int index )
    {
        if ( index >= 0 && index < BackGrounds.Count )
        {
            // Устанавливаем выбранный спрайт
            targetImage.sprite = BackGrounds [ index ];
        }
        else
        {
            Debug.LogError ( "Background index is out of range" );
        }
    }
}
