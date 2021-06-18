using UnityEngine;
namespace EZ
{
    public class FireSpeedBuff : BaseBuff
    {
        public FireSpeedBuff(Player player, BuffMgr buffMgr, float duration, int buffId, float val)
        {
            Init(player, buffMgr, duration, buffId, val);
        }
        public override void Reload(float duration, float val, float val2, float val3)
        {
            base.Reload(duration, val);
            SetVal(val);
        }
    }
}
