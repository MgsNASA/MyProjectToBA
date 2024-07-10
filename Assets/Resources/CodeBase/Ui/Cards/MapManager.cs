using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public MoneyManager MoneyManager;

    [SerializeField] private Image mapDisplay;
    [SerializeField] private List<Sprite> cards = new List<Sprite> ();

    private void Awake( )
    {
        Instance = this;
        MoneyManager = FindObjectOfType<MoneyManager> ();
    }

 

    public void UpdateCurrentMap( Sprite mapSprite )
    {
        mapDisplay.sprite = mapSprite;
    }
}
