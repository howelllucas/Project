using System;
using System.Collections.Generic;
namespace EZ
{
    public interface ISafeMapUpdate
    {
        void TimeUpdate(float dt,bool isEnd);
    }
    public class SafeMap<Tkey,TVal>
    {
        private Dictionary<Tkey, TVal> m_Datas;
        private Dictionary<Tkey, TVal> m_AddCache;
        private Dictionary<Tkey, bool> m_RemoveCache;
        private int m_TraversalCount = 0;
        private bool m_IsDirty = false;
		private bool m_IsClearData = false;
        public SafeMap()
        {
            Init();
        }
        private void Init()
        {
            m_Datas = new Dictionary<Tkey, TVal>();
            m_AddCache = new Dictionary<Tkey, TVal>();
            m_RemoveCache = new Dictionary<Tkey, bool>();
            m_TraversalCount = 0;
            m_IsDirty = false;
        }

        public Dictionary<Tkey,TVal> GetAll()
        {
            return m_Datas;
        }
        public bool TryGetValue(Tkey key,out TVal value)
        {
            return m_Datas.TryGetValue(key, out value);
        }

        public TVal Get(Tkey key)
        {
            TVal val;
            if(m_Datas.TryGetValue(key,out val))
            {
                return val;
            }
            else
            {
                return default(TVal);
            }
        }
        public void Add(Tkey key,TVal val)
        {
            if(m_TraversalCount > 0)
            {
                m_AddCache[key] = val;
                m_RemoveCache.Remove(key);
                m_IsDirty = true;
            }
            else
            {
                m_Datas[key] = val;
            }
        }

        public void Remove(Tkey key)
        {
            if(m_TraversalCount > 0)
            {
                m_RemoveCache.Add(key, true);
                m_AddCache.Remove(key);
                m_IsDirty = true;
            }
            else
            {
                m_Datas.Remove(key);
            }
        }

        public void Foreach(Func<Tkey, TVal, bool> func)
        {
            m_IsDirty = false;
            m_TraversalCount = m_TraversalCount + 1;
            foreach (KeyValuePair<Tkey, TVal> kv in m_Datas)
            {
                if (!m_RemoveCache.ContainsKey(kv.Key))
                {
                    if (func(kv.Key, m_Datas[kv.Key]))
                    {
                        break;
                    }
                }
            }
            ClearCacheData();
        }
  
        public void Update(float dt,Action<TVal,float> upFunc)
        {
			if(m_IsClearData)
			{
				Clear();
				m_IsClearData = false;
			}	
            m_IsDirty = false;
            m_TraversalCount = m_TraversalCount + 1;
            foreach (KeyValuePair<Tkey, TVal> kv in m_Datas)
            {
                if (!m_RemoveCache.ContainsKey(kv.Key))
                {
                    upFunc(kv.Value, dt);
                }
            }
            ClearCacheData();
        }
        private void ClearCacheData()
        {
            m_TraversalCount = m_TraversalCount - 1;
            if (m_IsDirty && m_TraversalCount == 0)
            {
                m_IsDirty = false;
                foreach (KeyValuePair<Tkey, TVal> kv in m_AddCache)
                {
                    if (!m_Datas.ContainsKey(kv.Key))
                    {
                        m_Datas.Add(kv.Key, m_AddCache[kv.Key]);
                    }
                }
                m_AddCache.Clear();

                foreach (Tkey key in m_RemoveCache.Keys)
                {
                    m_Datas.Remove(key);
                }
                m_RemoveCache.Clear();
            }
        }
        public void OnDestroy()
        {
			if(m_TraversalCount ==0)
			{	
				Clear();
				m_IsClearData = false;
			}
			else
			{
				m_IsClearData = true;
			}	          
        }

        public void Clear()
        {
			if(m_TraversalCount ==0)
			{
				m_Datas.Clear();
				m_AddCache.Clear();
				m_RemoveCache.Clear();
				m_TraversalCount = 0;
				m_IsDirty = false;
				m_IsClearData = false;
			}
			else
			{
				m_IsClearData = true;
			}
        }
        public int GetCount()
        {
            return m_Datas.Count;
        }

    }
}
