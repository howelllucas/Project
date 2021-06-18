using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class CombatAttr
    {
        public enum CombatAttrType
        {
            Attack = 1,                  //	����ֵ����
            Attack_Add = 2,              //	�����ٷֱ�����
            Gold_Add = 3,              //	��Ұٷֱ�����
            MoveSpeed = 4,              //	�ƶ��ٶ�����
            Crit_Rate = 5,              // ��������
            Crit_Damage = 6,            // �����˺�����
            Dodge = 7,                  // ��������
            Attack_Speed = 8,           //	�����ٶ�����
            Life_Add = 9,               //	Ѫ���ٷֱ�����


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
