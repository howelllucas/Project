using UnityEngine;

namespace EZ
{
    public class GameGuideMgr
    {

        Transform m_CurTsf;
        Transform m_CurTsfOriParent;
        GameObject m_GuideTsf;
        GameObject m_TipTsf;
        Vector3 m_OriPos;
        Vector3 m_OrSize;
        int m_BtnIndex;
        private int m_Active = 1;


        public GameGuideMgr()
        {
            RegisterListener();
        }

        public void ClearAll()
        {
            RmHandGuide(m_CurTsf);
            RmTip();
        }
        void ShowHandGuide(Transform targetUiTsf)
        {
            if (m_Active <= 0)
            {
                return;
            }
            RmHandGuide(m_CurTsf);
            m_OriPos = targetUiTsf.localPosition;
            m_OrSize = targetUiTsf.localScale;
            m_CurTsf = targetUiTsf;
            m_CurTsfOriParent = targetUiTsf.parent;
            m_BtnIndex = m_CurTsf.GetSiblingIndex();
            m_GuideTsf = Global.gApp.gResMgr.InstantiateObj("Prefabs/UI/NGhandUI");
            m_GuideTsf.transform.SetParent(Global.gApp.gUiMgr.GetTopCanvasTsf(), false);
            m_GuideTsf.transform.SetAsFirstSibling();
            m_GuideTsf.GetComponent<NGhandUI>().SetBtnTsf(targetUiTsf);
            //m_GuideTsf.SetActive(m_Active);
        }

        void ShowTip(int id, float y, bool rm, bool addMask)
        {
            if (m_Active <= 0)
            {
                return;
            }
            RmTip();
            m_TipTsf = Global.gApp.gResMgr.InstantiateObj("Prefabs/UI/NGPlotUI");
            m_TipTsf.transform.localPosition = m_TipTsf.transform.localPosition + new Vector3(0,y,0);
            m_TipTsf.transform.SetParent(Global.gApp.gUiMgr.GetTopCanvasTsf(), false);
            //m_TipTsf.transform.SetAsFirstSibling();
            m_TipTsf.GetComponent<NGPlotUI>().ShowText(id);
            //m_TipTsf.SetActive(m_Active);
            if (addMask)
            {
                Global.gApp.gGameCtrl.AddGlobalTouchMask();
            }
            if (rm)
            {
                m_TipTsf.AddComponent<DelayCallBack>().SetAction(() => { RmTip(); }, 2f);
                //Global.gApp.gGameCtrl.GetFrameCtrl().GetScene().GetTimerMgr().AddTimer(2, 1, (dt, end) => { RmTip(); });
            }
            
        }

        void RmTip()
        {
            if (m_TipTsf != null)
            {
                Object.Destroy(m_TipTsf);
                m_TipTsf = null;
                Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
            }
        }

        void RmHandGuide(Transform targetUi)
        {
            if (m_CurTsf != null && m_CurTsf == targetUi)
            {
                if(m_CurTsfOriParent != null)
                {
                    m_CurTsf.SetParent(m_CurTsfOriParent,true);
                    m_CurTsf.SetSiblingIndex(m_BtnIndex);
                    m_CurTsf.localPosition = m_OriPos;
                }
                if (m_GuideTsf != null)
                {
                    Object.Destroy(m_GuideTsf);
                    m_GuideTsf = null;
                }
                m_CurTsf = null;
                m_CurTsfOriParent = null;
                m_GuideTsf = null;
            }
        }

        private void HideGameGuideAD(bool active)
        {
            if (active)
            {
                m_Active++;
            } else
            {
                m_Active--;
            }
            //Debug.Log("HideGameGuideAD = "+ active + " m_Active = " + m_Active);
            if (m_GuideTsf != null)
            {
                m_GuideTsf.SetActive(m_Active > 0);
            }
            if (m_TipTsf != null)
            {
                m_TipTsf.SetActive(m_Active > 0);
            }
            if (m_Active > 0)
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.CheckGameGuide);
            } else
            {
                ClearAll();
            }
        }

        void RegisterListener()
        {
            Global.gApp.gMsgDispatcher.AddListener<Transform>(MsgIds.AddGameGuideUi, ShowHandGuide);
            Global.gApp.gMsgDispatcher.AddListener<Transform>(MsgIds.rmGameGuideUi, RmHandGuide);
            Global.gApp.gMsgDispatcher.AddListener<int, float, bool, bool>(MsgIds.AddGuidePlotUi, ShowTip);
            Global.gApp.gMsgDispatcher.AddListener(MsgIds.RmGuidePlotUi, RmTip);
            Global.gApp.gMsgDispatcher.AddListener<bool>(MsgIds.HideGameGuideAD, HideGameGuideAD);

            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.AddGameGuideUi);
            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.rmGameGuideUi);
            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.AddGuidePlotUi);
            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.RmGuidePlotUi);
            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.HideGameGuideAD);
        }
    }
}
