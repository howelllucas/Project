using EZ;
using EZ.Data;
using EZ.DataMgr;

//碎片处理逻辑
public class ChipItemStrategy : BaseItemStrategy
{
    private WeaponMgr weaponMgr;

    public ChipItemStrategy(WeaponMgr weaponMgr)
    {
        this.weaponMgr = weaponMgr;
    }

    public override double GetItem(int itemId)
    {
        ItemDTO itemDTO = weaponMgr.GetChip(itemId);
        if (itemDTO == null)
        {
            return 0d;
        }
        return weaponMgr.GetChip(itemId).num;
    }
    public override bool AddItem(ItemDTO itemDTO)
    {
        ItemDTO chip = weaponMgr.GetChip(itemDTO.itemId);
        if (chip == null)
        {
            chip = new ItemDTO(itemDTO.itemId, 0d, 0);
            weaponMgr.GetData().chipMap[itemDTO.itemId.ToString()] = chip;
        }
        chip.num += itemDTO.num;
        weaponMgr.SaveData();
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
