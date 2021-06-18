namespace EZ
{
    public class FrameCtrl
    {
        protected BaseScene m_Scene;
        protected  RenderMgr m_RenderMgr;
        public float m_WorldTime = 0;
        public FrameCtrl(BaseScene scene,RenderMgr renderMgr = null)
        {
            m_Scene = scene;
            m_RenderMgr = renderMgr; 
        }
        public void Init()
        {
            m_Scene.Init();
            if (m_RenderMgr != null )
            {
                m_RenderMgr.Init();
            }
        }
        public BaseScene GetScene()
        {
            return m_Scene;
        }
        public RenderMgr GetRenderMgr()
        {
            return m_RenderMgr;
        }

        public float GetWroldTime()
        {
            return m_WorldTime;
        }

        public virtual void OnDestroy()
        {
            m_Scene.OnDestroy();
            if (m_RenderMgr != null)
            {
                m_RenderMgr.OnDestroy();
            }
        }
        public virtual void Update(float dt)
        {
            if (m_WorldTime > 0)
            {
                m_WorldTime = m_WorldTime + dt;
                m_Scene.Update(dt);
            }
            else
            {
                m_WorldTime = m_WorldTime + dt;
            }
        }
    }
}
