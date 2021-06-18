//npc任务数据对象
using EZ.DataMgr;

public class NpcQuestItemDTO
{
    //npcId
    public string npcId;
    //任务id
    public int npcQuestId;
    //状态
    public int state;
    //进度
    public double cur;
    //锁定的红心掉落位置
    public int lockRedHeartIndex;
    // 锁定的worker 的index
    public int lockWorkerIndex;
    public NpcQuestItemDTO() { }
    public NpcQuestItemDTO(string npcId, int npcQuestId)
    {
        lockRedHeartIndex = -1;
        lockWorkerIndex = -1;
        this.npcId = npcId;
        this.npcQuestId = npcQuestId;
        this.state = NpcState.OnGoing;
    }
    public NpcQuestItemDTO(string npcId)
    {
        this.npcId = npcId;
        this.npcQuestId = -1;
        lockRedHeartIndex = -1;
        lockWorkerIndex = -1;
        this.state = NpcState.None;
    }

}
