

//道具类型
public class ItemTypeConstVal
{
    public static int BASE_MAIN_WEAPON = 1;
    public static int SUB_WEAPON = 2;
    public static int DROP_MAIN_WEAPON = 3;
    public static int DROP_ASSIST = 4;
    public static int CARRIER = 5;
    public static int SPECIAL = 6;
    public static int GOLD = 7;
    public static int DIAMOND = 8;
    public static int EXP = 9;
    public static int ENERGY = 11;
    public static int MDT = 12;
    public static int WEAPON_CHIP = 13;
    public static int PET = 15;
    public static int SUB_WEAPON_CHIP = 16;
    public static int PET_CHIP = 17;
    public static int HEART = 18;
    public static int NPC_AWARD = 100;
    public static int NPC = 101;
    public static int PHOTO = 102;
    public static int BADGE = 103;

    public static int[] AllItemType = new int[]
    {
            BASE_MAIN_WEAPON,
            SUB_WEAPON,
            DROP_MAIN_WEAPON,
            DROP_ASSIST,
            CARRIER,
            SPECIAL,
            GOLD,
            DIAMOND,
            EXP,
            ENERGY,
            MDT,
            WEAPON_CHIP,
            PET,
            SUB_WEAPON_CHIP,
            PET_CHIP,
            NPC_AWARD,
            NPC,
            HEART,
            PHOTO,
            BADGE,
    };



    public static bool isWeapon(int type)
    {
        if (type == BASE_MAIN_WEAPON || type == SUB_WEAPON || type == PET)
        {
            return true;
        }
        return false;
    }
}