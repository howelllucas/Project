using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class SniperrifleBullet : BaseBullet
    {
        [SerializeField] private float m_StartDamageTime = 1.8f;
        private bool m_Enable = false;
        private bool m_HitedPlayer = false;
        void Update()
        {
            if (!m_IsInDelayDestroy)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (m_CurTime >= m_LiveTime)
                {
                    GameObject effect = Global.gApp.gResMgr.InstantiateObj("Prefabs/Effect/enemy/2005_miss");
                    effect.transform.position = transform.position;
                    Destroy(gameObject);
                }
                else if (m_CurTime >= m_StartDamageTime && !m_Enable)
                {
                    m_Enable = true;
                    GetComponent<Collider2D>().enabled = true;
                    GetComponent<Rigidbody2D>().simulated = true;
                }
            }
        }

        public void Init(Vector3 position)
        {
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
            float angel = Random.Range(0, 360);
            float radio = Random.Range(0.0f, 1.0f );
            Vector3 newPos = position;
            newPos.x += 2 * Mathf.Cos(angel);
            newPos.y += 2 * Mathf.Sin(angel);
            transform.position = position * radio + newPos * (1 - radio);

            InitBulletEffect(transform);
            m_BulletEffect.GetComponent<DelayDestroy>().SetLiveTime(m_LiveTime);
            m_BulletEffect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            m_BulletEffect.transform.localPosition = new Vector3(0, 0, -0.15f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_IsInDelayDestroy)
            {
                return;
            }
            if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
            {
                collision.gameObject.GetComponentInParent<Player>().StartBackActOnVec(transform);
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);

                GameObject effect = Global.gApp.gResMgr.InstantiateObj("Prefabs/Effect/enemy/2005_hit");
                effect.transform.SetParent(collision.gameObject.transform, false);
                m_HitedPlayer = true;
                OnHitted();
            }
        }
        private void OnHitted()
        {
            m_IsInDelayDestroy = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject,0.1f);
        }
    }
}
