
using EZ;
using EZ.DataMgr;

//钻石处理逻辑
public class MDTItemStrategy : BaseItemStrategy
{
    private BaseAttrMgr baseAttrMgr;

    public MDTItemStrategy(BaseAttrMgr baseAttrMgr)
    {
        this.baseAttrMgr = baseAttrMgr;
    }

    public override double GetItem(int itemId)
    {
        return baseAttrMgr.GetMDT();
    }
    public override bool AddItem(ItemDTO itemDTO)
    {
        double curValue = GetItem(itemDTO.itemId);
        curValue = curValue + itemDTO.num;
        baseAttrMgr.SetMDT(curValue);
        baseAttrMgr.SaveData();
        Global.gApp.gMsgDispatcher.Broadcast<double>(MsgIds.MDTChanged, curValue);
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
