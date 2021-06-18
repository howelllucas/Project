using EZ.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class CtrlEleSawPitfallBullet : BaseBullet,ICtrlElementNode
    {

        [SerializeField] private Animator m_Animator;
        private Collider2D m_Collider2d;
        CElement.EleState m_CurEleState = CElement.EleState.None;
        private void Awake()
        {
            SetMeshRenderEnable(false);
            gameObject.AddComponent<DelayCallBack>().SetAction(() => { SetMeshRenderEnable(true); }, 0.1f);
        }
        private void SetMeshRenderEnable(bool isEnable)
        {
            MeshRenderer[] meshRenderers = transform.parent.parent.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = isEnable;
            }
        }
        private void Start()
        {
            GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
            double atk = weaponLevelData.Office_saw[(int)MWeapon.Atk];
            m_Damage = atk * DamageCoefficient;
        }
        private Collider2D GetCollider2D()
        {
            if (m_Collider2d == null)
            {
                m_Collider2d = GetComponent<Collider2D>();
            }
            return m_Collider2d;
        }
        void Update()
        {
            if(m_CurEleState == CElement.EleState.Close)
            {
                AnimatorStateInfo info = m_Animator.GetCurrentAnimatorStateInfo(0);
                float normalizedTime = info.normalizedTime - (int)info.normalizedTime;
                if (normalizedTime >= 0.98f || normalizedTime <= 0.05f)
                {
                    m_Animator.enabled = false;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag) && collision.gameObject.layer != GameConstVal.FlyMonsterLayer)
            {
                collision.gameObject.GetComponent<Monster>().OnHit_Pos(m_Damage, transform);
            }
        }

        void ICtrlElementNode.SetEleOpen()
        {
            m_Animator.enabled = true;
            m_CurEleState = CElement.EleState.Open;
        }

        void ICtrlElementNode.SetEleClose()
        {
            m_CurEleState = CElement.EleState.Close;
        }
        void ICtrlElementNode.SyncStartEleState(CElement.EleState eleState)
        {
            if (eleState == CElement.EleState.Open)
            {
                ((ICtrlElementNode)this).SetEleOpen();
            }
            else
            {
                ((ICtrlElementNode)this).SetEleClose();
            }
        }
    }
}
