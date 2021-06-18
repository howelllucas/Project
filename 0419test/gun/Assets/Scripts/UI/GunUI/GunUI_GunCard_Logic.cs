using EZ.Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tool;
using Game.Data;

namespace EZ
{
    public partial class GunUI_GunCard
    {
        public enum State
        {
            GunOpt,
            Campsite,
        }
        public State state;
        private GunCard_TableItem gunCardRes;
        private List<GameObject> starList = new List<GameObject>();

        private void Awake()
        {
            InitNode();

            starList.Clear();
            starList.Add(Icon1.gameObject);
            starList.Add(Icon2.gameObject);
            starList.Add(Icon3.gameObject);
            starList.Add(Icon4.gameObject);
            starList.Add(Icon5.gameObject);
        }
        public void Init(GunCard_TableItem res)
        {
            gunCardRes = res;
            if (gunCardRes == null)
                return;

            TipsTitle.text.text = LanguageMgr.GetText(gunCardRes.tid_name);
            IconBtn.image.sprite = Resources.Load(gunCardRes.icon, typeof(Sprite)) as Sprite;
            Frame.image.sprite = Resources.Load(TableMgr.singleton.ValueTable.GetCardQualityFrame(gunCardRes.rarity),
                                                typeof(Sprite)) as Sprite;

            int occupiedPointIndex;
            if (CampsiteMgr.singleton.CheckCardIsOccupied(res.id, out occupiedPointIndex))
                OccupiedFlag.gameObject.SetActive(true);
            else
                OccupiedFlag.gameObject.SetActive(false);

            var cardData = PlayerDataMgr.singleton.GetGunCardData(gunCardRes.id);
            if (cardData == null)
            {
                ChipCount.gameObject.SetActive(false);
                Level.gameObject.SetActive(false);
                IconBtn.button.enabled = false;
                if (LvupIcon.gameObject != null)
                    LvupIcon.gameObject.SetActive(false);
                if (StarupIcon.gameObject != null)
                    StarupIcon.gameObject.SetActive(false);
                for (int i = 0; i < starList.Count; ++i)
                {
                    starList[i].SetActive(false);
                }
                return;
            }

            ChipCount.gameObject.SetActive(true);
            Level.gameObject.SetActive(true);
            IconBtn.button.enabled = true;
            ChipCount.text.text = cardData.count.ToString();

            var gunStarRes = TableMgr.singleton.CardStarTable.GetItemByID(cardData.star);
            if (gunStarRes == null)
                return;

            Level.text.text = string.Format("Lv.{0}", cardData.level);

            for (int i = 0; i < starList.Count; ++i)
            {
                if (i < gunStarRes.star)
                {
                    starList[i].SetActive(true);
                }
                else
                {
                    starList[i].SetActive(false);
                }
            }


            if (state == State.GunOpt)
            {
                if (LvupIcon.gameObject != null)
                {
                    if (PlayerDataMgr.singleton.CanCardLvUp(gunCardRes.id))
                    {
                        LvupIcon.gameObject.SetActive(true);
                    }
                    else
                    {
                        LvupIcon.gameObject.SetActive(false);
                    }
                }

                if (StarupIcon.gameObject != null)
                {
                    if (PlayerDataMgr.singleton.CanCardStarUp(gunCardRes.id))
                    {
                        StarupIcon.gameObject.SetActive(true);
                    }
                    else
                    {
                        StarupIcon.gameObject.SetActive(false);
                    }
                }
            }

            if (NewIcon != null)
            {
                NewIcon.gameObject.SetActive(cardData.isNew);
            }
        }

        private void InitNode()
        {
            IconBtn.button.onClick.AddListener(OnClick);

        }

        private void OnClick()
        {
            PlayerDataMgr.singleton.SetCardNew(gunCardRes.id);
            if (NewIcon != null)
                NewIcon.gameObject.SetActive(false);
            Global.gApp.gUiMgr.OpenPanel(Wndid.GunInfoUI, gunCardRes);
            if (state == State.Campsite)
            {
                GunInfoUI infoPanel = Global.gApp.gUiMgr.GetPanelCompent<GunInfoUI>(Wndid.GunInfoUI);
                infoPanel.HideOptBtns();
            }
        }
    }
}