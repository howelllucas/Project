using EZ.Data;
using EZ.DataMgr;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class CampDetailUI
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
            int notFreshNum = 0;
            int freshNum = 0;
            //计算可生成npc总数
            foreach (ItemDTO itemDTO in Global.gApp.gSystemMgr.GetNpcMgr().NpcMap.Values)
            {
                ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(itemDTO.itemId);
                CampNpcItem npcItem = Global.gApp.gGameData.CampNpcConfig.Get(itemCfg.name);
                if (npcItem.notFresh != 1)
                {
                    notFreshNum += Convert.ToInt32(itemDTO.num);
                } else
                {
                    freshNum += Convert.ToInt32(itemDTO.num);
                }
            }

            int campLevel = Global.gApp.gSystemMgr.GetNpcMgr().CalCampLevel();
            string[] maxNpcNum = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_MAX_NUM).contents;
            string[] levelName = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_LEVEL_NAME).contents;
            string[] levelNameColor = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_LEVEL_NAME_COLOR).contents;

            LevelText.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(int.Parse(levelName[campLevel - 1]));
            LevelText.text.color = ColorUtil.GetColor(levelNameColor[campLevel - 1]);

            Cur.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(4391), (notFreshNum + freshNum));
            Max.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(4392), (int.Parse(maxNpcNum[(campLevel - 1) * 2 + 1]) + Global.gApp.gGameData.NotFreshNpcList.Count));

            DetailItemUI.gameObject.SetActive(false);
            for (int i = maxNpcNum.Length - 1; i >= 0; i -=2)
            {
                CampDetailUI_DetailItemUI itemUI = DetailItemUI.GetInstance();
                itemUI.gameObject.SetActive(true);
                int unlockNum = i == 1 ? 0 : int.Parse(maxNpcNum[i - 2]) + Global.gApp.gGameData.NotFreshNpcList.Count;
                itemUI.LockText.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(4393), unlockNum);
                itemUI.LockText.gameObject.SetActive(campLevel < (i + 1) / 2);
                itemUI.DName.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(int.Parse(levelName[(i - 1) / 2]));
                itemUI.DName.text.color = ColorUtil.GetColor(levelNameColor[(i - 1) / 2]);
                itemUI.Icon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.CAMP_LEVEL, (i + 1) / 2));
                Color old = itemUI.Icon.image.color;
                if ((i + 1) / 2 > campLevel)
                {
                    old.a = 0.5f;
                    itemUI.Lock.gameObject.SetActive(true);
                } else
                {
                    old.a = 1;
                    itemUI.Lock.gameObject.SetActive(false);
                }
                itemUI.Icon.image.color = old;
            }

            CloseBtn.button.onClick.AddListener(TouchClose);
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
