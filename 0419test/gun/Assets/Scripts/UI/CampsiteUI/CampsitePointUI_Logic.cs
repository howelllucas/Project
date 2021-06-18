using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using BitBenderGames;

namespace EZ
{
    public partial class CampsitePointUI
    {
        public Vector2 focusScreenPos = new Vector2(0.5f, 0.5f);
        private CampsitePointMgr pointDataMgr;
        private double lastRewardVal;
        private float lastInterval;
        private bool lastIsAuto;
        private double lastLvUpCost;

        private CanvasGroup dataChangePanelGroup;

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            LastBtn.button.onClick.AddListener(OnLastBtnClick);
            NextBtn.button.onClick.AddListener(OnNextBtnClick);
            DetailBtn.button.onClick.AddListener(OnDetailBtnClick);
            SetGunBtn.button.onClick.AddListener(OnSetGunBtnClick);
            LvUpBtn.buttonEx.onClick.AddListener(OnLvUpBtnClick);
            LvUpBtn.buttonEx.onLongPress.AddListener(OnLvUpBtnClick);
            AutoBtn.button.onClick.AddListener(OnAutoBtnClick);
            CloseBtn.button.onClick.AddListener(TouchClose);

            dataChangePanelGroup = DataChangePanel.gameObject.GetComponent<CanvasGroup>();
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            int dataIndex = int.Parse(arg as string);

            int nextIndex = CampsiteMgr.singleton.GetNextUnlockPoint(dataIndex);
            NextBtn.gameObject.SetActive(dataIndex != nextIndex);
            LastBtn.gameObject.SetActive(dataIndex != nextIndex);

            SetData(dataIndex);
            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.CampsitePointDataChange, RefreshData);
            CampsiteObjectMgr.Instance.SetFocusBuilding(dataIndex, focusScreenPos);
            CampsiteObjectMgr.Instance.camTouchInputCtrl.enabled = false;

            TaskUI taskPanel = Global.gApp.gUiMgr.GetPanelCompent<TaskUI>(Wndid.TaskUI);
            taskPanel?.HideRoot();

            HomeUI homePanel = Global.gApp.gUiMgr.GetPanelCompent<HomeUI>(Wndid.HomeUI);
            if (homePanel != null)
            {
                homePanel.HideBtns();
            }

