using UnityEngine;
using System.Collections.Generic;

namespace EZ
{
    public class BaseActiveRange : MonoBehaviour
    {
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

        [SerializeField]protected float ActiveTime = 2;
        [SerializeField]private int m_PropId = -1;
        [SerializeField]protected bool m_CanActive = false;
        private Collider2D m_Collider2D;
        protected bool m_Active = false;
        [System.Serializable]
        public struct ActiveInfo
        {
            public int PropId;
            public int count;
        }
        [SerializeField] protected ActiveInfo[] ActiveCondition;
        private Dictionary<int, int> m_ActiveCondition;
        protected virtual void Awake()
        {
            m_ActiveCondition = new Dictionary<int, int>();
            for (int i = 0; i < ActiveCondition.Length; i++)
            {
                m_ActiveCondition.Add(ActiveCondition[i].PropId, ActiveCondition[i].count);
            }
            m_Collider2D = GetComponent<CircleCollider2D>();
            if(ActiveCondition.Length == 0)
            {
                m_CanActive = true;
            }
            m_Collider2D.enabled = m_CanActive;
            RegisterListener();
        }
        protected void SetActiveTrue()
        {
            if (!m_Active)
            {
                m_Active = true;
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ActiveProp, m_PropId);
            }
        }

        protected void PropActive(int propId)
        {
            int curPropCount;
            if (m_ActiveCondition.TryGetValue(propId,out curPropCount))
            {
                curPropCount--;
                m_ActiveCondition[propId] = curPropCount; 
                if(curPropCount == 0)
                {
                    CheckCanActive();
                }
            }
        }

        private void CheckCanActive()
        {
            foreach(int count in m_ActiveCondition.Values)
            {
                if(count > 0)
                {
                    return;
                }
            }
            m_CanActive = true;
            m_Collider2D.enabled = true;
            UnRegisterListener();
        }
        protected void RegisterListener()
        {
            if (!m_CanActive)
            {
                Global.gApp.gMsgDispatcher.AddListener<int>(MsgIds.ActiveProp, PropActive);
            }
        }

        protected void UnRegisterListener()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int>(MsgIds.ActiveProp, PropActive);
        }

        protected virtual void OnDestroy()
        {
            UnRegisterListener();
        }
        public bool GetCanActive()
        {
            return m_CanActive;
        }
        public bool GetActive()
        {
            return m_Active;
        }
        public int GetPropId()
        {
            return m_PropId;
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
