namespace EZ
{
    public class MainFrameCtrl : FrameCtrl
    {
        public MainFrameCtrl(BaseScene scene,RenderMgr renderMgr = null):base(scene,renderMgr )
        {
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
