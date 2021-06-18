using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public enum PylonState
    {
        None,
        Active,
        CountDown,
        Frozen,
    }
    public class PylonEleCtrol : MonoBehaviour
    {
        PylonEle[] m_PylonEles;
        int m_Symbol = 1;
        Transform m_CurWall;
        Transform m_ChargingEffect;
        Transform m_ChargFullEffect;
        RepreatEffect m_ArrowEffect;
        private float m_StartPos = -0.54f;
        private float m_StepDis = -0.16f;
        List<GameObject> m_StepEffect = new List<GameObject>();
        private void Awake()
        {
            m_PylonEles = GetComponentsInChildren<PylonEle>();
            int index = 0;
            foreach(PylonEle pylonEle in m_PylonEles)
            {
                pylonEle.Init(this, index);
                index++;
            }
            gameObject.AddComponent<DelayCallBack>().SetAction(() => { SetPylonEleState(); }, 0.1f);
        }
        private void SetPylonEleState()
        {
            foreach (PylonEle pylonEle in m_PylonEles)
            {
                pylonEle.DelayInit();
            }
            m_PylonEles[0].SetPylonState(PylonState.CountDown);
        }
        public void ActiveWall(PylonEle pylonEle)
        {
            pylonEle.SetPylonState(PylonState.Active);
            int index = pylonEle.GetIndex();
            if (index == m_PylonEles.Length - 1)
            {
                m_Symbol = - 1;
            }
            else if(index == 0)
            {
                m_Symbol = 1;
            }
            int nextIndex = index + m_Symbol;
            PylonEle toPylonEle = m_PylonEles[nextIndex];
            AddWallImp(pylonEle, toPylonEle);
        }
        public void DestroyWall(PylonEle pylonEle)
        {
            pylonEle.SetPylonState(PylonState.Frozen);
            int index = pylonEle.GetIndex();
            if (index == m_PylonEles.Length - 1)
            {
                m_Symbol = -1;
            }
            else if (index == 0)
            {
                m_Symbol = 1;
            }
            int nextIndex = index + m_Symbol;
            PylonEle toPylonEle = m_PylonEles[nextIndex];
            toPylonEle.SetPylonState(PylonState.CountDown);
            if (m_CurWall != null)
            {
                m_CurWall.gameObject.SetActive(false);
            }
        }
        private void AddWallImp(PylonEle fromPylonEle,PylonEle toPylonEle)
        {
            //m_CurWall
            if(m_CurWall == null)
            {
                m_CurWall = Global.gApp.gResMgr.InstantiateObj(EffectConfig.PylonWallEffect).transform;
            }
            Vector3 dtVec3 = toPylonEle.transform.position - fromPylonEle.transform.position;
            float angleZ = EZMath.SignedAngleBetween(dtVec3, Vector3.up);
            m_CurWall.localEulerAngles = new Vector3(0, 0, angleZ + 90);
            m_CurWall.position = fromPylonEle.transform.position + dtVec3 / 2;
            m_CurWall.localScale = new Vector3(dtVec3.magnitude / 6.45f, 1, 1);
            m_CurWall.gameObject.SetActive(true);
        }

        public void SetChargingVisible(bool visible,Transform parent)
        {
            if(m_ChargingEffect == null)
            {
                m_ChargingEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.PylonChargingEffect).transform;
            }
            m_ChargingEffect.transform.position = parent.transform.position;
            m_ChargingEffect.gameObject.SetActive(visible);
        }
        public void RemoveChargingStepEffect(int step)
        {
            m_StepEffect[step - 1].gameObject.SetActive(false);
        }
        public void AddChargingStepEffect(PylonEle pylonEle,int step)
        {
            int createCount = step - m_StepEffect.Count;
            if (createCount > 0)
            {
                for(int i = 0; i< createCount; i++)
                {
                    m_StepEffect.Add(Global.gApp.gResMgr.InstantiateObj(EffectConfig.Eletower_02_quan));
                }
            }
            m_StepEffect[step - 1].gameObject.SetActive(true);
            Vector3 pylonElePos = pylonEle.transform.position;
            pylonElePos.z = m_StartPos + m_StepDis * (step - 1);
            m_StepEffect[step - 1].transform.position = pylonElePos;
        }
        public void DisableStepEffect()
        {
            foreach (GameObject go in m_StepEffect)
            {
                go.SetActive(false);
            }
        }
        public void AddArrowEffect(bool visible,PylonEle fromPylonEle)
        {
            if (m_ArrowEffect == null)
            {
                m_ArrowEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Eletower_02_zhiyin).GetComponent<RepreatEffect>();
            }
            if (visible)
            {
                int index = fromPylonEle.GetIndex();
                if (index == m_PylonEles.Length - 1)
                {
                    m_Symbol = -1;
                }
                else if (index == 0)
                {
                    m_Symbol = 1;
                }
                int nextIndex = index + m_Symbol;
                PylonEle toPylonEle = m_PylonEles[nextIndex];

                Vector3 dtVec3 = toPylonEle.transform.position - fromPylonEle.transform.position;
                float angleZ = EZMath.SignedAngleBetween(dtVec3, Vector3.up);
                m_ArrowEffect.transform.localEulerAngles = new Vector3(0, 0, angleZ + 90);
                m_ArrowEffect.transform.position = fromPylonEle.transform.position + dtVec3 / 2;
                float length = dtVec3.magnitude;
                m_ArrowEffect.transform.localScale = new Vector3(length, 1, 1);
                m_ArrowEffect.SetLength(length);
            }
            m_ArrowEffect.gameObject.SetActive(visible);
        }
        public void SetChargFullVisible(bool visible, Transform parent)
        {
            if (m_ChargFullEffect == null)
            {
                m_ChargFullEffect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.PylonChargFullEffect).transform;
            }
            m_ChargFullEffect.transform.position = parent.transform.position;
            m_ChargFullEffect.gameObject.SetActive(visible);
        }
    }
}
