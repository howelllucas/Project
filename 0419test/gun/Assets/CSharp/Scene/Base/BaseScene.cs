using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ
{
    public enum SceneType
    {
        NormalScene = 1,
        CarScene = 2,
        BreakOutSene = 3,
    }
    public abstract class BaseScene
    {
        public static float TimeScale = 1;

        protected Camera m_MainCameraCmpt;
        protected GameObject m_MainCamera;
        protected int m_PauseRef = 0;
        public static float GetDtTime()
        {
            return Time.deltaTime * TimeScale;
        }
        public virtual void Init()
        {
            Global.gApp.CurScene = this;
            TimeScale = 1;
            m_PauseRef = 0;
        }
        protected void InitCamera(string path)
        {
            m_MainCamera = Global.gApp.gResMgr.InstantiateObj(path);
            m_MainCameraCmpt = m_MainCamera.GetComponent<Camera>();
            Camera[] cameras = m_MainCamera.gameObject.GetComponentsInChildren<Camera>();
            for (int i = 0; i< cameras.Length;i++)
            {
                Camera camera = cameras[i];
                Global.gApp.gGameAdapter.AdaptCamera(ref camera);
            }
            Transform animNode = Global.gApp.gKeepNode.transform.Find("CameraNode/ShakeNode/CameraCtrlNode/AnimNode");
            m_MainCamera.transform.SetParent(animNode, false);
        }
        public GameObject GetCameraObj()
        {
            return m_MainCamera;
        }
        public Camera GetCameraCmpt()
        {
            return m_MainCameraCmpt;
        }
        public virtual SceneType GetSceneType()
        {
            return SceneType.NormalScene;
        }
        public virtual bool IsNormalPass()
        {
            return false;
        }
        public virtual void Update(float dt)
        {

        }
        public virtual Player GetMainPlayerComp()
        {
            return null;
        }
        public virtual GameObject GetMainPlayer()
        {
            return null;
        }
        public GunsPass_dataItem GetGunPassDataItem()
        {
            PassItem passData = GetPassData();
            if ((PassType)passData.passType == PassType.MainPass)
            {
                int curPassId = passData.id % 100000;
                GunsPass_dataItem weaponLevelData = Global.gApp.gGameData.GunPassDataConfig.Get(curPassId);
                return weaponLevelData;
            }
            else
            {
                PassItem passItem = Global.gApp.gSystemMgr.GetPassMgr().GetPassItem();
                int curPassId = passItem.id % 100000;
                GunsPass_dataItem weaponLevelData = Global.gApp.gGameData.GunPassDataConfig.Get(curPassId);
                return weaponLevelData;
            }
        }
        public virtual PassItem GetPassData()
        {
            return null;
        }
        public virtual PropMgr GetPropMgr()
        {
            return null;
        }
        public virtual WaveMgr GetWaveMgr()
        {
            return null;
        }
        public virtual PlayerMgr GetPlayerMgr()
        {
            return null;
        }

        public virtual void Resume()
        {
            m_PauseRef --;
            if (m_PauseRef == 0)
            {
                TimeScale = 1;
            }
        }
        public virtual void Pause()
        {
            m_PauseRef++;
            TimeScale = 0;
        }

        public virtual Map GetMap()
        {
            return null;
        }
        public virtual void GameLose()
        {

        }
        public virtual void GameWin()
        {
        }
        public virtual TimerMgr GetTimerMgr()
        {
            return null;
        }
        public virtual void OnDestroy()
        {
            Object.Destroy(m_MainCamera);
        }
        public virtual GameWinType GetGamePlayType()
        {
            return default(GameWinType);
        }
    }
}
