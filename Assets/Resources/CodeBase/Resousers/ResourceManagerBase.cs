using UnityEngine;

public abstract class ResourceManagerBase : MonoBehaviour
{
    protected PlayerResources playerResources;



    public abstract void SaveData( );

    public abstract void LoadData( );

    public abstract void Add( int amount );

    public abstract bool Spend( int amount );


    public abstract void GetCurrent( );
    public abstract int GetCurrentAmount( );
}
