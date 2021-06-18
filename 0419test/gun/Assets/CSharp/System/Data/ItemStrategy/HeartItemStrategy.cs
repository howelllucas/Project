
using EZ;
using EZ.DataMgr;

//钻石处理逻辑
public class HeartItemStrategy : BaseItemStrategy
{
    private BaseAttrMgr baseAttrMgr;

    public HeartItemStrategy(BaseAttrMgr baseAttrMgr)
    {
        this.baseAttrMgr = baseAttrMgr;
    }

    public override double GetItem(int itemId)
    {
        return baseAttrMgr.GetHeart();
    }
    public override bool AddItem(ItemDTO itemDTO)
    {
        double curValue = GetItem(itemDTO.itemId);
        curValue = curValue + itemDTO.num;
        baseAttrMgr.SetHeart(curValue);
        baseAttrMgr.SaveData();
        Global.gApp.gMsgDispatcher.Broadcast<double>(MsgIds.RedHeardChanged, curValue);
        return true;
    }
    public override bool ReduceItem(ItemDTO itemDTO)
    {
        if (!CanReduce(itemDTO))
        {
            return false;
        }
        itemDTO.num = -itemDTO.num;
        AddItem(itemDTO);
        return true;
    }

}
