using System;
using UnityEngine;
namespace EZ
{
    public class BubbleEffectNode :MonoBehaviour 
    {
        public GameObject ExplodeBullet;
        public float LiveTime = 1;
        public float m_CurTime = 0;
        private double m_Damge;
        private void Update()
        {

            m_CurTime += BaseScene.GetDtTime();
            if(m_CurTime > LiveTime)
            {
                DestroyEffect();
                Destroy(gameObject);
            }
        }
        public void SetDamage(double damage)
        {
            m_Damge = damage;
        }
        public void Restart()
        {
            m_CurTime = 0;
        }
        public void DestroyEffect()
        {
            if (ExplodeBullet != null)
            {
                GameObject newBullet = Instantiate(ExplodeBullet);
                newBullet.GetComponentInChildren<RocketExplode>().SetDamage(m_Damge);
                newBullet.transform.position = transform.position;
            }
        }
    }
}
