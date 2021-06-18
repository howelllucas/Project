using EZ.Data;
using EZ.DataMgr;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class CampTaskDetails
    {
        List<CampTaskDetails_DayTaskItemUI> m_ItemList = new List<CampTaskDetails_DayTaskItemUI>();
        List<CampTaskDetails_TotalTaskItem> m_TotalItemList = new List<CampTaskDetails_TotalTaskItem>();
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            RegisterListeners();
            InitNode();
            base.ChangeLanguage();
        }

        private void InitNode()
        {
            DayTaskItemUI.gameObject.SetActive(false);
            TotalTaskItem.gameObject.SetActive(false);
            List<NpcQuestItemDTO> nqidList = Global.gApp.gSystemMgr.GetNpcMgr().NpcQuestList;

            for (int i = 0; i < nqidList.Count; i++)
            {
                if (nqidList[i].npcQuestId == -1)
                {
                    continue;
                }
                CampNpcItem campNpcItem = Global.gApp.gGameData.CampNpcConfig.Get(nqidList[i].npcId);
                ItemItem npcItem = Global.gApp.gGameData.GetItemDataByName(nqidList[i].npcId);
                if (campNpcItem.notFresh == 1)
                {
                    CampTasksItem taskCfg = Global.gApp.gGameData.CampTasksConfig.Get(nqidList[i].npcQuestId);
                    if (taskCfg == null)
                    {
                        continue;
                    }
                    CampTaskDetails_TotalTaskItem itemUI = TotalTaskItem.GetInstance();
                    itemUI.InitNode(nqidList[i], i, this);
                    m_TotalItemList.Add(itemUI);
                } else
                {
                    CampTasksItem taskCfg = Global.gApp.gGameData.CampTasksConfig.Get(nqidList[i].npcQuestId);
                    if (taskCfg == null)
                    {
                        continue;
                    }
                    CampTaskDetails_DayTaskItemUI itemUI = DayTaskItemUI.GetInstance();
                    itemUI.InitNode(nqidList[i], i, this);
                    m_ItemList.Add(itemUI);
                }
                

            }
            m_none.gameObject.SetActive(m_TotalItemList.Count == 0);
            ChangeTotalShowIndex();
            ChangeShowIndex();

            CloseBtn.button.onClick.AddListener(TouchClose);
        }
        public void ChangeTotalShowIndex()
        {
            Dictionary<int, CampTaskDetails_TotalTaskItem> dic = new Dictionary<int, CampTaskDetails_TotalTaskItem>();
            int[] arr = new int[m_TotalItemList.Count];
            for (int i = 0; i < m_TotalItemList.Count; i++)
            {
                CampTaskDetails_TotalTaskItem itemUI = m_TotalItemList[i];
                arr[i] = itemUI.m_ShowIndex;
                dic[arr[i]] = itemUI;
            }
            Array.Sort(arr);
            for (int i = 0; i < arr.Length; i++)
            {
                dic[arr[i]].transform.SetSiblingIndex(i);
            }
        }
        public void ChangeShowIndex()
        {
            Dictionary<int, CampTaskDetails_DayTaskItemUI> dic = new Dictionary<int, CampTaskDetails_DayTaskItemUI>();
            int[] arr = new int[m_ItemList.Count];
            for (int i = 0; i < m_ItemList.Count; i ++)
            {
                CampTaskDetails_DayTaskItemUI itemUI = m_ItemList[i];
                arr[i] = itemUI.m_ShowIndex;
                dic[arr[i]] = itemUI;
            }
            Array.Sort(arr);
            for(int i = 0; i < arr.Length; i ++)
            {
                dic[arr[i]].transform.SetSiblingIndex(i);
            }
        }

        private void RegisterListeners()
        {
        }
        private void UnRegisterListeners()
        {
        }

        public override void Release()
        {
            UnRegisterListeners();
            base.Release();
        }
    }
}
