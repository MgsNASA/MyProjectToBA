using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomLoadingAnimation : MonoBehaviour
{
    public Sprite [ ] dropSprites;
    public int numDrops = 100;
    public float fadeOutTime = 2f;
    public float fadeInTime = 0.1f;
    public float destroyDelay = 2f;
    public float dropSpawnDelay = 0.01f;
    public Image backGround;
    public TextMeshProUGUI textToFade;
    public Image imageToFade;
    public Image SecondImageToFade;

    private RectTransform canvasRect;
    private Vector2 minCanvasPos;
    private Vector2 maxCanvasPos;

    private List<GameObject> drops = new List<GameObject> ();

    void Start( )
    {
        canvasRect = GetComponentInParent<Canvas> ().GetComponent<RectTransform> ();
        minCanvasPos = canvasRect.rect.min;
        maxCanvasPos = canvasRect.rect.max;

        StartCoroutine ( StartAnimationWithDelay () );
    }

    IEnumerator StartAnimationWithDelay( )
    {
        yield return new WaitForSecondsRealtime ( 1.0f ); // Задержка перед началом анимации

        StartCoroutine ( FadeOutTextAndImage () );
    }

    IEnumerator FadeOutTextAndImage( )
    {
        float startTime = Time.realtimeSinceStartup;

        while ( Time.realtimeSinceStartup < startTime + fadeOutTime )
        {
            float alpha = 1f - ( ( Time.realtimeSinceStartup - startTime ) / fadeOutTime );

            // Устанавливаем полную прозрачность для текста и изображений
            textToFade.color = new Color ( textToFade.color.r , textToFade.color.g , textToFade.color.b , alpha );
            imageToFade.color = new Color ( imageToFade.color.r , imageToFade.color.g , imageToFade.color.b , alpha );
            SecondImageToFade.color = new Color ( SecondImageToFade.color.r , SecondImageToFade.color.g , SecondImageToFade.color.b , alpha );

            yield return null;
        }

        StartCoroutine ( FillScreenWithDrops () );
    }

    IEnumerator FillScreenWithDrops( )
    {
        for ( int i = 0; i < numDrops; i++ )
        {
            int randomIndex = Random.Range ( 0 , dropSprites.Length );
            Sprite dropSprite = dropSprites [ randomIndex ];

            GameObject dropObject = new GameObject ( "Drop" );
            Image dropImage = dropObject.AddComponent<Image> ();
            dropImage.sprite = dropSprite;
            dropObject.transform.SetParent ( transform );

            RectTransform dropRect = dropObject.GetComponent<RectTransform> ();
            dropRect.sizeDelta = new Vector2 ( 500f , 500f );

            dropRect.anchoredPosition = new Vector2 ( Random.Range ( minCanvasPos.x , maxCanvasPos.x ) , Random.Range ( minCanvasPos.y , maxCanvasPos.y ) );

            drops.Add ( dropObject );

            yield return new WaitForSecondsRealtime ( dropSpawnDelay );
        }

        backGround.color = new Color ( backGround.color.r , backGround.color.g , backGround.color.b , 0f );
        StartCoroutine ( DestroyDropsAfterDelay () );
    }

    IEnumerator DestroyDropsAfterDelay( )
    {
        drops.Reverse (); // Реверс списка капель

        foreach ( var drop in drops )
        {
            yield return new WaitForSecondsRealtime ( destroyDelay );
            Destroy ( drop );
        }

        Destroy ( gameObject );
    }
}
