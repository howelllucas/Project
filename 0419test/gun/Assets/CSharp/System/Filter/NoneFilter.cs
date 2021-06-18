
public class NoneFilter : BaseFilter
{

    public override bool Filter(float[] condition)
    {
        return true;
    }

    public override string GetUnfinishTips(float[] condition)
    {
        return null;
    }

}
