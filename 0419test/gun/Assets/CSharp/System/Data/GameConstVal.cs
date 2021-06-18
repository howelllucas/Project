using UnityEngine;

namespace EZ
{
    public enum MWeapon
    {
        Atk = 0,
    }

    public enum GameWeapon
    {
        weapon_ak47,
        weapon_laser,
        weapon_laserX,
        weapon_rpg,
        weapon_scatter,
        weapon_firegun,
        weapon_mg,
        S_FireTurret,
        Car_001,
        weapon_howitzer,
        weapon_mine,
        weapon_awp,
        weapon_shotgun,
        weapon_elecgun,
        weapon_bouncegun,
        weapon_icecrossbow,
        weapon_firecrossbow,
        weapon_elecrossbow,
        weapon_boomerang,
        Car_003,
        weapon_tornado,
        Maniac,
        Robot,
        weaponChip,
    }

    public enum GameProp
    {
        GoldProp,
        CureProp,
        FireGunProp,
        LaserXProp,
        LaserProp,
        MedicalKitProp,
        RPGProp,
        ScatterProp,
        ShieldProp,
        CarProp,
        BuffProp,
        MGProp,
        FireCountTimesProp,
        FireCountTimesXProp,
        FireCurveIncProp,
        FireCurveIncXProp,
        FireSpeedProp,
        FireSpeedXProp,
        CollectPropA,
        AtkProp,
        AtkXProp,
        MoveSpeedProp,
        MoveSpeedXProp,
        NormalProp,
        CollectProp,
        PlayerEnergyProp,
        HowitzerProp,
        MineProp,
        AwpProp,
        ShotGunProp,
        ElecGunProp,
        BounceGunProp,
        IcecrossbowGunProp,
        FireCrossbowGunProp,
        EleCrossbowGunProp,
        NanoBoostProp,
        BoomerangGunProp,
        Car003Prop,
        ManiacProp,
        RobotProp,
        WeaponChipProp,
        DropRes,
    }
    public class GameConstVal
    {
        public static string LanguagePre = "tips:";
        public static string EmepyStr = "";
        public static string WAK47 = "weapon_ak47";
        public static string WLaser = "weapon_laser";
        public static string WLaserX = "weapon_laserX";
        public static string WAwp = "weapon_awp";
        public static string WRPG = "weapon_rpg";
        public static string WScatter = "weapon_scatter";
        public static string WFireGun = "weapon_firegun";
        public static string WShotGun = "weapon_shotgun";
        public static string WElecGun = "weapon_elecgun";
        public static string WTornadoGun = "weapon_tornado";
        public static string WPartriotGun = "weapon_patriot";
        public static string WBubbleGun = "weapon_bubble";
        #region 副武器 
        public static string WWeapon_Mine = "weapon_mine";
        public static string WWeapon_Traps = "weapon_traps";
        public static string WWeapon_Mushroom = "weapon_mushroom";
        public static string WWeapon_Charmroom = "weapon_charmroom";
        public static string WHowitzer = "weapon_howitzer";
        public static string WBoomerangeGun = "weapon_boomerang";
        public static string WWeaponElecMine = "weapon_elecmine";
        public static string WWeaponHgrenade = "weapon_hgrenade";
        public static string WWeaponDestroyer = "weapon_destroyer";
        public static string WWeaponIcegrenade = "weapon_icegrenade";
        public static string WWeaponBoomcrossbow = "weapon_boomcrossbow";
        #endregion
        public static string WBounceGun = "weapon_bouncegun";
        public static string WMG = "weapon_mg";
        public static string WSFireTurret = "S_FireTurret";
        public static string WSFireTurret002 = "S_Turret002";
        public static string WSFireTurret003 = "S_Turret003";
        public static string WSFireTurret005 = "S_Turret005";
        public static string WSFireTurret006 = "S_Turret006";
        public static string WSFireBoomTurret = "S_BoomTurret";
        public static string WTank_carrier = "Tank_carrier";
        public static string WTank_turret = "Tank_turret";
        public static string WCarrier = "Carrier";

        public static string WIcecrossbowGun = "weapon_icecrossbow";
        public static string WFirecrossbowGun = "weapon_firecrossbow";
        public static string WElecrossbowGun = "weapon_elecrossbow";
        public static string WCrossbowGun = "weapon_crossbow";
        // 机关
        public static string WOfficeSaw = "Office_saw";
        public static string WOfficePick = "Office_prick";
        public static string WOfficeBonfire = "Office_bonfire";
        public static string WOfficeEleTrom = "Office__electrom";
        // 宠物
        public static string WRobot = "Robot";
        public static string WUAV = "UAV";
        public static string WUAV002 = "UAV002";
        public static string WDog = "Dog";
        public static string WBeetle = "Beetle";
        public static string WManiac = "Maniac";
        public static string WDeadthWalker = "DeathWalker";
        // 碎片
        public static string WWeaponChip = "weaponChip";


