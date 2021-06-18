using EZ;
using EZ.Data;
using EZ.DataMgr;

//Npc物品处理逻辑
public class NpcAwardItemStrategy : BaseItemStrategy
{
    private NpcMgr mgr;

    public NpcAwardItemStrategy(NpcMgr mgr)
    {
        this.mgr = mgr;
    }

    public override double GetItem(int itemId)
    {
        ItemDTO itemDTO = mgr.GetNpcAward(itemId);
        if (itemDTO == null)
        {
            return 0d;
        }
        return itemDTO.num;
    }
    public override bool AddItem(ItemDTO itemDTO)
    {
        ItemDTO iDTO = mgr.GetNpcAward(itemDTO.itemId);
        if (iDTO == null)
        {
            iDTO = new ItemDTO(itemDTO.itemId, 0d, 0);
            mgr.GetData().npcAwardMap[itemDTO.itemId.ToString()] = iDTO;
        }
        iDTO.num += itemDTO.num;
        mgr.SaveData();
        Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIFresh);
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
