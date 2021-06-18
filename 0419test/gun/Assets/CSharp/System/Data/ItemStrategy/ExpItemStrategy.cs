using EZ;
using EZ.Data;
using EZ.DataMgr;

//经验处理逻辑
public class ExpItemStrategy : BaseItemStrategy
{
    private BaseAttrMgr baseAttrMgr;

    public ExpItemStrategy(BaseAttrMgr baseAttrMgr)
    {
        this.baseAttrMgr = baseAttrMgr;
    }

    public override double GetItem(int itemId)
    {
        return baseAttrMgr.GetExp();
    }
    public override bool AddItem(ItemDTO itemDTO)
    {
        double curExp = GetItem(itemDTO.itemId);
        curExp = curExp + itemDTO.num;
        float maxExp = Global.gApp.gGameData.HeroDataConfig.Get(Global.gApp.gGameData.HeroDataConfig.items.Length).expRequire;
        if (curExp > maxExp)
        {
            curExp = maxExp;
        }
        Global.gApp.gSystemMgr.GetBaseAttrMgr().ResetLevel(curExp);
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
