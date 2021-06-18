using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Serialization;

namespace Game.Tool
{
    public class SpriteAnim : ScriptAnimBase
    {
        [FormerlySerializedAs("m_target")]
        public Image target;
        [FormerlySerializedAs("m_sprites")]
        public Sprite[] sprites;

        private int currentIndex;

        protected override void Awake()
        {
            base.Awake();
            if (target == null)
                target = GetComponent<Image>();
        }

        public void ForceToStart()
        {
            SetSprite(0);
        }

        public void ForceToEnd()
        {
            SetSprite(sprites.Length - 1);
        }

        private void SetSprite(int index)
        {
            if (target == null || sprites.Length < 1)
                return;
            currentIndex = Mathf.Clamp(index, 0, sprites.Length - 1);
            if (!dir)
                currentIndex = sprites.Length - 1 - currentIndex;
            target.sprite = sprites[currentIndex];
        }

        protected override void DoOnPlay()
        {
            base.DoOnPlay();
            if (sprites.Length <= 1)
            {
                Stop();
                funcOnFinish?.Invoke();
            }
        }

        protected override void DoOnUpdate()
        {
            if (target == null)
                return;
            float t = NormalizeTime;
            int index = (int)(t * sprites.Length);

            if (index != currentIndex)
            {
                SetSprite(index);
            }
        }

        protected override void ResetParam()
        {
            base.ResetParam();
            ForceToStart();

            currentIndex = 0;
            funcOnFinish = null;
        }
    }
}