using EZ.Data;
using EZ.Util;
using System;
using System.Collections.Generic;

namespace EZ.DataMgr
{
    public class GameItemFactory
    {
        private static GameItemFactory m_Instance = new GameItemFactory();

        public Dictionary<int, BaseItemStrategy> m_ItemStrategyMap = new Dictionary<int, BaseItemStrategy>();
        public GameItemFactory()
        {
            m_ItemStrategyMap.Add(ItemTypeConstVal.GOLD, new GoldItemStrategy(Global.gApp.gSystemMgr.GetBaseAttrMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.DIAMOND, new DiamondItemStrategy(Global.gApp.gSystemMgr.GetBaseAttrMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.HEART, new HeartItemStrategy(Global.gApp.gSystemMgr.GetBaseAttrMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.EXP, new ExpItemStrategy(Global.gApp.gSystemMgr.GetBaseAttrMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.ENERGY, new EnergyItemStrategy(Global.gApp.gSystemMgr.GetBaseAttrMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.MDT, new MDTItemStrategy(Global.gApp.gSystemMgr.GetBaseAttrMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.BASE_MAIN_WEAPON, new WeaponItemStrategy(Global.gApp.gSystemMgr.GetWeaponMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.SUB_WEAPON, new SubWeaponItemStrategy(Global.gApp.gSystemMgr.GetWeaponMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.PET, new PetItemStrategy(Global.gApp.gSystemMgr.GetWeaponMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.SUB_WEAPON_CHIP, new ChipItemStrategy(Global.gApp.gSystemMgr.GetWeaponMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.PET_CHIP, new ChipItemStrategy(Global.gApp.gSystemMgr.GetWeaponMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.WEAPON_CHIP, new ChipItemStrategy(Global.gApp.gSystemMgr.GetWeaponMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.NPC_AWARD, new NpcAwardItemStrategy(Global.gApp.gSystemMgr.GetNpcMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.NPC, new NpcItemStrategy(Global.gApp.gSystemMgr.GetNpcMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.PHOTO, new NpcAwardItemStrategy(Global.gApp.gSystemMgr.GetNpcMgr()));
            m_ItemStrategyMap.Add(ItemTypeConstVal.BADGE, new NpcAwardItemStrategy(Global.gApp.gSystemMgr.GetNpcMgr()));
        }

        public bool CanReduce(ItemDTO itemDTO)
        {
            ItemItem itemConfig = GetConfig(itemDTO.itemId);
            bool result = m_ItemStrategyMap[itemConfig.showtype].CanReduce(itemDTO);
            return result;
        }

        public void AddItem(ItemDTO itemDTO)
        {
            ItemItem itemConfig = GetConfig(itemDTO.itemId);
            bool result = m_ItemStrategyMap[itemConfig.showtype].AddItem(itemDTO);
            itemDTO.result = result;
            itemDTO.after = m_ItemStrategyMap[itemConfig.showtype].GetItem(itemDTO.itemId);

            Global.gApp.gSystemMgr.GetNpcMgr().NpcQuestChange(FilterTypeConstVal.GET_ITEM, itemDTO.itemId, itemDTO.num);
            Global.gApp.gSystemMgr.GetNpcMgr().NpcQuestChange(FilterTypeConstVal.GET_ITEM_BY_TYPE, itemConfig.showtype, itemDTO.num);

            if (itemConfig.showtype == ItemTypeConstVal.NPC)
            {
                Global.gApp.gSystemMgr.GetNpcMgr().Fresh(false);
                Global.gApp.gSystemMgr.GetNpcMgr().ResetNpcAtkLevel();
            }

            ELKLog4Item elkLog = new ELKLog4Item(BehaviorTypeConstVal.LOG_ADD_ITEM, itemDTO);
            ELKLogMgr.GetInstance().MakeELKLog4Destroy(elkLog);
            ELKLogMgr.GetInstance().SendELKLog4Item(elkLog);

            //infoc 日志
            //InfoCLogUtil.instance.SendPropGainLog(itemDTO);
        }
        public void ReduceItem(ItemDTO itemDTO)
        {
            ItemItem itemConfig = GetConfig(itemDTO.itemId);
            bool result = m_ItemStrategyMap[itemConfig.showtype].ReduceItem(itemDTO);
            itemDTO.num = Math.Abs(itemDTO.num);
            itemDTO.result = result;
            itemDTO.after = m_ItemStrategyMap[itemConfig.showtype].GetItem(itemDTO.itemId);
            if (result)
            {
                ELKLog4Item elkLog = new ELKLog4Item(BehaviorTypeConstVal.LOG_REDUCE_ITEM, itemDTO);
                ELKLogMgr.GetInstance().MakeELKLog4Destroy(elkLog);
                ELKLogMgr.GetInstance().SendELKLog4Item(elkLog);


            	//infoc 日志
	            //InfoCLogUtil.instance.SendPropUseLog(itemDTO);
            }

        }

        //批量增加
        public void AddItem(List<ItemDTO> itemDTOList)
        {
            foreach (ItemDTO itemDTO in itemDTOList)
            {
                AddItem(itemDTO);
            }
        }
        
        //批量减少
        public bool ReduceItem(List<ItemDTO> itemDTOList)
        {
            foreach (ItemDTO itemDTO in itemDTOList)
            {
                ItemItem itemConfig = GetConfig(itemDTO.itemId);
                if (!m_ItemStrategyMap[itemConfig.showtype].CanReduce(itemDTO))
                {
                    Global.gApp.gMsgDispatcher.Broadcast<int, string>(MsgIds.ShowGameTipsByIDAndParam, 3080, Global.gApp.gGameData.ItemData.Get(itemDTO.itemId).gamename);
                    return false;
                }
            }
            foreach (ItemDTO itemDTO in itemDTOList)
            {
                ReduceItem(itemDTO);
            }
            return true;
        }

        //通过配置批量增加
        public void AddItem(string[] strs, int behaviorTypeConstVal)
        {
            List<ItemDTO> itemDTOList = DealItems(strs, behaviorTypeConstVal);
            AddItem(itemDTOList);
        }

        //通过配置批量减少
        public bool ReduceItem(string[] strs, int behaviorTypeConstVal)
        {
            List<ItemDTO> itemDTOList = DealItems(strs, behaviorTypeConstVal);
            if (itemDTOList == null)
            {
                return false;
            }
            bool result = ReduceItem(itemDTOList);
            return result;
        }


        //处理配置为物品
        public List<ItemDTO> DealItems(string[] strs, int behaviorTypeConstVal)
        {
            
            if (strs.Length % 2 == 1)
            {
                Global.gApp.gMsgDispatcher.Broadcast<int>(MsgIds.ShowGameTipsByID, 3081);
                return null;
            } else
            {
                List<ItemDTO> result = new List<ItemDTO>();
                for (int i = 0; i < strs.Length; i += 2)
                {
                    ItemDTO itemDTO = new ItemDTO(int.Parse(strs[i]), double.Parse(strs[i + 1]), behaviorTypeConstVal);
                    result.Add(itemDTO);
                }
                return result;
            }
        }


        public double GetItem(int itemId)
        {
            ItemItem itemConfig = GetConfig(itemId);
            return m_ItemStrategyMap[itemConfig.showtype].GetItem(itemId);
        }

        private ItemItem GetConfig(int itemId)
        {
            ItemItem itemConfig = Global.gApp.gGameData.ItemData.Get(itemId);
            if (itemConfig == null)
            {
                throw new BussinessException(1);

            }
            if (!m_ItemStrategyMap.ContainsKey(itemConfig.showtype))
            {
                throw new BussinessException(1);
            }
            return itemConfig;
        }

        public static GameItemFactory GetInstance()
        {
            return m_Instance;
        }

    }
}
