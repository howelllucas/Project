

using EZ.Data;
using UnityEngine;

namespace EZ
{
    public partial class FightUI_Itemtime
    {
        private float m_EffectTime;
        private float m_CurTime;
        private FightUI m_FightUi;
        private string m_Name;
        private string m_KeyName;
		private bool m_NeedUpdate = true;
      
        public void Init(float effectTime, string name, FightUI fightUi, ItemItem itemData)
        {
            m_CurTime = 0;
            m_EffectTime = effectTime;
            m_Name = name;
            m_KeyName = itemData.name;
            m_FightUi = fightUi;
            Itemicon.image.sprite = Global.gApp.gResMgr.LoadSprite(itemData.image_time,EffectConfig.FightUiAtlas);

     		if (m_Name.CompareTo("PlayerEnergyProp") == 0) 	
			{
				m_NeedUpdate = false;
			}
        }
        public void ResetTime(float effectTime, string name,ItemItem itemData)
        {
            if (effectTime > 0)
            {
                m_CurTime = 0;
                if (itemData.name.Equals(m_KeyName))
                {
                    m_EffectTime = Mathf.Max(effectTime,m_EffectTime);
                }
                else
                {
                    m_EffectTime = effectTime;
                }
                m_KeyName = itemData.name;
                m_Name = name;
                Itemicon.image.sprite = Global.gApp.gResMgr.LoadSprite(itemData.image_time,EffectConfig.FightUiAtlas);
            }
            else
            {
                m_FightUi.RemoveCountItem(this);
            }
        }
        private void Update()
        {
       		if ( !m_NeedUpdate ) return; 

            m_CurTime = m_CurTime + BaseScene.GetDtTime();
            if(m_CurTime <= m_EffectTime)
            {
                Timepro.image.fillAmount = 1 - m_CurTime / m_EffectTime;
            }
            else
            {
                m_FightUi.RemoveCountItem(this);
            }
        }

        public void SetPercent(float percent)
        {
            Timepro.image.fillAmount = percent;
        }

        public string GetName()
        {
            return m_Name;
        }
    }
}
