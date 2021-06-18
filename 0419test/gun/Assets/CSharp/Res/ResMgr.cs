using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace EZ
{
    public class ResMgr
    {
        protected Dictionary<string, GameObject> m_Prefabs;
        protected Dictionary<string, SpriteAtlas> m_SpriteAtlas;
        public ResMgr()
        {
            m_Prefabs = new Dictionary<string, GameObject>();
            m_SpriteAtlas = new Dictionary<string, SpriteAtlas>();
        }
        public GameObject LoadPrefab(string path)
        {
            if (m_Prefabs.ContainsKey(path))
            {
                GameObject prefab = m_Prefabs[path];
                return prefab;
            }
            else
            {
                GameObject prefab = LoadPrefabImp(path);
                m_Prefabs.Add(path, prefab);
                return prefab;
            }
        }
        public void UnLoadAssets()
        {
            m_Prefabs.Clear();
            m_SpriteAtlas.Clear();
            Resources.UnloadUnusedAssets();
        }
        public GameObject GetCachePrefab(string path)
        {
            if (m_Prefabs.ContainsKey(path))
            {
                GameObject prefab = m_Prefabs[path];
                return prefab;
            }
            return null;
        }
        public GameObject InstantiateObj(string path, ResourceRequest rr)
        {
            if (m_Prefabs.ContainsKey(path))
            {
                GameObject prefab = m_Prefabs[path];
                return prefab;
            }
            return (GameObject)Object.Instantiate(rr.asset);
        }
        public bool CheckHasPrefab(string path)
        {
            return m_Prefabs.ContainsKey(path);
        } 
        public GameObject InstantiateObj(string path)
        {
            GameObject obj = LoadPrefab(path);
            if (obj == null)
            {
                Debug.LogError("obj is null, the path : " + path);
                return new GameObject();
            }
            return Object.Instantiate(obj) as GameObject;
        }
        public Sprite LoadSprite(string name,string spritePath)
        {
            SpriteAtlas atlas = LoadSpriteAtlas(ref spritePath);
            return atlas.GetSprite(name);
        }
        public SpriteAtlas LoadSpriteAtlas(ref string path)
        {
            if (m_SpriteAtlas.ContainsKey(path))
            {
                SpriteAtlas spriteAtlas = m_SpriteAtlas[path];
                return spriteAtlas;
            }
            else
            {
                string fullPath = EffectConfig.EffectPath[path];
                SpriteAtlas spriteAtlas = LoadAssets<SpriteAtlas>(fullPath);
                m_SpriteAtlas[path] = spriteAtlas;
                return spriteAtlas;
            }
        }
        public U LoadAssets<U>(string path) where U:Object
        {
            return Resources.Load<U>(path);
        }

        virtual public GameObject LoadPrefabImp(string path)
        {
            return null;
        }
    }
}
