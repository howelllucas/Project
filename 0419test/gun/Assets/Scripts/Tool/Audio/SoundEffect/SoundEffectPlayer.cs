using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio
{
    public class SoundEffectPlayer
    {
        private SoundEffectCreater creater;
        private AudioSource source;
        private float volume;
        private bool isPlaying;
        private IEnumerator fadeIEnumerator;

        public SoundEffectPlayer(string name, bool loop, SoundEffectCreater creater)
        {
            source = creater.SpawnSource(name, loop);
            this.creater = creater;
            this.volume = source.volume;
            this.isPlaying = false;
        }

        public bool IsPlaying
        {
            get
            {
                return source != null ? source.isPlaying && isPlaying : false;
            }
        }

        public float Volume
        {
            get
            {
                return volume;
            }
            set
            {
                if (source != null)
                    source.volume = value;
                this.volume = value;
            }
        }

        public Vector3 Position
        {
            set
            {
                if (source != null)
                    source.transform.position = value;
            }
        }

        public void Play()
        {
            if (source == null)
            {
                Debug.LogError("该播放器已被删除!");
                return;
            }
            StopIEnumerator();
            if (!source.isPlaying)
                source.Play();
            isPlaying = true;
        }

        public void PlayDelay(float delay)
        {
            if (source == null)
            {
                Debug.LogError("该播放器已被删除!");
                return;
            }
            StopIEnumerator();
            if (!source.isPlaying)
                source.PlayDelayed(delay);
            isPlaying = true;
        }

        public void Stop()
        {
            if (source == null)
            {
                Debug.LogError("该播放器已被删除!");
                return;
            }
            StopIEnumerator();
            if (source.isPlaying)
                source.Stop();
            isPlaying = false;
        }

        public void FadeIn(float duration, float delay = 0)
        {
            if (source == null)
            {
                Debug.LogError("该播放器已被删除!");
                return;
            }
            StopIEnumerator();
            fadeIEnumerator = FadeInIEnumerator(duration, delay);
            creater.StartCoroutine(fadeIEnumerator);
            isPlaying = true;
        }

        IEnumerator FadeInIEnumerator(float duration, float delay)
        {
            if (delay > 0)
                yield return new WaitForSecondsRealtime(delay);
            float speed = volume / duration;
            if (!source.isPlaying)
            {
                source.volume = 0;
                source.Play();
            }

            while (source.volume < volume)
            {
                source.volume += speed * Time.unscaledDeltaTime;
                yield return null;
            }

            source.volume = volume;
            yield return null;
            fadeIEnumerator = null;
        }

        public void FadeOut(float duration, bool stopAutoDestroy, float delay = 0)
        {
            if (source == null)
            {
                Debug.LogError("该播放器已被删除!");
                return;
            }
            StopIEnumerator();
            fadeIEnumerator = FadeOutIEnumerator(duration, stopAutoDestroy, delay);
            creater.StartCoroutine(fadeIEnumerator);
            isPlaying = false;
        }

        IEnumerator FadeOutIEnumerator(float duration, bool stopAutoDestroy, float delay)
        {
            if (source == null)
            {
                Debug.LogError("该播放器已被删除!");
                yield break;
            }
            if (delay > 0)
                yield return new WaitForSecondsRealtime(delay);
            float speed = source.volume / duration;
            while (source.isPlaying && source.volume > 0)
            {
                source.volume -= speed * Time.unscaledDeltaTime;
                yield return null;
            }
            source.Stop();
            yield return null;
            fadeIEnumerator = null;
            if (stopAutoDestroy)
                Destroy();
        }

        void StopIEnumerator()
        {
            if (fadeIEnumerator != null)
                creater.StopCoroutine(fadeIEnumerator);
            fadeIEnumerator = null;
        }

        public void Destroy()
        {
            if (source == null)
            {
                Debug.LogError("该播放器已被删除!");
                return;
            }
            StopIEnumerator();
            creater.DespawnSource(source);
            source = null;
        }
    }
}

