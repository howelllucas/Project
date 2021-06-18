//技能项数据对象
public class SkillItemDTO
{
    //物品id
    public string id;
    //等级
    public int level;
    //状态
    public int state;

    public SkillItemDTO(string id, int level)
    {
        this.id = id;
        this.level = level;
        this.state = WeaponStateConstVal.NONE;
    }
    public SkillItemDTO(string id, int level, int state)
    {
        this.id = id;
        this.level = level;
        this.state = state;
    }

    public SkillItemDTO()
    {

    }

}
