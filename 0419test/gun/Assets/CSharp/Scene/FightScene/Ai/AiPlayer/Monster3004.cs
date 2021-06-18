using UnityEngine;
namespace EZ
{
    public class Monster3004 : Monster
    {
        AIStrollAct m_StrollAct;
        [SerializeField] private float m_LiveTime = 15.0f;
        private float m_CurTime;
        private float m_UnVisibleTime;
        bool m_IsNormalScene = false;
        private Vector2 m_Speed;
        private Vector2 m_DeadthSpeed;
        private bool m_UseRightBodyVec = true;
        private void Awake()
        {
            m_StrollAct = gameObject.GetComponent<AIStrollAct>();
        }
        public override void Init(GameObject player, Wave wave, EZ.Data.MonsterItem monster)
        {
            base.Init(player, wave, monster);
            m_UseRightBodyVec = true;
            m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            enabled = true;
            m_CurTime = 0;
            m_IsNormalScene = Global.gApp.CurScene.IsNormalPass();
            if (Global.gApp.CurScene.GetSceneType() == SceneType.BreakOutSene)
            {
                m_Speed = new Vector2(0, -monster.baseSpeed * 2f);
                m_DeadthSpeed = m_Speed;
                m_StrollAct.enabled = false;
                transform.localEulerAngles = new Vector3(0, 0, 180);
            }
            else if (Global.gApp.CurScene.GetSceneType() == SceneType.CarScene)
            {
                m_Speed = new Vector2(0, -monster.baseSpeed * 2.5f);
                m_DeadthSpeed = m_Speed;
                m_StrollAct.enabled = false;
                transform.localEulerAngles = new Vector3(0, 0, 180);
            }
            else
            {
                m_StrollAct.Init(player, wave, this);
            }
            Global.gApp.CurScene.GetWaveMgr().SetHas3004(true);
        }
        public override void Update()
        {
            base.Update();
            SetSpecialPassSpeed();
            CheckDeadth();
        }
        private void SetSpecialPassSpeed()
        {
            if (!m_IsNormalScene)
            {
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
                            SetSpeed(m_DeadthSpeed);
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
                            transform.Translate(m_DeadthSpeed * dtTime, Space.World);
                        }
                    }
                }
            }
        }
        private void CheckDeadth()
        {
            if (!m_InDeath)
            {
                float dtTime = BaseScene.GetDtTime();
                m_CurTime += dtTime;
                if (m_CurTime > m_LiveTime)
                {
                    if (!InCameraView)
                    {
                        m_UnVisibleTime += dtTime;
                    }
                    else
                    {
                        m_UnVisibleTime = 0;
                    }
                    if (m_UnVisibleTime > 3)
                    {
                        DeadthByTimeLimit();
                    }
                }
            }
        }
        private void DeadthByTimeLimit()
        {
            DeadthSimple();
            RecycleSelf();
        }
        protected override void SetCollisionEnable(bool enable)
        {
            if (m_IsNormalScene)
            {
                m_Rigidbody2D.simulated = enable;
            }
            m_Collider2D.enabled = enable;
        }

        public override void OnHittedDeath(double damage, bool ingnoreEffect = false, bool hitByCarrier = false)
        {
            if (InDeath) { return; }
            base.OnHittedDeath(damage, ingnoreEffect, hitByCarrier);
            Global.gApp.gShakeCompt.StartShake();
        }
        private void OnBecameVisible()
        {
            if (!m_IsNormalScene)
            {
                gameObject.layer = GameConstVal.CrossMonsterLayer;
                m_Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                m_UseRightBodyVec = false;
            }
        }
        protected override void RecycleSelf()
        {
            if (Global.gApp.CurScene.GetWaveMgr() != null)
            {
                Global.gApp.CurScene.GetWaveMgr().SetHas3004(false);
            }
            base.RecycleSelf();
        }
    }
}
