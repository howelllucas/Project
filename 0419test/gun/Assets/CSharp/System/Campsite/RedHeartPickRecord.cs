using EZ.DataMgr;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{

    public class RedHeartPickRecord : MonoBehaviour
    {
        float m_CurTime = 0;
        private int m_AdPickCount = 0;
        List<NpcRedHeartItemDTO> m_AddHeartInfo = new List<NpcRedHeartItemDTO>();
        // Update is called once per frame
        private void Update()
        {
            m_CurTime += Time.deltaTime;
            if(m_CurTime > 0.8f)
            {
                Flush();
            }
        }

        public void AddHeartInfo(NpcRedHeartItemDTO npcRedHeartItemDTO, Vector3 effectPos)
        {
            m_CurTime = 0;
            Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, SpecialItemIdConstVal.RED_HEART,1, effectPos);
            m_AddHeartInfo.Add(npcRedHeartItemDTO);
        }

        public void AddHeartInfo(Vector3 effectPos)
        {
            m_CurTime = 0;
            m_AdPickCount++;
            Global.gApp.gMsgDispatcher.Broadcast<int, int, Vector3>(MsgIds.ShowRewardGetEffect, SpecialItemIdConstVal.RED_HEART, 1, effectPos);
        }
        public void Flush(bool saveData = true)
        { 
            Global.gApp.gSystemMgr.GetNpcMgr().PickHeartImp(ref m_AddHeartInfo,m_AdPickCount, saveData);
            m_AddHeartInfo.Clear();
            m_AdPickCount = 0;
        }
    }
}
