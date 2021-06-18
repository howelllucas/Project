
using EZ.Data;
using System.Collections.Generic;

namespace EZ {
    public partial class LanguageConfigUI
    {
        List<LanguageConfigUI_BtnLanguage> m_list = new List<LanguageConfigUI_BtnLanguage>();
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);

            GeneralConfigItem cfg = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.LANGUAGE_LIST);
            BtnLanguage.gameObject.SetActive(false);
            for (int i = 0; i < cfg.contents.Length; i += 3)
            {
                LanguageConfigUI_BtnLanguage bl = BtnLanguage.GetInstance();
                bl.Init(cfg.contents[i + 1], cfg.contents[i], this);
                bl.transform.SetSiblingIndex(i);
                m_list.Add(bl);
            }

            BtnC.button.onClick.AddListener(TouchClose);

            RegisterListeners();
            ChangeLanguage();
        }

        public override void ChangeLanguage()
        {
            base.ChangeLanguage();

            foreach (LanguageConfigUI_BtnLanguage l in m_list)
            {
                l.UIFresh();
            }

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
