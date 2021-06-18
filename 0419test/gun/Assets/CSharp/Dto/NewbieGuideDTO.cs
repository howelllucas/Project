//新手引导数据对象
using System.Collections.Generic;

public class NewbieGuideDTO
{
    //当前引导id
    public int curId;
    //引导步骤
    public Dictionary<string, int> map;

    public NewbieGuideDTO()
    {
        map = new Dictionary<string, int>();
    }

}
