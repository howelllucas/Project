using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public class BulletCacheMgr
    {
        private Dictionary<string, List<BaseBullet>> m_BulletNodeCache = new Dictionary<string, List<BaseBullet>>();
        private ResMgr m_ResMgr;
        public BulletCacheMgr()
        {
            m_ResMgr = Global.gApp.gResMgr;
        }

        public void CacheBullet(string name, int count, int maxCount = -1)
        {
            if (maxCount > 0)
            {
                List<BaseBullet> bulletNodes;
                if (m_BulletNodeCache.TryGetValue(name, out bulletNodes) && bulletNodes.Count > maxCount)
                {
                    return;
                }
            }
            Transform bulletTsf = Global.gApp.gBulletNode.transform;
            for (int i = 0; i < count; i++)
            {
                BaseBullet bulletNode = CreateBullet(name);
                bulletNode.InitForCache();
                Recycle(name, bulletNode);
                bulletNode.transform.SetParent(bulletTsf);
            }
        }

        public BaseBullet GetBullet(string name)
        {
            List<BaseBullet> bulletNodes;
            BaseBullet bulletNode;
            if (m_BulletNodeCache.TryGetValue(name, out bulletNodes) && bulletNodes.Count > 0)
            {
                bulletNode = bulletNodes[0];
                bulletNodes.RemoveAt(0);
                return bulletNode;
            }
            else
            {
                bulletNode = CreateBullet(name);
                return bulletNode;
            }
        }

        private BaseBullet CreateBullet(string name)
        {
            GameObject bulletNode = Global.gApp.gResMgr.InstantiateObj(BulletConfig.BulletPath[name]);
            BaseBullet bulletNodeCmp = bulletNode.GetComponent<BaseBullet>();
            if(bulletNodeCmp == null)
            {
                bulletNodeCmp = bulletNode.GetComponentInChildren<BaseBullet>();
            }
            bulletNodeCmp.SetName(name);
            return bulletNodeCmp;
        }
        public void Recycle(string name, BaseBullet bulletNode)
        {
            List<BaseBullet> bulletNodes;
            if (!m_BulletNodeCache.TryGetValue(name, out bulletNodes))
            {
                bulletNodes = new List<BaseBullet>();
                m_BulletNodeCache[name] = bulletNodes;
            }
            bulletNode.transform.SetParent(Global.gApp.gBulletNode.transform, true);
            bulletNode.transform.position = new Vector3(1000, 0, 0);
            bulletNodes.Add(bulletNode);
        }
        public void Clear()
        {
            m_BulletNodeCache.Clear();
        }
    }
}
