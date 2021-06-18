using UnityEngine;

namespace EZ
{

    public abstract class BaseTurret : MonoBehaviour
    {
        protected static int m_Ref = 1;

        private bool m_InCameraView = false;

        public bool InCameraView
        {
            get
            {
                return m_InCameraView;
            }

            set
            {
                m_InCameraView = value;
            }
        }

        private void OnBecameInvisible()
        {
            InCameraView = false;
        }
        private void OnBecameVisible()
        {
            InCameraView = true;
        }
        public abstract void SetAtkRange(float atkRange);
        public abstract void PlayTurretAnim(string anim, bool forcePlay = false);
    }
}
