using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace EZ
{
    public partial class GunCardIcon : MonoBehaviour
    {
        public void Init(GunCard_TableItem res)
        {
            TipsTitle.text.text = LanguageMgr.GetText(res.tid_name);
            IconBtn.image.sprite = Resources.Load(res.icon, typeof(Sprite)) as Sprite;
            Frame.image.sprite = Resources.Load(TableMgr.singleton.ValueTable.GetCardQualityFrame(res.rarity),
                                                typeof(Sprite)) as Sprite;

            //根据品质设置亮边
        }

        public void ShowCardCount(int count)
        {
            CardCount.text.text = string.Format("x{0}", count);
            CardCount.gameObject.SetActive(true);
            ChipIcon.gameObject.SetActive(false);
        }

        public void ShowChipCount(int count)
        {
            ChipCount.text.text = string.Format("x{0}", count);
            ChipIcon.gameObject.SetActive(true);
            CardCount.gameObject.SetActive(false);
        }

        public void ShowNewFlag()
        {
            NewIcon.gameObject.SetActive(true);
        }

        public void HideNewFlag()
        {
            NewIcon.gameObject.SetActive(false);
        }

        public void SetHighlightVisible(bool visible)
        {
            //设置亮边Root显隐
        }
    }
}