using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorPanel : MonoBehaviour
{
    public TextMeshProUGUI TextProcent;
    public Image ImageIndicator;
    [SerializeField] public Color TextColor;
    [SerializeField] public Color ImageColor;
    public float fillSpeed = 0.1f; // �������� ���������� ������ ���������
    private float currentFill = 0f; // ������� ���������� ������ ���������


    public void ChangeText( float fillAmount )
    {
        int percentage = Mathf.RoundToInt ( fillAmount * 100 );
        TextProcent.text = $"{percentage}%";
    }

    // ����� ��� ���������� ��������� ����������
    public void IncreaseProgress( float amount )
    {
        currentFill += amount;
        currentFill = Mathf.Clamp01 ( currentFill ); // ������������ �������� �� 0 �� 1
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
