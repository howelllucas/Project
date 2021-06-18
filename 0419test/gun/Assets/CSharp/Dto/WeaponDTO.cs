//技能项数据对象
using EZ;
using System.Collections.Generic;

public class WeaponDTO
{
    //当前主武器
    public string curMainWeapon;
    //武器map
    public Dictionary<string, WeaponItemDTO> weaponMap;
    //当前子武器
    public string curSubWeapon;
    //当前随从
    public string curPet;
    //碎片map
    public Dictionary<string, ItemDTO> chipMap;

    public WeaponDTO()
    {
        curMainWeapon = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.DEFAULT_WEAPON).content;
        weaponMap = new Dictionary<string, WeaponItemDTO>();
        chipMap = new Dictionary<string, ItemDTO>();
        curSubWeapon = GameConstVal.EmepyStr;
        curPet = GameConstVal.EmepyStr;
    }
}
