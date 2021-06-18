using DG.Tweening;
using EZ.Data;
using Game;
using UnityEngine;

namespace EZ
{
    public partial class RewardEffectUi_RewardItem
    {
        private Vector3 m_LockPos;
        private GoodsType goods;
        private RewardEffectUi m_Parent;
        private TokenUI m_TokenUi;
        public void Init(GoodsType goods, Vector3 pos, Vector3 startPos, RewardEffectUi parent, TokenUI tokenUI)
        {
            this.goods = goods;
            m_Parent = parent;
            m_TokenUi = tokenUI;

            InitIcon(goods);
            
            GetComponent<RectTransform>().position = startPos;
            gameObject.SetActive(true);
            InitLockPos(goods, pos);
            
            

        }
        private void InitLockPos(GoodsType goods, Vector3 secondPos)
        {
            if(m_TokenUi != null)
            {
                RectTransform targetTsf = m_TokenUi.GetIconById(goods);
                m_LockPos = targetTsf.position;

                Tweener flyTweer = transform.DOMove(secondPos, 0.3f).OnComplete(() => {
                    Tweener tweener = Icon.rectTransform.DOMove(m_LockPos, 1f);
                    //tweener.SetSpeedBased();
                    //tweener.SetDelay(0.1f);
                    tweener.SetEase(Ease.InCubic);
                    tweener.SetLoops(1);
                    tweener.OnComplete(OnCompleteOne);
                    tweener.Play();
                });
                flyTweer.SetEase(Ease.OutCubic);

                
            }
            else
            {
                Destroy(gameObject);
            }
            Icon.gameObject.SetActive(true);
        }

        private void OnCompleteOne()
        {
            TokenUI tokenUi = Global.gApp.gUiMgr.GetPanelCompent<TokenUI>(Wndid.TokenUI);
            bool isEnd = m_Parent.EndOne(goods);
            if (tokenUi != null)
            {
                tokenUi.UnLockById(goods, isEnd, true);
            }
            Destroy(gameObject);
        }
       
        private void InitIcon(GoodsType goods)
        {
            //Icon.image.enabled = (goods.propEffect == null || goods.propEffect == string.Empty);
            //if (goods.propEffect == null || goods.propEffect == string.Empty)
            //{
            //    Icon.image.sprite = Global.gApp.gResMgr.LoadAssets<Sprite>(goods.image_grow);
            //}
            //else
            //{
            //    GameObject effect = UiTools.GetEffect(goods.propEffect, Icon.rectTransform);
            //}
            var goodsRes = TableMgr.singleton.GameGoodsTable.GetItemByID((int)goods);
            if(goodsRes!=null && !string.IsNullOrEmpty(goodsRes.propEffect))
            {
                GameObject effect = UiTools.GetEffect(goodsRes.propEffect, Icon.rectTransform);
                Icon.image.enabled = false;
            }
            else
            {
                Icon.image.sprite = GameGoodsMgr.singleton.GetGameGoodsIcon(goods);
                Icon.image.enabled = true;
            }

        }
        
    }
}
