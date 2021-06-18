using EZ.Data;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EZ
{
    public class GameCtrl
    {
        private FrameCtrl m_FrameCtrl = null;
        private TimerMgr m_TimerMgr;
        private TouchMask m_GlobalMask;
        private GameTipsMgr m_GameTipsMgr;
        public GameGuideMgr m_GameGuideMgr;

        public BulletCacheMgr BulletCache
        {
            get;
            set;
        }

        public EffectCacheMgr EffectCache
        {
            get;
            set;
        }

        public GameCtrl()
        {
            m_TimerMgr = new TimerMgr();
            m_GameTipsMgr = new GameTipsMgr();
            m_GameGuideMgr = new GameGuideMgr();
            EffectCache = new EffectCacheMgr();
            BulletCache = new BulletCacheMgr();
        }
        public void Update(float dt)
        {
            if (m_FrameCtrl != null)
            {
                m_FrameCtrl.Update(dt);
            }
            m_TimerMgr.Update(dt);
        }


        public void ChangeToFightScene(int levelId)
        {
            var levelRes = TableMgr.singleton.LevelTable.GetItemByID(levelId);
            if (levelRes == null)
                return;
            int passId = levelRes.passID;

            Application.targetFrameRate = 30;
            Global.gApp.gMsgDispatcher.Cleanup();
            TipsMgr.singleton.Clear();

            //PassData passData = Global.gApp.gGameData.GetPassData().First(l => l.id == passId);
            PassItem passData = Global.gApp.gGameData.PassData.Get(passId);
            var op = SceneManager.LoadSceneAsync(passData.scene);
            op.completed += (a) =>
            {
                Debug.Log("ChangeToFightScene");
                if ((SceneType)passData.sceneType == SceneType.NormalScene)
                {
                    ChangeToNormalFightScene(passData);
                }
                else
                {
                    ChangeToNormalFightScene(passData);
                }
            };
            //SceneManager.LoadScene(passData.scene);

        }
        public void ChangeToNormalFightScene(PassItem passData)
        {
            BaseScene scene = new FightScene(passData);
            FrameCtrl frameCtrl = new FightFrameCtrl(scene);
            ChangeCtrl(frameCtrl);
        }

        public void ChangeToCarFightScene(PassItem passData)
        {
            BaseScene scene = new FightScene(passData);
            FrameCtrl frameCtrl = new FightFrameCtrl(scene);
            ChangeCtrl(frameCtrl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from">1-logo 2-fight 3-campfight</param>
        public void ChangeToMainScene(int from, bool isSwitch = false)
        {
            Application.targetFrameRate = 60;
            Global.gApp.gMsgDispatcher.Cleanup();
            TipsMgr.singleton.Clear();
            //SceneManager.LoadScene(CampsiteMgr.singleton.SceneName);
            var op = SceneManager.LoadSceneAsync(CampsiteMgr.singleton.SceneName);
            MainScene scene = new MainScene();
            FrameCtrl frameCtrl = new MainFrameCtrl(scene);
            ChangeCtrl(frameCtrl);
            op.completed += (a) =>
            {

                Debug.Log("CacheAndOpenUI");
                scene.CacheAndOpenUI(from);
                if (isSwitch)
                    Global.gApp.gUiMgr.OpenPanel(Wndid.CampSwitchUI, CampsiteMgr.singleton.Id.ToString());
            };

        }

        public void ChangeToCampsiteFightScene(int levelId)
        {
            Debug.Log("Enter Camp Fight " + levelId);
            var levelRes = TableMgr.singleton.LevelTable.GetItemByID(levelId);
            if (levelRes == null)
                return;
            int passId = levelRes.passID;
            PassItem passData = Global.gApp.gGameData.PassData.Get(passId);
            if (passData == null)
                return;
            Application.targetFrameRate = 30;
            Global.gApp.gMsgDispatcher.Cleanup();
            TipsMgr.singleton.Clear();
            PlayerDataMgr.singleton.SetStageParam(levelId, true);
            //SceneManager.LoadScene(passData.scene);
            var op = SceneManager.LoadSceneAsync(passData.scene);
            op.completed += (a) =>
            {
                Debug.Log("ChangeToCampsiteFightScene");
                BaseScene scene = new CampsiteFightScene(levelId, passData);
                FrameCtrl frameCtrl = new FightFrameCtrl(scene);
                ChangeCtrl(frameCtrl);
            };

        }

        public FrameCtrl GetFrameCtrl()
        {
            return m_FrameCtrl;
        }
        private void ChangeCtrl(FrameCtrl frameCtrl)
        {
            DestroyCurFrame();
            m_FrameCtrl = frameCtrl;
            m_FrameCtrl.Init();
        }

        private void DestroyCurFrame()
        {
            Time.timeScale = 1;
            BaseScene.TimeScale = 1;
            EffectCache.Clear();
            BulletCache.Clear();
            Global.gApp.gResMgr.UnLoadAssets();
            if (m_FrameCtrl != null)
            {
                m_FrameCtrl.OnDestroy();
                m_FrameCtrl = null;
            }
            Time.timeScale = 1;
        }
        public void RemoveGlobalTouchMask()
        {
            if (m_GlobalMask != null)
            {
                m_GlobalMask.RemoveRef();
            }
        }
        public void AddGlobalTouchMask()
        {
            if (m_GlobalMask == null)
            {
                GameObject touchMask = Global.gApp.gResMgr.InstantiateObj(EffectConfig.TouchMaskUi);
                touchMask.transform.SetParent(Global.gApp.gUiMgr.GetTopCanvasTsf(), false);
                touchMask.transform.SetAsFirstSibling();
                m_GlobalMask = touchMask.GetComponent<TouchMask>();
            }
            m_GlobalMask.AddRef();

        }
    }
}