        //被动技能
        public static string SExAtk = "skill_exatk";
        public static string SExGold = "skill_exgold";
        public static string SExHp = "skill_exhp";
        public static string SExSpeed = "skill_exspeed";
        public static string SExCrit = "skill_excrit";
        public static string SExBuffTime = "skill_exbufftime";
        public static string SExDodge = "skill_exdodge";
        public static string SExBossHarm = "skill_exbossharm";
        public static string SExHitTime = "skill_exhittime";

        public static string PCarProp = "CarProp";
        public static string PGoldProp = "GoldProp";
        public static string PCureProp = "CureProp";
        public static string PFireGunProp = "FireGunProp";
        public static string PLaserXProp = "LaserXProp";
        public static string PLaserProp = "LaserProp";
        public static string PMedicalKitProp = "MedicalKitProp";
        public static string PRPGProp = "RPGProp";
        public static string PScatterProp = "ScatterProp";
        public static string PShieldProp = "ShieldProp";

        public static string Level = "_level";
        public static string State = "_state";
        public static string Cost = "_cost";
        public static string RoleLevel = "role_level";
        public static string RoleExp = "role_exp";
        public static string RoleGold = "gold";
        public static string RoleDiamond = "diamond";

        // tag ----------
        public static string GoldenBox = "GoldenBox";
        public static string NPCTag = "NPC";
        public static string CampBuilding = "CampBuilding";
        public static string MainRoleTag = "MainRole";
        public static string BillboardTag = "Billboard";
        public static string CampRedHeartTag = "RedHeart";
        public static string MonsterTag = "Monster";
        public static string MapTag = "StaticMap";
        public static string MainRoleBulletTag = "MainBullet";
        public static string MonsterBulletTag = "MonsterBullet";
        public static string PropTag = "Prop";
        public static string CarrierTag = "Carrier";
        public static string ShieldTag = "Shield";
        public static string DamageRangeTag = "MainRoleDamageRange";
        public static string MonsterActiveTag = "MonsterActNode";
        public static string LockEffect = "LockEffect";
        public static string ElecEffect = "ElecEffect";
        public static string FireNode2 = "FireNode2";
        public static string EffectNode = "Effect";
        public static string FirePoint = "FirePoint";
        public static string FirePoint1 = "FirePoint1";
        public static string FirePoint2 = "FirePoint2";
        public static string FireNode = "FireNode";
        public static string BodyNode = "BodyNode";
        public static string HpNode = "HpNode";
        public static string RotNode = "RotNode";
        public static string NameNode = "NameNode";
        public static string ElementNode = "ElementNode";
        public static string ParticleName = "01";
        public static string ExplodeCollision = "ExplodeCollision";
        public static int DefaultVal = 0;
        public static int FlyMonsterLayer = 10;
        public static int CrossMonsterLayer = 11;
        public static int StaticMapLayer = 12;
        public static int MainRoleLayer = 13;
        public static int MonsterLayer = 14;
        public static int OnlyMainRoleLayer = 19;
        public static int BossLayer = 20;
        public static int CampStaticMap = 23;
        //go name
        public static string Npc_drstrange = "Npc_drstrange";
        public static string Npc_OldWoman = "Npc_oldwoman";
        //
        public static int ShadowCullingMask = 7364864;
        public static int NoShadowCullingMask = 8448;
        // act name
        public static string ShowBossEnd = "showbossend";
        public static string ShowBoss = "showboss";
        public static string RoleDead = "roledead";
        public static string RoleReborn = "rolereborn";

        public static string Close = "close";
        public static string Death = "death";
        public static string Skill01 = "skill01";
        public static string Attack = "attack";
        public static string GunAtk = "atk";
        public static string Skill02 = "skill02";
        public static string Run = "run";
        public static string Shoot = "shoot";
        public static string Idle_Deploy = "idle_deploy";
        public static string Born = "born";
        public static string Show = "show";
        public static string Wait = "wait";
        public static string Fire = "fire";
        public static string Idle = "idle";
        public static string IdleUi = "idle_ui";
        public static string Run_left = "run_left";
        public static string Run_back = "run_back";
        public static string Run_right = "run_right";
        public static string Greet = "greet";
        public static string Cheer = "cheer";
        public static string Cry = "cry";
        public static string SpeedSymbolStr = "SpeedSymbol";
        public static int LasserMask = ~(1 << CampStaticMap);
        // 
        public static string Horizontal = "Horizontal";
        public static string Vertical = "Vertical";

        public static string TextMeshName = "Name";
        public static string Shader_BlinkSpeed = "_Speed";

        public static string BubbleEffect = "bubble_hit";
        public static string MainTex_ST = "_MainTex_ST";
    }
}




