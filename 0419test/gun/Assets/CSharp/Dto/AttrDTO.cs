//杂项数据对象
using EZ;
using EZ.Data;
using System;
using System.Collections.Generic;

public class AttrDTO
{
    //当前角色等级
    public int level;
    //当前角色经验
    public double exp;
    //金币
    public double gold;
    //钻石
    public double diamond;
    //能量
    public int energy;
    //上次能量记时时间
    public double lastEnergyTime;
    //MDT 狗牌
    public double MDT;
    //红心
    public double redHeart;

    public AttrDTO() {
        level = 1;
        GeneralConfigItem limitConfig = Global.gApp.gGameData.GeneralConfigData.Get(GeneralConfigConstVal.INITIAL_ENERGY);
        energy = int.Parse(limitConfig.content);
    }
}
