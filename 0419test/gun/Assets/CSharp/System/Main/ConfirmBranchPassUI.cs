using EZ.Data;
using EZ.DataMgr;

namespace EZ
{
    public partial class ConfirmBranchPassUI
    {
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            InitNode();
            base.ChangeLanguage();
        }
        private void InitNode()
        {
            m_Close.button.onClick.AddListener(TouchClose);
            m_Confirm.button.onClick.AddListener(EnterBranchPass);
            m_DelayBtn.button.onClick.AddListener(DelayEnterPass);
            PassBranchItem branckItem = Global.gApp.gSystemMgr.GetPassMgr().GetBranchPassItem();
            if (branckItem != null)
            {
                m_confirmtxt.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(branckItem.tipsId);
            }

        }
        private void DelayEnterPass()
        {
            Global.gApp.gSystemMgr.GetPassMgr().DelayBranchPass();
            MainUi mainUi = Global.gApp.gUiMgr.GetPanelCompent<MainUi>(Wndid.MainPanel);
            if(mainUi != null)
            {
                mainUi.FreshEnterBtn();
            }
            TouchClose();
        }
        private void EnterBranchPass()
        {
            string[] consumeItemStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.GAME_CONSUME_ITEMS).contents;
            bool result = GameItemFactory.GetInstance().ReduceItem(consumeItemStr, BehaviorTypeConstVal.OPT_GAME_CONSUME);
            if (!result)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.EnergyShowPanel);
                return;
            }
            int passId = Global.gApp.gSystemMgr.GetPassMgr().GetBranchPassId();
            Global.gApp.gUiMgr.ClossAllPanel();
            Global.gApp.gGameCtrl.ChangeToFightScene(passId);
        }
    }
}
