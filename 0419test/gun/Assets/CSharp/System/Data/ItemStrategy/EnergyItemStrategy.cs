
using EZ;
using EZ.Data;
using EZ.DataMgr;
using System;

//能量处理逻辑
public class EnergyItemStrategy : BaseItemStrategy
{
    private BaseAttrMgr baseAttrMgr;

    public EnergyItemStrategy(BaseAttrMgr baseAttrMgr)
    {
        this.baseAttrMgr = baseAttrMgr;
    }

    public override double GetItem(int itemId)
    {
        return baseAttrMgr.GetEnergy();
    }
    public override bool AddItem(ItemDTO itemDTO)
    {
        double curValue = GetItem(itemDTO.itemId);
        curValue = curValue + itemDTO.num;
        baseAttrMgr.SetEnergy(curValue);
        baseAttrMgr.SaveData();
        Global.gApp.gMsgDispatcher.Broadcast<double>(MsgIds.EnergyChanged, curValue);
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

        //如果低于最高值，开始刷新
        GeneralConfigItem limitConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.ENERGY_LIMIT);
        int limit = int.Parse(limitConfig.content);
        if (baseAttrMgr.GetEnergy() < limit)
        {
            baseAttrMgr.SetLastEnergyTime(DateTimeUtil.GetMills(DateTime.Now));
            baseAttrMgr.SaveData();
        }
        return true;
    }
}
