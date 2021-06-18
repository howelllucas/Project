using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace EZ
{
    public partial class CampsiteUnlockUI
    {
        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            ConfirmBtn.button.onClick.AddListener(OnConfirmBtnClick);
            CloseBtn.button.onClick.AddListener(TouchClose);
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            int dataIndex = int.Parse(arg as string);
            var pointDataMgr = CampsiteMgr.singleton.GetPointByIndex(dataIndex);
            desc.text.text = LanguageMgr.GetText("CampTab_Battle_Text",pointDataMgr.buildingRes.buildingName);
        }

        private void OnConfirmBtnClick()
        {
            CampsiteObjectMgr.Instance.RecordCamTrans();
            Global.gApp.gUiMgr.ClossAllPanel();
            Global.gApp.gGameCtrl.ChangeToCampsiteFightScene(CampsiteMgr.singleton.GetUnlockNextStageId());
        }
    }
}