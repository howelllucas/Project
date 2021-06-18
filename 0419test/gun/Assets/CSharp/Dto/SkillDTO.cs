using System.Collections.Generic;

//技能数据对象
public class SkillDTO
{
    //技能map
    public Dictionary<string, SkillItemDTO> itemMap;
    //随机技能次数
    public int times;

    public SkillDTO()
    {
        itemMap = new Dictionary<string, SkillItemDTO>();
    }
}
