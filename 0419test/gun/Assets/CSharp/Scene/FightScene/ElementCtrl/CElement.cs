using UnityEngine;

namespace EZ
{
    public interface ICtrlElement
    {
        void RevertEleState();
    }
    public interface ICtrlElementNode
    {
        void SetEleOpen();
        void SetEleClose();
        void SyncStartEleState(CElement.EleState state);
    }
    public class CElement : MonoBehaviour,ICtrlElement
    {
        public enum EleState
        {
            None = 0,
            Open = 1,
            Close = 2,
        }
        private ICtrlElementNode CtrlElementNode;
        public EleState m_CurEleState = EleState.Open;
        private void Awake()
        {
            CtrlElementNode = GetComponentInChildren<ICtrlElementNode>();
            CtrlElementNode.SyncStartEleState(m_CurEleState);
        }
        public void RevertEleState()
        {
            if (CtrlElementNode != null)
            {
                if(m_CurEleState == EleState.Open)
                {
                    SetEleState(EleState.Close);
                }
                else if(m_CurEleState == EleState.Close)
                {
                    SetEleState(EleState.Open);
                }
            }
            else
            {
                Debug.LogError(" no CtrlElement");
            }
        }
        private void ResetEleState()
        {
            if (m_CurEleState == EleState.Open)
            {
                CtrlElementNode.SetEleOpen();
            }
            else
            {
                CtrlElementNode.SetEleClose();
            }
        }
        public void SetEleState(EleState eleState)
        {
            if (m_CurEleState != eleState)
            {
                m_CurEleState = eleState;
                ResetEleState();
            }
        }
        public EleState GeEletState()
        {
            return m_CurEleState;
        }

    
    }
}
