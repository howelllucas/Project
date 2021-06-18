
using UnityEngine;

namespace EZ
{
    public class CarrieMgr {
        private float m_CurTime;
        private float m_LiveTime;
        private float m_MoveSpeed;

        private Player m_MainPlayer;
        private GameObject m_RoleNode;
        private GameObject m_CarrierRootNode;
        private GameObject m_Carrier;
        private Rigidbody2D m_Rigidbody2D;
        public CarrieMgr(Player player)
        {
            m_MainPlayer = player;
            m_RoleNode = m_MainPlayer.RoleNode.Find("hero").gameObject;
        }
        public void SetCarrier(GameObject carrier,float liveTime = 0,float moveSpeed = 1, float damageCoefficient = 1)
        {
            DestroyCarrier(false);

            m_MoveSpeed = moveSpeed;
            m_MainPlayer.SetCircleCollider2DEnable(false,carrier);
            m_RoleNode.SetActive(false);

            carrier.transform.SetParent(m_MainPlayer.transform.parent,false);
            carrier.transform.position = m_MainPlayer.transform.position;
            carrier.transform.rotation = m_MainPlayer.RoleNode.rotation;

            m_MainPlayer.transform.SetParent(carrier.transform,false);

            m_MainPlayer.transform.localPosition = Vector3.zero;
            m_MainPlayer.RoleNode.localEulerAngles = Vector3.zero;

            m_CarrierRootNode = carrier;

            m_Carrier = carrier.transform.Find(GameConstVal.CarrierTag).gameObject;
            m_Carrier.transform.SetParent(m_MainPlayer.RoleNode,false);
            m_Carrier.transform.localPosition = Vector3.zero;

            m_Rigidbody2D = m_CarrierRootNode.GetComponent<Rigidbody2D>();
            carrier.GetComponent<BaseCarrie>().Init(damageCoefficient);
            m_LiveTime = liveTime;
            m_CurTime = 0;

            if (carrier.GetComponent<BlinkTools>())
            {
                carrier.GetComponent<BlinkTools>().SetStartTime(m_LiveTime - 3);
            }
            else
            {
                carrier.AddComponent<BlinkTools>().SetStartTime(m_LiveTime - 3);
            }
        }
        public bool HasCarrier()
        {
            return m_CarrierRootNode != null; 
        }
        public void Move(float sx,float sy)
        {
            if (m_Rigidbody2D != null)
            {
                Vector2 velocity = new Vector2(sx, sy);
                m_Rigidbody2D.velocity = velocity.normalized * m_MoveSpeed  * BaseScene.TimeScale;
            }
        }
        public void DestroyCarrier(bool stopAudio)
        {
            if (m_Carrier != null)
            {
                m_MainPlayer.transform.SetParent(m_CarrierRootNode.transform.parent, false);

                m_MainPlayer.transform.position = m_CarrierRootNode.transform.position;
                m_MainPlayer.RoleNode.rotation = m_CarrierRootNode.transform.rotation;

                m_MainPlayer.SetCircleCollider2DEnable(true);
                m_RoleNode.SetActive(true);

                Object.Destroy(m_CarrierRootNode);
                Object.Destroy(m_Carrier);

                m_Carrier = null;
                m_CarrierRootNode = null;
                m_Rigidbody2D = null;
                if(stopAudio)
                {
                    Global.gApp.gAudioSource.StopPlayLoopAudio();
                }
            }
        }
	    public void Update(float dt) {
            if (m_Carrier)
            {
                m_CurTime = m_CurTime + dt;
                if(m_CurTime >= m_LiveTime)
                {
                    DestroyCarrier(true);
                    m_MainPlayer.GetPlayerData().ResetProtectTime(0.1f);
                }
            }
        }
    }
}
