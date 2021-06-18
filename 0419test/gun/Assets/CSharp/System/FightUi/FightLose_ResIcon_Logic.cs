using EZ.Data;
using UnityEngine;

namespace EZ
{

    public partial class FightLose_ResIcon
    {
        float m_CurTime = 0;
        float m_DelayTime = 0;
        public void Init(float delayTime,int count, ItemItem itemConfig)
        {
            m_DelayTime = delayTime;
            Content.gameObject.SetActive(false);
            m_Icon.image.sprite = Resources.Load(itemConfig.image_grow, typeof(Sprite)) as Sprite;
            Num.text.text = count.ToString();
        }
        private void Update()
        {
            m_CurTime += 0.033f;
            if (m_CurTime > m_DelayTime)
            {
                m_CurTime = -99999999;
                Content.gameObject.SetActive(true);
                enabled = false;
            }
        }

        public void SetDelayTimeZero()
        {
            m_DelayTime = 0;
        }
    }
}
