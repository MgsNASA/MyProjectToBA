using UnityEngine;
using System.Collections;

public class ScaleAnimation : MonoBehaviour
{
    public float animationDuration = 0.5f;
    public float scaleMultiplier = 2f;

    private Vector3 originalScale;
    private RectTransform rectTransform;

    private void Awake( )
    {
        rectTransform = GetComponent<RectTransform> ();
        originalScale = rectTransform.localScale;
    }

    public void PlayAnimation( )
    {
        StartCoroutine ( Animate () );
    }

    private IEnumerator Animate( )
    {
        Debug.Log (Time.timeScale);
        Vector3 targetScale = originalScale * scaleMultiplier;
        Vector3 initialScale = rectTransform.localScale;
        float startTime = Time.time;

        while ( Time.time < startTime + animationDuration )
        {
            float progress = ( Time.time - startTime ) / animationDuration;
            rectTransform.localScale = Vector3.Lerp ( initialScale , targetScale , progress );
            yield return null;
        }

        rectTransform.localScale = targetScale;

        yield return new WaitForSeconds ( 0.1f );

        startTime = Time.time;

        while ( Time.time < startTime + animationDuration )
        {
            float progress = ( Time.time - startTime ) / animationDuration;
            rectTransform.localScale = Vector3.Lerp ( targetScale , originalScale , progress );
            yield return null;
        }

        rectTransform.localScale = originalScale;
    }
}
