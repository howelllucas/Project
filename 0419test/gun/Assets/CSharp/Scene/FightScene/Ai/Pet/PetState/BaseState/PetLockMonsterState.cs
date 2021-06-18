using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public abstract class PetLockMonsterState : PetBaseState
    {
        [Tooltip("锁定怪物的时候 移动速度")]
        [SerializeField] protected float LockMoveSpeed = 4;
        [Tooltip("锁定敌人后如果 disThreadHoldTIme 没追上敌人那么就 检测其他状态")]
        [SerializeField] protected float DisThreadHoldTime = 3;
        [Tooltip("暂时没用到")]
        [SerializeField] protected float DisThreadHold = 3;
        [Tooltip("锁定锁定距离")]
        public float LockRadio = 50;

        [Tooltip("锁定锁定距离 偏移值 ")]
        public float LockRadioOffset = 0.3f;
        protected float m_SqrDisThreadHold;
        protected WaveMgr m_WaveMgr;
        protected GameObject m_LockMonsterGo;
        protected Transform m_LockMonsterTsf;
        protected Monster m_LockMonster;
        protected float m_LockRadioSqr = 1;
        protected float m_MonsterRadio = 1;
        public override void Init(GameObject playerGo, BasePet pet)
        {
            base.Init(playerGo, pet);
            m_SqrDisThreadHold = DisThreadHold * DisThreadHold;
            m_WaveMgr = m_PlayerGo.GetComponent<Player>().GetWaveMgr();
        }
        protected virtual void Update()
        {
            if (m_EnterState)
            {
                if(m_LockMonsterGo != null && !m_LockMonster.InDeath)
                {
                    m_CurTime += BaseScene.GetDtTime();
                    if(m_CurTime > DisThreadHoldTime)
                    {
                        CheckCanEnterOtherState();
                        return;
                    }
                    else
                    {
                        MoveToLockTsf();
                    }
                }
                else
                {
                    SetLockMonsterInner(null);
                    CheckCanEnterOtherState();
                }
            }
        }

        private void MoveToLockTsf()
        {
            Vector3 vector = (m_LockMonsterTsf.position - transform.position);
            //float angleZ = EZMath.SignedAngleBetween(vector, Vector3.up);
            //transform.localEulerAngles = new Vector3(0, 0, angleZ);
            //if (vector.sqrMagnitude >= m_SqrDisThreadHold)
            //{
            //    m_Pet.SetSpeed(vector.normalized * LockMoveSpeed);
            //}
            //else
            if(vector.sqrMagnitude <= m_LockRadioSqr)
            {
                if (m_Pet.CheckAtkState())
                {
                    m_Pet.ChangeToAtkState(m_LockMonsterTsf, m_LockMonster,m_MonsterRadio);
                }
                else
                {
                    CheckCanEnterOtherState();
                }
            }
        }

        public override bool CheckCanEnterOtherState()
        {
            if (CheckStateInner())
            {
                return true;
            }
            if(m_Pet.CheckFlashState())
            {
                m_Pet.ChangeToFlashState();
                return true;
            }
            if (m_Pet.CheckPursueState())
            {
                m_Pet.ChangeToPursueState();
                return true;
            }
            return false;
        }
        public override void StartState()
        {
            m_CurTime = 0;
            m_EnterState = true;
            StartAutoPath();
            Update();
        }

        protected virtual void StartAutoPath()
        {
            CircleCollider2D circleCollider2D = m_LockMonsterGo.GetComponent<CircleCollider2D>();
            m_MonsterRadio = circleCollider2D.radius * m_LockMonsterGo.transform.localScale.x;
            m_LockRadioSqr = m_MonsterRadio + m_Pet.CircleRadio;
            m_Pet.GetAutoPathComp().SetAutoPathEnable(true, m_LockRadioSqr + LockRadioOffset, LockMoveSpeed, m_LockMonsterTsf);
            m_LockRadioSqr = m_LockRadioSqr + 0.5f;
            m_LockRadioSqr = m_LockRadioSqr * m_LockRadioSqr;
        }
        // 内部调用 区分 外部调用。外部 是check 检测，不涉及到 自动寻路 状态 更改 ，内部需要调控 寻路状态
        protected void SetLockMonsterInner(GameObject monsterGo)
        {
            SetLockMonster(monsterGo);
            if (monsterGo)
            {
                StartAutoPath();
            }
            else
            {
                m_Pet.GetAutoPathComp().SetAutoPathEnable(false, 0.5f);
            }
        }
        public override void EndState()
        {
            m_EnterState = false;
            m_Pet.SetSpeed(Vector3.zero);
            m_Pet.GetAutoPathComp().SetAutoPathEnable(false, 0.5f);
        }
        // 内部调用 区分 外部调用。外部 是check 检测，不涉及到 自动寻路 状态 更改 ，内部需要调控 寻路状态
        private bool CheckStateInner()
        {
            bool canEnter = CheckState();
            if (m_LockMonsterGo != null)
            {
                m_CurTime = 0;
                StartAutoPath();
            }
            else
            {
                m_Pet.SetSpeed(Vector3.zero);
                m_Pet.GetAutoPathComp().SetAutoPathEnable(false, 0.5f);
            }
            return canEnter;
        }
        public override bool CheckState()
        {
            SearchLockMonster();
            if (m_LockMonsterGo != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void SetLockMonster(GameObject monsterGo)
        {
            m_LockMonsterGo = null;
            m_LockMonster = null;
            m_LockMonsterTsf = null;
            if (monsterGo != null)
            {
                m_LockMonsterGo = monsterGo;
                m_LockMonsterTsf = monsterGo.transform;
                m_LockMonster = monsterGo.GetComponent<Monster>();
            }
        }

        protected virtual void SearchLockMonster()
        {
            //if (!m_Pet.InCameraView)
            //{
            //    SetLockMonster(null);
            //    return;
            //}
            SearchNearestMonster();

        }

        public Monster SearchNearestPlayerMonster()
        {
            Vector3 m_position = m_PlayerGo.transform.position;
            Dictionary<int, Wave> waves = m_WaveMgr.GetWaves();
            float recordMag = float.MaxValue;
            GameObject recordGo = null; ;
            foreach (KeyValuePair<int, Wave> kv in waves)
            {
                List<Monster> monsters = kv.Value.GetMonsters();
                foreach (Monster monster in monsters)
                {
                    if (monster.gameObject != m_LockMonsterGo && monster.InShadowView)
                    {
                        Vector3 postion = monster.transform.position;
                        Vector3 dtPosition = m_position - postion;
                        float sqrMagnitude = dtPosition.sqrMagnitude;
                        if (sqrMagnitude <= recordMag)
                        {
                            recordMag = sqrMagnitude;
                            recordGo = monster.gameObject;
                        }
                    }
                }
            }
            SetLockMonster(recordGo);
            return m_LockMonster;
        }
        public Monster SearchNearestMonster()
        {
            Vector3 m_position = transform.position;
            Dictionary<int, Wave> waves = m_WaveMgr.GetWaves();
            float recordMag = float.MaxValue;
            GameObject recordGo = null; ;
            foreach (KeyValuePair<int, Wave> kv in waves)
            {
                List<Monster> monsters = kv.Value.GetMonsters();
                foreach (Monster monster in monsters)
                {
                    if (monster.gameObject != m_LockMonsterGo && monster.InShadowView)
                    {
                        Vector3 postion = monster.transform.position;
                        Vector3 dtPosition = m_position - postion;
                        float sqrMagnitude = dtPosition.sqrMagnitude;
                        if (sqrMagnitude <= recordMag)
                        {
                            recordMag = sqrMagnitude;
                            recordGo = monster.gameObject;
                        }
                    }
                }
            }
            SetLockMonster(recordGo);
            return m_LockMonster;
        }
        public Monster SerachRandomMonster()
        {
            Vector3 m_position = transform.position;
            Dictionary<int, Wave> waves = m_WaveMgr.GetWaves();
            float recordMag = float.MaxValue;
            GameObject recordGo = null; ;
            foreach (KeyValuePair<int, Wave> kv in waves)
            {
                List<Monster> monsters = kv.Value.GetMonsters();
                int monsterCount = monsters.Count;
                int startIndex = Random.Range(0, monsterCount);
                for (int i = 0; i < monsterCount; i++)
                {
                    int newIndex = startIndex % monsterCount;
                    Monster monster = monsters[newIndex];
                    if (monster.gameObject != m_LockMonsterGo && monster.InShadowView)
                    {
                        Vector3 postion = monster.transform.position;
                        Vector3 dtPosition = m_position - postion;
                        float sqrMagnitude = dtPosition.sqrMagnitude;
                        if (sqrMagnitude <= recordMag)
                        {
                            recordMag = sqrMagnitude;
                            recordGo = monster.gameObject;
                            break;
                        }
                    }
                }
            }
            SetLockMonster(recordGo);
            return m_LockMonster;
        }
        // 二次搜索 使用 
        public Monster GetLockMonster()
        {
            SearchLockMonster();
            return m_LockMonster;
        }
    }
}
