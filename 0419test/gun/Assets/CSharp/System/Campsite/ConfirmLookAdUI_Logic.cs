using EZ.Data;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{

    public partial class ConfirmLookAdUI
    {
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            InitNode();

            base.ChangeLanguage();
        }

        private void InitNode()
        {
            m_Btn1.button.onClick.AddListener(TouchClose);
            m_Btn2.button.onClick.AddListener(OnConfirm);
            CampHeart_paramsItem gpiCfg = Global.gApp.gGameData.CampHeartParamsConfig.Get(Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel());
            string addNumStr = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_AD_FALL_HEART).content;
            m_num.text.text = "X" + ((int)(double.Parse(addNumStr) * gpiCfg.coinParams + 0.5d)).ToString();
        }

        private void OnConfirm()
        {
            CampsiteUI campsiteUI = Global.gApp.gUiMgr.GetPanelCompent<CampsiteUI>(Wndid.CampsiteUI); ;
            //campsiteUI.CompleteAds(true);
            AdManager.instance.ShowRewardVedio(campsiteUI.CompleteAds, AdShowSceneType.CAMP_GET_HEART, 0, 0, 0);
            TouchClose();
        }
    }
}
