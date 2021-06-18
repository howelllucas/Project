using UnityEngine;
using System.Collections;
using EZ.DataMgr;
using EZ.Data;
using System.Collections.Generic;
using System;

namespace EZ
{
    public partial class Bubble
    {
        private bool m_HasTouch = false;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            int tipId = int.Parse(arg as string);
            desc.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(tipId);
            base.ChangeLanguage();
        }

        public void SetPos(float x, float y, float arrowX, float arrowY, float arrowRotationZ, Vector3 v)
        {
            gameObject.GetComponent<RectTransform>().position = v;
            Content.rectTransform.localPosition = new Vector3(x, y, 0);
            Arrow.rectTransform.localPosition = new Vector3(arrowX, arrowY, 0);
            Arrow.rectTransform.localEulerAngles = new Vector3(0, 0, arrowRotationZ);
        }

        private void Update()
        {
            if (m_HasTouch)
            {
                return;
            }
#if ((UNITY_ANDROID || UNITY_IOS || UNITY_WINRT || UNITY_BLACKBERRY) && !UNITY_EDITOR)
            int count = Input.touchCount;
            if (count > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    m_HasTouch = true;
                }
            }
          
#else
            if (Input.GetMouseButtonDown(0))
            {
                m_HasTouch = true;
            }
#endif

            if (m_HasTouch)
            {
                TouchClose();
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

