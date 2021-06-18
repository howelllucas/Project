using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class FightTrappedBehavior : FightNpcBaseBehavior
    {
        public override void EndBehavior()
        {
            base.EndBehavior();
            m_FightNpcPlayer.SetCollisionType(RigidbodyType2D.Dynamic);
        }

        private void Update()
        {
            Vector3 faceVec = m_PlayerGo.transform.position - transform.position;
            transform.localEulerAngles = new Vector3(0, 0, EZMath.SignedAngleBetween(faceVec, Vector3.up));
        }
        public override void StartBehavior()
        {
            base.StartBehavior();
            m_FightNpcPlayer.PlayAnim(GameConstVal.Greet);
            m_FightNpcPlayer.SetCollisionType(RigidbodyType2D.Static);
        }
    }
}
