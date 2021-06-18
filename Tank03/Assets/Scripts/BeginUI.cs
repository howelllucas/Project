using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginUI : BaseUI<BeginUI>
{
    public CustomGUIButton btnBegin;
    public CustomGUIButton btnSetting;
    public CustomGUIButton btnQuit;
    public CustomGUIButton btnRank;

    // Start is called before the first frame update
    void Start()
    {
        btnBegin.clickEvent += () =>
        {
            //切换场景
            SceneManager.LoadScene("gameScene");
        };
        btnSetting.clickEvent += () =>
        {
            //打开面板
            settingUI.Instance.ShowMe();
        };
        btnQuit.clickEvent += () =>
        {
            //退出游戏
            Application.Quit();
        };
        btnRank.clickEvent += () =>
        {
            //打开排行榜
        };


    }

   
}
