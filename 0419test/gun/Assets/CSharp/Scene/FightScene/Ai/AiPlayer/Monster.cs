using EZ.Data;
using EZ.DataMgr;
using UnityEngine;
namespace EZ
{
    public abstract class Monster : MonoBehaviour
    {
        public enum MonsterType
        {
            Normal = 1,
            Elites = 2,
            Boss = 3,
        }
        [SerializeField] protected string Deadth;
        public float Weight = 1;

        [SerializeField] private Animator MAnimator;
        [SerializeField] protected GPUSkinningPlayerMono SkinePlayerAnim;
        [SerializeField] private float BackStartSpeed = 0;
        [SerializeField] private float BackEndSpeed = 0f;
        [SerializeField] private float BackTime = 0f;

        protected AIPursueAct m_PursureAct;
        protected AIBeatBackAct m_BeatBackAct;
        protected Vector3 m_OriScale;
        protected bool m_InDeath = false;
        protected Wave m_Wave;
        protected double m_Hp;
        protected double m_MaxHp = 1;
        //protected float m_Damage = 0;
        //public float Damage
        //{
        //    get { return m_Damage; }
        //}
        protected bool m_UsingSkill;
        protected int m_Guid;
        protected int m_MonsterId;
        protected GameObject m_PlayerGo;
        protected Transform m_BodyNode;
        protected Transform m_HpNode;
        protected Rigidbody2D m_Rigidbody2D;
        protected Collider2D m_Collider2D;
        protected Player m_Player;
        protected float m_CurDeadTime = 0;
        protected float m_DeadTime = 0;
        private MonsterItem m_MonsterItem;
        private AIPauseAct m_AiPauseAct;
        [HideInInspector]
        public AiBuffMgr m_AiBuffMgr;
        AiBase[] m_AiBase;
        private float m_EffectDtTime = 0.05f;
        private float m_CurEffectTime = 0.05f;

        private float m_DamageParam = 1f;
        private float m_OriSpeed = 1;
        private float m_CurSpeed = 1;
        private int m_OriLayer = -100;
        public bool InCameraView
        {
            get; set;
        }
        private bool m_InShadowView = false; 
        public bool InShadowView
        {
            get
            {
                return m_InShadowView;
            }
            set
            {
                m_InShadowView = value;
                ShadowViewStateChanged();
                CheckLayerChanged();
            }
     
        }
        public bool InDeath
        {
            get
            {
                return m_InDeath;
            }

            set
            {
                if (value && m_DeadTime == 0)
                {
                    if(m_MonsterItem != null && MAnimator != null)
                    {
                        if (m_Wave != null)
                        {
                            m_DeadTime = m_Wave.GetDeadTime(MAnimator, m_MonsterItem);
                        }
                    }
                    else if(SkinePlayerAnim != null)
                    {
                        m_DeadTime = SkinePlayerAnim.GetDeadTime();
                    }
                    else
                    {
                        m_DeadTime = 1.0f;
                    }
                }
                if (value)
                {
                    SetRightBodyType(RigidbodyType2D.Dynamic);
                    SetAbsAnimSpeed(1);
                }
                m_InDeath = value;
                if (m_InDeath)
                {
                    if (m_AiBuffMgr != null)
                    {
                        m_AiBuffMgr.ClearBuff();
                    }
                }
            }
        }

        private float m_OriMass;
        protected bool m_DeadthBackAct = false;
        protected bool m_NormalBackAct = true;
        protected bool m_IsNormalMonster = false;
        public virtual void InitForCache(GameObject player, Wave wave, EZ.Data.MonsterItem monsterItem)
        {
            m_UsingSkill = false;
            enabled = true;
            InDeath = false;
            m_Wave = wave;
            m_CurDeadTime = 0;
            InitOnceInfo(player, monsterItem);
            SetCollisionEnable(false);
            PlayAnim(GameConstVal.Run, -1, Random.Range(0.0f, 1.0f));
            RecycleSelf();
        }

