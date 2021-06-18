using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;

namespace Game.Audio
{
    public class AudioMixerVolumeManager : InnerMonoSingleton<AudioMixerVolumeManager>
    {
        public AudioMixerVolumeController[] controllerList;
        private Dictionary<string, AudioMixerVolumeController> controllerDic = new Dictionary<string, AudioMixerVolumeController>();

        protected override void InitSingleton()
        {
            base.InitSingleton();
            for (int i = 0; i < controllerList.Length; i++)
            {
                controllerDic[controllerList[i].name] = controllerList[i];
            }
            DontDestroyOnLoad(gameObject);
        }

        public AudioMixerVolumeController this[string name]
        {
            get
            {
                AudioMixerVolumeController controller;
                if (controllerDic.TryGetValue(name, out controller))
                {
                    return controller;
                }
                Debug.LogError("AudioMixerController不存在:" + name);
                return null;
            }
        }
    }
}