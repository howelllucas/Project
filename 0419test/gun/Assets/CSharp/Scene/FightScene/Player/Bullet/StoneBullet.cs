using UnityEngine;
namespace EZ
{
    public class StoneBullet : BaseBullet
    {
        float m_UpFlyTime = 0.8f;
        private float m_DropFlyTime = 0.2f;
        private Vector3 m_StartPos;
        private Vector3 m_DestPos1;
        private Vector3 m_DestPos2;
        bool m_StartRun = false;
        bool m_StartSep2 = false;
        public void Init(Vector3 position)
        {
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
            transform.position = position;
        }
        public void Update()
        {
            if (m_StartRun)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                if (!m_StartSep2)
                {
                    float rate = m_CurTime / m_UpFlyTime;
                    rate = Mathf.Min(rate, 1);
                    transform.position = m_DestPos1 *rate + m_StartPos * (1 - rate);
                    if(m_CurTime >= m_UpFlyTime)
                    {
                        m_StartPos = m_DestPos2 + new Vector3(0, 0, -50);
                        m_StartSep2 = true;
                        m_CurTime = m_CurTime - m_UpFlyTime;
                    }
                }
                else
                {
                    float rate = m_CurTime / m_DropFlyTime;
                    rate = Mathf.Min(rate, 1);
                    transform.position = m_DestPos2 * rate + m_StartPos * (1 - rate);
                    if (m_CurTime >= m_DropFlyTime)
                    {
                        m_StartRun = false;
                        StartDamage();
                    }
                }
            }
        }
        public void Run(Vector3 pos)
        {
            transform.localEulerAngles = Vector3.zero;
            m_StartRun = true;
            m_StartPos = transform.position;
            m_DestPos2 = pos;
            m_DestPos1 = transform.position + new Vector3(0,0,-50);
            transform.SetParent(Global.gApp.gBulletNode.transform, false);
        }
        private void StartDamage()
        {
            InitBulletEffect();
            GetComponent<Collider2D>().enabled = true;
            GetComponent<Rigidbody2D>().simulated = true;
            enabled = true;
            transform.Find("stone").gameObject.SetActive(false);
            DelayDestroy delay = gameObject.AddComponent<DelayDestroy>();
            delay.SetLiveTime(0.1f);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
            {
                collision.gameObject.GetComponentInParent<Player>().StartBackActOnPos(transform);
            }
        }
        private void InitBulletEffect()
        {
            GameObject effect = Instantiate(BulletEffect);
            effect.transform.position = transform.position;
            DelayDestroy delayDestroy = effect.AddComponent<DelayDestroy>();
            delayDestroy.SetLiveTime(2.1f);
        }
    }
}
