using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TextNumberTween
    {
        Text target;
        IEnumerator tween;
        float duration;

        public TextNumberTween(Text txt, float duration)
        {
            this.target = txt;
            this.duration = duration;
        }

        public void DoTween(int targetNum)
        {
            int curNum;
            if (target == null || !int.TryParse(target.text, out curNum))
                return;

            if (tween != null && target != null)
            {
                target.StopCoroutine(tween);
            }

            tween = DoTweenIE(curNum, targetNum);
            target.StartCoroutine(tween);
        }

        IEnumerator DoTweenIE(int curNum, int targetNum)
        {
            float timer = 0;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                var t = timer / duration;
                target.text = Mathf.Lerp(curNum, targetNum, t).ToString("#");
                yield return null;
            }

            target.text = targetNum.ToString();
        }
    }

}
