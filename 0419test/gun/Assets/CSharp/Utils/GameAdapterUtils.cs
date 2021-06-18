using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public class GameAdapterUtils 
    {

        public static float DesignWidth = 1080;
        public static float DesignHeight = 1920;
        public static float MatchWidthOrHeight = 0;
        public static float RealHeight = UnityEngine.Screen.height;
        public static float RealWidth = UnityEngine.Screen.width;

        private static float m_CurAspect = 1.0f * UnityEngine.Screen.height / UnityEngine.Screen.width;
        private static float m_MinAspect = 16.0f / 9.0f;
        private static float m_MaxAspect = 18.0f / 9.0f;
        public GameAdapterUtils()
        {
            AdaptKeepCamera();
        }
        public void Update()
        {
#if (UNITY_EDITOR)
            float curAspect = 1.0f * UnityEngine.Screen.height / UnityEngine.Screen.width;
            if (m_CurAspect == curAspect)
            {
                return;
            }
            m_CurAspect = curAspect;
            AdaptKeepCamera();
            AdaptEditorAllCanvas();
#endif
            GL.Clear(false, true, Color.black);
        }
        private void AdaptEditorAllCanvas()
        {
            if (Global.gApp.gUiMgr.GetRootNodeTsf() != null)
            {
                Canvas[] allCanvas = Global.gApp.gUiMgr.GetRootNodeTsf().GetComponentsInChildren<Canvas>();
                float matchVal = m_CurAspect < m_MinAspect ? 1 : 0;
                foreach (Canvas canvas in allCanvas)
                {
                    CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
                    canvasScaler.matchWidthOrHeight = matchVal;
                    MatchWidthOrHeight = matchVal;
                }
            }
        }
        private void AdaptKeepCamera()
        {
            AdaptCamera(ref Global.gApp.gUICameraCmpt);
        }
        private Rect GetAdaptCameraInfo()
        {
            if(m_CurAspect < m_MinAspect)
            {

                float width = m_CurAspect / m_MinAspect;
                float offerX = (1 - width) / 2;
                float height = 1;
                float offferY = 0;
                return new Rect(offerX, offferY, width, height);
            }
            else if(m_CurAspect > m_MaxAspect)
            {
                float offerX = 0;
                float width = 1;
                float height = m_MaxAspect / m_CurAspect;
                float offferY = (1 - height) / 2;
                return new Rect(offerX, offferY, width, height);
            }
            else
            {
                return new Rect(0, 0, 1, 1);    
            }
        }
        public void AdaptCamera(ref Camera camera)
        {
            Rect rect = GetAdaptCameraInfo();
            camera.rect = rect;
            RealHeight = Screen.height * rect.height;
            RealWidth = Screen.width * rect.width;
        }
        public static void AdaptCanvas(Canvas canvas)
        {
            if(m_CurAspect < m_MinAspect)
            {
                CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
                canvasScaler.matchWidthOrHeight = 1;
                MatchWidthOrHeight = 1;
            }
        }
    }
}
