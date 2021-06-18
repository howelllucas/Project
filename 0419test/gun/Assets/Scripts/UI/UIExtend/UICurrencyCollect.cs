using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Tool;
using Game.Audio;

namespace Game.UI
{
    public class UICurrencyCollect : MonoBehaviour
    {
        [Header("[Boomb Param]")]
        public float range; //炸出范围
        public Vector2 timeToCollect; //飞去固定位置的时间
        [Header("[UnityObj]")]
        public GameObject effectObj;
        public Image prefab; //复制的预设
        public Transform parent;
        public AnimationCurve weightCurveBoom = AnimationCurve.Linear(0, 1, 1, 1);
        public AnimationCurve weightCurveFly = AnimationCurve.Linear(0, 1, 1, 1);
        public AnimationCurve tweenCurveBoom = AnimationCurve.Linear(0, 0, 1, 1);
        public AnimationCurve tweenCurveFly = AnimationCurve.Linear(0, 0, 1, 1);
        public AnimationCurve[] trackCurves;


        //--------------- status --------------
        public Sprite Sprite
        {
            get { return prefab.sprite; }
            set { prefab.sprite = value; }
        }

        public string SoundEffectName { get; set; }

        //--------------- private --------------
        private ScaleTween flyEndScale;
        private List<Pos2DTrackTweenMix> buffer = new List<Pos2DTrackTweenMix>();


        private void Start()
        {
            prefab.gameObject.SetActive(false);
        }
        //--------------- function ------------

        public void BoomToCollectCurrency(CurrencyCountLevel countLevel, Vector2 worldPosFrom, Vector2 worldPosTo, System.Action doOnFinish)
        {
            int coinCount = (int)countLevel;
            BoomToCollectCurrency(coinCount, worldPosFrom, worldPosTo, doOnFinish);
        }

        public void BoomToCollectCurrency(int coinCount, Vector2 worldPosFrom, Vector2 worldPosTo, System.Action doOnFinish)
        {
            //effectObj.gameObject.SetActive(true);

            //SoundEffectManager.Instance.PlaySoundEffectOneShot("gold_boom");

            Vector2 posFrom = prefab.transform.position;
            if (worldPosFrom != Vector2.zero)
                posFrom = parent.InverseTransformPoint(worldPosFrom);
            Vector2 posTo = parent.InverseTransformPoint(worldPosTo);
            Vector2 flyDir = posTo - posFrom;
            flyDir.Normalize();

            int finishFlyCount = 0;

            for (int i = 0; i < coinCount; i++)
            {
                Vector2 bombTo = Random.insideUnitCircle;

                int flyCurveIndex = 1;
                if (Mathf.Abs(Vector2.Dot(flyDir, bombTo)) <= 0.707f)//cos45
                {
                    Vector3 dir = Vector3.Cross(flyDir, bombTo);
                    if (dir.z < 0)
                        flyCurveIndex = 0;
                    else
                        flyCurveIndex = 2;
                }


                bombTo = bombTo * range + posFrom;
                float duration = Random.Range(timeToCollect.x, timeToCollect.y);

                var coin = CreateCoin(posFrom);
                coin.mixDatas[0].from = posFrom;
                coin.mixDatas[0].to = bombTo;
                coin.mixDatas[0].duration = duration;
                //coin.mixDatas[0].timeCurve = tweenCurveBoom;
                //coin.mixDatas[0].weightCurve = weightCurveBoom;

                coin.mixDatas[1].from = posFrom;
                coin.mixDatas[1].to = posTo;
                coin.mixDatas[1].duration = duration;
                coin.mixDatas[1].trackCurve = trackCurves[flyCurveIndex];
                //coin.mixDatas[1].timeCurve = tweenCurveFly;
                //coin.mixDatas[1].weightCurve = weightCurveFly;
                var flyEndScale = this.flyEndScale;
                coin.Play(() =>
                {
                    coin.gameObject.SetActive(false);
                    finishFlyCount++;
                    if (flyEndScale != null)
                        flyEndScale.Play();
                    if (finishFlyCount >= coinCount)
                    {
                        doOnFinish?.Invoke();
                        //effectObj.SetActive(false);
                        //if (!string.IsNullOrEmpty(SoundEffectName))
                        //{
                        //    SoundEffectManager.Instance.PlaySoundEffectOneShot(SoundEffectName);
                        //}
                    }
                    SoundEffectManager.Instance["2d"].PlaySoundEffectOneShot("get_gold_02");
                });

            }
        }

        public void AddCollectScaleTween(Transform target)
        {
            ScaleTween scaleTween = target.GetComponent<ScaleTween>();
            if (scaleTween == null)
            {
                scaleTween = target.gameObject.AddComponent<ScaleTween>();
                scaleTween.from = new Vector3(1, 1, 1);
                scaleTween.to = new Vector3(1.2f, 1.2f, 1.2f);
                scaleTween.pingPang = true;
                scaleTween.duration = 0.05f;
            }

            flyEndScale = scaleTween;
        }

        public void RemoveCollectScaleTween()
        {
            flyEndScale = null;
        }

        private Pos2DTrackTweenMix CreateCoin(Vector2 initPos)
        {
            for (int i = 0; i < buffer.Count; i++)
            {
                if (!buffer[i].gameObject.activeSelf)
                {
                    var img = buffer[i].GetComponent<Image>();
                    img.sprite = Sprite;
                    var rect = img.transform as RectTransform;
                    var prefabRect = prefab.transform as RectTransform;
                    rect.sizeDelta = prefabRect.sizeDelta;
                    //img.SetNativeSize();
                    buffer[i].gameObject.SetActive(true);
                    buffer[i].transform.localPosition = initPos;
                    return buffer[i];
                }
            }

            GameObject obj = GameObject.Instantiate(prefab.gameObject);
            obj.SetActive(true);
            obj.transform.SetParent(parent, false);
            obj.transform.localPosition = initPos;
            Pos2DTrackTweenMix tween = obj.AddComponent<Pos2DTrackTweenMix>();
            tween.local = true;
            tween.mixDatas = new Pos2DTrackTweenMix.Pos2DMixData[2];
            tween.mixDatas[0] = new Pos2DTrackTweenMix.Pos2DMixData()
            {
                timeCurve = tweenCurveBoom,
                trackCurve = AnimationCurve.Linear(0, 0, 1, 1),
                weightCurve = weightCurveBoom,
            };
            tween.mixDatas[1] = new Pos2DTrackTweenMix.Pos2DMixData()
            {
                timeCurve = tweenCurveFly,
                weightCurve = weightCurveFly,
            };

            buffer.Add(tween);
            return tween;
        }

        /// <summary>
        /// 数量级别
        /// </summary>
        public enum CurrencyCountLevel
        {
            OnlyOne = 1,
            Little = 3,
            Some = 8,
            Normal = 12,
            Much = 16,
            VeryMuch = 20
        }
    }
}