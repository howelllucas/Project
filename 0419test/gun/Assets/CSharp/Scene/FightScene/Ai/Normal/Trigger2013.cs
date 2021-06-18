using UnityEngine;

namespace EZ
{
    public class Trigger2013 : MonoBehaviour
    {

        AICrazyPursueXAct m_PursueXAct;
        private void Start()
        {
            m_PursueXAct = GetComponentInParent<AICrazyPursueXAct>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.CarrierTag) || collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
            {
                m_PursueXAct.TriggerPlayer();
            }
        }
    }
}
