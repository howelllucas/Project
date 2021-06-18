using UnityEngine;
using System.Collections;

namespace EZ.DataMgr
{
    public class DebugMgr
    {
        private static DebugMgr m_Instance = new DebugMgr();
        public int MonsterId = 0;

        public static DebugMgr GetInstance()
        {
            return m_Instance;
        }
    }
}