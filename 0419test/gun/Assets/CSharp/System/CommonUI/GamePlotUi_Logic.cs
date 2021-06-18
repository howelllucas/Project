
using EZ.Data;
using UnityEngine;

namespace EZ {
    public partial class FightPlotUI
    {
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            RegisterListener();

            base.ChangeLanguage();
        }

        private void RegisterListener()
        {
            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.ShowFightPlotByID, ShowFightPlotTips);
            Global.gApp.gMsgDispatcher.AddListener<string>(MsgIds.ShowFightPlotByStr, ShowFightPlotTips);
        }
        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.ShowFightPlotByID, ShowFightPlotTips);
            Global.gApp.gMsgDispatcher.RemoveListener<string>(MsgIds.ShowFightPlotByStr, ShowFightPlotTips);
        }

        void ShowFightPlotTips(int id)
        {
            //TipsItem tipsItem = Global.gApp.gGameData.TipsData.Get(id);
            //if (tipsItem != null)
            //{
            //    FightPlotUI_FightPlotUIItem plotItem = FightPlotUIItem.GetInstance();
            //    plotItem.gameObject.SetActive(true);
            //    plotItem.Plottxt.text.text = tipsItem.txtcontent;
            //    Destroy(plotItem.gameObject, 2.85f);
            //}
            //else
            //{
            //    Debug.LogError(" ShowFightPlotTips id 错误  " + id);
            //}
            FightPlotUI_FightPlotUIItem plotItem = FightPlotUIItem.GetInstance();
            plotItem.gameObject.SetActive(true);
            plotItem.Plottxt.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(id);
            Destroy(plotItem.gameObject, 2.85f);
        }

        void ShowFightPlotTips(string text)
        {
            FightPlotUI_FightPlotUIItem plotItem = FightPlotUIItem.GetInstance();
            plotItem.gameObject.SetActive(true);
            plotItem.Plottxt.text.text = text;
            Destroy(plotItem.gameObject, 2.85f);
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }
     }
}
