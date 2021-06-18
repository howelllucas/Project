using UnityEngine;
namespace EZ
{
    public class PlayerEnergy : BaseBuff
    {
        public PlayerEnergy(Player player, BuffMgr buffMgr, float duration, int buffId, float val)
        {
            Init(player, buffMgr, duration, buffId, val);
            SetVal(100);
        }

        public override void Update(float dt)
        {
            // 假定7秒消耗玩100体力，5秒恢复满100体力
            float dtdec = dt * 14;
            float dtinc = dt * 20;
         
            if (m_Player.Idle)
            {
                SetVal(GetVal() + dtinc > 100 ? 100 : GetVal() + dtinc);
            }else
            {
                SetVal((GetVal() - dtdec) <= 0 ? 0: (GetVal() - dtdec));
            }

            Global.gApp.gMsgDispatcher.Broadcast<string, string, float>(MsgIds.UpdatePlayerEnergy, "PlayerEnergyProp", "PlayerEnergyProp", GetVal()/100);

        }
    }
}

