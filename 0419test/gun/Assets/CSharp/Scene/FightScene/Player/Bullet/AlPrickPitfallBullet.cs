using EZ.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class AlPrickPitfallBullet : BaseBullet
    {
        private float m_NewCurTime;
        private float m_StartPosY;
        private void Awake()
        {
            SetMeshRenderEnable(false);
            gameObject.AddComponent<DelayCallBack>().SetAction(()=>{ SetMeshRenderEnable(true); },0.1f);
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
            m_StartPosY = -0.25f;
            GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
            double atk = weaponLevelData.Office_prick[(int)MWeapon.Atk];
            m_Damage = atk * DamageCoefficient;
        }
        void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            Vector3 localPos = transform.parent.localPosition;
            if (localPos.y > m_StartPosY)
            {
                transform.localPosition = new Vector3(0, 0,0);
            }
            else
            {
                transform.localPosition = new Vector3(1000, 1000, 1000);
            }
            if (m_CurTime >= m_LiveTime)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag) && collision.gameObject.layer != GameConstVal.FlyMonsterLayer)
            {
                collision.gameObject.GetComponent<Monster>().OnHit_Pos(m_Damage, transform);
            }
        }
    }
}
