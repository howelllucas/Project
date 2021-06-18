using EZ.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class AiElePitBullet : BaseBullet
    {
        private float m_DtTimeMin;
        private float m_DtTimeMax;
        private float m_NewCurTime;
        private Collider2D m_Collider2d;

        public float BuffTime = 1;
        public float BuffVal = -0.5f;
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

            if(isEnable)
            {
                GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.JiGuan_Electromagnetic);
                effect.transform.SetParent(transform.parent, false);
            }
        }
        private void Start()
        {
            m_Collider2d = GetComponent<Collider2D>();
            ItemItem weaponItem = Global.gApp.gGameData.GetItemDataByName(GameConstVal.WOfficeEleTrom);
            m_DtTimeMin = weaponItem.dtime;
            m_DtTimeMax = weaponItem.dtime + 0.1f;
            GunsPass_dataItem weaponLevelData = Global.gApp.CurScene.GetGunPassDataItem();
            double atk = weaponLevelData.Office__electrom[(int)MWeapon.Atk];
            m_Damage = atk * DamageCoefficient;
        }
        void Update()
        {
            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            m_NewCurTime += BaseScene.GetDtTime();
            if(m_NewCurTime > m_DtTimeMax)
            {
                transform.localPosition = Vector3.zero;
                m_NewCurTime = 0;
            }
            else if(m_NewCurTime > m_DtTimeMin)
            {
                transform.localPosition = new Vector3(1000,1000,1000);
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
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Pos(m_Damage, transform);
                if (monster.CheckCanAddHittedEffect())
                {
                    GameObject effect = GetHittedEnemyEffect();
                    effect.transform.SetParent(monster.transform, false);
                    effect.transform.position = monster.transform.position;
                }
                if(BuffTime > 0)
                {
                    monster.AddBuff(AiBuffType.MoveSpeed, BuffTime, BuffVal);
                }
            }
            else if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
            {
                if(BuffTime > 0)
                {
                    Player player = collision.gameObject.GetComponentInParent<Player>();
                    if (player == null)
                    {
                        player = collision.gameObject.GetComponentInChildren<Player>();
                    }
                    if (player != null)
                    {
                        player.GetBuffMgr().AddBuff(BuffType.MoveSpeed, BuffTime, BuffVal);
                    }
                }
            }
        }
    }
}
