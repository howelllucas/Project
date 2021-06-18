using System;
using UnityEngine;

namespace Game
{
    public class Value_TableItem : TableItem
    {
        public string id;
        public string value;

        public override void CollectFields(FieldPairTable fields)
        {
            fields.addField(this, "id", "id");
            fields.addField(this, "value", "value");
        }
    }

    public class Value_Table : Table<Value_TableItem>
    {
        public int init_weapon_id;              //��ʼ����id
        public int fuse_card_max_count;         //��ս��������
        public float fuse_card_atk_scale;         //�ںϿ��ƹ���������
        public string card_quality_color_1;         //����Ʒ��������ɫ
        public string card_quality_color_2;         //����Ʒ��������ɫ
        public string card_quality_color_3;         //����Ʒ��������ɫ
        public string card_quality_color_4;         //����Ʒ��������ɫ
        public int camp_task_max_count;         //Ӫ�������������
        public int refresh_data_time;           //��ʱˢ��ʱ��
        public int convert_key_gem;             //Կ�׵�ͬ�ڶ�����ʯ
        public int battle_revive_cost;             //ս���������ʯ
        public int box_init_award_count;            //�����ʼ���д���
        public int card_reset_cost;            //���Ƶȼ����û���
        public int open_box_ten_cost;            //����ʮ���黨��

        public float card_productionBonus_rarity_1; //	�������ʼӳ�ϡ��Ʒ��ϵ��
        public float card_productionBonus_rarity_2; //	�������ʼӳ�ʷʫƷ��ϵ��
        public float card_productionBonus_rarity_3; //	�������ʼӳɴ�˵Ʒ��ϵ��
        public float card_productionBonus_rate; //	�������ʼӳɳɳ���
        public int card_levelup_cost_k1;  //	�����������Ľ��ϵ��K1
        public int card_levelup_cost_k2;  //	�����������Ľ��ϵ��K2
        public int card_levelup_cost_k3;  //	�����������Ľ��ϵ��K3
        public int card_levelup_cost_k4;  //	�����������Ľ��ϵ��K4
        public float player_levelup_hp_k1;  //	��ɫ����ֵ�ɳ�ϵ��K1
        public float player_levelup_hp_k2;  //	��ɫ����ֵ�ɳ�ϵ��K2
        public float building_production_k1;    //	���ʲ�������ֵϵ��K1
        public float building_cost_k1;          //  ����ʵ����������ϵ��K1
        public float card_dps;  //	����dps����ֵ����
        public float card_rarity_capability_1;  //	����Ʒ��ǿ��ϵ��_ϡ��
        public float card_rarity_capability_2;  //	����Ʒ��ǿ��ϵ��_ʷʫ
        public float card_rarity_capability_3;  //	����Ʒ��ǿ��ϵ��_��˵
        public float combat_capability; //	ս��������ϵ��
        public float card_levelup_dpsrate;  //	��������DPS�ɳ���
        public float card_star_dpsrate; //	��������DPS�ɳ���
        public float camp_combat;	//	Ӫ��ս����Ѫ��ϵ��
        public string box_award_gun;	//	�鱦���ǰN��ָ������gunType
        public float player_levelup_production_k1;//��ɫ�������ʼӳɳɳ�ϵ��
        public int[] BoxAwardGunArr { get; private set; }
        public int[] CampLevel_1Arr { get; private set; }//Ӫ��1ָ�������ؿ�
        public int[] CampLevel_2Arr { get; private set; }//Ӫ��2ָ�������ؿ�

        public Value_Table()
            : base(new System.Collections.Generic.Dictionary<string, Value_TableItem>())
        {

        }

