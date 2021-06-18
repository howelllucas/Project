using UnityEngine;

namespace EZ
{
    public class CampsiteGoldProp : BaseProp
    {
        [SerializeField]private ParticleSystem m_Partical;
        private float m_StartTime = 0.8f;
        private float m_Time = 1f;
        private float m_CurTime = 0;
        private Player m_Player;
        private Vector3 m_DestPos;
        public void Init(Vector3 destPos, Vector3 starPos)
        {
            m_DestPos = destPos;
            m_StartTime = Random.Range(0.4f, 0.8f);
            m_Partical.time = Random.Range(0.0f, 1.0f);
            m_Partical.Play();
            m_CurTime = 0;
            transform.position = starPos;
        }
        public void Stop()
        {
            enabled = false;
            m_Partical.Stop();
        }
        public override void Update()
        {
            m_CurTime = m_CurTime + Time.deltaTime;
            if(m_CurTime > m_StartTime)
            {
                float ratio = (m_CurTime - m_StartTime) / m_Time;
                Vector3 myPosition = transform.position;
                Vector3 playerPosition = m_DestPos;
                Vector3 newPos = myPosition - playerPosition;
                if (newPos.sqrMagnitude > 1 && ratio < 1)
                {
                    float PosX = Mathf.Lerp(myPosition.x, playerPosition.x, ratio);
                    float PosY = Mathf.Lerp(myPosition.y, playerPosition.y, ratio);
                    transform.position = new Vector3(PosX, PosY, 0);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}



