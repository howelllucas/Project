using UnityEngine;
namespace EZ
{

    public class BaseAttrUi : MonoBehaviour
    {

        public virtual void Destroy()
        {
            Object.Destroy(gameObject);
        }
        public virtual void SetFloowNode(Transform transform)
        {

        }
        public virtual void SetHpPercent(float percent)
        {
        }
    }
}
