using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EZ
{
    public class RedHeartClick : MonoBehaviour, IPointerEnterHandler
    {

        public Action RespondClick;
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (RespondClick != null)
            {
                RespondClick();
            }
        }
    }
    public class RedHeartEle : MonoBehaviour
    {
        public AudioClip Clip;
        NpcBehavior m_NpcBehavior;
        CampsiteUI m_CampUI;
        int m_DropNum = 0;
        private static string IconBtnName = "IconBtn";
        public void SetInfo(NpcBehavior npcBehavior,int dropNum)
        {
            m_NpcBehavior = npcBehavior;
            if (m_DropNum == 0)
            {
                RedHeartClick iconBtn = transform.Find(IconBtnName).gameObject.AddComponent<RedHeartClick>();
                iconBtn.RespondClick = RespondClick;
            }
            m_DropNum = dropNum;
        }
        public void SetInfo(CampsiteUI campUi, int dropNum)
        {
            m_CampUI = campUi;
            if (m_DropNum == 0)
            {
                RedHeartClick iconBtn = transform.Find(IconBtnName).gameObject.AddComponent<RedHeartClick>();
                iconBtn.RespondClick = RespondClick;
            }
            m_DropNum = dropNum;
        }
        private void RespondClick()
        {
            if(m_NpcBehavior != null && m_DropNum > 0)
            {
                m_NpcBehavior.PickRedHeart(this,m_DropNum);
            }
            if(m_CampUI != null && m_DropNum > 0)
            {
                m_CampUI.PickRedHeart(this, m_DropNum);
            }
            Global.gApp.gAudioSource.PlayOneShot(Clip);
        }
        public void SetPos(Vector3 pos,RectTransform rectTransform)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = UiTools.WorldToRectPos(gameObject, pos, rectTransform);
        }
        public void SetUiPos(Vector2 pos)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
}
