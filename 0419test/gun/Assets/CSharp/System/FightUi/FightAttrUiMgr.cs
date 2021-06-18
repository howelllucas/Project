
using System.Collections.Generic;
using UnityEngine;

namespace EZ {

    public class FightAttrUiMgr
    {
        private Transform m_ParentNode;
        private Dictionary<int, BaseAttrUi> m_AttrUi;
        private FightUI m_FightUI;
        private BaseAttrUi m_PlayerAttrUi;
        public FightAttrUiMgr(Transform parentNode,FightUI fightUi)
        {
            m_FightUI = fightUi;
            m_ParentNode = parentNode;
            m_AttrUi = new Dictionary<int, BaseAttrUi>();
            RegisterListeners();
        }
        private void AddHpProgress(int guid,Transform monster)
        {
            GameObject bossHpNode = Global.gApp.gResMgr.InstantiateObj(EffectConfig.BossHpUi);
            bossHpNode.transform.SetParent(m_ParentNode, false);
            bossHpNode.transform.SetAsFirstSibling();
            BaseAttrUi baseAttr = bossHpNode.GetComponent<BaseAttrUi>();
            baseAttr.SetFloowNode(monster);
            m_AttrUi.Add(guid, baseAttr);
        }

        private void AddPlayerHpProgress(int guid, Transform player)
        {
            GameObject bossHpNode = Global.gApp.gResMgr.InstantiateObj(EffectConfig.BossHpUi);
            bossHpNode.transform.SetParent(m_ParentNode, false);
            bossHpNode.transform.SetAsFirstSibling();
            BaseAttrUi baseAttr = bossHpNode.GetComponent<BaseAttrUi>();
            baseAttr.SetFloowNode(player);
            m_PlayerAttrUi = baseAttr;
            //m_AttrUi.Add(guid, baseAttr);
        }

        private void AddPetName(int guid, string str,int state,Transform followNode)
        {
            if (state > 0)
            {
                GameObject petNameNode = Global.gApp.gResMgr.InstantiateObj(EffectConfig.PetNameUi);
                petNameNode.transform.SetParent(m_FightUI.PetNameNode.rectTransform, false);
                petNameNode.transform.SetAsFirstSibling();
                PetNameUi baseAttr = petNameNode.GetComponent<PetNameUi>();
                baseAttr.SetFloowNode(followNode);
                baseAttr.SetName(str);
                m_AttrUi.Add(guid, baseAttr);
            }
            else
            {
                BaseAttrUi baseAttrUi;
                if (m_AttrUi.TryGetValue(guid, out baseAttrUi))
                {
                    m_AttrUi.Remove(guid);
                    baseAttrUi.Destroy();
                }
            }
        }
        
        private void SetHpProgress(int guid,float progress)
        {
            BaseAttrUi baseAttrUi;
            if(m_AttrUi.TryGetValue(guid,out baseAttrUi))
            {
                baseAttrUi.SetHpPercent(progress);
            }
        }

        private void SetPlayerHpProgress(float cur, float max)
        {
            if (m_PlayerAttrUi == null)
                return;

            m_PlayerAttrUi.SetHpPercent(cur / max);
            
        }

        private void RemoveHpProgress(int guid,int monsterId, Monster monster)
        {
            BaseAttrUi baseAttrUi;
            if (m_AttrUi.TryGetValue(guid, out baseAttrUi))
            {
                m_AttrUi.Remove(guid);
                baseAttrUi.Destroy();
            }

        }
        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener<int, float>(MsgIds.MonsterHpChanged, SetHpProgress);
            Global.gApp.gMsgDispatcher.AddListener<int,int, Monster>(MsgIds.MonsterDead, RemoveHpProgress);
            Global.gApp.gMsgDispatcher.AddListener<int, Transform>(MsgIds.AddMonsterHpUi, AddHpProgress);
            Global.gApp.gMsgDispatcher.AddListener<int,string,int,Transform>(MsgIds.AddPetName, AddPetName);

            Global.gApp.gMsgDispatcher.AddListener<float, float>(MsgIds.MainRoleHpChange, SetPlayerHpProgress);
            Global.gApp.gMsgDispatcher.AddListener<int, Transform>(MsgIds.AddPlayerHpUi, AddPlayerHpProgress);
        }

        private void UnRegisterListeners()
        {
            Global.gApp.gMsgDispatcher.RemoveListener<int, float>(MsgIds.MonsterHpChanged, SetHpProgress);
            Global.gApp.gMsgDispatcher.RemoveListener<int, int,Monster>(MsgIds.MonsterDead, RemoveHpProgress);
            Global.gApp.gMsgDispatcher.RemoveListener<int, Transform>(MsgIds.AddMonsterHpUi, AddHpProgress);
            Global.gApp.gMsgDispatcher.RemoveListener<int, string, int, Transform>(MsgIds.AddPetName, AddPetName);

            Global.gApp.gMsgDispatcher.RemoveListener<float, float>(MsgIds.MainRoleHpChange, SetPlayerHpProgress);
            Global.gApp.gMsgDispatcher.RemoveListener<int, Transform>(MsgIds.AddPlayerHpUi, AddPlayerHpProgress);

        }
        public void Destroy()
        {
            UnRegisterListeners();
            foreach(BaseAttrUi baseAttrUi in m_AttrUi.Values)
            {
                baseAttrUi.Destroy();
            }
            m_AttrUi.Clear();
        }

    }
}


