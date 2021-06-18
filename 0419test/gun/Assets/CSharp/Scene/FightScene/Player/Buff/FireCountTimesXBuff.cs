namespace EZ
{
    public class FireCountTimesXBuff : BaseBuff
    {
        public FireCountTimesXBuff(Player player, BuffMgr buffMgr, float duration, int buffId, float val)
        {
            Init(player, buffMgr, duration, buffId, val);
        }
        public override void Reload(float duration, float val, float val2, float val3)
        {
            base.Reload(duration, val);
            AddVal(val);
        }
    }
}