        public virtual void Init(GameObject player, Wave wave,EZ.Data.MonsterItem monsterItem)
        {
            m_UsingSkill = false;
            enabled = true;
            InDeath = false;
            m_Wave = wave;
            m_CurDeadTime = 0;
            m_CurEffectTime = m_EffectDtTime;
            if (m_AiBuffMgr != null)
            {
                m_AiBuffMgr.ClearBuff();
            }
            InitOnceInfo(player, monsterItem);
            if (SkinePlayerAnim != null)
            {
                SkinePlayerAnim.enabled = true;
            }
            PlayAnim(GameConstVal.Run, -1, Random.Range(0.0f, 1.0f));
            SetAnimSpeed(1);

            InitNormalInfo(player, monsterItem);
            SetCollisionEnable(true);
            SetRightBodyType(RigidbodyType2D.Dynamic);
            m_Hp = m_MaxHp;

            Debug.Log("m_Hp " + m_Hp);
            if (monsterItem.hasHpSlider > 0)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int, Transform>(MsgIds.AddMonsterHpUi, m_Guid,m_HpNode);
            }
            if(monsterItem.hasShowAct > 0)
            {
                AIBossShow3001 show = gameObject.AddComponent<AIBossShow3001>();
                show.Init(player);
                show.AddApperaEffect();
            }
            if (m_AiPauseAct != null)
            {
                m_AiPauseAct.Init(this);
            }
            else
            {
                SetActEnable(true);
            }
    
        }
        public void SetActEnable(bool enable)
        {
            if (m_AiBase != null)
            {
                foreach (AiBase aiBase in m_AiBase)
                {
                    aiBase.enabled = enable;
                }
            }
        }
        private void InitNormalInfo(GameObject player, EZ.Data.MonsterItem monsterItem)
        {
            //PassItem passData = m_Wave.GetPassData();
            //if ((PassType)passData.passType == PassType.MainPass)
            //{
            m_MaxHp = monsterItem.baseHp * Game.PlayerDataMgr.singleton.StageHpParam;
            //m_Damage = 25 * (1 + 0.25f) ^ (怪攻击等级 - 1);
            //m_Damage = Mathf.Ceil(25 * Mathf.Pow((1 + 0.25f), (Game.PlayerDataMgr.singleton.StageAtkLevel - 1)));
            //}
            //else
            //{
            //    PassItem passItem = Global.gApp.gSystemMgr.GetPassMgr().GetPassItem();
            //    if (passItem != null)
            //    {
            //        m_MaxHp = monsterItem.baseHp * passItem.hpParam * passData.hpParam;
            //    }
            //    else
            //    {
            //        m_MaxHp = monsterItem.baseHp * passData.hpParam;
            //    }
            //    if ((SceneType)passData.sceneType == SceneType.BreakOutSene)
            //    {
            //        transform.localScale = m_OriScale * 1.6f;
            //    }
            //    else
            //    {
            //        transform.localScale = m_OriScale;
            //    }
            //}
            //m_MaxHp *= m_Wave.GetHpParam();
        }
        private void InitOnceInfo(GameObject player, EZ.Data.MonsterItem monsterItem)
        {
            if(m_PlayerGo != null)
            {
                return;
            }
            if (SkinePlayerAnim != null)
            {
                SkinePlayerAnim.Init();
            }
            m_OriLayer = gameObject.layer;
            m_BodyNode = transform.Find(GameConstVal.BodyNode);
            m_HpNode = transform.Find(GameConstVal.HpNode);
            m_AiBase = GetComponents<AiBase>();
            m_MonsterId = monsterItem.tag;
            m_PlayerGo = player;
            m_Player = player.GetComponent<Player>();
            m_MonsterItem = monsterItem;
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_OriMass = m_Rigidbody2D.mass;
            m_Collider2D = GetComponent<Collider2D>();
            m_AiPauseAct = GetComponentInChildren<AIPauseAct>();
            m_OriScale = transform.localScale;
            m_OriSpeed = monsterItem.aniSpeed;
            if (monsterItem.type == (int)MonsterType.Boss)
            {
                int skillLevel = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(GameConstVal.SExBossHarm);
                Skill_dataItem skillLevelData = Global.gApp.gGameData.SkillDataConfig.Get(skillLevel);
                m_DamageParam = (skillLevelData == null) ? 1f : skillLevelData.skill_exbossharm[0];
                m_DeadthBackAct = false;
            }
            if(monsterItem.type == (int)MonsterType.Normal)
            {
                m_IsNormalMonster = true;
            }
            else
            {
                m_IsNormalMonster = false;
            }
        }
        public virtual void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            if (InDeath)
            {
                m_CurDeadTime += dtTime;
                if (m_CurDeadTime >= m_DeadTime)
                {
                    RecycleSelf();
                }
            }
            else
            {
                if (m_AiBuffMgr != null)
                {
                    m_AiBuffMgr.Update(dtTime);
                }
            }
        }
        public virtual void OnHittedDeathByEndBorder()
        {
            DeadthSimple(true);
            RecycleSelf();
        }
        protected void CacheDeadthEffect(int initCount)
        {
            if (EffectConfig.EffectPath.ContainsKey(Deadth))
            {
               Global.gApp.gGameCtrl.EffectCache.CacheEffect(Deadth, initCount, EffectConfig.SpecialDeadtLimitCount);
            }
        }
        protected EffectNode AddDeadEffect(bool hitByCarrier)
        {
            EffectNode deadEffect;
            if (EffectConfig.EffectPath.ContainsKey(Deadth))
            {
                deadEffect = Global.gApp.gGameCtrl.EffectCache.GetEffect(Deadth);
            }
            else
            {
                if (!hitByCarrier)
                {
                    deadEffect = Global.gApp.gGameCtrl.EffectCache.GetEffect(EffectConfig.DeadEffect);
                }
                else
                {
                    deadEffect = Global.gApp.gGameCtrl.EffectCache.GetEffect(EffectConfig.DeadEffect1);
                }
            }
            deadEffect.transform.position = transform.position;
            return deadEffect;
        }
        public virtual void OnHittedDeath(double damage, bool ingnoreEffect = false,bool hitByCarrier = false)
        {
            if (!ingnoreEffect)
            {
                AddDeadEffect(hitByCarrier);
            }

            PlayAnim(GameConstVal.Death);
            //PassItem passData =Global.gApp.CurScene.GetPassData();
            //if (m_MonsterItem.dropCoinCount > 0)
            //{
            //   Global.gApp.CurScene.GetPropMgr().AddGold(transform.position, m_MonsterItem.dropCoinCount);
            //}
            //Global.gApp.CurScene.GetPropMgr().AddProp(transform.position, m_MonsterItem.dropId, passData);
            DeadthSimple();
        }
        protected void DeadthSimple(bool ingoreBroad = false)
        {
            InDeath = true;
            if (!m_DeadthBackAct)
            {
                SetCollisionEnable(false);
            }
            m_Wave.RemoveMonster(this, ingoreBroad);
            foreach (AiBase aiBase in m_AiBase)
            {
                aiBase.Death();
            }
            SetActEnable(false);
        }
        protected virtual void SetCollisionEnable(bool enable)
        {
            m_Rigidbody2D.simulated = enable;
            m_Collider2D.enabled = enable;
        }
        public virtual void HitDeadth()
        {
            double damage = m_MaxHp + 100;
            OnHitByOtherImp(damage);
            if (!InDeath && m_Hp <= 0)
            {
                OnHittedDeath(damage, false, true);
            }
        }
        protected virtual void HittedByCarChangeRightBodyProp()
        {
            if (m_UsingSkill)
            {
                SetRightBodyType(RigidbodyType2D.Dynamic);
            }
            ResetStaticByRigidProp();
        }
        public virtual void OnHitByCarrier(double damage)
        {
            HittedByCarChangeRightBodyProp();
            OnHitByOtherImp(damage);
            if (!InDeath && m_Hp <= 0)
            {
                OnHittedDeath(damage,false,true);
            }
        }
        public virtual void OnHit_Vec(double damage, Transform bulletTsf, bool ingnoreEffect = false)
        {
            OnHitImp(damage, bulletTsf);
            if (!InDeath && m_Hp <= 0)
            {
                OnHittedDeath(damage, ingnoreEffect);
            }
            if (TriggerBackAct())
            {
                m_BeatBackAct.OnHit_Vec(bulletTsf);
            }
        }
        public virtual void OnHit_Pos(double damage, Transform bulletTsf,bool ingnoreEffect = false)
        {
            OnHitImp(damage, bulletTsf);
            if (!InDeath && m_Hp <= 0)
            {
                OnHittedDeath(damage, ingnoreEffect, false);
            }
            if (TriggerBackAct())
            {
                m_BeatBackAct.OnHit_Pos(bulletTsf);
            }
        }
        public virtual void OnHit_Up(double damage, Transform bulletTsf,bool ingnoreEffect = false)
        {
            OnHitImp(damage, bulletTsf);
            if (!InDeath && m_Hp <= 0)
            {
                OnHittedDeath(damage, ingnoreEffect, false);
            }
            if (TriggerBackAct())
            {
                m_BeatBackAct.OnHit_Up(bulletTsf);
            }
        }
        public virtual void OnHit_Simple(double damage, Transform bulletTsf,bool ingnoreEffect = false)
        {
            OnHitImp(damage,bulletTsf);
            if (!InDeath && m_Hp <= 0)
            {
                OnHittedDeath(damage, ingnoreEffect, false);
            }
        }

        private void OnHitImp(double damage,Transform bulletTsf)
        {
            //BaseBullet bullet = bulletTsf.GetComponent<BaseBullet>();
            //int bulletIdInt = (int)bullet.GetBulletId();
            //if (bulletIdInt <= BaseBullet.MainBulletEndIndex && bulletIdInt >= BaseBullet.MainBulletStartIndex)
            {
                OnHitByRoleImp(damage);
            }
            //else
            //{
            //    OnHitByOtherImp(damage);
            //}
        }
        public void OnDamage(double damage)
        {
            m_Hp = m_Hp - damage;
            Debug.Log("OnDamage " + damage);
            OnHitAct();
            if (!InDeath && m_Hp <= 0)
            {
                OnHittedDeath(damage, false, false);
            }
        }
        protected bool TriggerCampKillBuff()
        {
            if (m_IsNormalMonster)
            {
                // 秒杀
                float killBRate = m_Player.GetBuffMgr().CampKillBRate;
                if (killBRate > 0 && m_Hp / m_MaxHp >= 0.999f)
                {
                    float curRate = Random.Range(1, 101);
                    if (killBRate >= curRate)
                    {

                        m_Hp = -1;
                        OnHitAct();
                        return true;
                    }
                }
                // 斩杀 血量低于 某个值 开始检测触发
                float killARate = m_Player.GetBuffMgr().CampKillARate;
                if (killARate > 0 && m_Hp / m_MaxHp < 0.1f)
                {
                    float curRate = Random.Range(1, 101);
                    if (killARate >= curRate)
                    {
                        m_Hp = -1;
                        OnHitAct();
                        return true;
                    }
                }
            }
            return false;
        }
        protected virtual void OnHitByRoleImp(double damage)
        {
            //if (TriggerCampKillBuff())
            //{
            //    return;
            //}
            if (damage <= 0)
                return;

            double oriDamage = damage;
            bool triggerCrit = false;
            int curVal = m_Player.GetCritVal();
            if (curVal > 0)
            {
                int randVal = Random.Range(0, 101);
                if (randVal < curVal)
                {
                    //GameObject missObj = Global.gApp.gResMgr.InstantiateObj(EffectConfig.DamageTextEffect);
                    //missObj.transform.position = transform.position;
                    //Vector3 globalScale = transform.lossyScale;
                    //missObj.transform.localScale = new Vector3(1.0f / globalScale.x, 1.0f / globalScale.y, 1.0f / globalScale.z);
                    damage = damage * m_Player.GetCritDamage();
                    triggerCrit = true;
                }
            }

            damage *= Random.Range(0.9f, 1.1f);
            GameObject missObj = Global.gApp.gResMgr.InstantiateObj(EffectConfig.DamageTextEffect);
            missObj.transform.position = transform.position;
            //Vector3 globalScale = transform.lossyScale;
            //missObj.transform.localScale = new Vector3(1.0f / globalScale.x, 1.0f / globalScale.y, 1.0f / globalScale.z);
            //missObj.transform.localScale = Vector3.one;
            var text = missObj.GetComponent<DamageText>();
            text.SetDamage((int)damage, triggerCrit);

            m_Hp = m_Hp - damage;
            Debug.Log("OnHitByRoleImp " + damage);
            OnHitAct();
            if(triggerCrit)
            {
                TriggerCampCripBuff(oriDamage);
            }
        }
        protected void TriggerCampCripBuff(double damage)
        {
            //不死可能触发 火buff
            if(m_Hp > 0)
            {
                float campCritFiredBuffDamageParam = m_Player.GetBuffMgr().CampCritFiredBuffDamageParam;
                if(campCritFiredBuffDamageParam > 0)
                {
                    AddBuff(AiBuffType.FireBuff, 2f, damage * campCritFiredBuffDamageParam * 0.2f, 0.25f);
                }
            }
            float campCritExplodDamageParam = m_Player.GetBuffMgr().CampCritExplodDamageParam;
            if(campCritExplodDamageParam > 0 )
            {
                GameObject go = Global.gApp.gResMgr.InstantiateObj(BulletConfig.BulletPath[BulletConfig.BulletCampExplode]);
                go.transform.SetParent(Global.gApp.gBulletNode.transform, false);
                go.transform.position = transform.position;
                go.GetComponentInChildren<RocketExplode>().AddExplodeEffect(damage * campCritExplodDamageParam);
            }
        }
        protected virtual void OnHitByOtherImp(double damage)
        {
            m_Hp = m_Hp - damage;
            Debug.Log("OnHitByOtherImp " + damage);
            OnHitAct();
        }
 
        protected virtual void OnHitAct()
        {
            if (m_AiPauseAct != null)
            {
                m_AiPauseAct.ResumeAi();
            }
            if (m_MonsterItem.hasHpSlider > 0)
            {
                float rate = (float)(m_Hp / m_MaxHp);
                if (m_Hp <= 0)
                {
                    Global.gApp.gMsgDispatcher.Broadcast<int, float>(MsgIds.MonsterHpChanged, m_Guid, 0);
                }
                else
                {
                    Global.gApp.gMsgDispatcher.Broadcast<int, float>(MsgIds.MonsterHpChanged, m_Guid, rate);
                }
            }
        }
        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        protected virtual void RecycleSelf()
        {
            InDeath = true;
            enabled = false;
            if(m_AiPauseAct)
            {
                m_AiPauseAct.Deadth();
            }
            SetActEnable(false);
            SetCollisionEnable(false);
            PlayerMgr playerMgr = Global.gApp.CurScene.GetPlayerMgr();
            if(playerMgr != null)
            {
                SetRightBodyType(RigidbodyType2D.Dynamic);
                playerMgr.Recycle(m_MonsterItem, this);
                if (SkinePlayerAnim != null)
                {
                    SkinePlayerAnim.enabled = false;
                }
            }
            else
            {
                DestroySelf();
            }
        }
        // When return true must triggerAct
        private  bool TriggerAct()
        {
            if (!m_UsingSkill)
            {
                m_UsingSkill = true;
                SetAbsAnimSpeed(1);
                CheckLayerChanged();
                return true;
            }
            else
            {
                return false;
            }
        }
        private void EndAct()
        {
            m_UsingSkill = false;
            ResetSpeed();
        }
        public virtual bool TriggerFirstAct()
        {
            return TriggerAct();  
        }
        public virtual void EndFirstAct()
        {
            EndAct();
        }
        public virtual bool TriggerSecondAct()
        {
            return TriggerAct();
        }
       
        public virtual void EndSecondAct()
        {
            EndAct();
        }

        public float GetBackStartSpeed()
        {
            return BackStartSpeed;
        }

        public float GetBackEndSpeed()
        {
            return BackEndSpeed;
        }
        public float GetBackTime()
        {
            return BackTime;
        }
        public int GetGuid()
        {
            return m_Guid;
        }
        public int GetMonsterId()
        {
            return m_MonsterId;
        }
        public MonsterItem GetMonsterItem()
        {
            return m_MonsterItem;
        }
        public void SetGuid(int guid)
        {
            m_Guid = guid;
        }
        public bool CheckCanAddHittedEffect()
        {
            if(m_CurEffectTime <= 0)
            {
                m_CurEffectTime = m_EffectDtTime;
                return true;
            }
            return false;
        }
        protected virtual void LateUpdate()
        {
            float dtTime = BaseScene.GetDtTime();
            if (dtTime > 0)
            {
                m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                m_CurEffectTime -= dtTime;
            } else
            {
                m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            
        }
        public virtual void SetAbsSpeed(Vector2 speed)
        {
            m_Rigidbody2D.velocity = speed ;
        }
        public virtual void SetSpeed(Vector2 speed)
        {
            if (m_Rigidbody2D.bodyType != RigidbodyType2D.Static)
            {
                float newSpeed = 1;
                if (m_AiBuffMgr != null)
                {
                    newSpeed += m_AiBuffMgr.GetIncMoveSpeed();
                }
                m_Rigidbody2D.velocity = speed * newSpeed;
            }
        }

        public virtual void EndBackAct()
        {
            if (m_UsingSkill) { return; }
            if (m_PursureAct != null && !m_PursureAct.TriggerScalePursue())
            {
                m_PursureAct.enabled = true;
                m_BeatBackAct.enabled = false;
            }
        }

        public virtual bool TriggerBackAct()
        {
            if (m_UsingSkill) { return false; }
            if (!m_NormalBackAct)
            {
                return false;
            }
            if(m_InDeath && !m_DeadthBackAct)
            {
                return false;
            }
            if (m_PursureAct != null)
            {
                if (m_PursureAct.TriggerScalePursue())
                {
                    m_PursureAct.enabled = true;
                    m_BeatBackAct.enabled = false;
                    m_PursureAct.SetHittedSpeedScaleEnable();
                    return false;
                }
                else
                {
                    m_PursureAct.enabled = false;
                    m_BeatBackAct.enabled = true;
                    return true;
                }
            }
            return false;
        }
        public void AddBuff(AiBuffType buffType, float effectTime, double val, float dtTime = 0)
        {
            if (m_InDeath || effectTime <= 0) { return; };
            if(m_AiBuffMgr == null)
            {
                m_AiBuffMgr = new AiBuffMgr(this);
            }
            m_AiBuffMgr.AddBuff(buffType, effectTime, val, dtTime);
        }
        public void AddHpByPercent(double percent)
        {
            double addHp = m_MaxHp * percent;
            m_Hp += addHp;
            if(m_Hp > m_MaxHp)
            {
                m_Hp = m_MaxHp;
            }
            if (m_MonsterItem.hasHpSlider > 0)
            {
                float rate = (float)(m_Hp / m_MaxHp);
                Global.gApp.gMsgDispatcher.Broadcast<int, float>(MsgIds.MonsterHpChanged, m_Guid, rate);
            }
        }
        public void PlayAnim(string anim,int layer = -1,float normalize = 0)
        {

            if(MAnimator != null)
            {
                MAnimator.Play(anim, layer, normalize);
            }
            else
            {
                SkinePlayerAnim.PlayAnim(anim, normalize);
            }
        }
        public void SetAnimFloat(string name,float val)
        {
            if (MAnimator != null)
            {
                MAnimator.SetFloat(name, val);
            }
        }
        // add for skill
        public void SetAbsAnimSpeed(float speed)
        {
            if (MAnimator != null)
            {
                MAnimator.speed = speed;
            }
            else
            {
                SkinePlayerAnim.SetSpeed(speed);
            }
        }
        // reset for Skill
        public void ResetSpeed()
        {
            SetAbsAnimSpeed(m_CurSpeed);
        }
        // curSpeed and record for reset
        public void SetAnimSpeed(float speed)
        {
            m_CurSpeed = speed * m_OriSpeed;
            if (!m_UsingSkill && !m_InDeath)
            {
                SetAbsAnimSpeed(m_CurSpeed);
            }
        }

        public Transform GetHpNode()
        {
            return m_HpNode;
        }
        public Transform GetBodyNode()
        {
            return m_BodyNode;
        }
        public void SetRightBodyType(RigidbodyType2D type2D)
        {
            m_Rigidbody2D.bodyType = type2D;
        }
        public void ResetStaticByRigidProp()
        {
            m_Rigidbody2D.drag = 0;
            m_Rigidbody2D.angularDrag = 0;
            m_Rigidbody2D.mass = m_OriMass;
        }
        public void SetStaticByRigidProp()
        {
            m_Rigidbody2D.drag = 10000;
            m_Rigidbody2D.angularDrag = 10000;
            m_Rigidbody2D.mass = 100000;
        }
        protected void ShadowViewStateChanged()
        {
            if(SkinePlayerAnim != null){
                SkinePlayerAnim.ShadowViewStateChanged(m_InShadowView);
            }
        }
        public void SetCharmMinePauseGo(GameObject go)
        {
            if (m_PursureAct != null && m_MonsterItem.type == (int)MonsterType.Normal)
            {
                m_PursureAct.SetTargetNode(go);
            }
        }
        protected virtual void CheckLayerChanged()
        {
            if (m_InShadowView || m_UsingSkill)
            {
                if (m_OriLayer >= 0)
                {
                    gameObject.layer = m_OriLayer;
                }
            }
            else
            {
                gameObject.layer = GameConstVal.CrossMonsterLayer;
            }
        }
    }
}