            bool openSetGun = PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.BuildSetGun);
            SetGunBtn.gameObject.SetActive(openSetGun);
            CardFrame.gameObject.SetActive(openSetGun);
            AutoBtn.gameObject.SetActive(openSetGun);
        }

        private void SetData(int dataIndex)
        {
            pointDataMgr = CampsiteMgr.singleton.GetPointByIndex(dataIndex);
            NameTxt.text.text = pointDataMgr.buildingRes.buildingName;
            var gunTypeRes = TableMgr.singleton.GunTypeTable.GetItemByID(pointDataMgr.buildingRes.gunType);
            //TypeImg.image.sprite = gunTypeRes.icon
            TypeTxt.text.text = LanguageMgr.GetText(gunTypeRes.tid_type);
            HomeUI homePanel = Global.gApp.gUiMgr.GetPanelCompent<HomeUI>(Wndid.HomeUI);
            if (homePanel != null)
            {
                homePanel.OnFocusPoint(dataIndex);
            }
            RefreshData(dataIndex);
        }

        private void RefreshData(int dataIndex)
        {
            if (pointDataMgr != null && dataIndex != pointDataMgr.index)
                return;
            LvTxt.text.text = string.Format("Lv.{0}", pointDataMgr.Lv);
            RewardTxt.text.text = UiTools.FormateMoney(pointDataMgr.OnceRewardVal);
            IntervalTxt.text.text = LanguageMgr.GetText("CampDetail_Text_Time", pointDataMgr.RewardInterval);
            AutoTipTxt.text.text = LanguageMgr.GetText("CampDetail_Tips_Auto", pointDataMgr.AutoLv);
            if (pointDataMgr.isAuto)
            {
                UIGray.Recovery(AutoIcon.image);
                AutoTxt.text.text = LanguageMgr.GetText("CampDetail_Rule_AutoOpen");
                AutoTxt.text.color = Color.green;
                AutoTipTxt.text.color = Color.green;
                DescTxt.text.text = LanguageMgr.GetText("CampDetail_Text_Upgrade");
            }
            else
            {
                UIGray.SetUIGray(AutoIcon.image);
                AutoTxt.text.text = LanguageMgr.GetText("CampDetail_Rule_AutoClose");
                AutoTxt.text.color = Color.red;
                AutoTipTxt.text.color = Color.red;
                if (pointDataMgr.equipGunId > 0)
                {
                    DescTxt.text.text = LanguageMgr.GetText("CampDetail_Text_NOAuto");
                }
                else
                {
                    DescTxt.text.text = LanguageMgr.GetText("CampDetail_Text_NoWeapon");
                }
            }

            var lvUpCost = pointDataMgr.GetLvUpCost(1);
            CostTxt.text.text = UiTools.FormateMoney(lvUpCost);
            if (lvUpCost <= CampsiteMgr.singleton.TotalRewardVal)
            {
                CostTxt.text.color = Color.white;
            }
            else
            {
                CostTxt.text.color = Color.red;
            }

            if (pointDataMgr.equipGunId > 0)
            {
                CardData.gameObject.SetActive(true);
                CardData.GunUI_GunCard.Init(TableMgr.singleton.GunCardTable.GetItemByID(pointDataMgr.equipGunId));
                SetGunTxt.text.text = LanguageMgr.GetText("CampDetail_Button_Change");
            }
            else
            {
                CardData.gameObject.SetActive(false);
                SetGunTxt.text.text = LanguageMgr.GetText("CampDetail_Button_Equi");
            }
        }

        public override void Recycle()
        {
            base.Recycle();
            CampsiteObjectMgr.Instance.CancelFocus();
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.CampsitePointDataChange, RefreshData);
            Global.gApp.gUiMgr.ClosePanel(Wndid.CampsitePointSetGunUI);
            if (CampsiteObjectMgr.Instance.camTouchInputCtrl != null)
                CampsiteObjectMgr.Instance.camTouchInputCtrl.enabled = true;
            TaskUI taskPanel = Global.gApp.gUiMgr.GetPanelCompent<TaskUI>(Wndid.TaskUI);
            taskPanel?.ResetRoot();

            HomeUI homePanel = Global.gApp.gUiMgr.GetPanelCompent<HomeUI>(Wndid.HomeUI);
            if (homePanel != null)
            {
                homePanel.ResetBtns();
                homePanel.OnCancelFocusPoint();
            }
        }

        public override void Release()
        {
            base.Release();
            CampsiteObjectMgr.Instance.CancelFocus();
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.CampsitePointDataChange, RefreshData);
            Global.gApp.gUiMgr.ClosePanel(Wndid.CampsitePointSetGunUI);
            if (CampsiteObjectMgr.Instance.camTouchInputCtrl != null)
                CampsiteObjectMgr.Instance.camTouchInputCtrl.enabled = true;
            TaskUI taskPanel = Global.gApp.gUiMgr.GetPanelCompent<TaskUI>(Wndid.TaskUI);
            taskPanel?.ResetRoot();
            HomeUI homePanel = Global.gApp.gUiMgr.GetPanelCompent<HomeUI>(Wndid.HomeUI);
            if (homePanel != null)
            {
                homePanel.ResetBtns();
                homePanel.OnCancelFocusPoint();
            }
        }

        private void OnLastBtnClick()
        {
            if (pointDataMgr != null)
            {
                int lastIndex = CampsiteMgr.singleton.GetLastUnlockPoint(pointDataMgr.index);
                CampsiteObjectMgr.Instance.SetFocusBuilding(lastIndex, focusScreenPos, 0);
                SetData(lastIndex);
            }
        }

        private void OnNextBtnClick()
        {
            if (pointDataMgr != null)
            {
                int nextIndex = CampsiteMgr.singleton.GetNextUnlockPoint(pointDataMgr.index);
                CampsiteObjectMgr.Instance.SetFocusBuilding(nextIndex, focusScreenPos, 0);
                SetData(nextIndex);
            }
        }

        private void OnDetailBtnClick()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.CampsitePointDetailUI, pointDataMgr);
        }

        private void OnSetGunBtnClick()
        {
            if (pointDataMgr != null)
            {
                RecordPointDatas();
                Global.gApp.gUiMgr.OpenPanel(Wndid.CampsitePointSetGunUI, pointDataMgr);
                CampsitePointSetGunUI panel = Global.gApp.gUiMgr.GetPanelCompent<CampsitePointSetGunUI>(Wndid.CampsitePointSetGunUI);
                panel.OnChangeGun -= CheckPointDataChange;
                panel.OnChangeGun += CheckPointDataChange;
            }
        }

        private void RecordPointDatas()
        {
            lastRewardVal = pointDataMgr.OnceRewardVal;
            lastInterval = pointDataMgr.RewardInterval;
            lastIsAuto = pointDataMgr.isAuto;
            lastLvUpCost = pointDataMgr.GetLvUpCost(1);
        }

        private void CheckPointDataChange()
        {
            var curRewardVal = pointDataMgr.OnceRewardVal;
            var curInterval = pointDataMgr.RewardInterval;
            var curIsAuto = pointDataMgr.isAuto;
            var curLvUpCost = pointDataMgr.GetLvUpCost(1);

            bool hasChange = false;
            if (curRewardVal != lastRewardVal)
            {
                RewardChangeItem.CampsitePointUI_DataChangeItem.Init
                    (UiTools.FormateMoney(lastRewardVal), UiTools.FormateMoney(curRewardVal), curRewardVal > lastRewardVal);
                RewardChangeItem.gameObject.SetActive(true);
                hasChange = true;
            }
            else
            {
                RewardChangeItem.gameObject.SetActive(false);
            }
            if (curInterval != lastInterval)
            {
                IntervalChangeItem.CampsitePointUI_DataChangeItem.Init
                   (LanguageMgr.GetText("CampDetail_Text_Time", lastInterval), LanguageMgr.GetText("CampDetail_Text_Time", curInterval), curInterval < lastInterval);
                IntervalChangeItem.gameObject.SetActive(true);
                hasChange = true;
            }
            else
            {
                IntervalChangeItem.gameObject.SetActive(false);
            }
            if (curIsAuto != lastIsAuto)
            {
                AutoChangeItem.CampsitePointUI_DataChangeItem.Init
                    (LanguageMgr.GetText(lastIsAuto ? "CampDetail_Rule_AutoOpen" : "CampDetail_Rule_AutoClose"),
                    LanguageMgr.GetText(curIsAuto ? "CampDetail_Rule_AutoOpen" : "CampDetail_Rule_AutoClose"),
                    curIsAuto);
                AutoChangeItem.gameObject.SetActive(true);
                hasChange = true;
            }
            else
            {
                AutoChangeItem.gameObject.SetActive(false);
            }

            if (hasChange)
            {
                StopAllCoroutines();
                StartCoroutine(ShowChangeDataPanelIE());
            }
        }

        IEnumerator ShowChangeDataPanelIE()
        {
            dataChangePanelGroup.alpha = 0;
            DataChangePanel.gameObject.SetActive(true);
            float fadeDuration = 0.5f;
            float timer = 0;
            while (timer < fadeDuration)
            {
                dataChangePanelGroup.alpha = timer / fadeDuration;
                yield return null;
                timer += Time.deltaTime;
            }
            dataChangePanelGroup.alpha = 1;
            yield return new WaitForSeconds(1f);
            while (timer > 0)
            {
                dataChangePanelGroup.alpha = timer / fadeDuration;
                yield return null;
                timer -= Time.deltaTime;
            }
            DataChangePanel.gameObject.SetActive(false);
        }

        private void OnAutoBtnClick()
        {
            AutoTip.gameObject.SetActive(!AutoTip.gameObject.activeSelf);
        }

        private void OnLvUpBtnClick()
        {
            if (pointDataMgr != null)
                CampsiteMgr.singleton.LvUpPoint(pointDataMgr.index, 1);
        }

    }
}