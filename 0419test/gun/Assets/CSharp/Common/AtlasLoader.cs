using UnityEngine;
using UnityEngine.U2D;
namespace EZ
{
    public class AtlasLoader : MonoBehaviour
    {
        void OnEnable()
        {
            SpriteAtlasManager.atlasRequested += RequestAtlas;
        }

        void OnDisable()
        {
            SpriteAtlasManager.atlasRequested -= RequestAtlas;
        }

        void RequestAtlas(string tag, System.Action<SpriteAtlas> callback)
        {
            SpriteAtlas atlas = Global.gApp.gResMgr.LoadSpriteAtlas(ref tag);
            if (atlas != null)
            {
                callback(atlas);
            }
        }
    }
}