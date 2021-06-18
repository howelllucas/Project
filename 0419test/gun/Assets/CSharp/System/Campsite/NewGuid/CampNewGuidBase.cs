using EZ.Data;
using EZ.DataMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public abstract class CampNewGuidBase:MonoBehaviour
    {
        protected CampStepItem m_CampStepItem;
        protected string CampbuildClip = "campbuild";
        public virtual void StartCampStep(int step)
        {
            CampStepItem[] campStepItems = Global.gApp.gGameData.CampStepConfig.items;
            m_CampStepItem = campStepItems[step];
        }

        public void AddPlot(int index,Action endCallBack)
        {
            //if (index < m_CampStepItem.dialogue.Length)
            //{
            //    int plotId = m_CampStepItem.dialogue[index];
            //    Global.gApp.gUiMgr.OpenPanel(Wndid.DialogueUI, plotId.ToString());
            //    DialogueUI dialogueUI = Global.gApp.gUiMgr.GetPanelCompent<DialogueUI>(Wndid.DialogueUI);
            //    dialogueUI.SetAciton(endCallBack);
            //}
            //else
            //{
            //    endCallBack();
            //}
        }
        protected abstract void EndCampStep();
        protected virtual void EndCampRender()
        {
            AddBrage();
            if(m_CampStepItem.brageId <= 0)
            {
                Destroy(this);
            }
        }

        protected void AddBrage()
        {
            if(m_CampStepItem.brageId > 0)
            {
                if(GameItemFactory.GetInstance().GetItem(m_CampStepItem.brageId) == 0d)
                {
                    ItemDTO itemDTO = new ItemDTO(m_CampStepItem.brageId,1, BehaviorTypeConstVal.OPT_CAMP_GENBADGE);
                    GameItemFactory.GetInstance().AddItem(itemDTO);
                }
                Global.gApp.gUiMgr.OpenPanel(Wndid.CampBadgeSuccessUI,this);
            }
        }

        public virtual void BrageClose()
        {
            Destroy(this);
        }
        public CampStepItem GetCampStepItem()
        {
            return m_CampStepItem;
        }
    }
}
