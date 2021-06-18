using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Tool;
using DG.Tweening;
using Game.Audio;

namespace Game.UI
{
    public class UICardCollect : MonoBehaviour
    {
        [Header("[Boomb Param]")]
        public float rangeX = 2.0f; //炸出范围
        public float rangeY = 1.0f; //炸出范围
        public float timeToCollect; //飞去固定位置的时间
        public PathType pathType = PathType.CatmullRom;
        public Ease easeType = Ease.InExpo;
        //public int count = 1;
        [Header("[UnityObj]")]
        public GameObject cardImg; //复制的预设
        public GameObject goldPrefab; //复制的预设
        public GameObject keyPrefab; //复制的预设
        public GameObject wantedPrefab; //复制的预设
        public Transform parent;

        private GameObject prefab;

        public string SoundEffectName { get; set; }

        public void CollectGold(Vector3 worldPosFrom, Vector3 worldPosTo, System.Action doOnFinish)
        {
            prefab = goldPrefab;

            BoomToCollect(worldPosFrom, worldPosTo, doOnFinish);

        }

        public void CollectWanted(Vector3 worldPosFrom, Vector3 worldPosTo, System.Action doOnFinish)
        {
            prefab = wantedPrefab;

            BoomToCollect(worldPosFrom, worldPosTo, doOnFinish);

        }

        public void CollectKey(Vector3 worldPosFrom, Vector3 worldPosTo, System.Action doOnFinish)
        {
            prefab = keyPrefab;

            BoomToCollect(worldPosFrom, worldPosTo, doOnFinish);

        }
        public void CollectCard(int quality, Vector3 worldPosFrom, Vector3 worldPosTo, System.Action doOnFinish)
        {
            prefab = cardImg;
            var image = prefab.GetComponent<Image>();
            image.sprite = ResourceMgr.singleton.GetSprite(SpriteAtlasNames.CARD, TableMgr.singleton.ValueTable.GetQualityCardBack(quality));

            BoomToCollect(worldPosFrom, worldPosTo, doOnFinish);
        }

        public void BoomToCollect(Vector3 worldPosFrom, Vector3 worldPosTo, System.Action doOnFinish)
        {
            //prefab.sprite = ResourceMgr.Texture2DToSprite(ResourceMgr.singleton.AddResource(TableMgr.singleton.ValueTable.GetQualityFrame(quality)) as Texture2D);

            //for (int i = 0; i < count; i++)
            {
                GameObject go = GameObject.Instantiate(prefab.gameObject, parent);

                worldPosFrom = new Vector3(worldPosFrom.x, worldPosFrom.y, worldPosTo.z);

                go.transform.position = worldPosFrom;
                go.SetActive(true);
                var vx = BaseRandom.Next(-rangeX, rangeX);
                var vy = BaseRandom.Next(0.0f, rangeY);
                var time = timeToCollect + BaseRandom.Next(0.0f, 0.3f);
                //List<int> vxy = new List<int>() { -1, 1 };
                Vector3[] path = new Vector3[2];
                //path[0] = go.transform.position;
                path[0] = go.transform.position - new Vector3(vx, vy, 0);
                path[1] = worldPosTo;

                //go.transform.DORotate(new Vector3(0, 180, 0), 0.5f).SetLoops(-1, LoopType.Incremental);
                go.transform.DOScale(Vector3.one, time).SetEase(easeType);
                var tweenPath = go.transform.DOPath(path, time, pathType);
                tweenPath.onComplete = () =>
                {
                    SoundEffectManager.Instance["2d"].PlaySoundEffectOneShot("get_card_01");
                    if (doOnFinish != null)
                        doOnFinish();
                    Destroy(go);

                };
                tweenPath.SetEase(easeType);
            }

        }
    }
}