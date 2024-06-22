using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;

    [SerializeField]
    private FloatingText floatingTextPrefab;

    [SerializeField]
    private Transform floatingTextParent;

    private void Awake( )
    {
        Instance = this;
    }

    public void CreateFloatingText( string text , Color color , Vector3 position , Vector3 direction )
    {
        if ( floatingTextPrefab == null )
        {
            Debug.LogError ( "FloatingTextManager: floatingTextPrefab is null" );
            return;
        }

        FloatingText floatingTextInstance = Instantiate ( floatingTextPrefab , position , Quaternion.identity , floatingTextParent );
        floatingTextInstance.Initialize ( text , color , direction );
    }
}
