using UnityEngine;
namespace EZ
{
    public class NaNoboostBuff : BaseBuff
    {
        public NaNoboostBuff(Player player, BuffMgr buffMgr, float duration, int buffId,float val,float val2,float val3, GameObject particlePrefab)
        {
            Init(player, buffMgr, duration, buffId,val,val2,val3);
            CreateParticle(particlePrefab);
        }
        public override void Reload(float duration, float val, float val2, float val3)
        {
            base.Reload(duration, val);
            SetVal(val,val2,val3);
        }
    }
}
