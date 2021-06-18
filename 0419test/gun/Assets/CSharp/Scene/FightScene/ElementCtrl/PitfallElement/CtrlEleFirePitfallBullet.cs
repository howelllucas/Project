using EZ.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class CtrlEleFirePitfallBullet : BaseBullet, ICtrlElementNode
    {
        private float m_DtTimeMin;
        private float m_DtTimeMax;
        private float m_NewCurTime;
        private Collider2D m_Collider2d;
        ParticleSystem m_FireEffect;
        CElement.EleState m_CurEleState = CElement.EleState.None;
        private void Awake()
        {
            SetMeshRenderEnable(false);
            gameObject.AddComponent<DelayCallBack>().SetAction(() => { SetMeshRenderEnable(true); }, 0.1f);
        }
        private void SetMeshRenderEnable(bool isEnable)
        {
            MeshRenderer[] meshRenderers = transform.parent.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = isEnable;
            }

            if (isEnable)
            {
                GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.JiGuan_huoyan_1);
                effect.transform.SetParent(transform.parent, false);
                m_FireEffect = effect.transform.Find(GameConstVal.ParticleName).GetComponent<ParticleSystem>();
                if(m_CurEleState == CElement.EleState.Open)
                {
                    m_FireEffect.Play();
                }
                else
                {
                    m_FireEffect.Stop();
                }
            }
        }
        private void Start()
        {
     
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WOfficeBonfire);
            m_DtTimeMin = weaponItem.dtime;
            m_DtTimeMax = weaponItem.dtime + 0.1f;
            GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
            double atk = weaponLevelData.Office_bonfire[(int)MWeapon.Atk];
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
            if (m_CurEleState == CElement.EleState.Open)
            {
                m_CurTime = m_CurTime + BaseScene.GetDtTime();
                m_NewCurTime += BaseScene.GetDtTime();
                if (m_NewCurTime > m_DtTimeMax)
                {
                    transform.localPosition = Vector3.zero;
                    m_NewCurTime = 0;
                }
                else if (m_NewCurTime > m_DtTimeMin)
                {
                    transform.localPosition = new Vector3(1000, 1000, 1000);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_CurEleState == CElement.EleState.Open)
            {
                if (collision.gameObject.CompareTag(GameConstVal.MonsterTag) && collision.gameObject.layer != GameConstVal.FlyMonsterLayer)
                {
                    Monster monster = collision.gameObject.GetComponent<Monster>();
                    monster.OnHit_Pos(m_Damage, transform);
                    if (monster.CheckCanAddHittedEffect())
                    {
                        GameObject effect = GetHittedEnemyEffect();
                        effect.transform.SetParent(monster.transform, false);
                        effect.transform.position = monster.transform.position;
                    }
                }
            }
        }

        void ICtrlElementNode.SetEleOpen()
        {
            m_CurEleState = CElement.EleState.Open;
            GetCollider2D().enabled = true;
            if(m_FireEffect != null)
            {
                m_FireEffect.Play();
            }
        }

        void ICtrlElementNode.SetEleClose()
        {
            m_CurEleState = CElement.EleState.Close;
            GetCollider2D().enabled = false;
            if (m_FireEffect != null)
            {
                m_FireEffect.Stop();
            }
        }
        void ICtrlElementNode.SyncStartEleState(CElement.EleState eleState)
        {
            if(eleState == CElement.EleState.Open)
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

