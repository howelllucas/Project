using UnityEngine;
namespace EZ
{
    public class TriggerEvent : MonoBehaviour
    {
        [SerializeField] private int m_TriggerId = -1001;
        private bool m_InCameraView = false;
        public bool InCameraView
        {
            get
            {
                return m_InCameraView;
            }

            set
            {
                m_InCameraView = value;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag) || collision.gameObject.CompareTag(GameConstVal.MainRoleTag) || collision.gameObject.CompareTag(GameConstVal.CarrierTag))
            {
                Global.gApp.gMsgDispatcher.Broadcast<int,Transform>(MsgIds.TriggerCollider, m_TriggerId,transform);
            }
        }

        public int GetTriggerId()
        {
            return m_TriggerId;
        }
        private void OnBecameInvisible()
        {

            InCameraView = false;
        }
        private void OnBecameVisible()
        {
            InCameraView = true;
        }
    }
}
