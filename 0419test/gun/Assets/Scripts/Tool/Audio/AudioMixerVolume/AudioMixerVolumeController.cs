using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Audio
{
    [System.Serializable]
    public class AudioMixerVolumeController
    {
        [SerializeField]
        private AudioMixer audioMixer;

        public AudioMixer AudioMixer
        {
            get
            {
                return audioMixer;
            }
        }

        public string name;
        public string volumeParameterName;

        private const float LowerVolumeBound = -80.0f;
        private const float UpperVolumeBound = 0.0f;

        public float Volume
        {
            get
            {
                if (muted)
                {
                    return volumeBeforeMute;
                }
                else
                {
                    return VolumeNoMutedGuards;
                }
            }
            set
            {
                float newVolumeT = Mathf.Clamp01(value);
                if (muted)
                {
                    volumeBeforeMute = newVolumeT;
                }
                else
                {
                    VolumeNoMutedGuards = newVolumeT;
                }
            }
        }

        private float VolumeNoMutedGuards
        {
            get
            {
                float volume = 0.0f;
                if (AudioMixer)
                {
                    AudioMixer.GetFloat(volumeParameterName, out volume);
                }

                volume = Mathf.InverseLerp(LowerVolumeBound, UpperVolumeBound, volume);
                return volume;
            }

            set
            {
                if (AudioMixer)
                {
                    AudioMixer.SetFloat(volumeParameterName,
                        Mathf.Lerp(LowerVolumeBound, UpperVolumeBound, value));
                }
            }
        }

        private bool muted;
        private float volumeBeforeMute;

        public bool Muted
        {
            get
            {
                return muted;
            }
            set
            {
                if (muted != value)
                {
                    if (value)
                    {
                        volumeBeforeMute = VolumeNoMutedGuards;
                        VolumeNoMutedGuards = 0.0f;
                    }
                    else
                    {
                        VolumeNoMutedGuards = volumeBeforeMute;
                    }
                }

                muted = value;
            }
        }
    }
}