public class BoostrapLoadingPanel : LoadingPanel
{
    public override void Show( ) => base.Show ();
    public override void Hide( )
    {
        base.Hide ();
        UiManager.Instance.ShowMainSceneWithAdditionalPanels ();
    }
    public override void SetProgress( float progress )
    {
        base.SetProgress ( progress );

    }
}