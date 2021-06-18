
using EZ.DataMgr;

public class BaseItemStrategy
{
    public virtual double GetItem(int itemId)
    {
        return -1f;
    }
    public virtual bool AddItem(ItemDTO itemDTO)
    {
        return false;
    }
    public virtual bool ReduceItem(ItemDTO itemDTO)
    {
        return false;
    }
    public bool CanReduce(ItemDTO itemDTO)
    {
        double curValue = GetItem(itemDTO.itemId);
        if (itemDTO.num < 0 || curValue - itemDTO.num < 0)
        {
            return false;
        }
        return true;
    }

}
