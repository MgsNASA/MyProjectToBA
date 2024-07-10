using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class LoadingPanel : MonoBehaviour
{
    [SerializeField]
    protected GameObject panel;
    [SerializeField]
    protected TextMeshProUGUI loadingText;
    [SerializeField]
    protected Slider progressBar;

    protected virtual void Awake( )
    {
        if ( panel == null )
            panel = this.gameObject;

        if ( loadingText == null )
            loadingText = panel.GetComponentInChildren<TextMeshProUGUI> ();

        if ( progressBar == null )
            progressBar = panel.GetComponentInChildren<Slider> ();
    }

    public virtual void Show( ) => panel.SetActive ( true );

    public virtual void Hide( ) => panel.SetActive ( false );

    public virtual void SetProgress( float progress )
    {
        if ( progressBar != null )
        {
            progressBar.value = progress;
        }

        if ( loadingText != null )
        {
            loadingText.text = $"{Mathf.Clamp ( progress * 100 , 0 , 100 ):0}%";
        }
    }
}
