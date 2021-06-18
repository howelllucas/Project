using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{

    public enum PortalState
    {
        None,
        Active,
        CountDown,
        Frozen,
    }
    public class PortalEleCtrol : MonoBehaviour
    {
        private List<PortalEle> m_ActivePortalEles;
        private List<PortalEle> m_CountDownPortalEles = new List<PortalEle>();
        private List<PortalEle> m_ForzenPortalEles = new List<PortalEle>();
        private void Awake()
        {
            PortalEle[] portalEles = GetComponentsInChildren<PortalEle>();
            m_ActivePortalEles = new List<PortalEle>(portalEles);
            foreach (PortalEle portalEle in m_ActivePortalEles)
            {
                portalEle.Init(this);
            }
            gameObject.AddComponent<DelayCallBack>().SetAction(() => { SetProtalEleState(); }, 0.1f);
        }
        private void SetProtalEleState()
        {
            foreach(PortalEle portalEle in m_ActivePortalEles)
            {
                portalEle.DelayInit();
            }
        }
        public void StartPortal(PortalEle portalEle)
        {
            if(m_ActivePortalEles.Count > 1)
            {
                portalEle.SetProtalState(PortalState.CountDown);
                m_ActivePortalEles.Remove(portalEle);
                m_CountDownPortalEles.Add(portalEle);

                int index = Random.Range(0, m_ActivePortalEles.Count);
                PortalEle toPortal = m_ActivePortalEles[index];
                toPortal.SetProtalState(PortalState.CountDown);
                m_ActivePortalEles.Remove(toPortal);
                m_CountDownPortalEles.Add(toPortal);
                StartPortalImp(portalEle, toPortal);

                if(m_ActivePortalEles.Count == 1)
                {
                    m_ActivePortalEles[0].SetPartlcleEnable(false);
                    m_ActivePortalEles[0].SetProtalState(PortalState.Frozen);
                    m_ForzenPortalEles.Add(m_ActivePortalEles[0]);
                    m_ActivePortalEles.RemoveAt(0);
                }
            }
            else
            {
                Debug.Log(" effor only portal");
            }
        }
        public void StartPortalImp(PortalEle fromPortalEle,PortalEle toPortalEle)
        {
            Player player = Global.gApp.CurScene.GetMainPlayerComp();
            if (!player.GetFight().HasCarrier())
            {
                fromPortalEle.AddAppearEffect();
                toPortalEle.AddAppearEffect();
                fromPortalEle.SetPartlcleEnable(false);
                toPortalEle.SetPartlcleEnable(false);
                player.LockMove(1);
                player.GetPlayerData().SetProtectTime(1);
                player.transform.position = toPortalEle.transform.position;
            }
        }
        public void ActiveProtal(PortalEle portalEle)
        {
            if(portalEle.GetProtalState() == PortalState.CountDown)
            {
                m_CountDownPortalEles.Remove(portalEle);
                if(m_ForzenPortalEles.Count == 1)
                {
                    ActiveProtal(m_ForzenPortalEles[0]);
                }
            }
            else if(portalEle.GetProtalState() == PortalState.Frozen)
            {
                m_ForzenPortalEles.Remove(portalEle);
            }
            portalEle.SetProtalState(PortalState.Active);
            m_ActivePortalEles.Add(portalEle);
        }
    }
}
