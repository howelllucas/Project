using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class CombatAttr
    {
        public enum CombatAttrType
        {
            Attack = 1,                  //	攻击值提升
            Attack_Add = 2,              //	攻击百分比提升
            Gold_Add = 3,              //	金币百分比提升
            MoveSpeed = 4,              //	移动速度提升
            Crit_Rate = 5,              // 暴击提升
            Crit_Damage = 6,            // 暴击伤害提升
            Dodge = 7,                  // 闪避提升
            Attack_Speed = 8,           //	攻击速度提升
            Life_Add = 9,               //	血量百分比提升


            CombatAttrType_Count,
        }

        public class PropertyData
        {
            private bool mNeedRecalc;
            private float mCurrentValue = 0.0f;

            private float mInitValue = 0.0f;
            private float mPropertyRevise = 0.0f;

            public PropertyData(float initValue)
            {
                mInitValue = initValue;
                mCurrentValue = initValue;
                mNeedRecalc = false;
            }

            public float GetValue()
            {
                if (mNeedRecalc)
                {
                    mCurrentValue = mInitValue + mPropertyRevise;
                    mNeedRecalc = false;
                }

                return mCurrentValue;
            }

            public void Revise(float value)
            {
                mPropertyRevise += value;
                mNeedRecalc = true;
            }

            public void Set(float value)
            {
                mInitValue = value;
                mNeedRecalc = true;
            }

        }

        private Dictionary<CombatAttrType, PropertyData> mProperties = new Dictionary<CombatAttrType, PropertyData>();

        public bool Init()
        {

            for (CombatAttrType i = CombatAttrType.Attack; i < CombatAttrType.CombatAttrType_Count; ++i)
            {   
                switch(i)
                {
                    case CombatAttrType.Attack:
                    case CombatAttrType.Attack_Add:
                    case CombatAttrType.Gold_Add:
                    case CombatAttrType.Life_Add:
                        {
                            mProperties.Add(i, new PropertyData(0));
                        }
                        break;
                    case CombatAttrType.MoveSpeed:
                    case CombatAttrType.Crit_Rate:
                    case CombatAttrType.Crit_Damage:
                    case CombatAttrType.Dodge:
                    case CombatAttrType.Attack_Speed:
                        {
                            mProperties.Add(i, new PropertyData(1));
                        }
                        break;
                }
                
            }

            return true;
        }

        public float GetValue(CombatAttrType propertyID)
        {
            PropertyData data;
            if (!mProperties.TryGetValue(propertyID, out data))
            {
                return 0.0f;
            }
            return data.GetValue();
        }

        public void Revise(CombatAttrType propertyID, float value)
        {
            if (!mProperties.ContainsKey(propertyID))
            {
                return;
            }
            mProperties[propertyID].Revise(value);
        }

        
    }
}
