using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;

namespace EZ
{
    public partial class HomeUI
    {
        Camera m_MainCamera;
        private GameObject m_ShowNode;
        private GameObject m_ControllerNode;
        private Transform m_FightNode;
        private InputModule clickChecker;
        private List<CampsitePointBubble> campsiteBubbles = new List<CampsitePointBubble>();

        private bool isTouchDown;
        private Vector2 touchDownPos;

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            if (m_ControllerNode != null)
            {
                m_ControllerNode.SetActive(true);
            }
            string param = arg as string;
            if (param == "expedition")
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.ExpeditionUI);
            }

            Global.gApp.gUiMgr.OpenPanel(Wndid.TaskUI);

            foreach (var node in campsiteBubbles)
            {
                node.gameObject.SetActive(true);
            }

            CampsiteClaimBtn.gameObject.SetActive(CampsiteMgr.singleton.CanClaimAll());

            CampsiteBubbleRoot.gameObject.SetActive(PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.BuildInteraction));
            ExpeditionBtn.gameObject.SetActive(PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.Expedition));
            RegisterListeners();
        }

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();

            DebugBtn.button.onClick.AddListener(OnDebugBtn);
            ClearDataBtn.button.onClick.AddListener(OnClearDataBtn);
            CampsiteClaimBtn.button.onClick.AddListener(CampsiteMgr.singleton.ClaimAllReward);
            ExpeditionBtn.button.onClick.AddListener(OnExpeditionBtnClick);

            m_ShowNode = GameObject.Find("SceneRoot");
            m_ControllerNode = m_ShowNode.transform.Find("ControllerNode").gameObject;
            m_ControllerNode.SetActive(true);
            m_FightNode = m_ControllerNode.transform.Find("FightNode");
            m_FightNode.gameObject.SetActive(true);
            m_MainCamera = Camera.main;
            Global.gApp.gGameAdapter.AdaptCamera(ref m_MainCamera);

            CampsiteObjectMgr.Instance.InitCampsiteObjects(m_FightNode.gameObject);
            var buildings = CampsiteObjectMgr.Instance.GetAllBuildings();
            for (int i = 0; i < buildings.Length; i++)
            {
                var building = buildings[i];
                var bubbleGo = Global.gApp.gResMgr.InstantiateObj("Prefabs/UI/CampsitePointBubble");
                bubbleGo.transform.SetParent(CampsiteBubbleRoot.rectTransform, false);
                var bubble = bubbleGo.GetComponent<CampsitePointBubble>();
                bubble.Init(building.PointDataIndex, building.BubbleNode);
                campsiteBubbles.Add(bubble);
            }

            double dt; double offlineReward;
            if (CampsiteMgr.singleton.CheckOfflineReward(out dt, out offlineReward))
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.CampsiteOfflineRewardUI);
            }

            InputDataDeal_Single inputDeal = new InputDataDeal_Single();
            inputDeal.funcOnTouchDown += CheckTouchDown;
            inputDeal.funcOnTouchUp += CheckClick;
            inputDeal.funcOnTouchDown += CheckCamCtrlValid;
            inputDeal.funcOnTouch += CheckCamCtrlValid;
            clickChecker = new InputModule(inputDeal);
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener<GameModuleType>(MsgIds.ModuleOpen, OnModuleOpen);
        }

        private void UnregisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<GameModuleType>(MsgIds.ModuleOpen, OnModuleOpen);
        }

        public void HideBtns()
        {
            BtnsRoot.gameObject.SetActive(false);
        }

        public void ResetBtns()
        {
            BtnsRoot.gameObject.SetActive(true);
        }
        
        public void OnFocusPoint(int index)
        {
            for (int i = 0; i < campsiteBubbles.Count; i++)
            {
                campsiteBubbles[i].gameObject.SetActive(campsiteBubbles[i].DataIndex == index);
            }
        }

        public void OnCancelFocusPoint()
        {
            for (int i = 0; i < campsiteBubbles.Count; i++)
            {
                campsiteBubbles[i].gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            clickChecker.Update(BaseScene.GetDtTime());
        }

        private void CheckCamCtrlValid(InputData input)
        {
            if (input.isInUI)
                CampsiteObjectMgr.Instance.camTouchInputCtrl.IsInputOnLockedArea = true;
        }

        private void CheckTouchDown(InputData input)
        {
            if (input.isInUI)
                return;


            isTouchDown = true;
            touchDownPos = input.screenPos;
        }

        private void CheckClick(InputData input)
        {
            if (!isTouchDown)
                return;
            isTouchDown = false;

            if (input.isInUI)
                return;

            if (Global.gApp.gUiMgr.CheckPanelExit(Wndid.CampsitePointUI))
            {
                Global.gApp.gUiMgr.ClosePanel(Wndid.CampsitePointUI);
            }
            else
            {
                if (PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.BuildInteraction))
                    return;

                Vector2 delta = input.screenPos - touchDownPos;
                delta.x /= Screen.width;
                delta.y /= Screen.height;

                if (Mathf.Abs(delta.x) > 0.01f || Mathf.Abs(delta.y) > 0.01f)//避免滑屏冲突
                    return;

                Vector3 screenPosition = input.screenPos;
                {
                    Ray ray = Camera.main.ScreenPointToRay(screenPosition);
                    RaycastHit hit;
                    bool isHit = Physics.Raycast(ray, out hit, 10000, 1 << GameConstVal.MainRoleLayer);
                    if (isHit)
                    {

                        if (hit.collider.gameObject.CompareTag(GameConstVal.CampBuilding))
                        {
                            CampsiteBuildingPoint point = hit.collider.gameObject.GetComponentInParent<CampsiteBuildingPoint>();
                            point.RespondClick();
                        }
                    }
                }
            }
        }

        private void OnDebugBtn()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.DebugPanel);
        }

        private void OnClearDataBtn()
        {
            var global = GameObject.Find("Global").GetComponent<Global>();
            global.EndGame();
            PlayerDataMgr.singleton.DeleteSaveData();
            Application.Quit();
        }

        private void OnExpeditionBtnClick()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.ExpeditionUI);
        }

        public override void Recycle()
        {
            base.Recycle();

            Global.gApp.gUiMgr.ClosePanel(Wndid.ExpeditionUI);
            Global.gApp.gUiMgr.ClosePanel(Wndid.TaskUI);
            Global.gApp.gUiMgr.ClosePanel(Wndid.CampsitePointUI);

            if (m_ControllerNode != null)
            {

                foreach (var node in campsiteBubbles)
                {
                    node.gameObject.SetActive(false);
                }
                m_ControllerNode.SetActive(false);
            }

            UnregisterListeners();
        }

        public override void Release()
        {
            base.Release();
            CampsiteObjectMgr.Instance.Relese();
            Debug.Log("Home UI Relese");

            if (m_ControllerNode != null)
            {
                m_ControllerNode.SetActive(false);
            }

            UnregisterListeners();
        }

        private void OnModuleOpen(GameModuleType module)
        {
            switch (module)
            {
                case GameModuleType.BuildInteraction:
                    CampsiteBubbleRoot.gameObject.SetActive(true);
                    break;
                case GameModuleType.Expedition:
                    ExpeditionBtn.gameObject.SetActive(true);
                    break;
            }
        }
    }
}