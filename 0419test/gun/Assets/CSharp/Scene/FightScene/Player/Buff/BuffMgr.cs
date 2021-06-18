

using EZ.Data;
using EZ.DataMgr;
using System;
using UnityEngine;

namespace EZ
{
    public enum BuffType
    {
        ShieldBuff = 0,
        FireSpeed = 1,
        FireSpeedX = 2,
        FireCountTimes = 3,
        FireCountTimesX = 4,
        FireCurveInc = 5,
        FireCurveIncX = 6,
        Atk = 7,
        AtkX = 8,
        MoveSpeed = 9,
        MoveSpeedX = 10,
        PlayerEnergy = 11,
        NaNoboostBuff = 12,
        CritBuff = 13,
        CritBuffX = 14,
        DodgeBuff = 15,
        DodgeBuffX = 16,
        OneKillBuff = 17,
        MaxHpBuff = 18,

        buff_atkNpc = 1001,//攻击
        buff_atk = 1002,//攻击
        buff_fireSpeed = 1003,//攻速
        buff_Beheading = 1004,//斩杀
        buff_second = 1005,//增加副武器攻击
        buff_robot = 1006,//增加机器人攻击
        buff_seckill = 1007,//秒杀
        buff_critBoom = 1008,//局内触发暴击时，同时触发爆炸
        buff_critIgnition = 1009,//局内触发暴击时，同时触发引燃
    }

    public class BuffMgr
    {
        private Player m_Player;
        private SafeMap<int, BaseBuff> m_BuffMap;
        private Action<BaseBuff, float> m_Action;
        #region CampBuff
        public float CampIncAtkA {get; private set; }//攻击 加成 大幅
        public float CampIncAtkB { get; private set; }//攻击加成小幅
        public float CampIncAtkSpeed { get; private set; }//攻击速度加成 
        public float CampSecondWpnIncAtk { get; private set; }//副武器攻击加成 
        public float CampPetIncAtk { get; private set; }//宠物攻击加成
        public float CampCritExplodDamageParam { get; private set; }//暴击爆炸 概率
        public float CampCritFiredBuffDamageParam { get; private set; }//暴击灼烧 概率
        public float CampKillARate { get; private set; }//斩杀概率
        public float CampKillBRate { get; private set; }//秒杀小怪概率
        #endregion
        public BuffMgr(Player player)
        {
            m_Player = player;
            m_BuffMap = new SafeMap<int, BaseBuff>();
            m_Action = UpdateImp;
            //InitCampBuff();
        }

        private void InitCampBuff()
        {
            NpcMgr npcMgr = Global.gApp.gSystemMgr.GetNpcMgr();
            CampIncAtkA = npcMgr.GetBuffParam(BuffType.buff_atkNpc.ToString())[0] - 1;
            CampIncAtkB = npcMgr.GetBuffParam(BuffType.buff_atk.ToString())[0] - 1; ;
            CampIncAtkSpeed = npcMgr.GetBuffParam(BuffType.buff_fireSpeed.ToString())[0] - 1; ;
            CampSecondWpnIncAtk = npcMgr.GetBuffParam(BuffType.buff_second.ToString())[0] - 1; ;
            CampPetIncAtk = npcMgr.GetBuffParam(BuffType.buff_robot.ToString())[0] -1 ;
            CampCritExplodDamageParam = npcMgr.GetBuffParam(BuffType.buff_critBoom.ToString())[0] / 100.0f;
            CampCritFiredBuffDamageParam = npcMgr.GetBuffParam(BuffType.buff_critIgnition.ToString())[0] / 100.0f ;
            CampKillARate = npcMgr.GetBuffParam(BuffType.buff_Beheading.ToString())[0]; ;
            CampKillBRate = npcMgr.GetBuffParam(BuffType.buff_seckill.ToString())[0]; ;
        }
        private void UpdateImp(BaseBuff buff,float dt)
        {
            buff.Update(dt);
        }

