using EZ.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;

namespace EZ
{

    public class BaseUi : MonoBehaviour
    {
        private string m_Name = null;
        protected UIInfo m_UiInfo;
        private bool m_TouchMaskFlag = false;
        private bool m_Inited = false;
        public int UILayer { get { return m_UiInfo != null ? m_UiInfo.Layer : 0; } }

        private string m_Language = GameConstVal.EmepyStr;

        public virtual void Init<T>(string name,UIInfo info,T arg)
        {
            if (!m_Inited)
            {
                m_Name = name;
                m_UiInfo = info;
                if (Wndid.gWndInfo.TryGetValue(name, out m_UiInfo))
                {
                    CreateTouchMask();
                }
                InitOnceInfo();
            }
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.Language, ChangeLanguage);
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.Language, ChangeLanguage);

            BubbleTip[] bts = gameObject.GetComponentsInChildren<BubbleTip>(true);
            foreach (BubbleTip bt in bts)
            {
                bt.AddButton();
            }
        }

        public virtual void ChangeLanguage()
        {
            string lgg = Global.gApp.gSystemMgr.GetMiscMgr().Language;
            if (lgg == null || lgg.Equals(GameConstVal.EmepyStr))
            {
                lgg = UiTools.GetLanguage();
            }
            if (!lgg.Equals(m_Language))
            {
                m_Language = lgg;
                Text[] ts = gameObject.GetComponentsInChildren<Text>(true);
                foreach (Text t in ts)
                {
                    t.font = Global.gApp.gGameData.GetFont(lgg);
                    if (UiTools.IsNumeric(t.text))
                    {
                        continue;
                    }
                    LanguageTip lt = t.GetComponent<LanguageTip>();
                    if (lt != null)
                    {
                        //Debug.Log(t.text + " use tip = " + lt.TipId);
                        t.text = Global.gApp.gGameData.GetTipsInCurLanguage(lt.TipId);
                    }
                    else
                    {
                        //Debug.Log("text = " + t.text + ", don't add component LanguageTip");
                    }
                }
            }
            
        }

        protected virtual void InitOnceInfo()
        {
            m_Inited = true;

            //给按钮添加音效
            Button[] btns = gameObject.GetComponentsInChildren<Button>(true);
            foreach (Button btn in btns)
            {
                btn.onClick.AddListener(Global.gApp.gAudioSource.CommonClickAutio);
            }
        }
        private void CreateTouchMask()
        {
            if (!m_UiInfo.NoTouchMask && !m_Inited)
            {
                GameObject touchMask = Global.gApp.gResMgr.InstantiateObj("Prefabs/UI/TouchMask") ;
                if(m_UiInfo.TouchMaskName == null)
                {
                    touchMask.transform.SetParent(transform, false);
                }
                else
                {
                    Transform maskNode = transform.Find(m_UiInfo.TouchMaskName);
                    touchMask.transform.SetParent(maskNode, false);
                }
                touchMask.transform.SetAsFirstSibling();
                if (m_UiInfo.TouchEmptyClose)
                {
                    touchMask.GetComponent<TouchMask>().AddCloseListener(m_Name,this);
                }
            }
        }

        protected virtual void BindEvents(List<Button> allBtn)
        {
            foreach (Button btn in allBtn)
            {
                btn.onClick.AddListener(
                    () =>
                    {
                        ClickCallBack(btn.gameObject);
                    }
                    );
            }
        }
        protected virtual void BindEvents(List<string> allBtn)
        {
            foreach (string btnName in allBtn)
            {
                Transform btnTsf = transform.Find(btnName);
                Button btn = btnTsf.GetComponent<Button>();
                btn.onClick.AddListener(
                    () =>{
                            ClickCallBack(btnTsf.gameObject);
                        }
                    );
            }
        }
        protected virtual void BindEvents<T>(List<string> allBtn, List<T> args)
        {

            int index = 0;
            foreach (string btnName in allBtn)
            {
                Transform btnTsf = transform.Find(btnName);
                Button btn = btnTsf.GetComponent<Button>();
                T val = args[index];
                index++;
                btn.onClick.AddListener(
                    () =>
                    {
                        ClickCallBack(btnTsf.gameObject, val);
                    }
                    );
            }
        }
        protected virtual void ClickCallBack<T>(GameObject obj, T arg = default(T))
        {

        }
        protected void OnClick()
        {

        }
        protected virtual void ClickCallBack(GameObject obj)
        {

        }
        public string GetName()
        {
            return m_Name;
        } 
        public virtual void Release()
        {
            RemoveTouchMask();
            if (m_UiInfo != null && m_UiInfo.AddRoot && GetComponentInChildren<Canvas>() == null)
            {
                Destroy(gameObject.transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            Global.gApp.gMsgDispatcher.RemoveListener(MsgIds.Language, ChangeLanguage);
        }
        public virtual void TouchClose()
        {
            Global.gApp.gUiMgr.ClosePanel(m_Name);
        }
        public virtual void Recycle()
        {
            RemoveTouchMask();
        }
        protected void RemoveTouchMask()
        {
            if (m_TouchMaskFlag)
            {
                m_TouchMaskFlag = false;
                Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
            }
        }
        protected void AddTouchMask()
        {
            if (!m_TouchMaskFlag)
            {
                m_TouchMaskFlag = true;
                Global.gApp.gGameCtrl.AddGlobalTouchMask();
            }
        }

        internal virtual void OnServerTimeFixed(bool success)
        {

        }

        internal virtual void OnDateTimeRefresh(DateTimeRefreshType type)
        {

        }
    }
}
