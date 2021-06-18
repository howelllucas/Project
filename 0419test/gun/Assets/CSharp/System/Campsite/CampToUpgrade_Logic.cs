using EZ.Data;
using EZ.DataMgr;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class CampToUpgrade
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
            int campLevel = Global.gApp.gSystemMgr.GetNpcMgr().CalCampLevel();
            string[] maxNpcNum = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_MAX_NUM).contents;
            string[] levelName = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_LEVEL_NAME).contents;
            string[] levelNameColor = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.CAMP_LEVEL_NAME_COLOR).contents;

            MatIcon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(string.Format(CommonResourceConstVal.CAMP_LEVEL, (campLevel - 1)));
            MatName.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(int.Parse(levelName[campLevel - 1]));
            MatName.text.color = ColorUtil.GetColor(levelNameColor[campLevel - 1]);

            CloseBtn.button.onClick.AddListener(TouchClose);
        }

        private void OnCloseBtn()
        {
            TouchClose();
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
