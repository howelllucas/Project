using DG.Tweening;
using EZ.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class CheckNetTypeConstVal
    {
        //正常
        public static string RIGHT = "0";
        //没网
        public static string NO_NET = "1";
        //改了时间
        public static string CHANGE_TIME = "2";

    }

    public partial class CheckNetUI
    {
        private string m_Type;
        public Action m_RightAction;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            m_Type = arg as string;
            UIFresh();

            RegisterListeners();

            base.ChangeLanguage();

            Btn1.button.onClick.AddListener(OnClose);
            Btn2.button.onClick.AddListener(OnRetry);
        }

        private void OnRetry()
        {
            string check = DateTimeUtil.CheckWebTime();
            if (check.Equals(CheckNetTypeConstVal.RIGHT))
            {
                TouchClose();
                m_RightAction();
            }
            if (!m_Type.Equals(check))
            {
                m_Type = check;
                UIFresh();

                Tips.text.text = Global.gApp.gGameData.GetTipsInCurLanguage(Tips.languageTip.TipId);
            }
        }

        private void OnClose()
        {
            Application.Quit();
        }

        private void UIFresh()
        {
            if (m_Type.Equals(CheckNetTypeConstVal.NO_NET))
            {
                Tips.languageTip.TipId = 4198;
            } else if (m_Type.Equals(CheckNetTypeConstVal.CHANGE_TIME))
            {
                Tips.languageTip.TipId = 4201;
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
