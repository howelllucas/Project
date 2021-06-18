using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Game.Scenes;
using System.IO;

namespace Game
{
    public class GameMain : MonoBehaviour, Game.ISingleton
    {
        public bool log;
        private static GameMain s_singleton;
        public static GameMain singleton { get { return s_singleton; } }

        public void ClearSingleton()
        {
            s_singleton = null;
        }

        public enum PauseType
        {
            Direct,
            Accumulated,
        }

        private int isPause = 0;
        private PauseType pauseType = PauseType.Direct;
        public bool IsPause
        {
            get { return isPause > 0; }
            set
            {
                switch (pauseType)
                {
                    case PauseType.Direct:
                        isPause = value ? 1 : 0;
                        break;
                    case PauseType.Accumulated:
                        int dir = value ? 1 : -1;
                        isPause += dir;
                        break;
                }
                Time.timeScale = isPause > 0 ? 0 : 1;
            }
        }
        public void SetPauseType(PauseType type, bool isPause)
        {
            pauseType = type;
            this.isPause = isPause ? 1 : 0;
            Time.timeScale = isPause ? 0 : 1;
        }

        public bool IsGameStart { get; private set; }

        public int ABTestVal { get; private set; }

        void Start()
        {
            s_singleton = this;
            new SingletonMgr();

            //DontDestroyOnLoad(gameObject);

            //Application.targetFrameRate = 60;

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            SingletonMgr.singleton.AddSingleton(new ResLoadMgr());

            SingletonMgr.singleton.AddSingleton(new ResourceMgr());

            //SingletonMgr.singleton.AddSingleton(new SceneMgr());

            SingletonMgr.singleton.AddSingleton(new TableMgr());

            SingletonMgr.singleton.AddSingleton(new UI.UIMgr());

            SingletonMgr.singleton.AddSingleton(new PlayerDataMgr());

            SingletonMgr.singleton.AddSingleton(new LanguageMgr());

            //SingletonMgr.singleton.AddSingleton(new GameGoodsMgr());

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

            //SingletonMgr.singleton.AddSingleton(new ServerMgr());

            //SingletonMgr.singleton.AddSingleton(new PurchaseMgr());

            //SingletonMgr.singleton.AddSingleton(new ShowBufferMgr());

            SingletonMgr.singleton.AddSingleton(new CampsiteMgr());

            SingletonMgr.singleton.AddSingleton(new DialogueMgr());

            Init();
        }


        public void Clear()
        {
            SingletonMgr.singleton.ClearAddReleases();

            if (GameMain.singleton != null)
            {
                Destroy(GameMain.singleton);
                GameMain.singleton.ClearSingleton();
            }
            if (SingletonMgr.singleton != null)
                SingletonMgr.singleton.ClearSingleton();
            GC.Collect();
            Resources.UnloadUnusedAssets();
            Destroy(gameObject);
        }
        void OnApplicationQuit()
        {

            Debug.Log("exit");
            Clear();

        }

        public void Init()
        {
//#if UNITY_ANDROID
//            ABTestVal = SdkdsNativeUtil.Instance.getABTestCRC32Type() == 0 ? 0 : 1;
//#else
//            ABTestVal = SdkdsNativeUtil.Instance.getABTestType();
//#endif

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

            //ServerMgr.singleton.Init();

            //PurchaseMgr.singleton.Init();

            //ShowBufferMgr.singleton.Init();

            CampTaskMgr.singleton.Init();

            Debug.Log("game init end");

            StartGame();


        }

        public void OnLoginComplete()
        {
            //PurchaseMgr.singleton.OnLoginComplete();
            StartGame();
        }

        public void StartGame()
        {
            Debug.unityLogger.logEnabled = log;
            IsGameStart = true;

            EZ.Global.gApp.Run();
            //UI.UIMgr.singleton.Open("LoginUI");
            //StatsMgr.singleton.ReportEvent("app_open");
            //StatsMgr.singleton.ReportEvent("app_resume");
#if GAME_DEBUG
            //UnityEngine.SceneManagement.SceneManager.LoadScene("CityScene");
            //SceneMgr.singleton.CreateCity();
#else
            //if (PlayerDataMgr.singleton.GetCurGuideStageIndex() == 1)
            //{
            //    //PlayerDataMgr.singleton.NewGuideID = 1;
            //    //PlayerDataMgr.singleton.EnterStage(0);
            //    //SceneMgr.singleton.CreateStage(0);

            //    SceneMgr.singleton.CreateGuideScene(1);
            //}
            //else
            //{
            //    //PlayerDataMgr.singleton.NewGuideID = 11;
            //    SceneMgr.singleton.CreateCity();
            //}

#endif
        }

        // Update is called once per frame
        void Update()
        {
            //if (!IsGameStart)
            //    return;

            if (UI.UIMgr.singleton != null)
                UI.UIMgr.singleton.Update();
            if (PlayerDataMgr.singleton != null)
                PlayerDataMgr.singleton.Update();
            //if (GuideMgr.singleton != null)
            //    GuideMgr.singleton.Update();
            //if (SceneMgr.singleton != null)
            //    SceneMgr.singleton.Update();
            if (InternetMgr.singleton != null)
                InternetMgr.singleton.Update();
            if (DateTimeMgr.singleton != null)
                DateTimeMgr.singleton.Update();
            //if (ShowBufferMgr.singleton != null)
            //    ShowBufferMgr.singleton.Update();
            if (CampsiteMgr.singleton != null)
                CampsiteMgr.singleton.Update();
        }

        void OnApplicationPause(bool isPause)
        {
            //Debug.Log("Game Pause:" + isPause);
            //SceneMgr.singleton?.OnApplicationPause(isPause);
            //UI.UIMgr.singleton?.BoradCast(UIEventType.PAUSE_STATE, isPause);

            //if (!isPause)
            //    StatsMgr.singleton.ReportEvent("app_resume");
            //else
            //    StatsMgr.singleton.ReportEvent("app_pause");
        }
    }
}