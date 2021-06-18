
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{

    public class EffectCacheMgr
    {
        private Dictionary<string, List<EffectNode>> m_EffectCache = new Dictionary<string, List<EffectNode>>();
        private Dictionary<string, List<EffectNode>> m_UsedEffectCache = new Dictionary<string, List<EffectNode>>();

        public void CacheEffect(string name, int count, int maxCount = -1)
        {
            if (maxCount > 0)
            {
                List<EffectNode> effectNodes;
                if (m_EffectCache.TryGetValue(name, out effectNodes) && effectNodes.Count > maxCount)
                {
                    return;
                }
            }
            Transform bulletTsf = Global.gApp.gBulletNode.transform;
            for (int i = 0; i < count; i++)
            {
                EffectNode effectNode = CreateEffect(name);
                Recycle(name, effectNode);
                effectNode.transform.SetParent(bulletTsf);
            }
        }
        public EffectNode GetEffect(string name, int createLimit, bool limitReturnNull = true)
        {
            List<EffectNode> effects;
            EffectNode effectNode;
            if (m_EffectCache.TryGetValue(name, out effects) && effects.Count > 0)
            {
                effectNode = effects[0];
                effects.RemoveAt(0);
                SignToUsed(name, effectNode);
                effectNode.Init();
                return effectNode;
            }
            else
            {
                if (m_UsedEffectCache.TryGetValue(name, out effects) && effects.Count >= createLimit)
                {
                    if (limitReturnNull)
                    {
                        return null;
                    }
                    effectNode = effects[0];
                    effectNode.Init();
                    return effectNode;
                }
                else
                {
                    effectNode = CreateEffect(name);
                    SignToUsed(name, effectNode);
                    effectNode.Init();
                    return effectNode;
                }
            }
        }
        public EffectNode GetEffect(GameObject prefab, int createLimit, bool limitReturnNull = true)
        {
            string guid = prefab.GetInstanceID().ToString();
            List<EffectNode> effects;
            EffectNode effectNode;
            if (m_EffectCache.TryGetValue(guid, out effects) && effects.Count > 0)
            {
                effectNode = effects[0];
                effects.RemoveAt(0);
                SignToUsed(guid, effectNode);
                effectNode.Init();
                return effectNode;
            }
            else
            {
                if (m_UsedEffectCache.TryGetValue(guid, out effects) && effects.Count >= createLimit)
                {
                    if (limitReturnNull)
                    {
                        return null;
                    }
                    effectNode = effects[0];
                    effectNode.Init();
                    return effectNode;
                }
                else
                {
                    effectNode = CreateEffect(guid, prefab);
                    SignToUsed(guid, effectNode);
                    effectNode.Init();
                    return effectNode;
                }
            }
        }
        public EffectNode GetEffect(string name)
        {
            List<EffectNode> effects;
            EffectNode effectNode;
            if (m_EffectCache.TryGetValue(name, out effects) && effects.Count > 0)
            {
                effectNode = effects[0];
                effects.RemoveAt(0);
            }
            else
            {
                effectNode = CreateEffect(name);
            }
            SignToUsed(name, effectNode);
            effectNode.Init();
            return effectNode;
        }
        public EffectNode GetEffect(GameObject prefab)
        {
            string guid = prefab.GetInstanceID().ToString();
            List<EffectNode> effects;
            EffectNode effectNode;
            if (m_EffectCache.TryGetValue(guid, out effects) && effects.Count > 0)
            {
                effectNode = effects[0];
                effects.RemoveAt(0);
            }
            else
            {
                effectNode = CreateEffect(guid, prefab);
            }

            SignToUsed(guid, effectNode);
            if (effectNode != null)
            {                
                effectNode.Init();
            }
            return effectNode;
        }
        private void SignToUsed(string name, EffectNode effectNode)
        {
            List<EffectNode> effects;
            if (!m_UsedEffectCache.TryGetValue(name, out effects))
            {
                effects = new List<EffectNode>();
                m_UsedEffectCache[name] = effects;
            }
            effects.Add(effectNode);
        }
        private void SignToUnused(string name, EffectNode effectNode)
        {
            List<EffectNode> effects;
            if (m_UsedEffectCache.TryGetValue(name, out effects))
            {
                effects.Remove(effectNode);
            }
        }
        private EffectNode CreateEffect(string name)
        {
            GameObject newEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.EffectPath[name]);
            EffectNode effectNode = newEffect.GetComponent<EffectNode>();
            effectNode.SetName(name);
            return effectNode;
        }
        private EffectNode CreateEffect(string guid, GameObject prefab)
        {
            GameObject newEffect = GameObject.Instantiate(prefab);
            EffectNode effectNode = newEffect.GetComponent<EffectNode>();
            if (effectNode == null)
                effectNode = newEffect.AddComponent<EffectNode>();
            effectNode.SetName(guid);
            return effectNode;
        }
        public void Recycle(string name, EffectNode effectNode)
        {
            List<EffectNode> effects;
            if (!m_EffectCache.TryGetValue(name, out effects))
            {
                effects = new List<EffectNode>();
                m_EffectCache[name] = effects;
            }
            SignToUnused(name, effectNode);
            effectNode.transform.SetParent(Global.gApp.gBulletNode.transform, true);
            effectNode.transform.position = new Vector3(1000, 0, 0);
            effects.Add(effectNode);
        }
        public void Clear()
        {
            m_EffectCache.Clear();
            m_UsedEffectCache.Clear();
        }
    }
}