        public void Update(float dt)
        {
            m_BuffMap.Update(dt, m_Action);
        }
        public void AddBuff(BuffType buffType,float effectTime,float val,float val2 = 0,float val3 = 0,GameObject effectPrefab = null)
        {
            BaseBuff buff;
            int buffId = (int)buffType;
            if (m_BuffMap.TryGetValue(buffId, out buff))
            {
                buff.Reload(effectTime, val, val2,val3);
            }
            else
            {
                if(buffType == BuffType.ShieldBuff)
                {
                    buff = new ShieldBuff(m_Player,this,effectTime,buffId ,effectPrefab);
                    m_BuffMap.Add(buffId, buff);
                }
                else if(buffType == BuffType.FireSpeed)
                {
                    buff = new FireSpeedBuff(m_Player, this, effectTime, buffId, val);
                    m_BuffMap.Add(buffId, buff);
                }
                else if(buffType == BuffType.FireSpeedX)
                {
                    buff = new FireSpeedXBuff(m_Player, this, effectTime, buffId,val );
                    m_BuffMap.Add(buffId, buff);
                }
                else if(buffType == BuffType.FireCountTimes)
                {
                    buff = new FireCountTimesBuff(m_Player, this, effectTime, buffId, val);
                    m_BuffMap.Add(buffId, buff);
                }
                else if(buffType == BuffType.FireCountTimesX)
                {
                    buff = new FireCountTimesXBuff(m_Player, this, effectTime, buffId, val);
                    m_BuffMap.Add(buffId, buff);
                }else if(buffType == BuffType.FireCurveInc)
                {
                    buff = new FireCurveInc(m_Player, this, effectTime, buffId, val);
                    m_BuffMap.Add(buffId, buff);
                }
                else if(buffType == BuffType.FireCurveIncX)
                {
                    buff = new FireCurveIncX(m_Player, this, effectTime, buffId, val);
                    m_BuffMap.Add(buffId, buff);
                }else if(buffType == BuffType.Atk)
                {
                    buff = new FireAtk(m_Player, this, effectTime, buffId, val);
                    m_BuffMap.Add(buffId, buff);
                }
                else if(buffType == BuffType.AtkX)
                {
                    buff = new FireAtkX(m_Player, this, effectTime, buffId, val);
                    m_BuffMap.Add(buffId, buff);
                }
                else if(buffType == BuffType.MoveSpeed)
                {
                    buff = new MoveSpeedBuff(m_Player, this, effectTime, buffId, val);
                    m_BuffMap.Add(buffId, buff);
                }
                else if(buffType == BuffType.MoveSpeedX)
                {
                    buff = new MoveSpeedXBuff(m_Player, this, effectTime, buffId, val);
                    m_BuffMap.Add(buffId, buff);
                }
                else if (buffType == BuffType.PlayerEnergy)
                {
                    buff = new PlayerEnergy(m_Player, this, effectTime, buffId, val);
                    m_BuffMap.Add(buffId, buff);
                }
                else if(buffType == BuffType.NaNoboostBuff)
                {
                    buff = new NaNoboostBuff(m_Player, this, effectTime, buffId, val,val2,val3,effectPrefab);
                    m_BuffMap.Add(buffId, buff);
                }
                else if (buffType == BuffType.CritBuff)
                {

                }
                else if (buffType == BuffType.CritBuffX)
                {

                }
                else if (buffType == BuffType.DodgeBuff)
                {

                }
                else if (buffType == BuffType.DodgeBuffX)
                {

                }
                else if (buffType == BuffType.OneKillBuff)
                {
                }
                else if (buffType == BuffType.MaxHpBuff)
                {
                    
                }
            }
        }
        public bool CheckHasBuff(BuffType buffType)
        {
            return CheckHasBuff((int)buffType);
        }

        public bool CheckHasBuff(int buffId)
        {
            return m_BuffMap.Get(buffId) != null;
        }

        public BaseBuff GetBuff(BuffType buffType)
        {
            return GetBuff((int)buffType);
        }
        public BaseBuff GetBuff(int buffId)
        {
            BaseBuff buff = null;
            m_BuffMap.TryGetValue(buffId, out buff);
            return buff;
        }

        public void RemoveBuff(BuffType buffType)
        {
            RemoveBuff((int)buffType);
        }
        public void RemoveBuff(int buffId)
        {
            BaseBuff buff;
            if(m_BuffMap.TryGetValue(buffId,out buff))
            {
                RemoveBuff(buff);
            }
        }
        public void RemoveBuff(BaseBuff buff)
        {
            buff.Destroy();
            m_BuffMap.Remove(buff.GetBuffId());
        }
        //=================================
      
        public float GetIncFireCountTimes()
        {
            float val = 0f;
            BaseBuff buff;
            if (m_BuffMap.TryGetValue((int)BuffType.FireCountTimes, out buff))
            {
                val += buff.GetVal();
            }
            if (m_BuffMap.TryGetValue((int)BuffType.FireCountTimesX, out buff))
            {

                val += buff.GetVal();
            }
            return val;
        }
        public float GetIncFireCurveInc()
        {
            float val = 0f;
            BaseBuff buff;
            if (m_BuffMap.TryGetValue((int)BuffType.FireCurveInc, out buff))
            {
                val += buff.GetVal();
            }
            if (m_BuffMap.TryGetValue((int)BuffType.FireCurveIncX, out buff))
            {
                val += buff.GetVal();
            }
            return val;
        }

        public float GetIncFireSpeed()
        {
            float val = 0;
            val += CampIncAtkSpeed;
            BaseBuff buff;
            if(m_BuffMap.TryGetValue((int)BuffType.FireSpeed, out buff))
            {
                val += buff.GetVal();
            }
            if(m_BuffMap.TryGetValue((int)BuffType.FireSpeedX,out buff)){

                val += buff.GetVal();
            }
            if (m_BuffMap.TryGetValue((int)BuffType.NaNoboostBuff, out buff))
            {
                val += buff.GetVal2();
            }
            return val;
        }

        public float GetIncFireAtk()
        {
            float val = 0;
            val += CampIncAtkA;
            val += CampIncAtkB;
            BaseBuff buff;
            if (m_BuffMap.TryGetValue((int)BuffType.Atk, out buff))
            {
                val += buff.GetVal();
            }
            if (m_BuffMap.TryGetValue((int)BuffType.AtkX, out buff))
            {
                val += buff.GetVal();
            }
            if (m_BuffMap.TryGetValue((int)BuffType.NaNoboostBuff, out buff))
            {
                val += buff.GetVal3();
            }
            return val;
        }

        public float GetIncMoveSpeed()
        {
            float val = 0;
            BaseBuff buff;
            if (m_BuffMap.TryGetValue((int)BuffType.MoveSpeed, out buff))
            {
                val += buff.GetVal();
            }
            if (m_BuffMap.TryGetValue((int)BuffType.MoveSpeedX, out buff))
            {
                val += buff.GetVal();
            }
            if(m_BuffMap.TryGetValue((int)BuffType.NaNoboostBuff, out buff))
            {
                val += buff.GetVal();
            }
            return val;
        }

        public bool HasEnergy()
        {
            float val = 0;
            BaseBuff buff;
            if (m_BuffMap.TryGetValue((int)BuffType.PlayerEnergy, out buff))
            {
                val += buff.GetVal();
                val = val > 100 ? 100 : val;
            }
            
            return val > 1;
        }
    }
}
