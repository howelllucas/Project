using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class SprayVenomBullet : BaseBullet
    {
        [SerializeField] private GameObject DimianBullet;
        void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if (m_CurTime < m_LiveTime)
            {
                transform.Translate(Vector3.right * m_Speed * BaseScene.GetDtTime(), Space.Self);
            }
            else
            {
                AddDiMianBullet();
                Destroy(gameObject);
            }
        }

        public override void Init(double damage, Transform firePoint, float dtAngleZ , float offset,float atkRange = 0)
        {
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
            InitBulletPose(damage, firePoint, dtAngleZ, offset);
            InitBulletEffect(firePoint);
            if (DimianBullet != null)
            {
                m_BulletEffect.transform.SetParent(Global.gApp.gBulletNode.transform, true);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject obj = collision.gameObject;

            if (obj.CompareTag(GameConstVal.MapTag))
            {

                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                OnHitted();
            }
            else if (obj.CompareTag(GameConstVal.DamageRangeTag))
            {
                Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                OnHitted();
            }
        }
        private void AddDiMianBullet()
        {
            if (DimianBullet != null)
            {
                GameObject effect = Instantiate(DimianBullet);
                Vector3 pos = transform.position;
                pos.z = 0;
                effect.transform.position = pos;
            }
        }
        private void OnHitted()
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            GameObject effect = Instantiate(HittedEnemyEffect);
            effect.transform.position = transform.position;
            Destroy(effect, 1);
            if(DimianBullet == null)
            {
                Destroy(gameObject);
            }
        }
    }
}
