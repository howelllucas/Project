using UnityEngine;
namespace EZ
{
    public class FireAtk : BaseBuff
    {
        public FireAtk(Player player, BuffMgr buffMgr, float duration, int buffId,float val)
        {
            Init(player, buffMgr, duration, buffId,val);
        }
        public override void Reload(float duration, float val,float val2, float val3)
        {
            base.Reload(duration, val);
            SetVal(val);
        }
    }
}
