using UnityEngine;

namespace EZ
{
    public class CampsiteMonster : Monster
    {
        public GameObject DeadDeffect;
        public GameObject CampsiteGoldProp;
        public Vector2Int defenceTimesRange = new Vector2Int(5, 20);
        private float m_Speed = 1f;
        private Vector3 m_PlayerPos;
        private double beHitDamage;
        public void Init(Vector3 playerPos)
        {
            if (SkinePlayerAnim != null)
            {
                SkinePlayerAnim.enabled = true;
                SkinePlayerAnim.Init();
            }
            m_BodyNode = transform.Find(GameConstVal.BodyNode);
            m_HpNode = transform.Find(GameConstVal.HpNode);
            InShadowView = true;
            InCameraView = true;
            m_PlayerPos = playerPos;
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Collider2D = GetComponent<Collider2D>();
            m_Hp = 1;
            m_MaxHp = 1;
            PlayAnim(GameConstVal.Run, -1, Random.Range(0.0f, 1.0f));
            beHitDamage = m_Hp / Random.Range(defenceTimesRange.x, defenceTimesRange.y + 1);
        }
        public override void Update()
        {
            float dtTime = BaseScene.GetDtTime();
            if (!m_InDeath && dtTime > 0)
            {
                Vector3 dtPos = m_PlayerPos - transform.position;
                transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(dtPos, Vector3.up));
                m_Rigidbody2D.velocity = dtPos.normalized * m_Speed;
            }
            else
            {
                m_Rigidbody2D.velocity = Vector2.zero;
            }
        }
        public override void OnHittedDeath(double damage, bool ingnoreEffect = false, bool hitByCarrier = false)
        {
            if (InDeath) { return; }
            GameObject deadEffect = Instantiate(DeadDeffect);
            deadEffect.gameObject.transform.SetParent(transform, false);
            PlayAnim(GameConstVal.Death);
            //Global.gApp.CurScene.GetPropMgr().AddProp(transform.position, m_DropId, passData.dropRate);
            DeadthOnHit();
        }

        protected override void OnHitByRoleImp(double damage)
        {
            m_Hp = m_Hp - beHitDamage;

            //Debug.Log("OnHitByRoleImp beHitDamage" + beHitDamage);
        }

        protected override void OnHitByOtherImp(double damage)
        {
            m_Hp = m_Hp - beHitDamage;
            //Debug.Log("OnHitByOtherImp beHitDamage" + beHitDamage);
        }
        protected void DeadthOnHit()
        {
            InDeath = true;
            SetCollisionEnable(false);
            //m_Wave.RemoveMonster(this, ingoreBroad);
            SetActEnable(false);
            Destroy(gameObject, m_DeadTime);
            GameObject effect = Instantiate(CampsiteGoldProp);
            effect.GetComponent<CampsiteGoldProp>().Init(m_PlayerPos, transform.position);

        }
    }
}
