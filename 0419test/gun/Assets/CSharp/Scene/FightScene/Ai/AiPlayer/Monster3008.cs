using UnityEngine;
namespace EZ
{
    public class Monster3008 : Monster
    {
        AISprayVenomAct m_SprayVenomAct;
        AIDunLandAct m_DunLandAct;
        private void Awake()
        {
            m_SprayVenomAct = gameObject.GetComponent<AISprayVenomAct>();
            m_DunLandAct = gameObject.GetComponent<AIDunLandAct>();
        }
        public override void Init(GameObject player, Wave wave, EZ.Data.MonsterItem monster)
        {
            base.Init(player, wave, monster);
            m_DeadthBackAct = false;
            m_NormalBackAct = false;
            m_SprayVenomAct.Init(player, wave, this);
            m_DunLandAct.Init(player, wave, this);
            SetRightBodyType(RigidbodyType2D.Static);
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            SetSpeed(Vector2.zero);
        }
        public override void OnHittedDeath(double damage, bool ingnoreEffect = false,bool hitByCarrier = false)
        {
            if (InDeath) { return; }
            base.OnHittedDeath(damage, ingnoreEffect,hitByCarrier);
            Global.gApp.gShakeCompt.StartShake();
        }

        protected override void HittedByCarChangeRightBodyProp()
        {
            // do nothing 
            ResetStaticByRigidProp();
        }

        public override bool TriggerSecondAct()
        {
            if (base.TriggerSecondAct())
            {
                SetRightBodyType(RigidbodyType2D.Dynamic);
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void EndSecondAct()
        {
            base.EndSecondAct();
            SetRightBodyType(RigidbodyType2D.Static);
        }
    }
}
