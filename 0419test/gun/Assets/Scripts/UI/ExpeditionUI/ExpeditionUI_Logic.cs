using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class ExpeditionUI
    {
        public int showLvItemCount = 5;
        private Transform m_WeaponBip;
        private GameObject m_ShowWeapon;
        private GunCard_TableItem curMainWeaponRes = null;
        private List<ExpeditionUI_LevelItem> levelItems = new List<ExpeditionUI_LevelItem>();
        private List<ExpeditionUI_LevelItem> bossItems = new List<ExpeditionUI_LevelItem>();
        private List<ExpeditionUI_LevelItem> allLevelItems = new List<ExpeditionUI_LevelItem>();
        private List<Transform> levelRootList = new List<Transform>();
        private int curLv = 0;
        private int startLv = 0;

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            StartBtn.button.onClick.AddListener(OnStartBtnClick);
            IdleRewardBtn.button.onClick.AddListener(OnIdleRewardBtnClick);
            QuickIdleBtn.button.onClick.AddListener(OnQuickIdleBtnClick);
            ChangeWeaponBtn.button.onClick.AddListener(OnChangeWeaponBtnClick);
            CloseBtn.button.onClick.AddListener(TouchClose);
            ChpaterBtn.button.onClick.AddListener(OnChapterBtnClick);
            StarBtn.button.onClick.AddListener(OnStarBtnClick);

            levelRootList.Clear();
            levelRootList.Add(LevelRoot1.gameObject.transform);
            levelRootList.Add(LevelRoot2.gameObject.transform);
            levelRootList.Add(LevelRoot3.gameObject.transform);
            levelRootList.Add(LevelRoot4.gameObject.transform);
            levelRootList.Add(LevelRoot5.gameObject.transform);
            levelRootList.Add(LevelRoot6.gameObject.transform);
            levelRootList.Add(LevelRoot7.gameObject.transform);
            levelRootList.Add(LevelRoot8.gameObject.transform);
            levelRootList.Add(LevelRoot9.gameObject.transform);
            levelRootList.Add(LevelRoot10.gameObject.transform);
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            Global.gApp.gMainRoleUI.Show();
            var dragRoleModel = RotateNode.gameObject.GetComponent<DragRoleModel>();
            Global.gApp.gMainRoleUI.AddDragCtrl(dragRoleModel);
            RefreshMainWeaponData();
            RefreshLevelList();
            RefreshIdleReward();
            RefreshIdleRate();
            RegisterListeners();
            InvokeRepeating("RefreshIdleReward", 60f, 60f);
            CommonUI commonUi = Global.gApp.gUiMgr.GetPanelCompent<CommonUI>(Wndid.CommonPanel);
            commonUi?.SetOrderLayer(info.Order - 1);
            TokenUI tokenUi = Global.gApp.gUiMgr.GetPanelCompent<TokenUI>(Wndid.TokenUI);
            tokenUi?.SetAddGoldValid(false);
            bool idleRewardOpen = PlayerDataMgr.singleton.ModuleIsOpen(GameModuleType.IdleReward);
            IdleRewardNode.gameObject.SetActive(idleRewardOpen);
            QuickIdleNode.gameObject.SetActive(idleRewardOpen);
        }

        public override void Recycle()
        {
            base.Recycle();
            UnregisterListeners();
            CommonUI commonUi = Global.gApp.gUiMgr.GetPanelCompent<CommonUI>(Wndid.CommonPanel);
            commonUi.ResetOrderLayer();
            TokenUI tokenUi = Global.gApp.gUiMgr.GetPanelCompent<TokenUI>(Wndid.TokenUI);
            tokenUi?.SetAddGoldValid(true);
            Global.gApp.gMainRoleUI.RemoveDragCtrl();
            Global.gApp.gMainRoleUI.Hide();
            CloseSubPanels();
        }

        public override void Release()
        {
            base.Release();
            UnregisterListeners();
            CommonUI commonUi = Global.gApp.gUiMgr.GetPanelCompent<CommonUI>(Wndid.CommonPanel);
            commonUi.ResetOrderLayer();
            TokenUI tokenUi = Global.gApp.gUiMgr.GetPanelCompent<TokenUI>(Wndid.TokenUI);
            tokenUi?.SetAddGoldValid(true);
            if (gameObject.activeSelf)
            {
                Global.gApp.gMainRoleUI.Hide();
            }
            CloseSubPanels();
        }

        private void CloseSubPanels()
        {
            Global.gApp.gUiMgr.ClosePanel(Wndid.GunListUI);
            Global.gApp.gUiMgr.ClosePanel(Wndid.IdleRewardUI);
            Global.gApp.gUiMgr.ClosePanel(Wndid.QuickIdleRewardUI);

        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.GunCardOpt, RefreshMainWeaponData);
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.IdleRewardClaim, RefreshIdleReward);
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.ChapterChange, RefreshLevelList);
            Global.gApp.gMsgDispatcher.AddListener<GameModuleType>(MsgIds.ModuleOpen, OnModuleOpen);
        }

        private void UnregisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.GunCardOpt, RefreshMainWeaponData);
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.IdleRewardClaim, RefreshIdleReward);
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.ChapterChange, RefreshLevelList);
            Global.gApp.gMsgDispatcher.RemoveListener<GameModuleType>(MsgIds.ModuleOpen, OnModuleOpen);
        }

        private void RefreshIdleReward()
        {
            IdleRewardTxt.text.text = IdleRewardMgr.singleton.GetRewardGold().ToSymbolString();
            IdleProgressImg.image.fillAmount = IdleRewardMgr.singleton.GetProcess();
        }

        private void RefreshIdleRate()
        {
            IdleRateTxt.text.text = LanguageMgr.GetText("Explore_Text_Efficiency", IdleRewardMgr.singleton.GetGoldPerMinute().ToSymbolString());
        }

        private void RefreshLevelList()
        {
            for (int i = 0; i < levelItems.Count; i++)
            {
                LevelItem.CacheInstance(levelItems[i]);
            }
            levelItems.Clear();
            for (int i = 0; i < bossItems.Count; i++)
            {
                LevelItem_Boss.CacheInstance(bossItems[i]);
            }
            bossItems.Clear();
            allLevelItems.Clear();
            var maxLv = PlayerDataMgr.singleton.GetMaxUnlockStageID();
            curLv = maxLv;
            Debug.Log("CurLv" + curLv);
            //startLv = (curLv - 1) / showLvItemCount * showLvItemCount + 1;
            startLv = PlayerDataMgr.singleton.DB.chapterData.startStageID;
            for (int i = 0; i < showLvItemCount; i++)
            {
                int lv = startLv + i;
                var res = TableMgr.singleton.LevelTable.GetItemByID(lv);
                if (res == null)
                    continue;

                ExpeditionUI_LevelItem item;
                if (res.bossLevel == 0)
                {
                    item = LevelItem.GetInstance();
                    levelItems.Add(item);
                }
                else
                {
                    item = LevelItem_Boss.GetInstance();
                    bossItems.Add(item);
                }

                allLevelItems.Add(item);
                //item.transform.SetAsLastSibling();                
                item.transform.SetParent(levelRootList[i]);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.Init(lv, maxLv);
                item.gameObject.SetActive(true);
                item.OnLevelClick += OnChooseLevel;
            }

            var stars = PlayerDataMgr.singleton.GetChapterStar();
            if (stars <= 10)
                StarTxt.text.text = string.Format("{0}/10", stars);
            else if (stars <= 20)
                StarTxt.text.text = string.Format("{0}/20", stars);
            else if (stars <= 30)
                StarTxt.text.text = string.Format("{0}/30", stars);

            if (maxLv >= PlayerDataMgr.singleton.DB.chapterData.startStageID + 9 &&
                PlayerDataMgr.singleton.IsFinishStage(maxLv))
                ChpaterBtn.gameObject.SetActive(true);
            else
                ChpaterBtn.gameObject.SetActive(false);
        }

        private void RefreshMainWeaponData()
        {
            int mainWeaponId = PlayerDataMgr.singleton.GetUseWeaponID();
            if (curMainWeaponRes != null && curMainWeaponRes.id == mainWeaponId)
                return;

            curMainWeaponRes = TableMgr.singleton.GunCardTable.GetItemByID(mainWeaponId);
            InitRole();
        }

        private void InitRole()
        {
            string weaponPath = "Prefabs/WeaponNew/MainUI/" + curMainWeaponRes.prefab;
            Global.gApp.gMainRoleUI.SetWeapon(weaponPath);
        }

        private void OnStartBtnClick()
        {
            string inputText = InputFieldCmp.inputField.text;
            int level;
            if (int.TryParse(inputText, out level))
            {
            }
            else
                level = curLv;

            var res = TableMgr.singleton.LevelTable.GetItemByID(level);
            if (res == null)
                return;

            Global.gApp.gUiMgr.OpenPanel(Wndid.LevelInfoUI, res);
            //if (PlayerDataMgr.singleton.EnterStage(level))
            //{
            //    Global.gApp.gUiMgr.ClossAllPanel();
            //    Global.gApp.gGameCtrl.ChangeToFightScene(level);
            //}
            //else
            //{
            //    Global.gApp.gMsgDispatcher.Broadcast<string>(MsgIds.ShowGameTipsByStr, "Stay tuned ");
            //}
        }

        private void OnChangeWeaponBtnClick()
        {
            if (curMainWeaponRes != null)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.GunListUI, curMainWeaponRes);
            }
        }

        private void OnIdleRewardBtnClick()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.IdleRewardUI);
        }

        private void OnQuickIdleBtnClick()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.QuickIdleRewardUI);
        }

        private void OnChooseLevel(int level)
        {
            curLv = level;
            for (int i = 0; i < allLevelItems.Count; i++)
            {
                allLevelItems[i].CurFlag.gameObject.SetActive(false);
            }
        }

        private void OnStarBtnClick()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.StarRewardUI);
        }

        private void OnChapterBtnClick()
        {
            if (PlayerDataMgr.singleton.DB.chapterData.starList.Count >= 3)
            {
                PlayerDataMgr.singleton.NextChapter();
            }
            else
                Global.gApp.gUiMgr.OpenPanel(Wndid.ChapterUI);
        }

        private void OnModuleOpen(GameModuleType module)
        {
            if (module == GameModuleType.IdleReward)
            {
                IdleRewardNode.gameObject.SetActive(true);
                QuickIdleNode.gameObject.SetActive(true);
            }
        }
    }
}