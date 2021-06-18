using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;
using Game.Pool;
using System;

namespace Game.Audio
{
    public class SoundEffectManager : InnerMonoSingleton<SoundEffectManager>
    {
        public SoundEffectCreater[] createrList;
        private Dictionary<string, SoundEffectCreater> createrDic = new Dictionary<string, SoundEffectCreater>();

        protected override void InitSingleton()
        {
            base.InitSingleton();

            for (int i = 0; i < createrList.Length; i++)
            {
                createrList[i].Init();
                createrDic[createrList[i].name] = createrList[i];
            }

            DontDestroyOnLoad(gameObject);
        }

        public SoundEffectCreater this[string name]
        {
            get
            {
                SoundEffectCreater creater;
                if (!createrDic.TryGetValue(name, out creater))
                {
                    creater = new SoundEffectCreater();
                    creater.name = name;
                    creater.root = transform;
                    creater.Init();
                    createrDic[name] = creater;
                }
                return creater;
            }
        }

        private void Update()
        {
            for (int i = 0; i < createrList.Length; i++)
            {
                createrList[i].Update();
            }
        }

    }

    [System.Serializable]
    public class SoundEffectCreater
    {
        public string name = "SourceCreater";
        public Transform root;
        public float volumeForAndroid = 1;
        public float volumeForIOS = 1;
        public AudioSource sourceTemplate;
        public string clipRootPath = "SoundEffect/";
        private SpawnPool spawnPool;
        private List<SoundEffectPlayer> autoDestroyList = new List<SoundEffectPlayer>();

        internal void Init()
        {
            if (root == null)
            {
                root = new GameObject(name).transform;
            }

            if (sourceTemplate == null)
            {
                var template = new GameObject("SourceTemplate" + name);
                template.transform.SetParent(root);
                sourceTemplate = template.AddComponent<AudioSource>();
            }

#if UNITY_ANDROID
            sourceTemplate.volume = volumeForAndroid;
#else
            sourceTemplate.volume = volumeForIOS;
#endif
            PrefabPool prefabPool = new PrefabPool(sourceTemplate.transform);
            spawnPool = PoolManager.Pools.Create("SoundEffect_" + name, root.gameObject);
            spawnPool.CreatePrefabPool(prefabPool);
        }

        public SoundEffectPlayer GetSoundEffectPlayer(string name, bool loop)
        {
            return new SoundEffectPlayer(name, loop, this);
        }

        public void PlaySoundEffectOneShot(string name)
        {
            SoundEffectPlayer player = GetSoundEffectPlayer(name, false);
            player.Play();
            autoDestroyList.Add(player);
        }

        public void PlaySoundEffectOneShot(string name, Vector3 pos)
        {
            SoundEffectPlayer player = GetSoundEffectPlayer(name, false);
            player.Position = pos;
            player.Play();
            autoDestroyList.Add(player);
        }

        public void PlaySoundEffectOneShot(string name, float delay)
        {
            SoundEffectPlayer player = GetSoundEffectPlayer(name, false);
            player.PlayDelay(delay);
            autoDestroyList.Add(player);
        }

        public void PlaySoundEffectOneShot(string name, float delay, Vector3 pos)
        {
            SoundEffectPlayer player = GetSoundEffectPlayer(name, false);
            player.Position = pos;
            player.PlayDelay(delay);
            autoDestroyList.Add(player);
        }

        internal AudioSource SpawnSource(string name, bool loop)
        {

            var sourceObj = spawnPool.Spawn(sourceTemplate.transform);
            AudioSource source = sourceObj.GetComponent<AudioSource>();
            source.loop = loop;

            AudioClip clip = ResourceMgr.singleton.AddResource(clipRootPath + name) as AudioClip;
            source.clip = clip;

            source.volume = sourceTemplate.volume;
            return source;
        }

        internal void DespawnSource(AudioSource source)
        {
            source.Stop();
            source.clip = null;
            source.volume = sourceTemplate.volume;
            spawnPool.Despawn(source.transform);
        }

        internal void Update()
        {
            for (int i = 0; i < autoDestroyList.Count; i++)
            {
                if (i >= autoDestroyList.Count)
                    break;
                var player = autoDestroyList[i];
                if (!player.IsPlaying)
                {
                    player.Destroy();
                    autoDestroyList.RemoveAt(i);
                    i--;
                }
            }
        }

        internal void StartCoroutine(IEnumerator fadeIEnumerator)
        {
            spawnPool.StartCoroutine(fadeIEnumerator);
        }

        internal void StopCoroutine(IEnumerator fadeIEnumerator)
        {
            spawnPool.StopCoroutine(fadeIEnumerator);
        }
    }
}

