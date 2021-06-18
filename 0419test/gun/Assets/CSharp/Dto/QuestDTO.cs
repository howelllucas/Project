//任务数据对象
using System;
using System.Collections.Generic;

public class QuestDTO
{
    //任务类型
    public int type;
    //任务map
    public Dictionary<string, QuestItemDTO> questItemDTOMap;
    //上次刷新时间
    public double lastMills;
    public QuestDTO()
    {

    }

    public QuestDTO(int type)
    {
        this.type = type;
        questItemDTOMap = new Dictionary<string, QuestItemDTO>();
    }

}
