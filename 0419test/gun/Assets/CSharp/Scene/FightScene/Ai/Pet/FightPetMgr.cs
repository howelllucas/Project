using UnityEngine;

namespace EZ
{
    public class FightPetMgr 
    {
        private GameObject m_PlayerGo;
        Player m_Player;
        #region First Pet
        private GameObject m_FirstPetGo;
        private BasePet m_FirstPet;
        private float m_FirsPettime = 0;
        private float m_FirstPetDuration = 0;
        #endregion

        #region second buff Pet
        private bool m_ChangeSecondPet = false;
        private float m_SecondPetTime = 0;
        private float m_SecondPetDuration = 0;
        private string m_SecondPetName = GameConstVal.EmepyStr;
        private GameObject m_SecondPetGo;
        private BasePet m_SecondPet;
        private int m_Guid = 1;
        #endregion
        public FightPetMgr(Player player)
        {
            m_Player = player;
            m_PlayerGo = player.gameObject;
        }
        public void Update(float dt)
        {
            if (m_ChangeSecondPet)
            {
                m_SecondPetTime = m_SecondPetTime + dt;
                if (m_SecondPetTime >= m_SecondPetDuration)
                {
                    m_ChangeSecondPet = false;
                    m_SecondPetDuration = 0;
                    ResetSecondPet();
                }
            }
        }

        public GameObject GetSecondCurPet()
        {
            return m_SecondPetGo;
        }
        public GameObject GetCurPet()
        {
            return m_FirstPetGo;
        }
        public void ResetSecondPet()
        {
            if (m_SecondPetGo != null)
            {
                Object.Destroy(m_SecondPetGo);
                m_SecondPetGo = null;
                m_SecondPet = null;
            }
            m_SecondPetName = string.Empty;
        }
        public void ChangeSecondPet(string petName,float keepTime)
        {
            if (!m_SecondPetName.Equals(keepTime))
            {
                m_SecondPetDuration = keepTime;
                ResetSecondPet();
            }
            else
            {
                m_SecondPetDuration = Mathf.Max(keepTime, m_SecondPetDuration);
            }
            m_SecondPetTime = 0;
            m_SecondPetGo = Global.gApp.gResMgr.InstantiateObj("Prefabs/Pet/" + petName);
            m_SecondPetGo.transform.SetParent(Global.gApp.gRoleNode.transform, false);
            m_SecondPet = m_SecondPetGo.GetComponent<BasePet>();
            if(m_SecondPetGo.GetComponent<BlinkTools>())
            {
                m_SecondPetGo.GetComponent<BlinkTools>().SetStartTime(m_SecondPetDuration - 3);
            }
            else
            {
                m_SecondPetGo.AddComponent<BlinkTools>().SetStartTime(m_SecondPetDuration - 3);
            }
            m_SecondPet.Init(m_PlayerGo, ++m_Guid);
            m_ChangeSecondPet = true;
        }
        public void ChangePet(string petName)
        {
            if (petName != null && !petName.Equals(GameConstVal.EmepyStr))
            {
                m_PlayerGo.AddComponent<DelayCallBack>().SetAction(() =>
                {
                    m_FirstPetGo = Global.gApp.gResMgr.InstantiateObj("Prefabs/Pet/" + petName);
                    m_FirstPetGo.transform.SetParent(Global.gApp.gRoleNode.transform, false);
                    m_FirstPet = m_FirstPetGo.GetComponent<BasePet>();
                    m_FirstPet.Init(m_PlayerGo, ++m_Guid);

                }, 0.1f, true);
            }

        }
        public void Destroy()
        {
            if(m_FirstPetGo != null)
            {
                Object.Destroy(m_FirstPetGo);
                m_FirstPetGo = null;
            }
            if(m_SecondPetGo != null)
            {
                Object.Destroy(m_SecondPetGo);
                m_SecondPetGo = null;
            }
        }
    }
}
