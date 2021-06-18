using EZ.Data;
using EZ.DataMgr;
using Game;
using System;
using UnityEngine;

namespace EZ
{
    public class App
    {
        public GameDatas gGameData;
        public SystemMgr gSystemMgr;
        public ResMgr gResMgr;
        public GameCtrl gGameCtrl;
        public UiMgr gUiMgr;
        public MsgDispatcher gMsgDispatcher;
        public TimerMgr gTimerMgr;
        public GameAdapterUtils gGameAdapter;

        public GameObject gUICamera;
        public Camera gUICameraCmpt;
        public GameObject gKeepNode;
        public GameObject gRoleNode;
        public GameObject gMapNode;
        public AutoCam gCamCompt;
        public ShakecameraControl gShakeCompt;
        public GameObject gBulletNode;
        public AudioSourceController gAudioSource;
        public MainRoleUICtrl gMainRoleUI;

        private BaseScene m_CurScene;

        public Light gLightCompt;
        public BaseScene CurScene
        {
            get { return m_CurScene; }
            set { m_CurScene = value; }
        }

        public void Awake(GameObject keepNode)
        {
            Debug.Log("InitNode");
            InitNode(keepNode);
            Debug.Log("InitOther");
            InitOther();

            //CheckTimeRight();

        }

        public void Start()
        {
            CheckTimeRight();
        }

        private void CheckTimeRight()
        {
            Debug.Log("Run");

            Run();
        }

        public void SetShadowMaskOpenState(bool enable)
        {
            if (enable)
            {
                gLightCompt.cullingMask = GameConstVal.ShadowCullingMask;
            }
            else
            {
                gLightCompt.cullingMask = GameConstVal.NoShadowCullingMask;
            }
        }
        private void InitOther()
        {
            gGameData = new GameDatas();
            gMsgDispatcher = new MsgDispatcher();
            gSystemMgr = new SystemMgr();
            gResMgr = new ResEditorMgr();
            gUiMgr = new UiMgr();
            gGameCtrl = new GameCtrl();
            gTimerMgr = new TimerMgr();
            gGameAdapter = new GameAdapterUtils();
        }

        public void ClearData()
        {
            gSystemMgr.ClearData();
            gSystemMgr.AfterInit();
        }

        public void ResetData()
        {
            gSystemMgr.ResetData();
        }

        private void InitNode(GameObject keepNode)
        {
            gKeepNode = keepNode;
            gRoleNode = keepNode.transform.Find("RoleNode").gameObject;
            gCamCompt = keepNode.transform.Find("CameraNode/ShakeNode/CameraCtrlNode").GetComponent<AutoCam>();
            gShakeCompt = keepNode.transform.Find("CameraNode/ShakeNode").GetComponent<ShakecameraControl>();
            gUICamera = keepNode.transform.Find("CameraNode/UICamera").gameObject;
            gUICameraCmpt = gUICamera.GetComponent<Camera>();
            gMapNode = keepNode.transform.Find("MapNode").gameObject;
            gBulletNode = keepNode.transform.Find("BulletNode").gameObject;
            gAudioSource = keepNode.transform.Find("Global").GetComponent<AudioSourceController>();
            gLightCompt = keepNode.transform.Find("DireLight").GetComponent<Light>();
            gMainRoleUI = keepNode.transform.GetComponentInChildren<MainRoleUICtrl>(true);
        }

        public void Run()
        {
            if (PlayerDataMgr.singleton.IsFirstLogin)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.OpeningUI, CampsiteMgr.singleton.Id.ToString());
                var ui = Global.gApp.gUiMgr.GetPanelCompent<OpeningUI>(Wndid.OpeningUI);
                ui.SetAciton(() => { gGameCtrl.ChangeToMainScene(1, true); });
            }
            else
                gGameCtrl.ChangeToMainScene(1);
        }
        public void Update(float dt)
        {
            if (gGameAdapter != null)
            {
                gGameAdapter.Update();
            }
            if (gGameCtrl != null)
            {
                gGameCtrl.Update(dt);
            }
            if (gTimerMgr != null)
            {
                gTimerMgr.Update(dt);
            }
            if (gUiMgr != null)
            {
                gUiMgr.Update();
            }
        }

        public void OnApplicationPause(bool isPause)
        {

        }


        public void OnApplicationFocus(bool isFocus)
        {

        }
    }
}

