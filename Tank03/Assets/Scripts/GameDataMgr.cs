using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr
{
    //单例模式
    private static GameDataMgr instence = new GameDataMgr();

    public static GameDataMgr Instence
    {
        get => instence;
    }

    public MusicData musicData ;

    public GameDataMgr()
    {
       
        //构造函数读取数据
        musicData = PlayerPrefsDataMgr.Instance.LoadData(typeof(MusicData), "Music") as MusicData;

        //判断第一次，如果是第一次给默认值，要不读取后都是0，进游戏音乐不播放
        if (musicData.isFirst == false)
        {
            musicData.BKValue = 1f;
            musicData.soundValue = 1f;
            musicData.isFirst = true;
            musicData.isOpenBK = true;
            musicData.isOpenSound = true;
            PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
        }
    }

    //给外部提供API，方便数据的改变存储
    //背景音乐开关
    public void OpenOrCloseBKMusic(bool isOpen)
    {
        musicData.isOpenBK = isOpen;
        PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
    }
    //音效开关
    public void OpenOrCloseBKSound(bool isOpen)
    {
        musicData.isOpenSound = isOpen;
        PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
    }
    //背景音乐大小
    public void ChangeBKValue(float value)
    {
        musicData.BKValue = value;
        PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
    }
    //音效大小
    public void ChangeSoundValue(float value)
    {
        musicData.soundValue = value;
        PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
    }
}
