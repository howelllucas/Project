
using EZ;
using EZ.DataMgr;

//金币处理逻辑
public class GoldItemStrategy : BaseItemStrategy
{
    private BaseAttrMgr baseAttrMgr;

    public GoldItemStrategy(BaseAttrMgr baseAttrMgr)
    {
        this.baseAttrMgr = baseAttrMgr;
    }

    public override double GetItem(int itemId)
    {
        return baseAttrMgr.GetGold();
    }
    public override bool AddItem(ItemDTO itemDTO)
    {
        double curValue = GetItem(itemDTO.itemId);
        curValue = curValue + itemDTO.num;
        baseAttrMgr.SetGold(curValue);
        baseAttrMgr.SaveData();
        Global.gApp.gMsgDispatcher.Broadcast<double>(MsgIds.GoldChanged, curValue);
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