        protected override bool FillData()
        {
            base.FillData();

            init_weapon_id = ParseInt("init_weapon_id", 5);
            fuse_card_max_count = ParseInt("fuse_card_max_count", 5);
            fuse_card_atk_scale = ParseFloat("fuse_card_atk_scale", 0);
            card_quality_color_1 = ParseString("card_quality_color_1", "");
            card_quality_color_2 = ParseString("card_quality_color_2", "");
            card_quality_color_3 = ParseString("card_quality_color_3", "");
            card_quality_color_4 = ParseString("card_quality_color_4", "");
            camp_task_max_count = ParseInt("camp_task_max_count", 3);
            refresh_data_time = ParseInt("refresh_data_time", 8);
            convert_key_gem = ParseInt("convert_key_gem", 10);
            battle_revive_cost = ParseInt("battle_revive_cost", 10);
            box_init_award_count = ParseInt("box_init_award_count", 10);
            card_reset_cost = ParseInt("card_reset_cost", 10);
            open_box_ten_cost = ParseInt("open_box_ten_cost", 10);

            card_productionBonus_rarity_1 = ParseFloat("card_productionBonus_rarity_1", 1);
            card_productionBonus_rarity_2 = ParseFloat("card_productionBonus_rarity_2", 1);
            card_productionBonus_rarity_3 = ParseFloat("card_productionBonus_rarity_3", 1);
            card_productionBonus_rate = ParseFloat("card_productionBonus_rate", 1);
            card_levelup_cost_k1 = ParseInt("card_levelup_cost_k1", 1);
            card_levelup_cost_k2 = ParseInt("card_levelup_cost_k2", 1);
            card_levelup_cost_k3 = ParseInt("card_levelup_cost_k3", 1);
            card_levelup_cost_k4 = ParseInt("card_levelup_cost_k4", 1);
            player_levelup_hp_k1 = ParseFloat("player_levelup_hp_k1", 1);
            player_levelup_hp_k2 = ParseFloat("player_levelup_hp_k2", 1);
            building_production_k1 = ParseFloat("building_production_k1", 1);
            //building_cost_k1 = ParseFloat("building_cost_k1", 2);
            card_dps = ParseFloat("card_dps", 1);
            card_rarity_capability_1 = ParseFloat("card_rarity_capability_1", 1);
            card_rarity_capability_2 = ParseFloat("card_rarity_capability_2", 1);
            card_rarity_capability_3 = ParseFloat("card_rarity_capability_3", 1);
            combat_capability = ParseFloat("combat_capability", 1);
            card_levelup_dpsrate = ParseFloat("card_levelup_dpsrate", 1);
            card_star_dpsrate = ParseFloat("card_star_dpsrate", 1);
            camp_combat = ParseFloat("camp_combat", 1);

            box_award_gun = ParseString("box_award_gun", "");
            player_levelup_production_k1 = ParseFloat("player_levelup_production_k1", 0f);
            string[] parser = new string[] { "|" };
            BoxAwardGunArr = TableItem.SpliteToInts(box_award_gun, parser);

            string campLevel_1 = ParseString("campLevel_1", "");
            string campLevel_2 = ParseString("campLevel_2", "");

            CampLevel_1Arr = TableItem.SpliteToInts(campLevel_1, parser);
            CampLevel_2Arr = TableItem.SpliteToInts(campLevel_2, parser);

            return true;
        }

        int ParseInt(string id, int idefault)
        {
            string value = "";
            try
            {
                value = this[id].value;
                return int.Parse(value);
            }
            catch (Exception)
            {
                Debug.LogErrorFormat("Value Table Pirse  error. id:{0}.value:{1}", id, value);
                return idefault;
            }
        }
        float ParseFloat(string id, float idefault)
        {
            string value = "";
            try
            {
                value = this[id].value;
                return float.Parse(value);
            }
            catch (Exception)
            {
                Debug.LogErrorFormat("Value Table Pirse  error. id:{0}.value:{1}", id, value);
                return idefault;
            }
        }
        string ParseString(string id, string idefault)
        {
            string value = "";
            try
            {
                value = this[id].value;
                return value;
            }
            catch (Exception)
            {
                Debug.LogErrorFormat("Value Table Pirse  error. id:{0}.value:{1}", id, value);
                return idefault;
            }
        }
        ////////////////////////////////////////////////////////////
        ///
        public string GetQualityName(int quality)
        {
            switch ((CardQualityType)quality)
            {
                case CardQualityType.RARE:
                    return LanguageMgr.GetText("CardPage_Text_Rare");
                case CardQualityType.EPIC:
                    return LanguageMgr.GetText("CardPage_Text_Epic");
                case CardQualityType.LEGEND:
                    return LanguageMgr.GetText("CardPage_Text_Legend");
            }

            return "";
        }

