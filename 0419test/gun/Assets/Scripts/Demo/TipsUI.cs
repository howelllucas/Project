using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Tool;

namespace Game.UI
{
    public class TipsUI : InnerMonoSingleton<TipsUI>
    {
        public Text tipTxt;
        public TweenBase closeTween;
        public GameObject mask;

        protected override void InitSingleton()
        {
            base.InitSingleton();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            CloseTipImmediately();
        }

        public void ShowTip(TipsInfo info)
        {
            CancelInvoke();
            tipTxt.text = info.tipStr;
            closeTween.Stop();
            closeTween.ForceTo(0);
            closeTween.gameObject.SetActive(true);
            mask.SetActive(info.openMask);
            if (info.autoClose)
            {
                if (info.duration > 0)
                    Invoke("CloseTip", info.duration);
                else
                    CloseTip();
            }
        }

        public void CloseTip()
        {
            closeTween.Play(CloseTipImmediately);
        }

        public void CloseTipImmediately()
        {
            closeTween.gameObject.SetActive(false);
            mask.SetActive(false);
        }
    }

    public class TipsInfo
    {
        public string tipStr;
        public bool openMask;
        public bool autoClose;
        public float duration;
    }
}