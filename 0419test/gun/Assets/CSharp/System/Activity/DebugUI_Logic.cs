using EZ.Data;
using EZ.DataMgr;
using Game;
using Shining.VibrationSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class DebugUI
    {
        public static float OffsetTime = 0;
        private int m_RandomMax = 0;
        List<SkillItem> m_RandomList = new List<SkillItem>();
        private int m_LevelSkillTimes = 0;
        private int[] m_LevelSecurity;
        private ItemDTO m_ComsumeItem;
        private int m_PlotIndex = 0;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            Dropdown moduleDD = ModuleDropdown.gameObject.GetComponent<Dropdown>();
            moduleDD.options.Clear();
            foreach (var item in System.Enum.GetNames(typeof(Game.GameModuleType)))
            {
                moduleDD.options.Add(new Dropdown.OptionData(item));
            }
            ModuleBtn.button.onClick.AddListener(OnModuleBtnClick);
            AllModuleBtn.button.onClick.AddListener(OnAllModuleBtnClick);

            Btn1.button.onClick.AddListener(TouchClose);
            RegisterListeners();

            VibeBtn.button.onClick.AddListener(OnVibeBtn);

            Dropdown dialogueDD = DialogueDropdown.gameObject.GetComponent<Dropdown>();
            //foreach (DialogueItem itemConfig in Global.gApp.gGameData.DialogueConfig.items)
            //{
            //    dialogueDD.options.Add(new Dropdown.OptionData(itemConfig.id.ToString()));
            //}
            foreach(var item in TableMgr.singleton.DialogueTable.GetDialogueGroups())
            {
                dialogueDD.options.Add(new Dropdown.OptionData(item.Key.ToString()));
            }
            DialogueBtn.button.onClick.AddListener(OnDialogue);

            Dropdown itemDD = ItemDropdown.gameObject.GetComponent<Dropdown>();
            foreach (ItemItem itemConfig in Global.gApp.gGameData.ItemData.items)
            {
                if (GameItemFactory.GetInstance().m_ItemStrategyMap.ContainsKey(itemConfig.showtype))
                {
                    itemDD.options.Add(new Dropdown.OptionData(itemConfig.name));
                }
            }
            itemDD.value = 0;
            ItemBtn.button.onClick.AddListener(OnItemBtn);
            ItemReduceBtn.button.onClick.AddListener(OnItemReduceBtn);

            //------------
            NextDay.button.onClick.AddListener(NextDayCall);
            JS1.button.onClick.AddListener(JieSuanX1);
            JS3.button.onClick.AddListener(JieSuanX3);
            OnLineReward1.button.onClick.AddListener(OnLineRewardX1);
            OnLineReward3.button.onClick.AddListener(OnLineRewardX3);
            PassCheck.button.onClick.AddListener(CheckPassData);

            MCampStep1.button.onClick.AddListener(CampGuid1);
            MCampStep2.button.onClick.AddListener(CampGuid2);
            MCampStep3.button.onClick.AddListener(CampGuid3);
            MCampStep4.button.onClick.AddListener(CampGuid4);
            MCampStep5.button.onClick.AddListener(CampGuid5);

            MResetCampGuid.button.onClick.AddListener(ResetCampGuid);
            MResetBranck.button.onClick.AddListener(ResetBranck);
            MPlotTest.button.onClick.AddListener(ShowPlot);

            Dropdown passCarDD = PassCarDropdown.gameObject.GetComponent<Dropdown>();
            Dropdown passBreakOutDD = PassBreakOutDropdown.gameObject.GetComponent<Dropdown>();

            int minNormalId = int.MaxValue;
            int maxNormalId = int.MinValue;
            foreach (PassItem itemConfig in Global.gApp.gGameData.PassData.items)
            {
                switch (itemConfig.sceneType)
                {
                    case (int)SceneType.NormalScene:
                        if ((int)itemConfig.id / 100000 == 1)
                        {
                            if (itemConfig.id < minNormalId)
                            {
                                minNormalId = itemConfig.id;
                            }
                            if (itemConfig.id > maxNormalId)
                            {
                                maxNormalId = itemConfig.id;
                            }
                        }
                        break;
                    case (int)SceneType.CarScene:
                        passCarDD.options.Add(new Dropdown.OptionData(itemConfig.id.ToString()));
                        break;
                    case (int)SceneType.BreakOutSene:
                        passBreakOutDD.options.Add(new Dropdown.OptionData(itemConfig.id.ToString()));
                        break;
                }
            }
            GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
            PassNumSlider.slider.minValue = int.Parse(initPassIdConfig.content) + 1;
            PassNumSlider.slider.maxValue = maxNormalId;
            PassNum.inputField.text = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId().ToString();
            PassNumSlider.slider.value = Global.gApp.gSystemMgr.GetPassMgr().GetCurPassId();

            //设置怪
            Dropdown passDD = PassDropdown.gameObject.GetComponent<Dropdown>();
            int mIndex = 0;
            for (int i = 0; i < Global.gApp.gGameData.MosterData.items.Length; i++)
            {
                MonsterItem item = Global.gApp.gGameData.MosterData.items[i];
                passDD.options.Add(new Dropdown.OptionData(item.tag.ToString() + "." + item.name));
                if (item.tag == DebugMgr.GetInstance().MonsterId)
                {
                    mIndex = i + 1;
                }
            }

            passDD.value = mIndex;
            passCarDD.value = 0;
            passBreakOutDD.value = 0;

            //设置技能
            m_LevelSkillTimes = int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.LEVEL_SKILL_TIMES).content);
            //设置各个技能
            m_RandomList.Clear();
            m_RandomMax = 0;
            foreach (SkillItem skillItem in Global.gApp.gGameData.SkillData.items)
            {
                if (Global.gApp.gSystemMgr.GetSkillMgr().CanLevelUp(skillItem))
                {
                    m_RandomMax += skillItem.weight;
                    m_RandomList.Add(skillItem);
                }
            }
            string[] levelStrs = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.HP_SKILL_SECURITY_AT_THE_END).contents;
            m_LevelSecurity = new int[levelStrs.Length];
            m_LevelSecurity = new int[levelStrs.Length];
            for (int i = 0; i < levelStrs.Length; i++)
            {
                m_LevelSecurity[i] = int.Parse(levelStrs[i]);
            }

            Skill.button.onClick.AddListener(OnSkillBtn);
            MonsterBtn.button.onClick.AddListener(OnMonsterBtn);
            PassBtn.button.onClick.AddListener(OnPassBtn);
            PassCarBtn.button.onClick.AddListener(OnPassCarBtn);
            PassBreakOutBtn.button.onClick.AddListener(OnPassBreakOutBtn);

            FirstPurchase.button.onClick.AddListener(OnFirstPurchase);

            base.ChangeLanguage();
        }

        private void OnModuleBtnClick()
        {
            Game.GameModuleType module = (Game.GameModuleType)ModuleDropdown.gameObject.GetComponent<Dropdown>().value;
            Game.PlayerDataMgr.singleton.OpenModule(module);
        }

        private void OnAllModuleBtnClick()
        {
            foreach (Game.GameModuleType module in System.Enum.GetValues(typeof(Game.GameModuleType)))
            {
                Game.PlayerDataMgr.singleton.OpenModule(module);
            }
        }


        private void OnFirstPurchase()
        {
            Global.gApp.gSystemMgr.GetMiscMgr().FirstPurchase = 1;
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
        }

        private void OnMonsterBtn()
        {
            Dropdown dd = PassDropdown.gameObject.GetComponent<Dropdown>();
            string monsterId = dd.captionText.text.Split('.')[0];
            if (int.TryParse(monsterId, out DebugMgr.GetInstance().MonsterId))
            {
                Global.gApp.gMsgDispatcher.Broadcast<string>(MsgIds.ShowGameTipsByStr, string.Format("成功设置所有关卡的怪都为{0}", dd.captionText.text));
            }
        }

        private void StartGame(int selectPassId)
        {
            PassItem newPassItem = Global.gApp.gSystemMgr.GetPassMgr().GetPassItemById(selectPassId);
            if (newPassItem != null)
            {
                Global.gApp.gUiMgr.ClossAllPanel();
                Global.gApp.gGameCtrl.ChangeToFightScene(newPassItem.id);
                return;
            }
        }

        private void OnPassBtn()
        {
            int targetId = (int)PassNumSlider.slider.value;
            Global.gApp.gSystemMgr.GetPassMgr().SetCurPassId(targetId);
            Global.gApp.gMsgDispatcher.Broadcast<string>(MsgIds.ShowGameTipsByStr, string.Format("成功设置当前关卡为{0}", targetId));
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
        }

        private void OnPassCarBtn()
        {
            Dropdown dd = PassCarDropdown.gameObject.GetComponent<Dropdown>();
            int selectPassId;
            if (int.TryParse(dd.captionText.text, out selectPassId))
            {
                StartGame(selectPassId);
            }
        }
        private void OnPassBreakOutBtn()
        {
            Dropdown dd = PassBreakOutDropdown.gameObject.GetComponent<Dropdown>();
            int selectPassId;
            if (int.TryParse(dd.captionText.text, out selectPassId))
            {
                StartGame(selectPassId);
            }
        }
        private void OnVibeBtn()
        {
            Vibe vibe = new Vibe((long)(VibeDurSlider.slider.value * 1000));
            vibe.Amplitude((byte)(VibeAmpSlider.slider.value));
            Vibrations.instance.Vibrate(vibe);
        }
        private void OnDialogue()
        {
            Dropdown dd = DialogueDropdown.gameObject.GetComponent<Dropdown>();
            DialogueMgr.singleton.ShowDialogue(int.Parse(dd.captionText.text));
            //Global.gApp.gUiMgr.OpenPanel(Wndid.DialogueUI, dd.captionText.text);
            //DialogueUI dialogueUI = Global.gApp.gUiMgr.GetPanelCompent<DialogueUI>(Wndid.DialogueUI);
            //dialogueUI.SetAciton(() => { });
        }
        private void OnItemBtn()
        {
            Dropdown dd = ItemDropdown.gameObject.GetComponent<Dropdown>();
            ItemItem itemConfig = Global.gApp.gGameData.GetItemDataByName(dd.captionText.text);
            if (itemConfig != null)
            {
                ItemDTO itemDTO = new ItemDTO(itemConfig.id, ItemNumSlider.slider.value, BehaviorTypeConstVal.OPT_GM);
                GameItemFactory.GetInstance().AddItem(itemDTO);
                Global.gApp.gMsgDispatcher.Broadcast<string>(MsgIds.ShowGameTipsByStr, string.Format("成功添加{0}个\"{1}\"", itemDTO.num, dd.captionText.text));
            } else
            {
                Global.gApp.gMsgDispatcher.Broadcast<string>(MsgIds.ShowGameTipsByStr, string.Format("不支持添加\"{0}\"", dd.captionText.text));
            }

        }

        private void OnSkillBtn()
        {


            int nextLevel = (Global.gApp.gSystemMgr.GetSkillMgr().GetTimes() / m_LevelSkillTimes + 1) * m_LevelSkillTimes;
            int timesLimit = (Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel() / m_LevelSkillTimes) * m_LevelSkillTimes;
            if (timesLimit <= Global.gApp.gSystemMgr.GetSkillMgr().GetTimes())
            {
                Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3045, nextLevel.ToString());
                return;
            }
            m_ComsumeItem = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillUpdateItem();
            if (m_ComsumeItem == null)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 4177);
                return;
            }

            if (m_RandomList.Count == 0)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3042);
                return;
            }

            GameItemFactory.GetInstance().ReduceItem(m_ComsumeItem);
            if (!m_ComsumeItem.result)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1006);
                return;
            }

            int lastIndex = 0;

            //保底逻辑
            bool securiy = false;
            for (int i = 0; i < m_LevelSecurity.Length; i++)
            {
                bool tmp = Global.gApp.gSystemMgr.GetSkillMgr().GetTimes() >= m_LevelSecurity[i] && Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(GameConstVal.SExHp) < i + 1;
                if (tmp)
                {
                    securiy = tmp;
                    break;
                }
            }


            if (securiy)
            {
                for (int i = 0; i < m_RandomList.Count; i++)
                {
                    if (m_RandomList[i].id.Equals(GameConstVal.SExHp))
                    {
                        lastIndex = i;
                        break;
                    }
                }
            }
            else
            {
                int value = UnityEngine.Random.Range(0, m_RandomMax);
                int cur = 0;

                for (int i = 0; i < m_RandomList.Count; i++)
                {
                    if (value >= cur && value < cur + m_RandomList[i].weight)
                    {
                        lastIndex = i;
                        break;
                    }
                    cur += m_RandomList[i].weight;
                }
            }

            Global.gApp.gSystemMgr.GetSkillMgr().SetTimes();
            SkillItem finalConfig = m_RandomList[lastIndex];
            Global.gApp.gSystemMgr.GetSkillMgr().LevelUp(finalConfig.id);
            m_ComsumeItem = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillUpdateItem();

            m_RandomList.Clear();
            m_RandomMax = 0;
            foreach (SkillItem item in Global.gApp.gGameData.SkillData.items)
            {
                if (Global.gApp.gSystemMgr.GetSkillMgr().CanLevelUp(item))
                {
                    m_RandomMax += item.weight;
                    m_RandomList.Add(item);
                }
            }

        }
        private void NextDayCall()
        {
            OffsetTime += 24 * 60 * 60 * 1000;
        }
        private void OnLineRewardX1()
        {
            OnLineReward(1);
        }
        private void CheckPassData()
        {
            PassItem[] items = Global.gApp.gGameData.PassData.items;
            foreach (PassItem passItem in items)
            {
                foreach (int waveId in passItem.waveID)
                {
                    WaveItem waveData = Global.gApp.gGameData.WaveData.Get(waveId);
                    if (waveData == null)
                    {
                        Debug.LogError("PassData " + passItem.id + " 的  " + "waveData " + waveId + "不存在 ");
                        continue;
                    }
                    EZ.Data.Monster monsterData = Global.gApp.gGameData.MosterData;
                    if (waveData.enemyNum.Length > 1)
                    {
                        if (waveData.enemyNum.Length != waveData.enemyID.Length)
                        {
                            Debug.LogError("waveData " + waveData.id + "配置错误 ememy count 不等于 ememyId count");
                            continue;
                        }
                        foreach (int enemyId in waveData.enemyID)
                        {
                            MonsterItem monsterItem = monsterData.Get(enemyId);
                            if (monsterItem == null)
                            {
                                Debug.LogError("waveData " + waveData.id + "  EnemyId " + enemyId + " does not exist!");
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (waveData.enemyNum.Length == 0)
                        {
                            Debug.LogError("waveData " + waveData.id + "配置错误 ememy count 未配置");
                            continue;
                        }
                    }
                    int totalCount = 0;
                    foreach (int count in waveData.enemyNum)
                    {
                        totalCount += count;
                    }
                    if (totalCount == 0)
                    {
                        Debug.LogError("waveData " + waveData.id + "  Enemy 数量为0  !");
                    }
                }
            }
            Debug.Log("=============ok nor error===========");
        }
        private void OnLineRewardX3()
        {
            OnLineReward(3);
        }
        private void JieSuanX1()
        {
            JieSuan(1);
        }

        private void JieSuanX3()
        {
            JieSuan(3);
        }
        private void OnLineReward(float times)
        {
            FallBox_dataItem config = Global.gApp.gGameData.FallBoxDataConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
            ItemDTO addItemDTO = new ItemDTO(SpecialItemIdConstVal.GOLD, config.coin * times, BehaviorTypeConstVal.OPT_RECEIVE_SUPPLY);
            GameItemFactory.GetInstance().AddItem(addItemDTO);

        }
        private void JieSuan(float times, PassItem passItem = null)
        {
            if (passItem == null)
            {
                passItem = Global.gApp.gSystemMgr.GetPassMgr().GetPassItem();
            }
            ItemDTO itemDTO = new ItemDTO(SpecialItemIdConstVal.GOLD, passItem.goldParam * passItem.coinNum * times, BehaviorTypeConstVal.OPT_BALANCE);
            itemDTO.param2 = times;
            GameItemFactory.GetInstance().AddItem(itemDTO);


            ItemDTO item1DTO = new ItemDTO(SpecialItemIdConstVal.EXP, passItem.levelEXP, BehaviorTypeConstVal.OPT_BALANCE);
            GameItemFactory.GetInstance().AddItem(item1DTO);
            int branckId = Global.gApp.gSystemMgr.GetPassMgr().GetBranchPassId();
            Global.gApp.gSystemMgr.GetPassMgr().GameSucess(passItem.id, 0, 0);

            int[] sourceParam = passItem.sourceParam;
            if (sourceParam.Length == 2)
            {
                int dropId = sourceParam[0];
                int dropTimes = sourceParam[1];
                DropItem dropData = Global.gApp.gGameData.DropData.Get(dropId);
                if (dropData != null)
                {
                    for (int i = 0; i < dropTimes; i++)
                    {
                        int curRate = 0;
                        int randomRate = Random.Range(0, 10001);
                        int index = 0;
                        foreach (int rate in dropData.rate)
                        {
                            curRate = curRate + rate;
                            if (randomRate < curRate)
                            {
                                string path = "Prefabs/Prop/" + dropData.prop[index];
                                GameObject propGo = Global.gApp.gResMgr.InstantiateObj(path);
                                NpcProp npcProp = propGo.GetComponent<NpcProp>();
                                string itemName = string.Empty;
                                if (npcProp != null)
                                {
                                    itemName = npcProp.GetItemName();
                                }
                                else
                                {
                                    CampResProp resProp = propGo.GetComponent<CampResProp>(); ;
                                    if (resProp != null)
                                    {
                                        itemName = resProp.GetItemName();
                                    }
                                }
                                Destroy(propGo);
                                if (!itemName.Equals(string.Empty))
                                {
                                    ItemItem itemItem = Global.gApp.gGameData.GetItemDataByName(itemName);
                                    ItemDTO addItemDTO = new ItemDTO(itemItem.id, 1, BehaviorTypeConstVal.OPT_BALANCE);
                                    GameItemFactory.GetInstance().AddItem(addItemDTO);
                                }
                                break;
                            }
                            index++;
                        }
                    }

                }
            }


            if (branckId > 0)
            {
                PassItem newPassItem = Global.gApp.gGameData.PassData.Get(branckId);
                JieSuan(times, newPassItem);
            }

        }
        private void OnItemReduceBtn()
        {
            Dropdown dd = ItemDropdown.gameObject.GetComponent<Dropdown>();
            ItemItem itemConfig = Global.gApp.gGameData.GetItemDataByName(dd.captionText.text);
            if (itemConfig != null && itemConfig.showtype != ItemTypeConstVal.NPC)
            {
                ItemDTO itemDTO = new ItemDTO(itemConfig.id, ItemNumSlider.slider.value, BehaviorTypeConstVal.OPT_GM);
                GameItemFactory.GetInstance().ReduceItem(itemDTO);
                if (itemDTO.result)
                {
                    Global.gApp.gMsgDispatcher.Broadcast<string>(MsgIds.ShowGameTipsByStr, string.Format("成功扣除{0}个\"{1}\"", -itemDTO.num, dd.captionText.text));
                } else
                {
                    Global.gApp.gMsgDispatcher.Broadcast<string>(MsgIds.ShowGameTipsByStr, string.Format("\"{0}\"数量不足", dd.captionText.text));
                }
            }
            else
            {
                Global.gApp.gMsgDispatcher.Broadcast<string>(MsgIds.ShowGameTipsByStr, string.Format("不支持扣除\"{0}\"", dd.captionText.text));
            }

        }

        private void RegisterListeners()
        {
        }
        private void UnRegisterListeners()
        {
        }



        private void CampGuid1()
        {

            ItemDTO itemDTO = new ItemDTO((int)FightNpcPlayer.FightDropNPC.Npc_oldwoman, 1, BehaviorTypeConstVal.OPT_GM);
            GameItemFactory.GetInstance().AddItem(itemDTO);

            ItemDTO itemDTO1 = new ItemDTO((int)FightNpcPlayer.FightDropNPC.Npc_boy, 1, BehaviorTypeConstVal.OPT_GM);
            GameItemFactory.GetInstance().AddItem(itemDTO1);

            ItemDTO itemDTO2 = new ItemDTO((int)FightNpcPlayer.FightDropNPC.Npc_doctor03, 1, BehaviorTypeConstVal.OPT_GM);
            GameItemFactory.GetInstance().AddItem(itemDTO2);

            ItemDTO itemDTO3 = new ItemDTO((int)FightNpcPlayer.FightDropNPC.Npc_human00, 1, BehaviorTypeConstVal.OPT_GM);
            GameItemFactory.GetInstance().AddItem(itemDTO3);

            ItemDTO itemDTO4 = new ItemDTO((int)FightNpcPlayer.FightDropNPC.Npc_human03, 1, BehaviorTypeConstVal.OPT_GM);
            GameItemFactory.GetInstance().AddItem(itemDTO4);

            ItemDTO itemDTO5 = new ItemDTO((int)FightNpcPlayer.FightDropNPC.Npc_human04, 1, BehaviorTypeConstVal.OPT_GM);
            GameItemFactory.GetInstance().AddItem(itemDTO5);

            ItemDTO itemDTO6 = new ItemDTO((int)FightNpcPlayer.FightDropNPC.Npc_police01, 1, BehaviorTypeConstVal.OPT_GM);
            GameItemFactory.GetInstance().AddItem(itemDTO6);
        }
        private void CampGuid2()
        {
            ItemDTO itemDTO = new ItemDTO((int)FightNpcPlayer.FightDropNPC.Npc_worker01, 1, BehaviorTypeConstVal.OPT_GM);
            GameItemFactory.GetInstance().AddItem(itemDTO);
        }
        private void CampGuid3()
        {
            ItemDTO itemDTO = new ItemDTO((int)FightNpcPlayer.FightDropNPC.Npc_worker, 1, BehaviorTypeConstVal.OPT_GM);
            GameItemFactory.GetInstance().AddItem(itemDTO);
        }
        private void CampGuid4()
        {
            ItemDTO itemDTO = new ItemDTO((int)FightNpcPlayer.FightDropNPC.Npc_recycle, 1, BehaviorTypeConstVal.OPT_GM);
            GameItemFactory.GetInstance().AddItem(itemDTO);
        }
        private void CampGuid5()
        {
            ItemDTO itemDTO = new ItemDTO((int)FightNpcPlayer.FightDropNPC.Npc_drstrange, 1, BehaviorTypeConstVal.OPT_GM);
            GameItemFactory.GetInstance().AddItem(itemDTO);
        }
        private void ResetBranck()
        {
            Global.gApp.gSystemMgr.GetPassMgr().ResetBranckPass(); 
        }
        private void ShowPlot()
        {
            if (m_PlotIndex < Global.gApp.gGameData.DialogueConfig.items.Length)
            {
                //int dialogId = Global.gApp.gGameData.DialogueConfig.items[m_PlotIndex].id;
                //Global.gApp.gUiMgr.OpenPanel(Wndid.DialogueUI, dialogId.ToString());
                //DialogueUI dialogueUI = Global.gApp.gUiMgr.GetPanelCompent<DialogueUI>(Wndid.DialogueUI);
                //dialogueUI.SetAciton(ShowPlot);
                //m_PlotIndex++;
            }
        }
        private void ResetCampGuid()
        {
            Global.gApp.gSystemMgr.GetCampGuidMgr().ResetGuid();
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }
    }
}
