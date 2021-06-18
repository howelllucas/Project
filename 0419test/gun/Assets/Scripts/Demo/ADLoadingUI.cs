using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ADLoadingUI : UIObject
    {
        public int duration;
        public GameObject closeRoot;
        private System.Action doOnFinish = null;
        private IEnumerator Clocker = null;

        protected override void OnOpen(object val)
        {
            base.OnOpen(val);

            if (val != null)
                doOnFinish = val as System.Action;
            closeRoot.SetActive(false);
            Clocker = ClockTimer();
            StartCoroutine(Clocker);

            //var mainUI = UIMgr.singleton.FindUIObject<MainUI>();
            //if (mainUI != null)
            //{
            //    mainUI.audioCom.mute = true;
            //    Debug.Log("Pause");
            //}
        }

        IEnumerator ClockTimer()
        {
            int clock = duration;
            while (clock > 0)
            {
                yield return new WaitForSecondsRealtime(1);
                clock -= 1;
            }

            closeRoot.SetActive(true);
        }

        public void CloseClock()
        {
            if (Clocker != null)
            {
                StopCoroutine(Clocker);
                Clocker = null;
            }
        }

        public void ShowWarning()
        {
            CloseClock();
            closeRoot.SetActive(true);
        }

        public void OnButtonClick()
        {
            if (doOnFinish != null)
                doOnFinish?.Invoke();
            Close();
        }
        protected override void OnClose()
        {
            base.OnClose();

            //var mainUI = UIMgr.singleton.FindUIObject<MainUI>();
            //if (mainUI != null)
            //{
            //    mainUI.audioCom.mute = false;
            //    Debug.Log("UnPause");
            //}
        }
    }
}