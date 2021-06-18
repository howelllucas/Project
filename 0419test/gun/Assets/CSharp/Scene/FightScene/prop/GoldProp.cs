using UnityEngine;

namespace EZ
{
    public class GoldProp : BaseProp
    {
        public AudioClip MAudioClip;
        [SerializeField]private ParticleSystem m_Partical;
        private float m_StartTime = 0.8f;
        private float m_Time = 1f;
        private float m_CurTime = 0;
        private Transform m_MainPlayerTsf;
        private Player m_Player;
        private float m_Val = 1;
        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            m_MainPlayerTsf =Global.gApp.CurScene.GetMainPlayer().transform;
            m_Player = Global.gApp.CurScene.GetMainPlayerComp();
        }
        public void Init()
        {
            enabled = true;
            m_StartTime = Random.Range(0.4f, 0.8f);
            m_Partical.time = Random.Range(0.0f, 1.0f);
            m_Partical.Play();
            m_CurTime = 0;
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
                Vector3 playerPosition = m_MainPlayerTsf.position;
                Vector3 newPos = myPosition - playerPosition;
                if (newPos.sqrMagnitude > 1 && ratio < 1)
                {
                    float PosX = Mathf.Lerp(myPosition.x, playerPosition.x, ratio);
                    float PosY = Mathf.Lerp(myPosition.y, playerPosition.y, ratio);
                    transform.position = new Vector3(PosX, PosY, 0);
                }
                else
                {
                    m_Player.GetPlayerData().AddGold(1.0f);
                    Global.gApp.CurScene.GetPropMgr().RemoveGoldProp(this);
                }
            }
        }
    }
}



