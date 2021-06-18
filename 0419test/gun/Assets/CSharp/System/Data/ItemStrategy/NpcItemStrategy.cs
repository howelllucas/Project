using EZ;
using EZ.Data;
using EZ.DataMgr;

//Npc处理逻辑
public class NpcItemStrategy : BaseItemStrategy
{
    private NpcMgr mgr;

    public NpcItemStrategy(NpcMgr mgr)
    {
        this.mgr = mgr;
    }

    public override double GetItem(int itemId)
    {
        ItemDTO itemDTO = mgr.GetNpc(itemId);
        if (itemDTO == null)
        {
            return 0d;
        }
        return itemDTO.num;
    }
    public override bool AddItem(ItemDTO itemDTO)
    {
        ItemDTO iDTO = mgr.GetNpc(itemDTO.itemId);
        if (iDTO == null)
        {
            iDTO = new ItemDTO(itemDTO.itemId, 0d, 0);
            mgr.GetData().npcMap[itemDTO.itemId.ToString()] = iDTO;
        }
        iDTO.num += itemDTO.num;
        ItemItem itemCfg = Global.gApp.gGameData.ItemData.Get(itemDTO.itemId);
        CampNpcItem npcCfg = Global.gApp.gGameData.CampNpcConfig.Get(itemCfg.name);
        if (npcCfg.notFresh == 1 && iDTO.num > 1)
        {
            iDTO.num = 1;
        }
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
