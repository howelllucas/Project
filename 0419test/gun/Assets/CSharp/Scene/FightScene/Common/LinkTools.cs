using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{

    public class LinkTools : MonoBehaviour
    {

        private Monster m_StartMonster;
        private Monster m_EndMonster;

        public void Init(Monster startMonster,Monster endMonster)
        {
            if (startMonster.GetBodyNode().position.z > endMonster.GetBodyNode().position.z)
            {
                m_StartMonster = startMonster;
                m_EndMonster = endMonster;
            }
            else
            {
                m_StartMonster = endMonster;
                m_EndMonster = startMonster;
            }
            gameObject.AddComponent<DelayDestroy>().SetLiveTime(0.3f);
            CalcPos();

        }
        void Update()
        {
            if(m_StartMonster.InDeath || m_EndMonster.InDeath)
            {
                enabled = false;
            }
            else
            {
                CalcPos();
            }
        }
        void CalcPos()
        {
            float magnitude = (m_StartMonster.transform.position - m_EndMonster.transform.position).magnitude;
            transform.localScale = new Vector3(1, 1, magnitude / 2.3f);
            Vector3 newPos = m_StartMonster.GetBodyNode().position;
            transform.position = newPos;
            float angleZ = EZMath.SignedAngleBetween(m_EndMonster.transform.position - m_StartMonster.transform.position, Vector3.up);
            transform.localEulerAngles = new Vector3(0, 0, angleZ + 90);
        }
    }
}
