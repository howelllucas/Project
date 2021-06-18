using DG.Tweening;
using EZ.Data;
using EZ.DataMgr;
using EZ.Util;
using Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class ShopUI
    {
        private ShopGroupUI[] groupArr;

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            groupArr = GetComponentsInChildren<ShopGroupUI>();
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            if (groupArr != null)
            {
                for (int i = 0; i < groupArr.Length; i++)
                {
                    groupArr[i].Init();
                }
            }
            FocusGroup(arg as string);
            RegisterListeners();
        }

        private void FocusGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
                return;
            for (int i = 0; i < groupArr.Length; i++)
            {
                var group = groupArr[i];
                if (group != null && group.groupName == groupName)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(Content.rectTransform);
                    Content.rectTransform.localPosition = new Vector3(Content.rectTransform.localPosition.x, -group.Pos - Viewport.rectTransform.rect.height / 2, Content.rectTransform.localPosition.z);
                    group.PlayShowAnim();
                    break;
                }
            }
        }

        public override void Recycle()
        {
            base.Recycle();
            UnregisterListener();
        }

        public override void Release()
        {
            base.Release();
            UnregisterListener();
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.ShopBuySuccess, Refresh);
        }

        private void UnregisterListener()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.ShopBuySuccess, Refresh);
        }

        //每次购买成功后调用
        private void Refresh(int changeItemId = -1)
        {
            if (groupArr != null)
            {
                for (int i = 0; i < groupArr.Length; i++)
                {
                    groupArr[i].Refresh(changeItemId);
                }
            }
        }

    }
}
