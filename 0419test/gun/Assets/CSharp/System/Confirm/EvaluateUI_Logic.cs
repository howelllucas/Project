
using EZ.Data;
using EZ.DataMgr;
using UnityEngine;

namespace EZ
{

    public partial class EvaluateUI
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
            BtnC.button.onClick.AddListener(TouchClose);
            CancelBtn.button.onClick.AddListener(TouchClose);
            ConfirmBtn.button.onClick.AddListener(OnConfim);

        }

        private void RegisterListeners()
        {
        }
        

        public void OnConfim()
        {
            Debug.Log("去评分");
            ////SDKDSAnalyticsManager.trackEvent(AFInAppEvents.af_mz_rate);
            //SdkdsNativeUtil.Instance.pullupAppReview();
            TouchClose();
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
