//杂项数据对象
using System;
using System.Collections.Generic;

public class PassDTO
{
    //当前关卡id
    public bool passMaxPass;
    public bool hasOpenTankUi;
    public int curPassId;
    public int enterTimes;
    public List<int> branchPass = new List<int>();
    public double recordTime;

    //游玩关卡次数记录
    public Dictionary<string, int> passTimesDic;

    // 当前必要关卡延迟
    public int curFBPPassOffset;
    // 当前非必要关卡延迟
    public int curOBPPassOffset;
    // 强制关卡是否通过
    public bool fBPPassed;
    // 当前强制关卡序号
    public int fBPIndex;
    // 强制关卡 偏移
    public int fBPOffset;
    // 非强制关卡是否通过
    public bool oBPPassed;
    // 非强制关卡序号
    public int oBPIndex;
    // 非强制关卡 偏移
    public int oBPOffset;
    public PassDTO()
    {
        this.curPassId = 100001;
        enterTimes = 0;
        recordTime = -1;
        passMaxPass = false;
        hasOpenTankUi = false;
        passTimesDic = new Dictionary<string, int>();

        curFBPPassOffset = 0;
        curOBPPassOffset = 0;
        fBPPassed = false;
        fBPIndex = -1;
        fBPOffset = 0;
        oBPPassed = false;
        oBPIndex = 0;
        oBPOffset = 0;
    }
}
