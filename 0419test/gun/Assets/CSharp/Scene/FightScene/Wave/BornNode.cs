using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class BornNode : MonoBehaviour
    {
        private float m_CurTime = 0;
        private float m_DtTime = 0.5f;
        private bool m_RealOutMap = false;
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
                    m_RealOutMap = true;
                        //Global.gApp.CurScene.GetWaveMgr().GetOnlyWave().StartCreateMonster(m_localPosition + m_ParentTsf.position);
                }
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
            }
        }
        public bool GetIsOutMap()
        {
            return m_RealOutMap;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == GameConstVal.StaticMapLayer)
            {
                m_NoEnterMap = false;
                m_RealOutMap = false;
                m_CurTime = 0;
                m_TriggerCollider.Add(collision);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == GameConstVal.StaticMapLayer)
            {
                m_NoEnterMap = false;
                m_RealOutMap = false;
                m_CurTime = 0;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == GameConstVal.StaticMapLayer)
            {
                m_TriggerCollider.Remove(collision);
                if(m_TriggerCollider.Count == 0)
                {
                    m_NoEnterMap = true;
                    m_CurTime = 0;
                }
            }
        }
    }
}
