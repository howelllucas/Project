using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class AudioSourceController : MonoBehaviour
    {
        private AudioSource gAudioSource_ui;
        private AudioSource gAudioSource;
        private AudioSource gSecondAudioSource;
        private AudioSource gMusicSource;
        private HashSet<string> m_FrameStartAudioSet = new HashSet<string>();
        private float m_DtTime;
        private float m_StartTime;
        private float m_CurTime;
        private float m_DtRange;
        private float m_CurDtTime;
        private float m_CurClearTime = 0;
        private float m_CurClearDtTime = 0.1f;
        private void Awake()
        {
            float aT = audioVolumnTmp;
            if (aT != -1f)
            {
                audioVolumn = aT;
                audioVolumnTmp = -1f;
            }
            float mT = musicVolumnTmp;
            if (mT != -1f)
            {
                musicVolumn = mT;
                musicVolumnTmp = -1f;
            }

            //mAudioVolumn = PlayerPrefs.GetFloat("audio_stgate", 1);
            mMusicVolumn = PlayerPrefs.GetFloat("music_state", 1);
            mVibe = PlayerPrefs.GetInt("vibe_state", 1);

            gAudioSource_ui = gameObject.AddComponent<AudioSource>();
            gAudioSource = gameObject.AddComponent<AudioSource>();
            gMusicSource = gameObject.AddComponent<AudioSource>();
            gSecondAudioSource = gameObject.AddComponent<AudioSource>();
            gAudioSource.volume = mAudioVolumn;
            gSecondAudioSource.volume = mMusicVolumn;
            gMusicSource.volume = mMusicVolumn;
        }

        public void LateUpdate()
        {
            m_CurClearTime += Time.deltaTime;
            if (m_CurClearTime >= m_CurClearDtTime)
            {
                m_FrameStartAudioSet.Clear();
                m_CurClearTime -= m_CurClearDtTime;
            }
        }

        public AudioSource GetAudioSource() { return gAudioSource; }
        public AudioSource GetMusicSource() { return gMusicSource; }

        public float audioVolumnTmp
        {
            get { return PlayerPrefs.GetFloat("audio_stgate_tmp", -1f); }
            set
            {
                PlayerPrefs.SetFloat("audio_stgate_tmp", value);
            }
        }
        public float musicVolumnTmp
        {
            get { return PlayerPrefs.GetFloat("music_stgate_tmp", -1f); }
            set
            {
                PlayerPrefs.SetFloat("music_stgate_tmp", value);
            }
        }

        [Range(0, 1)]
        private float mAudioVolumn = 1;
        public float audioVolumn
        {
            get { return mAudioVolumn; }
            set
            {
                mAudioVolumn = value;
                gAudioSource.volume = value;
                PlayerPrefs.SetFloat("audio_stgate", value);
            }
        }

        [Range(0, 1)]
        private float mMusicVolumn = 1;
        public float musicVolumn
        {
            get { return mMusicVolumn; }
            set
            {
                mMusicVolumn = value;
                gMusicSource.volume = value;
                gSecondAudioSource.volume = value;
                PlayerPrefs.SetFloat("music_state", value);
            }
        }
        //震动
        [Range(0, 1)]
        private int mVibe = 1;
        public int vibe
        {
            get { return mVibe; }
            set
            {
                mVibe = value;
                PlayerPrefs.SetInt("vibe_state", value);
            }
        }

        private void Update()
        {
            if (gSecondAudioSource.isPlaying)
            {
                m_CurTime += BaseScene.GetDtTime();
                if (m_CurTime >= m_CurDtTime)
                {
                    m_CurTime = 0;
                    m_CurDtTime = m_DtTime + Random.Range(0, m_DtRange);
                    gSecondAudioSource.time = m_StartTime;
                }
            }
        }
        public void PlayOneShot(AudioClip clip, bool isUI = false)
        {
            if (clip != null && !m_FrameStartAudioSet.Contains(clip.name))
            {
                if (isUI)
                {
                    gAudioSource_ui.PlayOneShot(clip);
                }
                else if (mAudioVolumn > 0f)
                {
                    gAudioSource.PlayOneShot(clip);
                }
                m_FrameStartAudioSet.Add(clip.name);
            }
        }

        public AudioSource PlayLoopSource(string clipName)
        {
            if (mAudioVolumn > 0)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.loop = true;
                AudioClip audioClip = Resources.Load<AudioClip>("Sounds/" + clipName);
                audioSource.clip = audioClip;
                audioSource.Play();
                return audioSource;
            }
            else
            {
                return null;
            }
        }
        public void DestroyLoopSource(AudioSource audioSource)
        {
            if (audioSource != null)
            {
                audioSource.Stop();
                Destroy(audioSource);
            }
        }
        public void PlayOneShot(string clipName, bool isUI = false)
        {
            if (isUI || mAudioVolumn > 0f)
            {
                AudioClip audioClip = Resources.Load<AudioClip>("Sounds/" + clipName);
                PlayOneShot(audioClip,isUI);
            }
        }

        public void CommonClickAutio()
        {
            Global.gApp.gAudioSource.PlayOneShot("main_click", true);
        }

        public void StopPlayLoopAudio()
        {
            gSecondAudioSource.clip = null;
            gSecondAudioSource.Stop();
        }
        public void PlayLoop(AudioClip clip, float delay, float startTime, float dtTime, float dtRange)
        {
            if (clip != null)
            {
                m_StartTime = startTime;
                m_DtTime = dtTime;
                m_DtRange = dtRange;
                m_CurDtTime = dtTime + Random.Range(0, dtRange);
                m_CurTime = 0;
                gSecondAudioSource.loop = true;
                gSecondAudioSource.clip = clip;
                gSecondAudioSource.time = startTime;
                //gSecondAudioSource.PlayDelayed(delay);
                gSecondAudioSource.Play();
            }
        }

        public void UnPause()
        {
            gAudioSource_ui.UnPause();
            gAudioSource.UnPause();
            gSecondAudioSource.UnPause();
            gMusicSource.UnPause();
        }
        public void Pause()
        {
            gAudioSource_ui.Pause();
            gAudioSource.Pause();
            gSecondAudioSource.Pause();
            gMusicSource.Pause();
        }
    }
}
