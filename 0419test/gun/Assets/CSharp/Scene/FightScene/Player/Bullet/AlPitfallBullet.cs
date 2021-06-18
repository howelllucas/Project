using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class AlPitfallBullet : BaseBullet
    {
        [SerializeField]private float m_MonsterDamage = 1;
        void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (m_CurTime >= m_LiveTime)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                collision.gameObject.GetComponent<Monster>().OnHit_Pos(m_MonsterDamage, transform);
            }
        }
    }
}
