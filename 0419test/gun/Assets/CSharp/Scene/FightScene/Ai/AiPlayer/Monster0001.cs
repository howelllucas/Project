using EZ.Data;
using UnityEngine;
namespace EZ
{
    public class Monster0001 : Monster
    {
        [Tooltip("死亡之后需要隐藏的节点，一般 2014 使用")]
        public GameObject m_DeadHideNode;
        public GameObject m_DeadBullet;
        [SerializeField]private float m_AppendDeadTime;
        private float m_BaseSpeed;
        private Vector2 m_Speed;
        private Vector2 m_DeadThSpeed;
        private bool m_UseRightBodyVec = true;
        public ParticleSystem CtlParticle;
        public override void InitForCache(GameObject player, Wave wave, MonsterItem monsterItem)
        {
            base.InitForCache(player, wave, monsterItem);
            CacheDeadthEffect(5);
        }
        protected override void RecycleSelf()
        {
            base.RecycleSelf();
            if (CtlParticle != null)
            {
                CtlParticle.Stop();
            }
        }
        public override void Init( GameObject player, Wave wave, EZ.Data.MonsterItem monster)
        {
            base.Init(player, wave, monster);
            if(m_DeadHideNode != null)
            {
                m_DeadHideNode.SetActive(true);
            }
            m_UseRightBodyVec = true;
            m_Collider2D.isTrigger = false;
            gameObject.layer = GameConstVal.MonsterLayer;
            m_BaseSpeed = monster.baseSpeed;
            if (Global.gApp.CurScene.GetSceneType()  == SceneType.BreakOutSene)
            {
                m_DeadThSpeed = new Vector2(0, -m_BaseSpeed * 0.7f);
                m_BaseSpeed *= 0.7f;
            }
            else
            {
                m_DeadThSpeed = new Vector2(0, -m_BaseSpeed * 0.8f);
            }
            m_Speed = new Vector2(0, -m_BaseSpeed);

            transform.localEulerAngles = new Vector3(0, 0, 180);
            if (CtlParticle != null)
            {
                CtlParticle.Play();
            }
        }

        public override void Update()
        {
            base.Update();
            float dtTime = BaseScene.GetDtTime();
            if (dtTime > 0)
            {
                dtTime = 0.0333333f;
                if (m_UseRightBodyVec)
                {
                    if (!m_InDeath)
                    {
                        SetSpeed(m_Speed);
                    }
                    else
                    {
                        SetSpeed(m_DeadThSpeed);
                    }
                }
                else
                {
                    SetSpeed(Vector2.zero);
                    if (!m_InDeath)
                    {
                        transform.Translate(m_Speed * dtTime, Space.World);
                    }
                    else
                    {
                        transform.Translate(m_DeadThSpeed * dtTime, Space.World);
                    }
                }
            }
            else
            {
                SetSpeed(Vector2.zero);
            }
        }
        protected override void SetCollisionEnable(bool enable)
        {
            m_Rigidbody2D.simulated = enable;
            m_Collider2D.enabled = enable;
        }
        public override void OnHittedDeath(double damage, bool ingnoreEffect = false, bool hitByCarrier = false)
        {
            if (InDeath) { return; }
            if (!ingnoreEffect)
            {
                EffectNode deadEffect = AddDeadEffect(hitByCarrier);
                if(EffectConfig.EffectPath.ContainsKey(Deadth))
                {
                    deadEffect.transform.SetParent(transform, true);
                    deadEffect.transform.position = transform.position;
                    m_DeadTime = Mathf.Max(deadEffect.GetLiveTime(), m_DeadTime);
                }
            }
            base.OnHittedDeath(damage, true,hitByCarrier);
            Global.gApp.gShakeCompt.StartShake();
            if (m_DeadHideNode != null)
            {
                m_DeadHideNode.SetActive(false);
            }
            if (m_DeadBullet != null)
            {
                GameObject venomBullet = Instantiate(m_DeadBullet);
                venomBullet.transform.position = transform.position;
                venomBullet.transform.SetParent(transform, true);
                m_DeadTime += m_AppendDeadTime;
                venomBullet.GetComponent<BaseBullet>().SetLiveTime(m_DeadTime);
            }
        }
        private void OnBecameVisible()
        {
            gameObject.layer = GameConstVal.CrossMonsterLayer;
            m_Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            m_Collider2D.isTrigger = true;
            m_UseRightBodyVec = false;
        }
        protected override void CheckLayerChanged()
        {
        }
    }
}
