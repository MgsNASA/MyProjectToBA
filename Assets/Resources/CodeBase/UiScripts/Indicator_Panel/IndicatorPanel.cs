using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorPanel : MonoBehaviour
{
    public TextMeshProUGUI TextProcent;
    public Image ImageIndicator;
    [SerializeField] public Color TextColor;
    [SerializeField] public Color ImageColor;
    public float fillSpeed = 0.1f; // —корость заполнени€ полосы прогресса
    private float currentFill = 0f; // “екущее заполнение полосы прогресса


    public void ChangeText( float fillAmount )
    {
        int percentage = Mathf.RoundToInt ( fillAmount * 100 );
        TextProcent.text = $"{percentage}%";
    }

    // ћетод дл€ увеличени€ прогресса индикатора
    public void IncreaseProgress( float amount )
    {
        currentFill += amount;
        currentFill = Mathf.Clamp01 ( currentFill ); // ќграничиваем значение от 0 до 1
        UpdateIndicator ();
        Debug.Log ( $"Progress increased by {amount}. Current progress: {currentFill }%" );
    }

    private void UpdateIndicator( )
    {
        ImageIndicator.fillAmount = currentFill;
        ChangeText ( currentFill );
     
    }

    private void OnValidate( )
    {
        if ( ImageIndicator != null )
        {
            ImageIndicator.color = ImageColor;
            TextProcent.color = TextColor;
        }
    }

    public float GetFillAmount( )
    {
        return currentFill;
    }

 
}
