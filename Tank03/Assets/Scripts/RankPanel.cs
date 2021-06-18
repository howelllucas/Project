using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : BaseUI<settingUI>
{
    //关联public的 控件对象

    public CustomGUIButton btnClose;

    //因为控件较多 拖的话 工作量太大了 我们直接偷懒 通过代码找
    
    private List<CustomGUILabel> labPM = new List<CustomGUILabel>();
    private List<CustomGUILabel> labName = new List<CustomGUILabel>();
    private List<CustomGUILabel> labScore = new List<CustomGUILabel>();
    private List<CustomGUILabel> labTime = new List<CustomGUILabel>();

    // Start is called before the first frame update
    void Start()
    {
        //print(this.transform.Find("PM/LabelPM1").name);
        for (int i = 1; i <= 10 ; i++)
        {
            //小知识应用 找子对象的子对象 可以通过 斜杠来区分父子关系
            labPM.Add(this.transform.Find("PM/labPM" + i).GetComponent<CustomGUILabel>());
            labName.Add(this.transform.Find("Name/labName" + i).GetComponent<CustomGUILabel>());
            labScore.Add(this.transform.Find("Score/labScore" + i).GetComponent<CustomGUILabel>());
            labTime.Add(this.transform.Find("Time/labTime" + i).GetComponent<CustomGUILabel>());
        }
        //处理事件监听逻辑

        btnClose.clickEvent += () =>
        {
            HideMe();
            BeginUI.Instance.ShowMe();
        };

        HideMe();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        UpdatePanelInfo();
    }

    public void UpdatePanelInfo()
    {
        //处理根据排行榜数据 更新面板
    }
}
