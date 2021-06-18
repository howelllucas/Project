
//任务单项数据对象
public class QuestItemDTO
{
    //任务id
    public int id;
    //当前值
    public double cur;
    //状态
    public int state;

    public QuestItemDTO()
    {

    }

    public QuestItemDTO(int questId, double cur)
    {
        this.id = questId;
        state = QuestStateConstVal.UNFINISH;
        this.cur = cur;
    }

}
