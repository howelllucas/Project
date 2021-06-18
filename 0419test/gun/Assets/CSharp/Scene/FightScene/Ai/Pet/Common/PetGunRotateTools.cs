using UnityEngine;

namespace EZ
{

    public class PetGunRotateTools : MonoBehaviour
    {

        public enum FaceMode 
        {
            None = 0,
            FaceToMonster = 1,
            FaceToOri = 2,
        }
        private FaceMode m_RelativeSpace = FaceMode.None;
        private Vector3 m_DstUpVec = Vector3.zero;
        void Update()
        {
            FaceToEnemyImp();
        }
        public void SetFaceToOri()
        {
            m_RelativeSpace = FaceMode.FaceToOri;
        }
        public void SetFaceToMonster(Vector3 destVec)
        {
            m_RelativeSpace = FaceMode.FaceToMonster;
            destVec.z = 0;
            m_DstUpVec = destVec;
        }


        private void FaceToEnemyImp()
        {
            if (m_RelativeSpace == FaceMode.FaceToMonster)
            {
                float angleZ = EZMath.SignedAngleBetween(m_DstUpVec, Vector3.up);
                CalcAnglesWorld(angleZ);
            }
            else
            {
                CalcAnglesLocal(0);
            }
        }
        private void CalcAnglesLocal(float angleZ)
        {
            Vector3 eulerAngle = transform.localEulerAngles;
            float dtAngle = angleZ - eulerAngle.z;
            if (dtAngle > 180)
            {
                dtAngle = (dtAngle) - 360;
            }
            else if (dtAngle < -180)
            {
                dtAngle = (dtAngle) + 360;
            }
            dtAngle = eulerAngle.z + dtAngle * BaseScene.GetDtTime() * 5;
            transform.localEulerAngles = new Vector3(0, 0, dtAngle);
        }
        private void CalcAnglesWorld(float angleZ)
        {
            Vector3 eulerAngle = transform.eulerAngles;
            float dtAngle = angleZ - eulerAngle.z;
            if (dtAngle > 180)
            {
                dtAngle = (dtAngle) - 360;
            }
            else if (dtAngle < -180)
            {
                dtAngle = (dtAngle) + 360;
            }
            dtAngle = eulerAngle.z + dtAngle * BaseScene.GetDtTime() * 8;
            transform.eulerAngles = new Vector3(0, 0, dtAngle);
        }
    }
}
