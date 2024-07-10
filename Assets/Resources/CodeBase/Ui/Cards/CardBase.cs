using UnityEngine;

public abstract class CardBase : MonoBehaviour
{

    // Методы, которые должны быть реализованы в наследниках
    protected abstract void UpdateButtonState( );
    public abstract void CheckAvailability( );
    protected abstract void SaveState( );
    protected abstract void LoadState( );

    // Виртуальный метод, который может быть переопределен
    protected virtual void OnButtonClick( )
    {
        Debug.Log ( "Default OnButtonClick implementation" );
    }

    protected const string StateKey = "CardState"; // Общий ключ для сохранения состояния в PlayerPrefs

    protected virtual void Start( )
    {
        // Пытаемся загрузить состояние из сохранений
      //  LoadState ();
        UpdateButtonState ();
    }

    protected virtual void OnEnable( )
    {
        CheckAvailability ();
    }

    protected virtual void OnDisable( )
    {
        SaveState (); // Сохраняем состояние при выключении объекта
    }
}

    