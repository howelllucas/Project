using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioMgr : BaseManager<AudioMgr>
{
    
    private AudioSource BKMusic = null;
    //
    private float musicValue = 1;
    //存放音效的列表容器
    private List<AudioSource> SoundList = new List<AudioSource>();
    private AudioSource Sound = null;
    private float soundValue = 1;
    private GameObject soundObj=null;

    public  AudioMgr()
    {
        MonoMgr.GetInstance().AddUpdateListener(audioUpdate);
    }
    private void audioUpdate()
    {
        for (int i = SoundList.Count-1; i >=0 ; --i)
        {
            if (!SoundList[i].isPlaying)
            {

                GameObject.Destroy(SoundList[i]);
                SoundList.Remove(SoundList[i]);
            }
        }
    }

    //背景播放
    public void PlayBKMusic(string name)
    {
        if (BKMusic==null)
        {
            GameObject BkMusicGo = new GameObject();
            BkMusicGo.name = "BKMusic";
            BKMusic=BkMusicGo.AddComponent<AudioSource>();
        }
        ResMgr.GetInstance().LoadAsync<AudioClip>(name,(c)=>
        {
            BKMusic.clip = c;
            BKMusic.volume = musicValue;
            BKMusic.Play();
        });
        
    }
    //背景暂停
    public void PauseBKMusic()
    {
        if (BKMusic==null)
        {
            return;
        }
        BKMusic.Pause();
    }
    //背景停止
    public void StopBKMusic()
    {
        if (BKMusic == null)
        {
            return;
        }
        BKMusic.Stop();
    }
    //背景音量大小
    public void changeBKValue(float v)
    {
        musicValue = v;
        if (BKMusic == null)
        {
            return;
        }
        BKMusic.volume = musicValue;
    }
    //播放音效
    public void PlaySound(string name, bool isloop, UnityAction<AudioSource> callback)
    {
        if (soundObj == null)
        {
            soundObj = new GameObject();
            soundObj.name = name;
        }

        AudioSource soundAu = soundObj.AddComponent<AudioSource>();

        ResMgr.GetInstance().LoadAsync<AudioClip>(name, (c) =>
        {
            soundAu.clip = c;
            soundAu.volume = soundValue;
            soundAu.Play();
            soundAu.loop = isloop;
            SoundList.Add(soundAu);
            callback(soundAu);
        });
    }
        //停止音效
    public void StopSound(AudioSource source)
    {
        if (SoundList.Contains(source))
        {
            source.Stop();
            SoundList.Remove(source);
            GameObject.Destroy(source);
        }
        
    }
    //音效大小
    public void ChangeSoundValue(float v)
    {
        soundValue = v;
        for (int i = 0; i < SoundList.Count; i++)
        {
            SoundList[i].volume = v;
        }
    }
}
