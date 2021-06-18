using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{

    public partial class FightPause
    {

        private bool m_HasCallResume = false;
        private bool m_HasCallPause = false;
        public override void Init<T>(string name, UIInfo info, T arg)
        {

            Pause();
            base.Init(name, info, arg);
            FlushSoundState();

            Btn1.button.onClick.AddListener(OnClickQuit);
            Btn2.button.onClick.AddListener(TouchClose);
            BtnC.button.onClick.AddListener(TouchClose);

            BtnmusicOff.button.onClick.AddListener(OnClickMusicOff);
            BtnmusicOn.button.onClick.AddListener(OnClickMusicOn);
            BtnvolOff.button.onClick.AddListener(OnClickAudioOff);
            BtnvolOn.button.onClick.AddListener(OnClickAudioOn);
            BtnVibeOff.button.onClick.AddListener(OnClickVibeOff);
            BtnVibeOn.button.onClick.AddListener(OnClickVibeOn);

            base.ChangeLanguage();
        }

        #region 

        private void OnClickResume()
        {
            Resume();
        }

        private void OnClickQuit()
        {
            Resume();
            if (Global.gApp.CurScene is CampsiteFightScene)
                Global.gApp.gGameCtrl.ChangeToMainScene(3);
            else
                Global.gApp.gGameCtrl.ChangeToMainScene(2);

        }
        public override void TouchClose()
        {
            Resume();
            base.TouchClose();
        }
        private void Pause()
        {
            if (!m_HasCallPause)
            {
                m_HasCallPause = true;
                Global.gApp.gAudioSource.Pause();
                Global.gApp.CurScene.Pause();
            }
        }
        private void Resume()
        {
            if (!m_HasCallResume)
            {
                m_HasCallResume = true;
                Global.gApp.gAudioSource.UnPause();
                Global.gApp.CurScene.Resume();
            }
        }

        public override void Release()
        {
            Resume();
            base.Release();
        }
        #endregion

        #region

        private void FlushSoundState()
        {

            bool musicOn = Global.gApp.gAudioSource.musicVolumn == 1;
            bool audioOn = Global.gApp.gAudioSource.audioVolumn == 1;
            bool vibeOn = Global.gApp.gAudioSource.vibe == 1;

            BtnmusicOff.gameObject.SetActive(!musicOn);
            BtnmusicOn.gameObject.SetActive(musicOn);
            BtnvolOff.gameObject.SetActive(!audioOn);
            BtnvolOn.gameObject.SetActive(audioOn);
            BtnVibeOff.gameObject.SetActive(!vibeOn);
            BtnVibeOn.gameObject.SetActive(vibeOn);
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

        #endregion
    }
}
