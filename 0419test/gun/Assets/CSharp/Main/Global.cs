/*EZ It's an abbreviation of my daughter's name -- daiyizhuo create by daiqixiang 2019 - 5 - */
using EZ.DataMgr;
using EZ.Util;
using System;
using System.Collections;
using UnityEngine;
using Game;
namespace EZ
{
    public class Global : MonoBehaviour
    {
        public bool log;
        public static App gApp = new App();

        private double m_LeaveFocusMills = 0f;

        private bool isGameStart = false;
        private const Int64 GAME_ID = 1055;

        public static bool m_SDK = false;

        void Awake()
        {
            DontDestroyOnLoad(gameObject.transform.parent.gameObject);
            GameConstVal.LasserMask = (1 << GameConstVal.StaticMapLayer) | (1 << GameConstVal.MonsterLayer) | (1 << GameConstVal.CrossMonsterLayer) | (1 << GameConstVal.FlyMonsterLayer) | (1 << GameConstVal.BossLayer);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Debug.unityLogger.logEnabled = log;

            Debug.Log("InitAll");
            InitAll();

            Login();
        }

        private void InitAll()
        {
            gApp.Awake(gameObject.transform.parent.gameObject);
            InitSingletons();
        }

        private void InitSingletons()
        {
            new SingletonMgr();

            #region Add
            SingletonMgr.singleton.AddSingleton(new ResLoadMgr());

            SingletonMgr.singleton.AddSingleton(new ResourceMgr());

            //SingletonMgr.singleton.AddSingleton(new SceneMgr());

            SingletonMgr.singleton.AddSingleton(new TableMgr());

            SingletonMgr.singleton.AddSingleton(new Game.UI.UIMgr());

            SingletonMgr.singleton.AddSingleton(new PlayerDataMgr());

            SingletonMgr.singleton.AddSingleton(new LanguageMgr());

            SingletonMgr.singleton.AddSingleton(new GameGoodsMgr());

            SingletonMgr.singleton.AddSingleton(new CampTaskMgr());

            //SingletonMgr.singleton.AddSingleton(new WantedTaskMgr());

            SingletonMgr.singleton.AddSingleton(new TipsMgr());

            //SingletonMgr.singleton.AddSingleton(new GuideMgr());

            //SingletonMgr.singleton.AddSingleton(new StatsMgr());

            //SingletonMgr.singleton.AddSingleton(new AdsMgr());

            SingletonMgr.singleton.AddSingleton(new InternetMgr());

            SingletonMgr.singleton.AddSingleton(new DateTimeMgr());


            //SingletonMgr.singleton.AddSingleton(new PlayerRightsMgr());


            //SingletonMgr.singleton.AddSingleton(new FileListMgr());

            SingletonMgr.singleton.AddSingleton(new ServerMgr());

            SingletonMgr.singleton.AddSingleton(new PurchaseMgr());

            //SingletonMgr.singleton.AddSingleton(new ShowBufferMgr());

            SingletonMgr.singleton.AddSingleton(new CampsiteMgr());

            SingletonMgr.singleton.AddSingleton(new IdleRewardMgr());

            SingletonMgr.singleton.AddSingleton(new ShopMgr());

            SingletonMgr.singleton.AddSingleton(new DrawBoxMgr());

            SingletonMgr.singleton.AddSingleton(new DialogueMgr());
            #endregion

            #region Init
            TableMgr.singleton.LoadTables(TableMgr.singleton.CollectTables());

            LanguageMgr.singleton.Init();
            LanguageMgr.singleton.ManualSetLanguage(LanguageMgr.LANGUAGE_CHINESE);

            PlayerDataMgr.singleton.Init();

            //WantedTaskMgr.singleton.Init();

            //StatsMgr.singleton.Init();

            //AdsMgr.singleton.Init();

            InternetMgr.singleton.Init();

            DateTimeMgr.singleton.Init();
            
            CampsiteMgr.singleton.Init();

            //PlayerRightsMgr.singleton.Init();

            //FileListMgr.singleton.Init();

            ServerMgr.singleton.Init(GAME_ID);

            PurchaseMgr.singleton.Init();

            //ShowBufferMgr.singleton.Init();
            IdleRewardMgr.singleton.Init();

            DrawBoxMgr.singleton.Init();

            CampTaskMgr.singleton.Init();
            #endregion
        }

        private void Login()
        {
            ServerMgr.singleton.Login(OnLogin);
        }

        private void OnLogin(bool success)
        {
            if (success)
            {
                PurchaseMgr.singleton.OnLoginComplete();
                ServerMgr.singleton.RequestGetResource(null);
                if (!DateTimeMgr.singleton.HasFixedServer)
                {
                    DateTimeMgr.singleton.onLocalTimeFixed += OnLocalTimeFixed;
                    DateTimeMgr.singleton.FixedLocalTime();
                }
                else
                {
                    StartGame();
                }
            }
            else
            {
                //弹窗？
                Login();
            }
        }

