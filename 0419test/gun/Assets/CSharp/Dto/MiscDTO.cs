//杂项数据对象
using System;
using System.Collections.Generic;

public class MiscDTO
{
    //上次登录那日0点时间
    public double lastLoginDay;
    //累计登录天数
    public int sumLoginDay;
    //连续登录天数
    public int continuousLoginDay;
    public int bbb;

    //留存天数
    public int retainDays;
    //创建时间
    public double createDate;
    //上次登录时间点
    public double lastLoginTime;

    //上次离线时间点
    public double lastOffline;

    //首次进主界面
    public int firstMain;

    #region 看视频得能量
    public int AdEnegyTimes;
    public double LastAdEnegyMills;
    #endregion

    #region 看视频开箱子
    public int AdBoxTimes;
    public double LastAdBoxMills;
    #endregion

    #region 首冲提醒
    public int EveryDayFP;
    public int FirstPurchase;
    #endregion

    #region 在线奖励 
    // 游戏在线时长
    public double totalOnLineTime;
    public int getTimesEachDay;

    #endregion

    #region 看视频得狗牌的那种

    public int VideoMDTData;
    public int VideoMDTDatatTimes;
    public int VideoMDTDataGift;
    public double LastVideoMDTDataMills;

    #endregion
    //验单记录
    public Dictionary<string, int> orderDic;
    //累计充值(美分)
    public double sumPurchase;
    //累计充值次数
    public int sumPurchaseTimes;
    //累计观看广告次数
    public int sumAdTimes;
    //当前语言
    public string language;
    //是否弹出过倒计时充值
    public int showTimeGiftToday;
    //今日启动次数
    public int startTimesToday;
    //限时活动结束时间
    public string timeGiftEndTime;
    //限时活动触发时间
    public double timeGiftStartTime;
    //充值次数记录
    public Dictionary<string, int> purchaseTimesDic;

    public bool HasOpendCampUi;

    public List<int> Dialogues;

    public MiscDTO()
    {
        HasOpendCampUi = false;
        sumLoginDay = 1;
        lastLoginDay = DateTimeUtil.GetMills(DateTime.Today);
        createDate = DateTimeUtil.GetMills(DateTime.Now);
        continuousLoginDay = 1;
        bbb = 1000;
        AdEnegyTimes = 0;
        totalOnLineTime = 0;
        orderDic = new Dictionary<string, int>();
        purchaseTimesDic = new Dictionary<string, int>();
        Dialogues = new List<int>();
    }
}
