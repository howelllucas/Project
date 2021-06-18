using EZ.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class GameConfigUI
    {
        private Action m_ConfirmAction;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            FlushSoundState();

            Level1.Level1.BtnLanguage.button.onClick.AddListener(OnBtnLanguage);
            Level1.Level1.BtnmusicOff.button.onClick.AddListener(OnClickMusicOff);
            Level1.Level1.BtnmusicOn.button.onClick.AddListener(OnClickMusicOn);
            Level1.Level1.BtnvolOff.button.onClick.AddListener(OnClickAudioOff);
            Level1.Level1.BtnvolOn.button.onClick.AddListener(OnClickAudioOn);
            Level1.Level1.BtnVibeOff.button.onClick.AddListener(OnClickVibeOff);
            Level1.Level1.BtnVibeOn.button.onClick.AddListener(OnClickVibeOn);
            BtnC.button.onClick.AddListener(OnClickResume);
            Level1.Level1.Btn2.button.onClick.AddListener(OnClickResume);
            Level1.Level1.about.button.onClick.AddListener(()=>{ OnOpenLevel(2); });
            Level1.Level1.AboatTxt.button.onClick.AddListener(() => { OnOpenLevel(2); });
            Level1.Level1.AboatArrow.button.onClick.AddListener(() => { OnOpenLevel(2); });
            Level2.Level2.BtnDM.button.onClick.AddListener(() => { OnOpenLevel(3); });
            Level2.Level2.BtnDM.button.onClick.AddListener(() => { OnOpenLevel(3); });
            Level2.Level2.BtnTS.button.onClick.AddListener(OnOpenTS);
            Level2.Level2.BtnPP.button.onClick.AddListener(OnOpenPP);
            Level2.Level2.BtnAC.button.onClick.AddListener(OnOpenAC);
            Level2.Level2.ReturnBtn.button.onClick.AddListener(() => { OnOpenLevel(1); });
            Level3.Level3.BtnOOIA.button.onClick.AddListener(OnOpenOOIA);
            Level3.Level3.BtnDMD.button.onClick.AddListener(OnOpenDMD);
            Level3.Level3.ReturnBtn.button.onClick.AddListener(() => { OnOpenLevel(2); });
            Level4.Level4.CancelBtn.button.onClick.AddListener(OnCancel);
            Level4.Level4.ConfirmBtn.button.onClick.AddListener(OnConfirm);

            OnOpenLevel(1);
			//Level3.Level3.OOIA.gameObject.SetActive(SdkdsGDPRUtils.Instance.checkIsGDPREnforcedCountry());
            //Level2.Level2.DM.gameObject.SetActive(SdkdsGDPRUtils.Instance.checkIsGDPREnforcedCountry());
            ChangeLanguage();
        }

        #region

        public override void ChangeLanguage()
        {
            base.ChangeLanguage();

            GeneralConfigItem cfg = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.LANGUAGE_LIST);
            for (int i = 0; i < cfg.contents.Length; i += 3)
            {
                string lt = cfg.contents[i + 1];
                if (lt.Equals(Global.gApp.gSystemMgr.GetMiscMgr().Language))
                {
                    Level1.Level1.languageTxt.text.text = cfg.contents[i];
                    break;
                }
            }
        }

        private void OnBtnLanguage()
        {
            Global.gApp.gUiMgr.OpenPanel(Wndid.LanguageConfigUI);
        }

        private void FlushSoundState()
        {

            bool musicOn = Global.gApp.gAudioSource.musicVolumn == 1;
            bool audioOn = Global.gApp.gAudioSource.audioVolumn == 1;
            bool vibeOn = Global.gApp.gAudioSource.vibe == 1;

            Level1.Level1.BtnmusicOff.gameObject.SetActive(!musicOn);
            Level1.Level1.BtnmusicOn.gameObject.SetActive(musicOn);
            Level1.Level1.BtnvolOff.gameObject.SetActive(!audioOn);
            Level1.Level1.BtnvolOn.gameObject.SetActive(audioOn);
            Level1.Level1.BtnVibeOff.gameObject.SetActive(!vibeOn);
            Level1.Level1.BtnVibeOn.gameObject.SetActive(vibeOn);
        }

        private void OnClickMusicOff()
        {
            if (Global.gApp.gAudioSource.musicVolumn == 1) return;
            Global.gApp.gAudioSource.musicVolumn = 1;
            FlushSoundState();
        }
        private void OnClickMusicOn()
        {
            if (Global.gApp.gAudioSource.musicVolumn == 0) return;
            Global.gApp.gAudioSource.musicVolumn = 0;
            FlushSoundState();
        }
        private void OnClickAudioOff()
        {
            if (Global.gApp.gAudioSource.audioVolumn == 1) return;
            Global.gApp.gAudioSource.audioVolumn = 1;
            FlushSoundState();
        }
        private void OnClickAudioOn()
        {
            if (Global.gApp.gAudioSource.audioVolumn == 0) return;
            Global.gApp.gAudioSource.audioVolumn = 0;
            FlushSoundState();
        }

        private void OnClickVibeOff()
        {
            if (Global.gApp.gAudioSource.vibe == 1) return;
            Global.gApp.gAudioSource.vibe = 1;
            FlushSoundState();
        }
        private void OnClickVibeOn()
        {
            if (Global.gApp.gAudioSource.vibe == 0) return;
            Global.gApp.gAudioSource.vibe = 0;
            FlushSoundState();
        }

        private void OnClickResume()
        {
            base.TouchClose();
        }

        private void OnOpenLevel(int level)
        {
            for (int i = 1; i < 5; i ++)
            {
                Transform t = transform.Find("bg/i_Level" + i);
                t.gameObject.SetActive(i == level);
            }
        }
       
        private void OnOpenTS()
        {
            string url = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.TERMS_OF_SERVICE).content;
            Application.OpenURL(url);
        }
        private void OnOpenPP()
        {
            string url = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.PRIVACY_PALICY).content;
            Application.OpenURL(url);
        }
        private void OnOpenAC()
        {
            string url = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.AD_CHOICES).content;
            Application.OpenURL(url);
        }
        private void OnOpenDMD()
        {
            m_ConfirmAction = DeleteMyData;
            OnOpenLevel(4);
            Level4.Level4.desc.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(999999);
            Level4.Level4.Content.rectTransform.localPosition = new Vector3(Level4.Level4.Content.rectTransform.localPosition.x, 0f, Level4.Level4.Content.rectTransform.localPosition.z);
        }
        private void DeleteMyData()
        {
            Global.gApp.ClearData();

            Global.gApp.gSystemMgr.GetMiscMgr().m_DeleteData = 1;
            Global.gApp.gUiMgr.ClosePanel(Wndid.CommonPanel);
            Global.gApp.gUiMgr.OpenPanel(Wndid.CommonPanel);
            Global.gApp.gUiMgr.ClosePanel(Wndid.HomeUI);
            Global.gApp.gUiMgr.OpenPanel(Wndid.HomeUI);
        }
        private void OnOpenOOIA()
        {
            m_ConfirmAction = OptOutOfInterestBasedAds;
            OnOpenLevel(4);
            Level4.Level4.desc.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(999998);
            Level4.Level4.Content.rectTransform.localPosition = new Vector3(Level4.Level4.Content.rectTransform.localPosition.x, 0f, Level4.Level4.Content.rectTransform.localPosition.z);
        }
        private void OptOutOfInterestBasedAds()
        {
            //bool cur = SdkdsGDPRUtils.Instance.checkIfGDPRAgreedAdStayInformed();
            //SdkdsGDPRUtils.Instance.setGDPRAgreedAdStayInformed(!cur);
        }
        private void OnConfirm()
        {
            if (m_ConfirmAction != null)
            {
                m_ConfirmAction();
            }
            OnCancel();
        }
        private void OnCancel()
        {
            m_ConfirmAction = null;
            OnOpenLevel(3);
        }
        #endregion
    }
}
