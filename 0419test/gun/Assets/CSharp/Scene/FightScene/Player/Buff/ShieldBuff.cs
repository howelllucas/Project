using UnityEngine;
namespace EZ
{
    public class ShieldBuff:BaseBuff
    {
        public ShieldBuff(Player player, BuffMgr buffMgr,float duration,int buffId,GameObject particlePrefab = null)
        {
            Init(player,buffMgr,duration,buffId);
            if(particlePrefab != null)
            {
                CreateParticle(particlePrefab);
            }
        }
    }
}