        private void OnLocalTimeFixed(bool success)
        {
            if (success)
            {
                DateTimeMgr.singleton.onLocalTimeFixed -= OnLocalTimeFixed;
                StartGame();
            }
            else
            {
                //弹窗
                Debug.Log("你没网玩个P");
            }

        }

        private void SingletonsOnStart()
        {
            CampsiteMgr.singleton.OnGameStart();
        }

        private void StartGame()
        {
            SingletonsOnStart();
            gApp.Start();
            isGameStart = true;
        }

        public void EndGame()
        {
            isGameStart = false;
        }

        private void UpdateSingletons()
        {
            if (Game.UI.UIMgr.singleton != null)
                Game.UI.UIMgr.singleton.Update();
            if (PlayerDataMgr.singleton != null)
                PlayerDataMgr.singleton.Update();
            //if (GuideMgr.singleton != null)
            //    GuideMgr.singleton.Update();
            //if (SceneMgr.singleton != null)
            //    SceneMgr.singleton.Update();
            if (DateTimeMgr.singleton != null)
                DateTimeMgr.singleton.Update();
            //if (ShowBufferMgr.singleton != null)
            //    ShowBufferMgr.singleton.Update();
            if (CampsiteMgr.singleton != null)
                CampsiteMgr.singleton.Update();
            if (IdleRewardMgr.singleton != null)
                IdleRewardMgr.singleton.Update();
        }

        private void Update()
        {
            if (InternetMgr.singleton != null)
                InternetMgr.singleton.Update();

            if (!isGameStart)
                return;

            UpdateSingletons();

            float dtTime = Time.deltaTime;
            if (gApp != null)
            {
                gApp.Update(dtTime);
            }
        }

        private void ClearSingletons()
        {
            SingletonMgr.singleton.ClearAddReleases();

            if (SingletonMgr.singleton != null)
                SingletonMgr.singleton.ClearSingleton();
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }

        private void OnDestroy()
        {
            ClearSingletons();

            //Debug.Log("gApp != null  ====  " + (gApp != null));
            //Debug.Log("gApp != null && gApp.gSystemMgr != null  ====  " + (gApp != null && gApp.gSystemMgr != null));
            //Debug.Log("gApp != null && gApp.gSystemMgr != null && gApp.gSystemMgr.GetMiscMgr().m_DeleteData == 0  ====  " + (gApp != null && gApp.gSystemMgr != null && gApp.gSystemMgr.GetMiscMgr().m_DeleteData == 0));
            if (gApp != null && gApp.gSystemMgr != null && gApp.gSystemMgr.GetMiscMgr().m_DeleteData == 0)
            {
                gApp.gSystemMgr.GetMiscMgr().FreshMicInfo();
                ELKLogMgr.GetInstance().SendELKLog4Destroy();
            }

            //SDK
            ////SDKDSAnalyticsManager.stopTrack();

#if (UNITY_EDITOR)
            if (gApp != null)
            {
                gApp.gGameData = null;
            }
            Resources.UnloadUnusedAssets();
#endif

        }

        private void SingletonsOnPause(bool pause)
        {
            if (CampsiteMgr.singleton != null)
                CampsiteMgr.singleton.OnApplicationPause(pause);
        }

        private void OnApplicationPause(bool pause)
        {
            SingletonsOnPause(pause);
            gApp.OnApplicationPause(pause);
        }

        private void OnApplicationFocus(bool focus)
        {
            gApp.OnApplicationFocus(focus);


            //Debug.Log("OnApplicationFocus = " + focus + ", m_LeaveFocusMills = " + m_LeaveFocusMills);
            if (focus)
            {
                if (m_LeaveFocusMills != 0f && DateTimeUtil.GetMills(DateTime.Now) - m_LeaveFocusMills > DateTimeUtil.m_Hour_Secs * DateTimeUtil.m_Sec_Mills)
                {
                    CheckTimeRight();
                }

            }
            else
            {
                m_LeaveFocusMills = DateTimeUtil.GetMills(DateTime.Now);
            }
        }

        private void CheckTimeRight()
        {
            m_LeaveFocusMills = 0f;
            gApp.ResetData();
            gApp.gGameCtrl.m_GameGuideMgr.ClearAll();
            if (gApp.CurScene is MainScene)
            {
                gApp.gUiMgr.ClossAllPanel(true);
            }

            gameObject.AddComponent<DelayCallBack>().SetAction(() =>
            {
                gApp.gGameCtrl.ChangeToMainScene(1);
            }, 1f);
        }
    }
}
