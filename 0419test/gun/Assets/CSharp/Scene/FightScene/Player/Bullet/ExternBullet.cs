using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class ExternBullet : BaseBullet
    {
        void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (m_CurTime >= m_LiveTime)
            {
                Recycle();
            }
        }

        public override void Init(double damage, Transform firePoint, float dtAngleZ, float offset, float atkRange = 0)
        {
            base.Init(damage, firePoint, dtAngleZ, offset, atkRange);
            Player player = Global.gApp.CurScene.GetMainPlayerComp();
            transform.SetParent(player.RoleNode, false);
            transform.localPosition = new Vector3(0, 0.8f, 0);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject obj = collision.gameObject;

            if (obj.CompareTag(GameConstVal.MonsterTag))
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Vec(m_Damage, transform);
            }
        }
    }
}
