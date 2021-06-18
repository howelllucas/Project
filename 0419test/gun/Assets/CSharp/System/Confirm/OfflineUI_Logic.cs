using UnityEngine;
using System.Collections;
using EZ.Data;
using System;
using EZ.DataMgr;

namespace EZ
{
    public partial class OfflineUI
    {
        private ItemDTO m_AddItemDTO;
        private ItemDTO m_ConsumeItemDTO;
        private GeneralConfigItem offlineConfig;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            RegisterListeners();
            //处理离线奖励
            double nowMills = DateTimeUtil.GetMills(DateTime.Now);
            offlineConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.OFFLINE_PARAMS);
            float count = (float)((nowMills - Global.gApp.gSystemMgr.GetMiscMgr().GetLastOffline()) / 1000) * float.Parse(offlineConfig.contents[0]);
            float limit = float.Parse(offlineConfig.contents[3]);
            if (count > limit)
            {
                count = limit;
            }
            m_AddItemDTO = new ItemDTO(Convert.ToInt32(offlineConfig.contents[2]), count, BehaviorTypeConstVal.OPT_OFFLINE);

            Itemvaluetxt.text.text = UiTools.FormateMoneyUP(count);
            m_ConsumeItemDTO = new ItemDTO(Convert.ToInt32(offlineConfig.contents[4]), float.Parse(offlineConfig.contents[5]), BehaviorTypeConstVal.OPT_OFFLINE);

            MoneyIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.RESOURCE_PATH, m_ConsumeItemDTO.itemId));
            AdText.text.text = "× " + offlineConfig.contents[7];
            MoneyText.text.text = m_ConsumeItemDTO.num + " × " + offlineConfig.contents[6];

            AdBtn.button.onClick.AddListener(OnAdBtn);
            MoneyBtn.button.onClick.AddListener(OnMoneyBtn);
            Btn1.button.onClick.AddListener(TouchClose);

            base.ChangeLanguage();
        }

        private void OnAdBtn()
        {
            AddTouchMask();
            m_AddItemDTO.num = m_AddItemDTO.num * float.Parse(offlineConfig.contents[7]);
            //Global.gApp.gMsgDispatcher.Broadcast<float>(MsgIds.GainDelayShow, 1.5f);
            Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, m_AddItemDTO.itemId, 15, Vector3.zero);
            GameItemFactory.GetInstance().AddItem(m_AddItemDTO);
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
            TouchClose();
            //gameObject.AddComponent<DelayCallBack>().SetAction(TouchClose, 1.5f);
        }

        private void OnMoneyBtn()
        {
            AddTouchMask();
            GameItemFactory.GetInstance().ReduceItem(m_ConsumeItemDTO);
            bool reduceResult = m_ConsumeItemDTO.result;

            if (!reduceResult)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 1006);
                RemoveTouchMask();
                return;
            }
            //Global.gApp.gMsgDispatcher.Broadcast<float>(MsgIds.GainDelayShow, 1.5f);
            m_AddItemDTO.num = m_AddItemDTO.num * float.Parse(offlineConfig.contents[6]);
            Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, m_AddItemDTO.itemId, 10, Vector3.zero);
            GameItemFactory.GetInstance().AddItem(m_AddItemDTO);
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
            TouchClose();
            //gameObject.AddComponent<DelayCallBack>().SetAction(TouchClose, 1.5f);
        }

        public override void TouchClose()
        {
            Global.gApp.gSystemMgr.GetMiscMgr().SetLastOffline(DateTimeUtil.GetMills(DateTime.Now));
            Global.gApp.gSystemMgr.GetMiscMgr().SaveData();
            base.TouchClose();
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
