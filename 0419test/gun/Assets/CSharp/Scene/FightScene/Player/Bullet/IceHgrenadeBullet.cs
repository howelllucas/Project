using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class IceHgrenadeBullet : BaseBullet
    {
        [Tooltip(" 手雷爆炸范围")]
        public float ExplodeRadio = 3;
        [Tooltip(" 落地 爆战 速度阈值")]
        public float SpeedThreshold = 0.9f;
        [Tooltip(" 起始水平速度 用于 子弹")]
        public float HorizontalSpeed = 15f;
        [Tooltip(" 起始掉落速度 用于 模型显示")]
        public float VerticalSpeed = 15f;
        [Tooltip("落地反弹 速速衰减系数 ")]
        public float VerticalDamp = 0.5f;
        [Tooltip("水平减速度 ")]
        public float HorizontalG = 10f;
        private bool m_CanAtk = false;
        private float m_CurHorizontalSpeed = 0;
        private float m_SpeedTmp = 1;

        public GameObject ExplodEffect;

        [Tooltip("随机 爆炸 子弹 ")]
        public GameObject ExtraBullet;
        [Tooltip("随机爆炸 子弹数量")]
        public int ExtraBulletCount;
        void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            m_CurTime += dtTime;
            if (!m_CanAtk)
            {
                if (dtTime > 0)
                {
                    m_CurHorizontalSpeed = HorizontalSpeed - HorizontalG * m_CurTime;
                    transform.Translate(Vector3.right * m_CurHorizontalSpeed * m_SpeedTmp * BaseScene.GetDtTime(), Space.Self);
                }
            }
        }
        
        public override void Init(double damage, Transform firePoint, float dtAngleZ, float offset, float atkRange = 0)
        {
            base.Init(damage, firePoint, dtAngleZ, offset, atkRange);
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
            InitBulletPose(damage, firePoint, dtAngleZ, offset);
            InitBulletEffect(firePoint);
            m_BulletEffect.GetComponent<BounceTools>().SetBounceInfo(VerticalSpeed, VerticalDamp, SpeedThreshold, firePoint.position.z, EndCall);
        }
        private void EndCall()
        {
            m_SpeedTmp = 0;
            m_CurTime = 0;
            m_CanAtk = true;
            Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
            GetComponent<CircleCollider2D>().radius = ExplodeRadio;
            AddExplodEffect();
            AddExternExplod();
            Destroy(gameObject, 0.3f);
        }
        private void AddExplodEffect() 
        {
            GameObject go = Instantiate(ExplodEffect);
            go.transform.position = transform.position;
        }
        private void AddExternExplod()
        {
            if(ExtraBullet != null)
            {
                for (int i = 0; i < ExtraBulletCount - 1; i++)
                {
                    GameObject extraBullet = Instantiate(ExtraBullet);
                    extraBullet.GetComponentInChildren<HgrenadeExplode>().AddExplodeEffect(m_Damage, transform.position);
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject obj = collision.gameObject;

            if (m_CanAtk)
            {
                if (obj.CompareTag(GameConstVal.MonsterTag))
                {
                    Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
                    Monster monster = collision.gameObject.GetComponent<Monster>();
                    monster.OnHit_Simple(m_Damage,transform);
                    monster.AddBuff(AiBuffType.MoveSpeed, GetBackTime(), -1);
                }
            }
            else
            {
                if (obj.CompareTag(GameConstVal.MapTag) || obj.CompareTag(GameConstVal.ShieldTag))
                {
                    HorizontalSpeed = -HorizontalSpeed;
                    HorizontalG = -HorizontalG;
                }
            }
          
        }
    }
}
