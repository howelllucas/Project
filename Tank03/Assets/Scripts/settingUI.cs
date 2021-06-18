using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingUI : BaseUI<settingUI>
{
    //关联控件
    public CustomGUISlider sliderMusic;
    public CustomGUISlider sliderSound;
    public CustomGUIToggle toggleMusic;
    public CustomGUIToggle toggleSound;
    public CustomGUIButton btnClose;
    // Start is called before the first frame update
    void Start()
    {
        sliderMusic.changeValue += (value) =>
        {
            GameDataMgr.Instence.ChangeBKValue(value);
            //处理音乐值
        };
        sliderSound.changeValue += (value) =>
        {
            GameDataMgr.Instence.ChangeSoundValue(value);
            //处理音效值
        };
        toggleMusic.changeValue += (value) =>
        {
            GameDataMgr.Instence.OpenOrCloseBKMusic(value);
            //处理音效值
        };
        toggleSound.changeValue += (value) =>
        {
            GameDataMgr.Instence.OpenOrCloseBKSound(value);
            //处理音效值
        };
        btnClose.clickEvent += () =>
        {
            HideMe();
            BeginUI.Instance.ShowMe();
        };
        HideMe();
    }

    // 更新面板信息
    public void UpdatePanelInfo()
    {
        //变量存储music数据 用来关联显示面板
        MusicData data = GameDataMgr.Instence.musicData;
        //关联数据到显示面板
        sliderMusic.nowValue = data.BKValue;
        sliderSound.nowValue = data.soundValue;
        toggleMusic.isSel = data.isOpenBK;
        toggleSound.isSel = data.isOpenSound;
    }

    public override void ShowMe()
    {
        base.ShowMe();
        UpdatePanelInfo();
    }
}
