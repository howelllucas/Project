using EZ.Data;
using EZ.DataMgr;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class CampResourcesDetailUI
    {
        
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            RegisterListeners();
            InitNode();
            base.ChangeLanguage();
        }

        private void InitNode()
        {
            string[] curConsume = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_DAY_COST).contents;
            string maxDayStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_MAX_STORE_DAY).content;
            double maxDayD = double.Parse(maxDayStr);
            int curId = 0;
            SortedDictionary<int, double> maxCousumeCountMap = new SortedDictionary<int, double>();
            Dictionary<string, ItemDTO> npcMap = Global.gApp.gSystemMgr.GetNpcMgr().NpcMap;
            double totlaNpcNum = 0;
            foreach (KeyValuePair<string, ItemDTO> kvValue in npcMap)
            {
                totlaNpcNum += kvValue.Value.num;
            }
            for (int i = 0; i < curConsume.Length; i +=2)
            {
                curId = int.Parse(curConsume[i]);
                double curMat = totlaNpcNum * double.Parse(curConsume[i + 1]) * maxDayD;
                maxCousumeCountMap[curId] = curMat;
            }
            double totalCount = 0d;
            double totalMaxCount = 0d;
            ResourceItemUI.gameObject.SetActive(false);
            foreach (int id in maxCousumeCountMap.Keys)
            {
                double curV = GameItemFactory.GetInstance().GetItem(id);
                double maxV = maxCousumeCountMap[id];
                totalCount += curV;
                totalMaxCount += maxV;
                ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(id);

                CampResourcesDetailUI_ResourceItemUI itemUI = ResourceItemUI.GetInstance();
                itemUI.gameObject.SetActive(true);
                itemUI.Materials.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(itemCfg.image_grow);
                itemUI.Progress.image.fillAmount = Convert.ToSingle(curV / maxV);
                itemUI.IName.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(itemCfg.sourceLanguage);
                itemUI.Amount.text.text = Math.Ceiling(curV).ToString("0.##") + "/" + Math.Ceiling(maxV).ToString("0.##");
                itemUI.ConsumeADayAmount.text.text = (maxV / maxDayD).ToString("0.#");
                itemUI.MaintainedDaysAmount.text.text = ((curV * maxDayD) / maxV).ToString("0.#");

                itemUI.transform.SetSiblingIndex(id);
            }

            int index = -1;
            if (totalMaxCount > 0)
            {
                double rata = (totalCount / (totalMaxCount)) * 100;
                string[] stateJudge = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_HEATH_DEFINITION).contents;
                for (int i = 0; i < stateJudge.Length; i++)
                {
                    if (rata <= double.Parse(stateJudge[i]))
                    {
                        index = i;
                        break;
                    }
                }
                if (index < 0)
                {
                    index = stateJudge.Length;
                }
            }
            else
            {
                index = 0;
            }

            string[] stateIds = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_NPC_HEATH_NAME).contents;
            DName.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(int.Parse(stateIds[index]));
            if (index == 0)
            {

                DName.text.color = new Color(255.0f / 255, 39.0f / 255, 39.0f / 255);
            }
            else if (index == 1)
            {

                DName.text.color = new Color(255.0f / 255, 234.0f / 255, 99.0f / 255);
            }
            else if (index == 2)
            {
                DName.text.color = new Color(183.0f / 255, 245.0f / 255, 60.0f / 255);
            }
            string[] dws = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_SOURCE_HEALTH_DETAIL_WORD).contents;
            Text.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(int.Parse(dws[index]));

            CloseBtn.button.onClick.AddListener(TouchClose);
            KnowBtn.button.onClick.AddListener(TouchClose);
        }

        private void RegisterListeners()
        {
        }
        private void UnRegisterListeners()
        {
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }
    }
}