        public string GetCardQualityFrame(int quality)
        {
            switch ((CardQualityType)quality)
            {
                case CardQualityType.RARE:
                    return "ResourcesSprites/Card/card_bg1";
                case CardQualityType.EPIC:
                    return "ResourcesSprites/Card/card_bg2";
                case CardQualityType.LEGEND:
                    return "ResourcesSprites/Card/card_bg3";
            }

            return "";
        }

        public string GetCardQualityEquipFrame(int quality)
        {
            switch ((CardQualityType)quality)
            {
                case CardQualityType.RARE:
                    return "ResourcesSprites/Card/card_equip_frame1";
                case CardQualityType.EPIC:
                    return "ResourcesSprites/Card/card_equip_frame2";
                case CardQualityType.LEGEND:
                    return "ResourcesSprites/Card/card_equip_frame3";
            }

            return "";
        }

        public string GetCardQualityEffectImg(int quality)
        {
            switch ((CardQualityType)quality)
            {
                case CardQualityType.RARE:
                    return "ResourcesSprites/Card/card_effect1";
                case CardQualityType.EPIC:
                    return "ResourcesSprites/Card/card_effect2";
                case CardQualityType.LEGEND:
                    return "ResourcesSprites/Card/card_effect3";
            }

            return "";
        }

        public string GetChipQualityFrame(int quality)
        {
            switch ((CardQualityType)quality)
            {
                case CardQualityType.RARE:
                    return "Card_Chip_blu";
                case CardQualityType.EPIC:
                    return "Card_Chip_zi";
                case CardQualityType.LEGEND:
                    return "Card_Chip_cheng";
            }

            return "";
        }

        public string GetQualityCardBack(int quality)
        {
            switch ((CardQualityType)quality)
            {
                case CardQualityType.RARE:
                    return "CharUI_Quality2";
                case CardQualityType.EPIC:
                    return "CharUI_Quality3";
                case CardQualityType.LEGEND:
                    return "CharUI_Quality4";
            }

            return "";
        }

        public string GetQualityBottomBack(int quality)
        {
            switch ((CardQualityType)quality)
            {
                case CardQualityType.RARE:
                    return "Bottom_blu";
                case CardQualityType.EPIC:
                    return "Bottom_zi";
                case CardQualityType.LEGEND:
                    return "Bottom_cheng";
            }

            return "";
        }

        public string GetRewardIcon(int quality)
        {
            switch ((CardQualityType)quality)
            {
                case CardQualityType.RARE:
                    return "Card_Quality2";
                case CardQualityType.EPIC:
                    return "Card_Quality3";
                case CardQualityType.LEGEND:
                    return "Card_Quality4";
            }

            return "";
        }

        public string GetQualityColorString(int quality)
        {
            string format = " <color={1}>{0}</color> ";
            string color = null;
            switch ((CardQualityType)quality)
            {
                case CardQualityType.RARE:
                    color = card_quality_color_2;
                    break;
                case CardQualityType.EPIC:
                    color = card_quality_color_3;
                    break;
                case CardQualityType.LEGEND:
                    color = card_quality_color_4;
                    break;
            }

            return color == null ? "" : string.Format(format, "{0}", color);
        }

        public Color GetQualityColor(int quality)
        {
            string colorStr = null;
            switch ((CardQualityType)quality)
            {
                case CardQualityType.RARE:
                    colorStr = card_quality_color_2;
                    break;
                case CardQualityType.EPIC:
                    colorStr = card_quality_color_3;
                    break;
                case CardQualityType.LEGEND:
                    colorStr = card_quality_color_4;
                    break;
            }

            Color color;
            if (colorStr != null && ColorUtility.TryParseHtmlString(colorStr, out color))
            {
                return color;
            }
            return Color.black;
        }

        
        public string GetTargetName(int target)
        {
            switch ((TargetType)target)
            {
                case TargetType.Front:
                    return LanguageMgr.GetText("Attack_Target_1");
                case TargetType.Random:
                    return LanguageMgr.GetText("Attack_Target_2");
            }

            return "-";
        }

    
    }
}
