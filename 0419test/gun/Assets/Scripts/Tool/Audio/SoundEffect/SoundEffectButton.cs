using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Audio
{
    [RequireComponent(typeof(Button))]
    public class SoundEffectButton : MonoBehaviour
    {
        public string soundEffectName;

        private void Awake()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(PlaySoundEffect);
        }

        private void PlaySoundEffect()
        {
            SoundEffectManager.Instance["2d"].PlaySoundEffectOneShot(soundEffectName);
        }
    }
}
