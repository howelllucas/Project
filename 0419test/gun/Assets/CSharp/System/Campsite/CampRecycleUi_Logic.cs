using EZ.Data;
using EZ.DataMgr;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class CampRecycleUi
    {
        private Dictionary<int, double> m_RecyclyMat = new Dictionary<int, double>();
        NpcBehavior m_NpcBehavior;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            m_NpcBehavior = arg as NpcBehavior;
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            RectTransform parentRectTsf = parentCanvas.GetComponent<RectTransform>();
            m_AdaptNode.rectTransform.anchoredPosition = UiTools.WorldToRectPos(gameObject, m_NpcBehavior.GetTaskUINode().position, parentRectTsf);
            GenerateRecycleMat();
            InitNode();
            base.ChangeLanguage();
        }
        private void GenerateRecycleMat()
        {
            CampRecycleItem[] campRecycleItems = Global.gApp.gGameData.CampRecycleConfig.items;

            string[] curConsume = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_DAY_COST).contents;
            string maxDayStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_MAX_STORE_DAY).content;
            double maxDayD = double.Parse(maxDayStr);
            Dictionary<int, double> maxCousumeCountMap = new Dictionary<int, double>();
            Dictionary<string, ItemDTO> npcMap = Global.gApp.gSystemMgr.GetNpcMgr().NpcMap;
            double totlaNpcNum = 0;
            foreach (KeyValuePair<string, ItemDTO> kvValue in npcMap)
            {
                totlaNpcNum += kvValue.Value.num;
            }
            int curId = 0;
            for (int i = 0; i < curConsume.Length; i++)
            {
                if (i % 2 == 1)
                {
                    maxCousumeCountMap[curId] = totlaNpcNum * double.Parse(curConsume[i]) * maxDayD;
                }
                else
                {
                    curId = int.Parse(curConsume[i]);
                    maxCousumeCountMap.Add(i, 0);
                }
            }
            // 把当前所有的 材料全部 加到表里去
            foreach (CampRecycleItem campRecycleItem in campRecycleItems)
            {
                double count = GameItemFactory.GetInstance().GetItem(campRecycleItem.id);
                if(count > 0)
                {
                    m_RecyclyMat.Add(campRecycleItem.id,count);
                }
            }
            foreach (KeyValuePair<int,double> keyValuePair in maxCousumeCountMap)
            {
                if (m_RecyclyMat.ContainsKey(keyValuePair.Key))
                {
                    double leftVal = m_RecyclyMat[keyValuePair.Key] - keyValuePair.Value;
                    if(leftVal > 0)
                    {
                        m_RecyclyMat[keyValuePair.Key] = leftVal;
                    }
                    else
                    {
                        m_RecyclyMat.Remove(keyValuePair.Key);
                    }
                }
            }
            List<NpcQuestItemDTO> npcQuestList = Global.gApp.gSystemMgr.GetNpcMgr().NpcQuestList;
            Dictionary<int,double> m_QuestMatCount = new Dictionary<int, double>();
            foreach(NpcQuestItemDTO npcQuestItemDTO in npcQuestList)
            {
               // FilterTypeConstVal
                if (npcQuestItemDTO.state != NpcState.None && npcQuestItemDTO.state != NpcState.Received && npcQuestItemDTO.npcQuestId > 0)
                {
                    CampTasksItem campTasksItem = Global.gApp.gGameData.CampTasksConfig.Get(npcQuestItemDTO.npcQuestId);
                    if (campTasksItem != null && campTasksItem.taskCondition[0] == FilterTypeConstVal.GET_ITEM)
                    {
                        int matId = (int)campTasksItem.taskCondition[1];
                        if (!m_QuestMatCount.ContainsKey(matId))
                        {
                            m_QuestMatCount.Add(matId, 0);
                        }
                        m_QuestMatCount[matId] += campTasksItem.taskCondition[2];
                    }
                }
            }

            foreach (KeyValuePair<int, double> keyValuePair in m_QuestMatCount)
            {
                if (m_RecyclyMat.ContainsKey(keyValuePair.Key))
                {
                    double leftVal = m_RecyclyMat[keyValuePair.Key] - keyValuePair.Value;
                    if (leftVal > 0)
                    {
                        m_RecyclyMat[keyValuePair.Key] = leftVal;
                    }
                    else
                    {
                        m_RecyclyMat.Remove(keyValuePair.Key);
                    }
                }
            }
        }
        private void InitNode()
        {
            ExchangeAllBtn.button.onClick.AddListener(ExChangeAllUnUseMat);
            ExpansionBtn.button.onClick.AddListener(ShowAllExPansionMat);
            AllExchangeMatBg.button.onClick.AddListener(CloseAllExPansionMat);
            double exchangeCoinCount = 0;
            foreach(KeyValuePair<int, double> keyValue in m_RecyclyMat)
            {
                CampRecycleItem campRecycleItem = Global.gApp.gGameData.CampRecycleConfig.Get(keyValue.Key);
                exchangeCoinCount += campRecycleItem.price * ((int)keyValue.Value);
            }
            Gold_paramsItem gpiCfg = Global.gApp.gGameData.GoldParamsConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
            exchangeCoinCount *= gpiCfg.coinParams;
            Num.text.text = UiTools.FormateMoneyUP(exchangeCoinCount);

            GeneratePartyRecycleMat();
            GenerateAllRecycleMat();

        }
        private void ExChangeAllUnUseMat()
        {
            if (m_RecyclyMat.Count > 0)
            {
                Gold_paramsItem gpiCfg = Global.gApp.gGameData.GoldParamsConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
                double coinParams  = gpiCfg.coinParams;
                bool excangeSucess = false;
                foreach (KeyValuePair<int, double> keyValue in m_RecyclyMat)
                {
                    int count = (int)keyValue.Value;
                    if (count > 0)
                    {
                        ItemDTO reduceItemDTO = new ItemDTO(keyValue.Key, count, BehaviorTypeConstVal.OPT_EXCHANGE_REDUCE_ITEM);
                        GameItemFactory.GetInstance().ReduceItem(reduceItemDTO);

                        CampRecycleItem campRecycleItem = Global.gApp.gGameData.CampRecycleConfig.Get(keyValue.Key);
                        ItemDTO addItemDTO = new ItemDTO(SpecialItemIdConstVal.GOLD, count * campRecycleItem.price * coinParams, BehaviorTypeConstVal.OPT_EXCHANGE_ADD_COIN);
                        GameItemFactory.GetInstance().AddItem(addItemDTO);
                        excangeSucess = true;
                    }
                }
                if (excangeSucess)
                {
                    Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, SpecialItemIdConstVal.GOLD, 20, AdaptNode.rectTransform.position);
                }
                TouchClose();
            }
            else
            {
                TouchClose();
            }
        }
        private void GeneratePartyRecycleMat()
        {
            int index = 0;
            foreach (KeyValuePair<int, double> keyValue in m_RecyclyMat)
            {
                int val = (int)keyValue.Value;
                if (val > 0)
                {
                    ItemItem item = Global.gApp.gGameData.ItemData.Get(keyValue.Key);
                    CampRecycleUi_ExchangeItem exchangeItem = ExchangeItem.GetInstance();
                    exchangeItem.transform.SetSiblingIndex(keyValue.Key);
                    exchangeItem.gameObject.SetActive(true);
                    exchangeItem.Icon.image.sprite = Resources.Load(item.image_grow, typeof(Sprite)) as Sprite;
                    exchangeItem.Count.text.text = val.ToString();
                    index++;
                    if (index == 4)
                    {
                        break;
                    }
                }
            }
        }
        private void GenerateAllRecycleMat()
        {
            foreach (KeyValuePair<int, double> keyValue in m_RecyclyMat)
            {
                int val = (int)keyValue.Value;
                if (val > 0)
                {
                    ItemItem item = Global.gApp.gGameData.ItemData.Get(keyValue.Key);
                    CampRecycleUi_NewExchangeItem exchangeItem = NewExchangeItem.GetInstance();
                    exchangeItem.transform.SetSiblingIndex(keyValue.Key);
                    exchangeItem.gameObject.SetActive(true);
                    exchangeItem.Icon.image.sprite = Resources.Load(item.image_grow, typeof(Sprite)) as Sprite;
                    exchangeItem.Count.text.text = val.ToString();
                }
            }
        }
        private void ShowAllExPansionMat()
        {
            AllExchangeMatBg.gameObject.SetActive(true);
            UnExpansionImage.gameObject.SetActive(true);
        }
        private void CloseAllExPansionMat()
        {
            AllExchangeMatBg.gameObject.SetActive(false);
            UnExpansionImage.gameObject.SetActive(false);
        }
     
    }
}
