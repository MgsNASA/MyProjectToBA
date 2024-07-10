using System.Collections;
using UnityEngine;

public class AnimatedLoadingPanel : MonoBehaviour
{
    [SerializeField] private Animator panelAnimator; // Добавлено поле для аниматора
    [SerializeField]
    private string targetPanelName;

    protected void Awake( )
    {
        if ( panelAnimator == null )
        {
            panelAnimator = GetComponent<Animator> ();
        }

    }

    public void Show( )
    {
        gameObject.SetActive ( true );
        panelAnimator.SetTrigger ( "Show" );
        Debug.Log ( "Show called" );
    }

    public void Hide( )
    {
        panelAnimator.SetTrigger ( "Hide" );
        Debug.Log ( "Hide called" );
    }

    public void SetTargetPanel( string panelName )
    {
        targetPanelName = panelName;
        Debug.Log ( "SetTargetPanel called with: " + panelName );
    }

    // Этот метод будет вызван из анимационного события после завершения анимации Show
    public void OnShowAnimationComplete( )
    {
        Debug.Log ( "OnShowAnimationComplete called" );
        SwitchPanel ();
    }

    // Этот метод будет вызван из анимационного события после завершения анимации Hide
    public void OnHideAnimationComplete( )
    {
        Debug.Log ( "OnHideAnimationComplete called" );
        gameObject.SetActive ( false );
    }

    public void SwitchPanel( )
    {
        if ( !string.IsNullOrEmpty ( targetPanelName ) )
        {
            
            UiManager.ShowPanel ( targetPanelName );
            Debug.Log ( "Switched to panel: " + targetPanelName );
            targetPanelName = null;
        }
        else
        {
            Debug.LogWarning ( "Target panel name is null or empty." );
        }
    }
    public  void HidePanels( )
    {
        UiManager.HideAllPanels ();
    }

}
