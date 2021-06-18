
using EZ.Data;
using EZ.DataMgr;
using System;
using System.Collections.Generic;
namespace EZ
{

    public partial class ConfirmEnterPassUI
    {
        private PassItem m_Pass;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            RegisterListeners();

            m_Pass = arg as PassItem;

            GeneralConfigItem initPassIdConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INIT_PASS_ID);
            confirmtxt.text.text = string.Format(Global.gApp.gGameData.GetTipsInCurLanguage(4260), m_Pass.id % Convert.ToInt32(initPassIdConfig.content));

            Btn1.button.onClick.AddListener(TouchClose);
            Btn2.button.onClick.AddListener(OnStartGame);

            base.ChangeLanguage();
        }

        private void OnStartGame()
        {
            string[] consumeItemStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.REPASS_ENERGY_TAKE).contents;
            bool result0 = GameItemFactory.GetInstance().ReduceItem(consumeItemStr, BehaviorTypeConstVal.OPT_GAME_CONSUME);
            if (!result0)
            {
                Global.gApp.gUiMgr.OpenPanel(Wndid.EnergyShowPanel);
                return;
            }
            Global.gApp.gSystemMgr.GetPassMgr().EnterPass();
            Global.gApp.gUiMgr.ClossAllPanel();
            Global.gApp.gGameCtrl.ChangeToFightScene(m_Pass.id);
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
