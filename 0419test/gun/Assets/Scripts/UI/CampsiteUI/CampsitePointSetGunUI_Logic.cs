using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ
{
    public partial class CampsitePointSetGunUI
    {
        public event System.Action OnChangeGun;
        private CampsitePointMgr pointDataMgr;
        
        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            CloseBtn.button.onClick.AddListener(TouchClose);
        }

        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            pointDataMgr = arg as CampsitePointMgr;
            NameTxt.text.text = pointDataMgr.buildingRes.buildingName;
            var gunTypeRes = TableMgr.singleton.GunTypeTable.GetItemByID(pointDataMgr.buildingRes.gunType);
            //TypeImg.image.sprite = gunTypeRes.icon

            #region 装备数据
            if (pointDataMgr.equipGunId > 0)
            {
                EquipCardRoot.gameObject.SetActive(true);
                NoneCardRoot.gameObject.SetActive(false);
                RemoveBtn.button.onClick.AddListener(OnRemoveBtnClick);
                CardData.CampsiteCardData.Init(pointDataMgr, pointDataMgr.equipGunId);
                CardData.CampsiteCardData.RegisterSkillClickListener(ShowSkillDesc);
            }
            else
            {
                CardData.gameObject.SetActive(false);
                EquipCardRoot.gameObject.SetActive(false);
                NoneCardRoot.gameObject.SetActive(true);
            }
            #endregion

            #region 选择列表
            var limitGunType = pointDataMgr.buildingRes.gunType;
            var gunCards = PlayerDataMgr.singleton.GetCardsByType(limitGunType);
            List<int> selectOrderList = new List<int>();
            for (int i = 0; i < gunCards.Count; i++)
            {
                //if (PlayerDataMgr.singleton.GetUseWeaponID() == gunCards[i])
                //    continue;
                if (pointDataMgr.equipGunId == gunCards[i])
                    continue;
                selectOrderList.Add(gunCards[i]);
            }

            selectOrderList.Sort((a, b) =>
            {
                double rewardFactor_a;float intervalFactor_a;
                pointDataMgr.GetCardTotalFactorOnPoint(a, out rewardFactor_a, out intervalFactor_a);
                double rewardFactor_b; float intervalFactor_b;
                pointDataMgr.GetCardTotalFactorOnPoint(b, out rewardFactor_b, out intervalFactor_b);
                return (rewardFactor_a * intervalFactor_a).CompareTo(rewardFactor_b * intervalFactor_b);
            });

            for (int i = 0; i < selectOrderList.Count; i++)
            {
                var setItem = SetItem.GetInstance();
                setItem.gameObject.SetActive(true);
                setItem.Init(pointDataMgr, selectOrderList[i]);
                setItem.onSelect -= OnSelectItem;
                setItem.onSelect += OnSelectItem;
                setItem.CardData.CampsiteCardData.RegisterSkillClickListener(ShowSkillDesc);
            }
            #endregion
            
        }

        private void OnRemoveBtnClick()
        {
            if (pointDataMgr != null)
            {
                CampsiteMgr.singleton.PointRemoveGun(pointDataMgr.index);
                CheckPointDataChange();
            }
            TouchClose();
        }

        private void ShowSkillDesc(CampSkill target)
        {
            target.SetDescState(SkillDescRoot.gameObject, SkillDescTxt.text);
        }

        private void OnSelectItem(int cardId)
        {
            CampsiteMgr.singleton.PointEquipGun(pointDataMgr.index, cardId);
            CheckPointDataChange();
            TouchClose();
        }

        private void CheckPointDataChange()
        {
            OnChangeGun?.Invoke();
        }
    }
}
