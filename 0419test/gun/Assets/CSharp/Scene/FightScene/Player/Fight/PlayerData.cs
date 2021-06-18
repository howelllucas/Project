
using EZ.Data;
using EZ.DataMgr;
using Game;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class PlayerData
    {
        private float m_Hp;
        private float m_MaxHp;
        private float m_Lv;
        private Player m_Player;
        private BuffMgr m_BuffMgr;
        private float m_DamageTime = -11000;
        private float m_ProtectTime = 3;
        private double m_GainGold;
        private double m_GoldRate;
        private int m_ReviveTimes = int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.MAX_REVIVE_TIMES).content);
        private int m_TimerId = -1001;
        private Transform m_HeroNode;
        private bool m_DirtyBroadGold = false;
        private FightScene m_FightScene;
        Dictionary<string, int> m_DropRes = new Dictionary<string, int>();
        public PlayerData(Player player,BuffMgr buffMgr)
        {
            m_HeroNode = player.transform.Find("ModelNode/hero");
            m_FightScene = Global.gApp.CurScene as FightScene;
            m_GainGold = 0;
            m_Player = player;
            m_BuffMgr = buffMgr;
            //int skillLevel = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(GameConstVal.SExGold);
            //Skill_dataItem skillLevelData = Global.gApp.gGameData.SkillDataConfig.Get(skillLevel);
            //float skillParam = (skillLevelData == null) ? 1f : skillLevelData.skill_exgold[0];
            //float skillParam = 1.0f + m_Player.GetWeaponMgr().GetCombatAttrValue(Game.CombatAttr.CombatAttrType.Gold_Add);
            //PassItem passData = Global.gApp.CurScene.GetPassData();
            //if ((PassType)passData.passType == PassType.MainPass)
            //{
            //    m_GoldRate = passData.goldParam * skillParam;

            //    Debug.Log("m_GoldRate " + m_GoldRate);
            //}
            //else
            //{
            //    int roleLv = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
            //    PassSpecial_dataItem passSpecial = Global.gApp.gGameData.PassSpecialCfg.Get(roleLv);
            //    m_GoldRate = passSpecial.coinparam * skillParam;
            //}
        }
        public void OnHit(float damage)
        {
            if (m_Hp <= 0)
            {
                return;
            }
            float currentTime = m_FightScene.GetCurTime();
            if (CanBeHitted() && m_Hp > 0)
            {
                ResetProtectTime();
                AddHp(-damage);
                AddDamageUiEffect();
				Shining.VibrationSystem.Vibrations.instance.Vibrate30ms();
            }
        }
        
        public bool CanBeHitted()
        {
            float currentTime = m_FightScene.GetCurTime();
            if (currentTime >= m_DamageTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void AddDamageUiEffect()
        {
            GameObject damageUiEffect = Global.gApp.gResMgr.InstantiateObj("Prefabs/UI/BeHitted");
            damageUiEffect.transform.SetParent(Global.gApp.gUiMgr.GetUiCanvasTsf(), false);
            damageUiEffect.transform.SetAsFirstSibling();
        }
        public void PausePrtect()
        {
            float currentTime = m_FightScene.GetCurTime();
            if(m_DamageTime < currentTime)
            {
                m_DamageTime = currentTime + 0.1f;
            }
        }
        public void ResetProtectTime(float offset = 0)
        {
            float currentTime = m_FightScene.GetCurTime();
            m_DamageTime = currentTime + m_ProtectTime + Mathf.Abs(offset);
            Global.gApp.CurScene.GetTimerMgr().RemoveTimer(m_TimerId);
            if(offset == 0)
            {
                AddBinkEffectImp(20);
                m_TimerId = Global.gApp.CurScene.GetTimerMgr().AddTimer(2.8f, 1, AddBlinkEffect);
            }
            else if(offset > 0)
            {
                AddBinkEffectImp(0);
                GameObject effect = Global.gApp.gResMgr.InstantiateObj(EffectConfig.InvincibleEffect);
                effect.transform.SetParent(m_Player.transform, false);
            }
            else
            {
                AddBinkEffectImp(0);
            }
        }
        public void SetProtectTime(float protectTime)
        {
            float currentTime = m_FightScene.GetCurTime();
            m_DamageTime = currentTime + protectTime;
        }
        private void AddBlinkEffect(float dt,bool end)
        {
            if (end)
            {
                AddBinkEffectImp(0);
            }
        }
        private void AddBinkEffectImp(float speed)
        {

            if (m_HeroNode != null)
            {
                Renderer[] allRenders = m_Player.GetComponentsInChildren<Renderer>();
                foreach (Renderer mesh in allRenders)
                {
                    mesh.material.SetFloat("_Speed", speed);
                }
            }
        }
        public bool CheckCanAddHp()
        {
            return m_Hp < m_MaxHp  && m_Hp > 0; 
        }
        public void AddHp(float hp)
        {
            float curHp = m_Hp + hp;
            curHp = Math.Max(curHp, 0);
            curHp = Math.Min(curHp, m_MaxHp);
            SetHp(curHp);
        }
        public void SetHp(float hp)
        {
            m_Hp = hp;
            Global.gApp.gMsgDispatcher.Broadcast<float,float>(MsgIds.MainRoleHpChange, hp,m_MaxHp);
            if(m_Hp <= 0)
            {
                if (Global.gApp.CurScene.IsNormalPass())
                {
                    ShowNormalModelAdvertising();
                }
                else
                {
                    ShowSpecialModelAdvertising();
                }

            }
        }
        private void ShowSpecialModelAdvertising()
        {
            if (m_ReviveTimes > 0)
            {
                ResetProtectTime(-2);
                Global.gApp.CurScene.Pause();
                m_Player.SetColliderState(false);
                FightResultManager.instance.ShowReivePopup((bool tmpR) =>
                {
                    Global.gApp.CurScene.Resume();
                    ResetProtectTime(1f);
                    if (tmpR)
                    {
                        m_ReviveTimes--;
                        m_Hp = m_MaxHp;
                        Global.gApp.gMsgDispatcher.Broadcast<float, float>(MsgIds.MainRoleHpChange, m_Hp, m_MaxHp);
                        m_Player.SetColliderState(true);
                    }
                    else
                    {
                        FightResultManager.instance.SetRetryType(FightResultManager.RetryType.NONE);
                        Global.gApp.CurScene.GameLose();
                    }
                });
            }
            else
            {
                Global.gApp.CurScene.GameLose();
            }
        }
        private void ShowNormalModelAdvertising()
        {
            if (m_ReviveTimes > 0)
            {
                ResetProtectTime(-2);
                Global.gApp.gGameCtrl.AddGlobalTouchMask();
                Global.gApp.CurScene.Pause();
                m_Player.SetColliderState(false);
                m_Player.gameObject.AddComponent<DelayCallBack>().SetAction(ShowNormalModelAdvertisingImp, 3.2f, true);
                //m_Player.gameObject.AddComponent<DelayCallBack>().SetAction(() => { Global.gApp.CurScene.GetPropMgr().AddProp(m_Player.transform.position + new Vector3(0.5f, 0.5f, 0), 100004); }, 2, true);
                m_Player.gameObject.AddComponent<DelayCallBack>().SetAction(() => {
                    m_Player.GetFight().SetAnimSpeed(1);
                    m_Player.GetFight().PlayAnim(GameConstVal.Death, GameConstVal.Death);
                    m_Player.GetFight().SetAnimSpeed(1);
                }, 0.1f, true);
                Global.gApp.gCamCompt.GetComponentInChildren<MoveToBoss>().StartRoleDead();
            }
            else
            {
                Global.gApp.CurScene.GameLose();
            }
        }
        private void ShowNormalModelAdvertisingImp()
        {
            Global.gApp.gGameCtrl.RemoveGlobalTouchMask();
            FightUI fightUI = Global.gApp.gUiMgr.GetPanelCompent<FightUI>(Wndid.FightPanel);
            if(fightUI != null)
            {
                fightUI.PauseFromAd();
            }
            FightResultManager.instance.ShowReivePopup((bool tmpR) =>
            {
                Global.gApp.CurScene.Resume();
                ResetProtectTime(1f);
                if (tmpR)
                {
                    m_ReviveTimes--;
                    m_Hp = m_MaxHp;
                    Global.gApp.gMsgDispatcher.Broadcast<float, float>(MsgIds.MainRoleHpChange, m_Hp, m_MaxHp);
                    Global.gApp.gCamCompt.GetComponentInChildren<MoveToBoss>().StartReborn();
                    m_Player.GetFight().PlayAnim(GameConstVal.Idle, GameConstVal.Idle);
                    m_Player.LockMove(0.7f);
                    m_Player.gameObject.AddComponent<DelayCallBack>().SetAction(()=>{ m_Player.SetColliderState(true);}, 0.7f, true);
                }
                else
                {
                    FightResultManager.instance.SetRetryType(FightResultManager.RetryType.NONE);
                    Global.gApp.CurScene.GameLose();
                }
            });
        }
        public float GetHp()
        {
            return m_Hp;
        }

        public float GetMaxHp()
        {
            return m_MaxHp;
        }

        public int GetHitHp()
        {
            return (int)(m_MaxHp - m_Hp);
        }
        public float GetLv()
        {
            return m_Lv;
        }
        public double GetGold() { return m_GainGold; }
        public void AddGold(double gold)
        {
            gold = m_GoldRate * gold * UnityEngine.Random.Range(0.9f, 1.1f);
            m_GainGold = m_GainGold + gold;
            m_DirtyBroadGold = true;
        }
        public void AddDropResIfNotExit(string name,int count)
        {
            int curCount = 0;
            m_DropRes.TryGetValue(name, out curCount);
            if(curCount == 0)
            {
                AddDropRes(name, count);
            }
        }
        public void AddDropRes(string name,int count)
        {
            int curCount;
            if(m_DropRes.TryGetValue(name,out curCount))
            {
                curCount += count;
                m_DropRes[name] = curCount;
            }
            else
            {
                m_DropRes[name] = count;
            }
        }
        public int GetDropResCount(string name)
        {
            int curCount = 0;
            m_DropRes.TryGetValue(name, out curCount);
            return curCount;
        }
        public void Update()
        {
            if(m_DirtyBroadGold)
            {
                m_DirtyBroadGold = false;
                Global.gApp.gMsgDispatcher.Broadcast<double>(MsgIds.FightGainGold, m_GainGold);
            }
        }
        public int GetReviveTimes()
        {
            return int.Parse(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.MAX_REVIVE_TIMES).content) - m_ReviveTimes;
        }

        public void Init(Dictionary<string,float> attr)
        {

        }
        public void Init()
        {
            //SkillItem skillItem = Global.gApp.gGameData.SkillData.Get(GameConstVal.SExHp);
            //m_MaxHp = 2;//skillItem.init_param[0];

            //int skillLevel = Global.gApp.gSystemMgr.GetSkillMgr().GetSkillLevel(GameConstVal.SExHp);
            //Skill_dataItem skillLevelData = Global.gApp.gGameData.SkillDataConfig.Get(skillLevel);
            //float addParam = (skillLevelData == null) ? m_MaxHp : skillLevelData.skill_exhp[0];
            
            var life = Mathf.Ceil( TableMgr.singleton.ValueTable.player_levelup_hp_k1 * 
                                    Mathf.Pow((1 + TableMgr.singleton.ValueTable.player_levelup_hp_k2), 
                                    (PlayerDataMgr.singleton.GetPlayerLevel() - 1)) );
            life *= 1.0f + m_Player.GetWeaponMgr().GetCombatAttrValue(CombatAttr.CombatAttrType.Life_Add);
            m_MaxHp = life;
            SetHp(m_MaxHp);

            m_Lv = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();

            Debug.Log("m_MaxHp " + m_MaxHp);
        }
        public void Destroy()
        {
           Global.gApp.CurScene.GetTimerMgr().RemoveTimer(m_TimerId);
            //ItemDTO itemDTO = new ItemDTO(SpecialItemIdConstVal.GOLD, m_GainGold, BehaviorTypeConstVal.BALANCE);
            //GameItemFactory.GetInstance().AddItem(itemDTO);
        }
    }
}
