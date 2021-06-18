using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class NetConnectingUI : UIObject
    {
        public float duration = 0.3f;
        public GameObject tipsRoot;

        private IEnumerator Clocker = null;

        protected override void OnOpen(object val)
        {
            base.OnOpen(val);

            tipsRoot.SetActive(false);
            Clocker = ClockTimer();
            StartCoroutine(Clocker);

        }

        IEnumerator ClockTimer()
        {
            float clock = duration;
            while (clock > 0)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                clock -= 0.1f;
            }

            tipsRoot.SetActive(true);
        }

        public void CloseClock()
        {
            if (Clocker != null)
            {
                StopCoroutine(Clocker);
                Clocker = null;
            }
        }


        protected override void OnClose()
        {
            base.OnClose();

            CloseClock();
        }
    }
}