using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class SingletonMgr : Singleton<SingletonMgr>
    {
        public Stack<ISingleton> singletons = new Stack<ISingleton>();

        public void AddSingleton(ISingleton singleton)
        {
            singletons.Push(singleton);
        }

        public void ClearAddReleases()
        {
            while( singletons.Count > 0 )
            {
                singletons.Pop().ClearSingleton();
            }
        }
    }
}
