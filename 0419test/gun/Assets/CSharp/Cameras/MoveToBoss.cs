using System;
using System.Collections;
using UnityEngine;
namespace EZ
{
    public class MoveToBoss : MonoBehaviour
    {
        private bool m_Acting;
        private Transform m_DestTsf;
        private Action m_CallBack;
        private float m_EndTime;
        private float m_CurTime = 0;
        private Vector3 m_OriPosition;
        private Vector3 m_DestPosition;
        private AutoCam m_AutoAcam;
        public Animator m_Animator;
        private bool m_MoveToBoss = false;
        void Awake()
        {
            m_AutoAcam = GetComponent<AutoCam>();
            m_Animator.enabled = false;
        }
        public void StartAct(Transform dstTsf, Action callBack, float endTime)
        {
            enabled = true;
            m_DestTsf = dstTsf;
            m_CallBack = callBack;
            m_CurTime = 0;
            m_EndTime = endTime;
            m_OriPosition = transform.position;
            m_DestPosition = dstTsf.position;
            m_AutoAcam.enabled = false;
            m_MoveToBoss = true;
        }
        public void StartRoleDead()
        {
            m_Animator.enabled = true;
            m_Animator.speed = 1;
            enabled = true;
            m_AutoAcam.enabled = false;
            m_Animator.Play(GameConstVal.RoleDead);
        }
        public void StartReborn()
        {
            Global.gApp.gGameCtrl.AddGlobalTouchMask();
            m_Animator.enabled = true;
            m_Animator.speed = 1;
            enabled = true;
            m_AutoAcam.enabled = false;
            m_Animator.Play(GameConstVal.RoleReborn);
            gameObject.AddComponent<DelayCallBack>().SetAction(StartRebornCall,0.67f,true);
        }
        void StartRebornCall()
        {
            m_Animator.enabled = false;
            enabled = false;
            m_AutoAcam.enabled = true;
            Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
        }
        public void StartShowBossAnim(Action callBack)
        {
            m_Animator.enabled = true;
            m_Animator.Play(GameConstVal.ShowBoss);
            m_CallBack = callBack;
            gameObject.AddComponent<DelayCallBack>().SetAction(ShowBossdCallBack, 0.67f,true);
        }
        public void StartShowBossEndAnim(Action callBack)
        {
            m_Animator.enabled = true;
            m_Animator.Play(GameConstVal.ShowBossEnd);
            m_CallBack = callBack;
            gameObject.AddComponent<DelayCallBack>().SetAction(ShowBossdEndCallBack, 0.5f,true);
        }
        public void Ended()
        {
            m_Animator.enabled = false;
            enabled = false;
            m_AutoAcam.enabled = true;
        }
        public void Reset()
        {
            m_Animator.enabled = false;
            enabled = false;
            m_Animator.transform.localPosition = Vector3.zero;
            m_Animator.transform.localEulerAngles = Vector3.zero;
            m_Animator.transform.localScale = new Vector3(1, 1, 1);
        }
        void ShowBossdEndCallBack()
        {
            m_CallBack();
        }
        void ShowBossdCallBack()
        {
            m_CallBack();
        }
        void Update()
        {
            if (m_MoveToBoss)
            {
                m_CurTime = m_CurTime + Time.deltaTime;
                float rate = m_CurTime / m_EndTime;
                rate = Mathf.Min(rate, 1);
                float posX = Mathf.Lerp(m_OriPosition.x, m_DestPosition.x, rate);
                float posY = Mathf.Lerp(m_OriPosition.y, m_DestPosition.y, rate);
                Vector3 pos = new Vector3(posX, posY, m_OriPosition.z);
                transform.position = pos;
                if (rate >= 1)
                {
                    m_MoveToBoss = false;
                    m_CallBack();
                }
            }
        }
    }
}
