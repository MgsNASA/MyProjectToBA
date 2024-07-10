using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleManager : MonoBehaviour
{
    public static CircleManager Instance;
    public MoneyManager MoneyManager;

    [SerializeField] private List<CardCircle> circles = new List<CardCircle> ();

    private void Start( )
    {
        Instance = this;
        MoneyManager = FindObjectOfType<MoneyManager> ();
    }

    public void UpdateCircleSelection( CardCircle selectedCircle )
    {
        foreach ( CardCircle circle in circles )
        {
            if ( circle != selectedCircle && circle.CurrentState == CardCircle.State.Choose )
            {
                circle.CurrentState = CardCircle.State.Purchased;
            }
        }

        if ( selectedCircle != null )
        {
            selectedCircle.CurrentState = CardCircle.State.Choose;
        }
    }


}
