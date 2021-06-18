using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class ExpeditionUI_LevelItem
    {
        public Action<int> OnLevelClick = null;

        private List<GameObject> starList = new List<GameObject>();
        private int level = 0;

        private void Awake()
        {
            PassFlag.button.onClick.AddListener(OnClick);


        }
        public void Init(int lv,int curLv)
        {
            level = lv;
            PassFlag.gameObject.SetActive(lv <= curLv);
            CurFlag.gameObject.SetActive(lv == curLv);
            WillFlag.gameObject.SetActive(lv > curLv);

            starList.Clear();
            starList.Add(Star1.gameObject);
            starList.Add(Star2.gameObject);
            starList.Add(Star3.gameObject);

            foreach (var star in starList)
            {
                star.SetActive(false);
            }

            LvTxt.text.text = level.ToString();
            //LvTxt.gameObject.SetActive(true);

            var stageData = PlayerDataMgr.singleton.GetStageData(level);
            if (stageData == null)
                return;

            if (stageData.starList.Count > 0)
            {
                //LvTxt.gameObject.SetActive(false);

                if (stageData.starList.Count == 1)
                {
                    starList[0].SetActive(true);
                }
                else if (stageData.starList.Count == 2)
                {
                    starList[1].SetActive(true);
                }
                else if (stageData.starList.Count == 3)
                {
                    starList[2].SetActive(true);
                }
            }
            
        }

        private void OnClick()
        {
            if (OnLevelClick != null)
                OnLevelClick(level);

            CurFlag.gameObject.SetActive(true);

            Debug.Log("OnLevelClick " + level);
        }
    }
}