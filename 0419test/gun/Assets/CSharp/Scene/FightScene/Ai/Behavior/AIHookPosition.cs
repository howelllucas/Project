using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class AIHookPosition : MonoBehaviour
    {
        private float m_CurTime = 0;
        private float m_DtTime = 0.5f;
        private bool m_CanUseSkill = false;
        private bool m_NoEnterMap = true;
        private Transform m_ParentTsf;
        private Vector3 m_localPosition;
        private List<Collider2D> m_TriggerCollider = new List<Collider2D>();
        private void Start()
        {
            m_localPosition = transform.localPosition;
            m_ParentTsf = transform.parent;
        }
        private void Update()
        {
            if (m_NoEnterMap)
            {
                if (m_CurTime >= m_DtTime)
                {
                    m_CanUseSkill = true;
                }
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
            }
        }
        public bool CanUseSkill()
        {
            return m_CanUseSkill;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MapTag))
            {
                m_NoEnterMap = false;
                m_CanUseSkill = false;
                m_CurTime = 0;
                m_TriggerCollider.Add(collision);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MapTag))
            {
                m_NoEnterMap = false;
                m_CanUseSkill = false;
                m_CurTime = 0;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MapTag))
            {
                m_TriggerCollider.Remove(collision);
                if (m_TriggerCollider.Count == 0)
                {
                    m_NoEnterMap = true;
                    m_CurTime = 0;
                }
            }
        }
    }
}

