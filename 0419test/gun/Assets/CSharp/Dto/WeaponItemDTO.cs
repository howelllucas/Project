//技能项数据对象
public class WeaponItemDTO
{
    //物品id
    public string id;
    //等级
    public int level;
    //状态
    public int state;
    //起始 等级
    public int startLv;
    // 品质等级
    public int qualityLv;

    public WeaponItemDTO()
    {

    }

    public WeaponItemDTO(string id, int level)
    {
        this.id = id;
        this.level = level;
        qualityLv = 0;
        startLv = -1;
        state = WeaponStateConstVal.NONE;
    }

}
