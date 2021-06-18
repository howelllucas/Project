using EZ.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ.DataMgr
{
    public class WeaponMgr:BaseDataMgr<WeaponDTO>
    {
        public WeaponMgr()
        {
            OnInit();
        }

        public override void OnInit()
        {
            base.OnInit();
            Init("weapon");
            if (m_Data == null)
            {
                m_Data = new WeaponDTO();
            }
        }

        public void AfterInit()
        {
            //初始化默认武器状态
            SetWeaponOpenState(Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.DEFAULT_WEAPON).content, WeaponStateConstVal.EXIST);

            if (m_Data.curSubWeapon != null && !m_Data.curSubWeapon.Equals(GameConstVal.EmepyStr))
            {
                if (GetWeaponOpenState(m_Data.curSubWeapon) == WeaponStateConstVal.NONE)
                {
                    m_Data.curSubWeapon = GameConstVal.EmepyStr;
                }
            }

            if (m_Data.curPet != null && !m_Data.curPet.Equals(GameConstVal.EmepyStr))
            {
                if (GetWeaponOpenState(m_Data.curPet) == WeaponStateConstVal.NONE)
                {
                    m_Data.curPet = GameConstVal.EmepyStr;
                }
            }
        }

        //碎片
        public ItemDTO GetChip(int itemId)
        {
            ItemDTO itemDTO = null;
            m_Data.chipMap.TryGetValue(itemId.ToString(), out itemDTO);
            return itemDTO;
        }

        //武器等级
        public int GetWeaponShowLevel(string weaponName)
        {
            int curLv = GetWeaponLevel(weaponName);
            int startLv = GetWeaponStartLevel(weaponName);
            startLv = Mathf.Max(1, startLv);
            return (curLv - startLv + 1);
        }
        public int GetWeaponLevel(string weaponName)
        {
            WeaponItemDTO itemDTO;
            if (!m_Data.weaponMap.TryGetValue(weaponName, out itemDTO))
            {
                itemDTO = new WeaponItemDTO(weaponName, 1);
                m_Data.weaponMap.Add(weaponName, itemDTO);
            }
            return itemDTO.level;
        }
        //武器起始等级
        public int GetWeaponStartLevel(string weaponName)
        {
            WeaponItemDTO itemDTO;
            if (!m_Data.weaponMap.TryGetValue(weaponName, out itemDTO))
            {
                itemDTO = new WeaponItemDTO(weaponName, 1);
                m_Data.weaponMap.Add(weaponName, itemDTO);
            }
            return itemDTO.startLv;
        }
        public void SetWeaponLevel(string weaponName, int level)
        {
            int oriLevel = GetWeaponLevel(weaponName);
            if (oriLevel != level)
            {
                m_Data.weaponMap[weaponName].level = level;
                SaveData();
            }
        }

        //武器开放状态
        public int GetWeaponOpenState(string weaponName)
        {
            WeaponItemDTO itemDTO;
            if (!m_Data.weaponMap.TryGetValue(weaponName, out itemDTO))
            {
                itemDTO = new WeaponItemDTO(weaponName, 1);
                m_Data.weaponMap.Add(weaponName, itemDTO);
            }
            return itemDTO.state;
        }

        //初始化武器时，检查武器状态是否
        public bool GetWeaponOpenState(ItemItem itemConfig)
        {
            int state = GetWeaponOpenState(itemConfig.name);
            if (state == WeaponStateConstVal.NONE && FilterFactory.GetInstance().Filter(itemConfig.opencondition))
            {
                if (Convert.ToInt32(itemConfig.opencondition[0]) != FilterTypeConstVal.CUR_ITEM_NUM)
                {
                    state = WeaponStateConstVal.NEW;
                }
                if (itemConfig.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON)
                {
                    InitStartLv(itemConfig.name);
                }
                
                SetWeaponOpenState(itemConfig.name, state);
            }
            return state >= WeaponStateConstVal.EXIST;
        }
        private void InitStartLv(string weaponName)
        {
            if (m_Data.weaponMap[weaponName].startLv < 0)
            {
                ItemItem itemItem = Global.gApp.gGameData.GetItemDataByName(weaponName);
                if (itemItem.opencondition[0] == FilterTypeConstVal.SUM_LOGIN_DAY && itemItem.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON)
                {
                    int roleLv = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
                    int startLv = roleLv * itemItem.lvParam + itemItem.morelv;
                    Guns_dataItem[] items = Global.gApp.gGameData.GunDataConfig.items;
                    int maxLv = 0;
                    foreach (Guns_dataItem gunDataItem in items)
                    {
                        double[] value = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(weaponName, gunDataItem);
                        if (value != null && value.Length > 0)
                        {
                            maxLv++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    startLv = Mathf.Min(startLv, maxLv);
                    m_Data.weaponMap[weaponName].startLv = startLv;
                    SetWeaponLevel(weaponName, Mathf.Max(startLv,1));
                }
                else
                {
                    m_Data.weaponMap[weaponName].startLv = 1;
                }
            }
        }

        public bool HaveNewWeapon(int itemType)
        {
            for (int i = 0; i < Global.gApp.gGameData.ItemTypeMapData[itemType].Count; i++)
            {
                ItemItem itemConfig = Global.gApp.gGameData.ItemTypeMapData[itemType][i];
                bool have = HaveNewWeapon(itemConfig);
                if (have)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanUpdateWeapon(int itemType)
        {
            ItemItem itemConfig;
            if (itemType == ItemTypeConstVal.BASE_MAIN_WEAPON)
            {
                itemConfig = Global.gApp.gGameData.GetItemDataByName(Global.gApp.gSystemMgr.GetWeaponMgr().GetCurMainWeapon());
            } else if (itemType == ItemTypeConstVal.SUB_WEAPON && Global.gApp.gSystemMgr.GetWeaponMgr().GetCurSubWeapon() != null 
                && !Global.gApp.gSystemMgr.GetWeaponMgr().GetCurSubWeapon().Equals(GameConstVal.EmepyStr))
            {
                itemConfig = Global.gApp.gGameData.GetItemDataByName(Global.gApp.gSystemMgr.GetWeaponMgr().GetCurSubWeapon());
            } else if (itemType == ItemTypeConstVal.PET && Global.gApp.gSystemMgr.GetWeaponMgr().GetCurPet() != null
                && !Global.gApp.gSystemMgr.GetWeaponMgr().GetCurPet().Equals(GameConstVal.EmepyStr))
            {
                itemConfig = Global.gApp.gGameData.GetItemDataByName(Global.gApp.gSystemMgr.GetWeaponMgr().GetCurPet());
            } else
            {
                return false;
            }
            
            bool can = CanUpdateWeapon(itemConfig);
            if (can)
            {
                return true;
            }
            return false;
        }

        //是否有新武器
        public bool HaveNewWeapon(ItemItem itemConfig)
        {
            GetWeaponOpenState(itemConfig);
            int state = GetWeaponOpenState(itemConfig.name);
            if (state == WeaponStateConstVal.NEW)
            {
                return true;
            }
            return false;
        }

        //检查武器状态是否可以升级
        public bool CanUpdateWeapon(ItemItem itemConfig)
        {
            GetWeaponOpenState(itemConfig);
            int level = GetWeaponLevel(itemConfig.name);
            int state = GetWeaponOpenState(itemConfig.name);
            int errorCode = itemConfig.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON ? CanLevelUp(itemConfig.name) : CanLevelUpSub(itemConfig.name);
            //未到可升级等级
            if (errorCode > 0)
            {
                return false;
            }
            
            double[] costValue = itemConfig.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON ? ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(itemConfig.name + "_cost", Global.gApp.gGameData.GunDataConfig.Get(level)) 
                : ReflectionUtil.GetValueByProperty<GunsSub_dataItem, double[]>("base_cost_" + itemConfig.qualevel, Global.gApp.gGameData.GunSubDataConfig.Get(level));
            if (GameItemFactory.GetInstance().GetItem((int)costValue[0]) < costValue[1])
            {
                return false;
            }
            return true;
        }


        public void SetWeaponOpenState(string weaponName,int openState)
        {
            int oriOpenState = GetWeaponOpenState(weaponName);
            if(oriOpenState != openState)
            {
                m_Data.weaponMap[weaponName].state = openState;
                SaveData();
            }
        }

        //换武器
        public bool SetCurMainWeapon(string weaponName)
        {
            if (m_Data.curMainWeapon == null || !m_Data.curMainWeapon.Equals(weaponName))
            {
                m_Data.curMainWeapon = weaponName;
                SaveData();
                return true;
            }
            return false;
        }

        //换副武器
        public bool SetCurSubWeapon(string weaponName)
        {
            if (m_Data.curSubWeapon == null || !m_Data.curSubWeapon.Equals(weaponName))
            {
                m_Data.curSubWeapon = weaponName;
                SaveData();
                return true;
            }
            return false;
        }

        //换宠物
        public bool SetCurPet(string weaponName)
        {
            if (m_Data.curPet == null || !m_Data.curPet.Equals(weaponName))
            {
                m_Data.curPet = weaponName;
                SaveData();
                return true;
            }
            return false;
        }

        public int GetCurMainWeaponId()
        {
            ItemItem item = Global.gApp.gGameData.GetItemDataByName(m_Data.curMainWeapon);
            if (item != null)
            {
                return item.id;
            }
            else
            {
                return 0;
            }
        }
        public string GetCurMainWeapon()
        {
            return m_Data.curMainWeapon;
        }

        public int GetCurSubWeaponId()
        {
            ItemItem item = Global.gApp.gGameData.GetItemDataByName(m_Data.curSubWeapon);
            if (item != null)
            {
                return item.id;
            }
            else
            {
                return 0;
            }
        }

        public string GetCurSubWeapon()
        {
            return m_Data.curSubWeapon;
        }

        public int GetCurPetId()
        {
            ItemItem item = Global.gApp.gGameData.GetItemDataByName(m_Data.curPet);
            if (item != null)
            {
                return item.id;
            }
            else
            {
                return 0;
            }
        }

        public string GetCurPet()
        {
            return m_Data.curPet;
        }

        //武器升级
        public bool LevelUp(string weaponName)
        {
            int oriLevel = GetWeaponLevel(weaponName);
            ItemItem itemItem = Global.gApp.gGameData.GetItemDataByName(weaponName);
            if (itemItem.showtype == ItemTypeConstVal.BASE_MAIN_WEAPON)
            {
                Guns_dataItem nextLevelCfg = Global.gApp.gGameData.GunDataConfig.Get(oriLevel + 1);
                
                nextLevelCfg = Global.gApp.gGameData.GunDataConfig.Get(oriLevel + 1);

                if (nextLevelCfg == null)
                {
                    return false;
                }
            }
            else
            {
                GunsSub_dataItem nextLevelCfg = Global.gApp.gGameData.GunSubDataConfig.Get(oriLevel + 1);

                nextLevelCfg = Global.gApp.gGameData.GunSubDataConfig.Get(oriLevel + 1);

                if (nextLevelCfg == null)
                {
                    return false;
                }
            }
            SetWeaponLevel(weaponName, ++oriLevel);
            return true;
        }

        //求出当前品质
        //public int GetQuaLevel(ItemItem itemConfig, int curLevel)
        //{
        //    int qualevel = 0;
        //    if (itemConfig.qualevel.Length == 0)
        //    {
        //        return qualevel;
        //    }
        //    for (int i = 0; i < itemConfig.qualevel.Length - 1; i++)
        //    {
        //        if (itemConfig.qualevel[i] <= curLevel && curLevel < itemConfig.qualevel[i + 1])
        //        {
        //            qualevel = i;
        //            return qualevel;
        //        }
        //    }
        //    return itemConfig.qualevel.Length - 2;
        //}

        public Dictionary<string, int> GetWeaponLvMap()
        {
            Dictionary<string, int> weaponLvMap = new Dictionary<string, int>();
            foreach (WeaponItemDTO itemDTO in m_Data.weaponMap.Values)
            {
                if (itemDTO.state > WeaponStateConstVal.NONE)
                {
                    weaponLvMap[itemDTO.id] = itemDTO.level;
                }
            }
            return weaponLvMap;
        }

        public int CanLevelUp(string weaponName)
        {
            ItemItem itemConfig = Global.gApp.gGameData.GetItemDataByName(weaponName);
            int gunLevel = GetWeaponLevel(itemConfig.name);
            int roleLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
            int state = GetWeaponOpenState(weaponName);
            if (state == WeaponStateConstVal.NONE)
            {
                return 1004;
            }
            Guns_dataItem nextLevelCfg = Global.gApp.gGameData.GunDataConfig.Get(gunLevel + 1);
            if (nextLevelCfg == null)
            {
                return 3046;
            }
            double[] weaponParams = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]> (itemConfig.name, nextLevelCfg);
            if (weaponParams == null || weaponParams.Length == 0)
            {
                return 3046;
            }

            int startLv = GetWeaponStartLevel(itemConfig.name);
            startLv = Mathf.Max(1, startLv);
            int maxLv = startLv + itemConfig.maxlv - 1;
            if (gunLevel >= maxLv)
            {
                return 3046;
            }
            //20191014 陈冬要求去掉角色等级对武器升级的限制
            //if (gunLevel >= roleLevel)
            //{
            //    return 3082;
            //}
            return 0;
        }

        public int CanLevelUpSub(string weaponName)
        {
            ItemItem itemConfig = Global.gApp.gGameData.GetItemDataByName(weaponName);
            int gunLevel = GetWeaponLevel(itemConfig.name);
            int roleLevel = Global.gApp.gSystemMgr.GetBaseAttrMgr().GetLevel();
            int state = GetWeaponOpenState(weaponName);
            if (state == WeaponStateConstVal.NONE)
            {
                if (itemConfig.opencondition[0] == FilterTypeConstVal.CUR_ITEM_NUM)
                {
                    return 1008;
                } else
                {
                    return 3091;
                }
            }
            GunsSub_dataItem nextLevelCfg = Global.gApp.gGameData.GunSubDataConfig.Get(gunLevel + 1);
            if (nextLevelCfg == null)
            {
                return 3046;
            }

            double[] weaponParams = ReflectionUtil.GetValueByProperty<GunsSub_dataItem, double[]>("base_params_" + itemConfig.qualevel, nextLevelCfg);
            if (weaponParams == null || weaponParams.Length == 0)
            {
                return 3046;
            }

            //20191014 陈冬要求去掉角色等级对武器升级的限制
            //if (gunLevel >= roleLevel)
            //{
            //    return 3082;
            //}
            return 0;
        }

        public void UpdateQualityLv(ItemItem itemConfig,Action<int> resultCallBack)
        {
            WeaponItemDTO itemDTO;
            if (m_Data.weaponMap.TryGetValue(itemConfig.name, out itemDTO))
            {
                if(itemDTO.qualityLv == 0)
                {
                    bool isSucess = true;
                    for(int i = 0; i < 2; i ++)
                    {
                        float[] condition = new float[]
                        {
                            itemConfig.supercondition[0],
                            itemConfig.supercondition[i * 2 + 1],
                            itemConfig.supercondition[i * 2 + 2],
                        };
                        isSucess = isSucess && FilterFactory.GetInstance().Filter(condition);
                        if(!isSucess)
                        {
                            ItemItem reduceCfg = Global.gApp.gGameData.ItemData.Get(Convert.ToInt32(condition[1]));
                            Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 1008, Global.gApp.gGameData.GetTipsInCurLanguage(reduceCfg.sourceLanguage));
                            break;
                        }
                    }
                    if(isSucess)
                    {
                        ItemDTO reduceItemDTO = new ItemDTO(Convert.ToInt32(itemConfig.supercondition[1]), itemConfig.supercondition[2], BehaviorTypeConstVal.OPT_CAMP_UPGRADE_QUALITY);
                        reduceItemDTO.paramStr1 = itemConfig.name;
                        reduceItemDTO.paramStr2 = itemDTO.qualityLv.ToString();
                        GameItemFactory.GetInstance().ReduceItem(reduceItemDTO);
                        if (reduceItemDTO.result)
                        {
                            ItemDTO reduceItemDTO2 = new ItemDTO(Convert.ToInt32(itemConfig.supercondition[3]), itemConfig.supercondition[4], BehaviorTypeConstVal.OPT_CAMP_UPGRADE_QUALITY);
                            reduceItemDTO2.paramStr1 = itemConfig.name;
                            reduceItemDTO.paramStr2 = itemDTO.qualityLv.ToString();
                            GameItemFactory.GetInstance().ReduceItem(reduceItemDTO2);
                            if (reduceItemDTO2.result)
                            {
                                itemDTO.qualityLv++;
                                SaveData();
                                resultCallBack(0);
                            }
                            else
                            {
                                resultCallBack(1008);
                            }
                        }
                        else
                        {
                            resultCallBack(1008);
                        }
                    }
                    else
                    {
                        resultCallBack(1008);
                    }
                }
                else
                {
                    resultCallBack(1008);
                }
            }
            else
            {
                resultCallBack(1008);
            }
        }
        public int GetQualityLv(string weaponName)
        {
            ItemItem itemConfig = Global.gApp.gGameData.GetItemDataByName(weaponName);
            return GetQualityLv(itemConfig);
        }
        public int GetQualityLv(ItemItem itemConfig)
        {
            WeaponItemDTO itemDTO;
            if (m_Data.weaponMap.TryGetValue(itemConfig.name, out itemDTO))
            {
                return itemDTO.qualityLv;
            }
            else
            {
                return 0;
            }
        }
        protected override void Init(string filePath)
        {
            base.Init(filePath);
        }
    }
}
