using DG.Tweening;
using Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EZ
{

    public partial class BoxOpenUI
    {
        private Animator cardAnim;

        private List<DrawCardData> cardList = new List<DrawCardData>();
        private int index = 0;
        private int cardIdx = -1;
        private int boxCount = 0;
        private bool showCard = false;
        private bool flyCard = false;

        private int newCardCount;

        protected override void InitOnceInfo()
        {
            base.InitOnceInfo();
            cardAnim = CardRoot.gameObject.GetComponent<Animator>();
            CloseBtn.button.onClick.AddListener(TouchClose);
            var clickEntry = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerClick,
                callback = new EventTrigger.TriggerEvent()
            };
            clickEntry.callback.AddListener((e) => { CardFly(); });
            ClickObj.gameObject.GetComponent<EventTrigger>().triggers.Add(clickEntry);
        }

        public void ShowOpenBox(int boxId, List<DrawCardData> resultList)
        {
            var bosRes = TableMgr.singleton.BoxTable.GetItemByID(boxId);
            if (bosRes == null)
                return;

            //boxDownImg.sprite = ResourceMgr.singleton.GetSprite(SpriteAtlasNames.CARD, string.Format("{0}_01", bosRes.icon));
            //cardTopImg.sprite = ResourceMgr.singleton.GetSprite(SpriteAtlasNames.CARD, string.Format("{0}_02", bosRes.icon));
            //cardOpenImg.sprite = ResourceMgr.singleton.GetSprite(SpriteAtlasNames.CARD, string.Format("{0}_03", bosRes.icon));

            cardList.Clear();
            cardList.AddRange(resultList);
            newCardCount = 0;
            for (int i = 0; i < cardList.Count; i++)
            {
                if (cardList[i].isNew)
                    newCardCount++;
            }

            OpenStart();
        }

        public void ShowOpenBtn(CurrencyType costType, int cost, UnityEngine.Events.UnityAction onClick)
        {
            OpenCostIcon.image.sprite = GameGoodsMgr.singleton.GetCurrencyIcon(costType);
            OpenCostTxt.text.text = string.Format("{0}/{1}", PlayerDataMgr.singleton.GetCurrency(costType), cost);
            OpenBtn.button.onClick.RemoveAllListeners();
            OpenBtn.button.onClick.AddListener(TouchClose);
            OpenBtn.button.onClick.AddListener(onClick);
            OpenBtn.gameObject.SetActive(true);
        }

        void OpenStart()
        {
            Global.gApp.gAudioSource.PlayOneShot("box_open", true);
            //SoundEffectManager.Instance["2d"].PlaySoundEffectOneShot("box_open");

            Invoke("CardFly", 2.5f);

            boxCount = cardList.Count;
            BoxCountTxt.text.text = boxCount.ToString();

        }

        public void CardFly()
        {
            if (flyCard)
                return;

            ClickObj.gameObject.SetActive(false);

            TipsTxt.gameObject.SetActive(false);
            NewCardInfo.gameObject.SetActive(false);

            cardIdx++;
            if (cardIdx >= cardList.Count)
            {
                CardRoot.gameObject.SetActive(false);

                BoxRoot.rectTransform.DOLocalMoveY(30.0f, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    ShowRoot.gameObject.SetActive(true);
                    CloseNode.gameObject.SetActive(true);
                    //optionalGuide?.SetGuideTarget(closeObj, closeObj.transform.position, BattleGestureUI.HandType.Down, 3f, 1);
                });

                return;
            }

            var item = TableMgr.singleton.GunCardTable.GetItemByID(cardList[cardIdx].cardID);
            if (item == null)
                return;

            flyCard = true;

            CardRoot.gameObject.SetActive(false);
            CardRoot.gameObject.SetActive(true);
            if (item.rarity == 2)
            {
                Global.gApp.gAudioSource.PlayOneShot("box_get2", true);
                //SoundEffectManager.Instance["2d"].PlaySoundEffectOneShot("box_get2");
                cardAnim.SetTrigger("Zi");
                Invoke("ShowCard", 5.0f);
            }
            else if (item.rarity == 3)
            {
                Global.gApp.gAudioSource.PlayOneShot("box_get2", true);
                //SoundEffectManager.Instance["2d"].PlaySoundEffectOneShot("box_get2");
                cardAnim.SetTrigger("Cheng");
                Invoke("ShowCard", 6.0f);
            }
            else
            {
                Global.gApp.gAudioSource.PlayOneShot("box_get", true);
                //SoundEffectManager.Instance["2d"].PlaySoundEffectOneShot("box_get");
                cardAnim.SetTrigger("Normal");
                Invoke("ShowCard", 1.8f);
            }

            NewCardIcon.GunCardIcon.Init(item);
            var listCardIcon = ListCardIcon.GetInstance();
            listCardIcon.Init(item);
            listCardIcon.transform.SetAsLastSibling();
            listCardIcon.gameObject.SetActive(true);
            CardName.text.text = LanguageMgr.GetText(item.tid_name);
            if (cardList[cardIdx].isNew)
            {
                NewCardIcon.GunCardIcon.ShowNewFlag();
                NewCardIcon.GunCardIcon.ShowCardCount(1);
                Card2Chip.gameObject.SetActive(false);
                listCardIcon.ShowNewFlag();
                listCardIcon.ShowCardCount(1);
                listCardIcon.SetHighlightVisible(true);
            }
            else
            {
                NewCardIcon.GunCardIcon.HideNewFlag();
                NewCardIcon.GunCardIcon.ShowCardCount(1);
                Card2Chip.gameObject.SetActive(true);
                CardChipCount.text.text = string.Format("x{0}", 10);
                listCardIcon.HideNewFlag();
                listCardIcon.ShowChipCount(10);
                listCardIcon.SetHighlightVisible(true);
            }
        }

        public void ShowCard()
        {
            var cardData = cardList[cardIdx];

            if (cardData.isNew)
            {
                int beforeCardCount = PlayerDataMgr.singleton.GetCollectCardCount() - newCardCount;
                newCardCount--;
                int afterCardCount = PlayerDataMgr.singleton.GetCollectCardCount() - newCardCount;
                IdleBeforeTxt.text.text = LanguageMgr.GetText("ShopTab_Box_TimeText", IdleRewardMgr.singleton.GetMaxIdleHour(beforeCardCount));
                IdleAfterTxt.text.text = LanguageMgr.GetText("ShopTab_Box_TimeText", IdleRewardMgr.singleton.GetMaxIdleHour(afterCardCount));
                CampOfflineBeforeTxt.text.text = LanguageMgr.GetText("ShopTab_Box_TimeText", CampsiteMgr.singleton.GetMaxOfflineHour(beforeCardCount));
                CampOfflineAfterTxt.text.text = LanguageMgr.GetText("ShopTab_Box_TimeText", CampsiteMgr.singleton.GetMaxOfflineHour(afterCardCount));
                NewCardInfo.gameObject.SetActive(true);
            }

            index++;


            boxCount--;
            BoxCountTxt.text.text = boxCount.ToString();

            TipsTxt.gameObject.SetActive(true);
            ClickObj.gameObject.SetActive(true);
            flyCard = false;

        }

    }

}
