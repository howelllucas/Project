using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class VenomBullet : BaseBullet
    {
        [SerializeField]private GameObject m_DamageEffect;
        private EffectNode m_EffectNode;
        private void Awake()
        {
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
            m_EffectNode = Global.gApp.gGameCtrl.EffectCache.GetEffect(EffectConfig.Death2001);
            m_EffectNode.transform.localPosition = Vector3.zero;
            m_EffectNode.transform.SetParent(transform, false);
        }
        void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();

            if (m_CurTime >= m_LiveTime)
            {
                DestroyBullet();
            }
        }

        private void DestroyBullet()
        {
            m_EffectNode.Stop();
            m_EffectNode = null;
            Destroy(gameObject);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
            {
                GameObject obj = Instantiate(m_DamageEffect);
                obj.transform.SetParent(collision.gameObject.transform, false);
            }
        }
    }
}
