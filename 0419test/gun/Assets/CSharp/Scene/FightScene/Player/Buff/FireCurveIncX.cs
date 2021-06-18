namespace EZ
{
    public class FireCurveIncX : BaseBuff
    {
        public FireCurveIncX(Player player, BuffMgr buffMgr, float duration, int buffId, float val)
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
