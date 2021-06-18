using UnityEngine;

namespace EZ
{
    public class ResEditorMgr:ResMgr
    {
        public ResEditorMgr()
        {

        }
        public override GameObject LoadPrefabImp(string path)
        {
            return Resources.Load(path) as GameObject;
        }
    }
}
