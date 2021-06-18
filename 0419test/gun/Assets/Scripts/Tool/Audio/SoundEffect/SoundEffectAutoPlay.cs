using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    public class SoundEffectAutoPlay : MonoBehaviour
    {
        public string type = "3d";
        public string soundEffectName;
        public float delay;

        void Start()
        {
            if (delay <= 0)
                SoundEffectManager.Instance[type].PlaySoundEffectOneShot(soundEffectName);
            else
                SoundEffectManager.Instance[type].PlaySoundEffectOneShot(soundEffectName, delay);
        }


    }
}

