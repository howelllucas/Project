
using System.Collections.Generic;
namespace EZ
{
    public class UIInfo
    {
        public string ResPath;
        public string TouchMaskName = null;
        public int Order = 10;
        public bool NoTouchMask = false;
        public bool AddRoot = false;
        public bool TouchEmptyClose = false;
        public bool NetOpen = false;
        public bool NetOpenShowLoading = true;
        public int Layer = 0;//根据此数值判断当前UI是否在顶层
    }
    public class Wndid
    {
        public static string TaskStateNode = "Prefabs/UI/TaskStateNode";
        public static string WorkerRewardNode = "Prefabs/UI/WorkerRewardNode";
        public static string GuardRewardNode = "Prefabs/UI/GuardRewardNode";
        public static string FightNpcProgress = "Prefabs/UI/FightNpcProgress";
        public static string ReadHeart = "Prefabs/UI/ReadHeart";
        public static string DialogUI = "Prefabs/UI/DialogNode";
        public static string CommonPanel = "CommonPanel";
        public static string MainPanel = "MainPanel";
        public static string LogoPanel = "LogoPanel";
        public static string ThanksUI = "ThanksUI";
        public static string SuperGunUI = "SuperGunUI";
        public static string FightPanel = "FightUI";
        public static string FightWinPanel = "FightWinUI";
        public static string FightRespawnPanel = "FightRespawnUI";
        public static string FightLosePanel = "FightLoseUI";
        public static string FightResultPanel = "FightResultUI";
        public static string FightPausePanel = "FightPauseUI";
        public static string WeaponPanel = "WeaponUI";
        public static string ShopPanel = "ShopUI";
        public static string CharacterLevelPanel = "CharUILevel";
        public static string CharacterSkillPanel = "CharUISkill";
        public static string SupportPanel = "SupportUI";
        public static string FightPlotUI = "FightPlotUI";
        public static string ConfirmPanel = "ConfirmUI";
        public static string EnergyShowPanel = "EnergyShow";
        public static string GameConfigPanel = "GameConfigUI";
        public static string RewardEffectUi = "RewardEffectUi";
        public static string GetRewardUI = "GetRewardUI";
        public static string NextDayWeaponUi = "NextDayWeaponUi";
        public static string LevelDetail = "LevelDetailUI";
        public static string OfflinePanel = "OfflineUI";
        public static string SevenDayPanel = "SevenDayUI";
        public static string DebugPanel = "DebugUI";
        public static string SkillUI = "SkillUI";
        public static string SkillUpUI = "SkillUpUI";
        public static string NoMDTUI = "NoMDTUI";
        public static string GetMoneyUI = "GetMoneyUI";
        public static string OpenBoxUI = "OpenBoxUI";
        public static string EvaluateUI = "EvaluateUI";
        public static string CampsiteUI = "CampsiteUI";
        public static string FirstPurchaseUI = "FirstPurchaseUI";

        public static string TaskDetailsUi = "TaskDetailsUi";
        public static string FightNpcPlotUi = "FightNpcPlotUi";

        public static string OpenMixBoxUI = "OpenMixBoxUI";
        public static string LoadingUI = "LoadingUI";

        public static string LanguageConfigUI = "LanguageConfigUI";

        public static string TimeGiftUI1 = "TimeGiftUI1";
        public static string TimeGiftUI2 = "TimeGiftUI2";
        public static string CheckNetUI = "CheckNetUI";

        public static string WeaponRaiseUI = "WeaponRaiseUI";
        public static string CampRecycleUi = "CampRecycleUi";
        public static string ExchangeConfirmUI = "ExchangeConfirmUI";
        public static string CampMatBag = "CampMatBag";
        public static string CampMatRecycleDetailUi = "CampMatRecycleDetailUi";
        public static string CampDetailUI = "CampDetailUI";
        public static string WeaponRaiseSucess_UI = "WeaponRaiseSucess_UI";
        public static string CampResourcesDetailUI = "CampResourcesDetailUI";
        public static string ExchangeSuccessUI = "ExchangeSuccessUI";
        public static string ConfirmEnterPassUI = "ConfirmEnterPassUI";
        public static string CampTaskDetails = "CampTaskDetails";
        public static string ConfirmBranchPassUI = "ConfirmBranchPassUI";
        public static string DialogueUI = "DialogueUI";
        public static string OpeningUI = "OpeningUI";

        public static string ConfirmLookAdUI = "ConfirmLookAdUI";
        public static string CampBUFF = "CampBUFF";
        public static string Bubble = "Bubble";
        public static string CampToUpgrade = "CampToUpgrade";
        public static string CampBadgeSuccessUI = "CampBadgeSuccessUI";
        public static string CampBadgeUI = "CampBadgeUI";
        public static string CampSwitchUI = "CampSwitchUI";

        public static string HomeUI = "HomeUI";
        public static string GunUI = "GunUI";
        public static string GunInfoUI = "GunInfoUI";
        public static string GunListUI = "GunListUI";
        public static string GunStarUpUI = "GunStarUpUI";
        public static string GunSkillLvUpUI = "GunSkillLvUpUI";
        public static string GunSkillInfoUI = "GunSkillInfoUI";
        public static string GunStarUpShowUI = "GunStarUpShowUI";
        public static string GunChipUI = "GunChipUI";
        public static string PlayerInfoUI = "PlayerInfoUI";
        public static string GunResetUI = "GunResetUI";
        public static string GunCostUI = "GunCostUI";

        public static string IdleRewardUI = "IdleRewardUI";
        public static string QuickIdleRewardUI = "QuickIdleRewardUI";

        public static string CampsitePointUI = "CampsitePointUI";
        public static string CampsitePointSetGunUI = "CampsitePointSetGunUI";
        public static string CampsitePointDetailUI = "CampsitePointDetailUI";
        public static string CampsiteOfflineRewardUI = "CampsiteOfflineRewardUI";

        public static string CampsiteUnlockUI = "CampsiteUnlockUI";


        public static string ExpeditionUI = "ExpeditionUI";
        public static string StarRewardUI = "StarRewardUI";
        public static string LevelInfoUI = "LevelInfoUI";
        public static string ChapterUI = "ChapterUI";

        public static string TokenUI = "TokenUI";

        public static string TaskUI = "TaskUI";
        public static string TaskInfoUI = "TaskInfoUI";
        public static string TaskTipsUI = "TaskTipsUI";

        public static string BoxOpenUI = "BoxOpenUI";

        public static string NoDiamondUI = "NoDiamondUI";
        public static string DiamondBuyUI = "DiamondBuyUI";

        public static Dictionary<string, UIInfo> gWndInfo = new Dictionary<string, UIInfo>()
        {
            {CommonPanel,new UIInfo(){ResPath = "Prefabs/UI/CommonUI",NoTouchMask = true,AddRoot = true,Order = 35, Layer = 1 }},
            //{MainPanel,new UIInfo(){ResPath = "Prefabs/UI/MainUi",AddRoot = true,Order = -10,TouchMaskName = "Canvas1"}},
            //{LogoPanel,new UIInfo(){ResPath = "Prefabs/UI/LogoUi",AddRoot = true,Order = 10}},
            {FightPanel,new UIInfo(){ResPath = "Prefabs/UI/FightUI",AddRoot = true,NoTouchMask = true }},
            {FightWinPanel,new UIInfo(){ResPath = "Prefabs/UI/FightWin",TouchEmptyClose =  true}},
            {FightLosePanel,new UIInfo(){ResPath = "Prefabs/UI/FightLose",TouchEmptyClose = true}},
            {FightResultPanel,new UIInfo(){ResPath = "Prefabs/UI/FightResult",TouchEmptyClose = false}},
            {FightRespawnPanel,new UIInfo(){ResPath = "Prefabs/UI/FightRespawn",TouchEmptyClose = false}},
            {FightPausePanel,new UIInfo(){ResPath = "Prefabs/UI/FightPause",AddRoot = true,TouchEmptyClose = true,Order = 45}},
            //{WeaponPanel,new UIInfo(){ResPath = "Prefabs/UI/WeaponUI",Order = -10,AddRoot = true,TouchMaskName = "Canvas1"}},
            //{WeaponRaiseUI,new UIInfo(){ResPath = "Prefabs/UI/WeaponRaiseUI",Order = -10,AddRoot = true,TouchEmptyClose = true,TouchMaskName = "Canvas1"}},
            {ShopPanel,new UIInfo(){ResPath = "Prefabs/UI/ShopUI", Layer = 1 }},
            //{CharacterLevelPanel,new UIInfo(){ResPath = "Prefabs/UI/CharUI_Level",AddRoot = true,Order = 0,TouchMaskName = "Canvas1"}},
            //{CharacterSkillPanel,new UIInfo(){ResPath = "Prefabs/UI/CharUI_Skill",AddRoot = true,Order = 0,TouchMaskName = "Canvas1"}},
            //{SupportPanel,new UIInfo(){ResPath = "Prefabs/UI/SupportUI"} },
            {FightPlotUI,new UIInfo(){ResPath = "Prefabs/UI/FightPlotUI",NoTouchMask = true,AddRoot = true,Order = 40}},
            //{ConfirmPanel,new UIInfo(){ResPath = "Prefabs/UI/ConfirmUI",TouchEmptyClose = true,AddRoot = true,Order = 40}},
            //{ThanksUI,new UIInfo(){ResPath = "Prefabs/UI/ThanksUI",TouchEmptyClose = true,AddRoot = true,Order = 40}},
            //{SuperGunUI,new UIInfo(){ResPath = "Prefabs/UI/SuperGunUI",TouchEmptyClose = true,AddRoot = true,Order = 40}},
            //{EnergyShowPanel,new UIInfo(){ResPath = "Prefabs/UI/EnergyShow",TouchEmptyClose = true,AddRoot = true,Order = 50 }},
            //{GameConfigPanel,new UIInfo(){ResPath = "Prefabs/UI/GameConfigUI",TouchEmptyClose = true,AddRoot = true,Order = 40}},
            {RewardEffectUi,new UIInfo(){ResPath = "Prefabs/UI/RewardEffectUi",NoTouchMask = true,AddRoot = true,Order = 100}},// 比普通ui 高一点
            //{GetRewardUI,new UIInfo(){ResPath = "Prefabs/UI/GetRewardUI",AddRoot = true,Order = 40}},// 比普通ui 高一点
            //{NextDayWeaponUi,new UIInfo(){ResPath = "Prefabs/UI/NextDayWeapon",TouchEmptyClose = true,AddRoot = true,Order = 44}},// 比普通ui 高一点
            //{LevelDetail,new UIInfo(){ResPath = "Prefabs/UI/LevelDetail",TouchEmptyClose = true,AddRoot = true,Order = 40}},
            //{OfflinePanel,new UIInfo(){ResPath = "Prefabs/UI/OfflineUI",TouchEmptyClose = true}},
            //{SevenDayPanel,new UIInfo(){ResPath = "Prefabs/UI/SevenDayUI",TouchEmptyClose = true,AddRoot = true,Order = 43}},
            {DebugPanel,new UIInfo(){ResPath = "Prefabs/UI/DebugUI",TouchEmptyClose = true}},
            //{SkillUI,new UIInfo(){ResPath = "Prefabs/UI/SkillUI"}},
            //{SkillUpUI,new UIInfo(){ResPath = "Prefabs/UI/SkillUp_UI",AddRoot = true,Order = 40 }},
            //{NoMDTUI,new UIInfo(){ResPath = "Prefabs/UI/NoMDTUI",AddRoot = true,Order = 40}},
            //{GetMoneyUI,new UIInfo(){ResPath = "Prefabs/UI/GetMoneyUI",TouchEmptyClose = true,AddRoot = true,Order = 40}},
            //{OpenBoxUI,new UIInfo(){ResPath = "Prefabs/UI/OpenBoxUI",AddRoot = true,Order = 40}},
            //{EvaluateUI,new UIInfo(){ResPath = "Prefabs/UI/EvaluateUI",TouchEmptyClose = true,AddRoot = true,Order = 40}},
            //{CampsiteUI,new UIInfo(){ResPath = "Prefabs/UI/CampsiteUI",NoTouchMask = true }},
            //{FirstPurchaseUI,new UIInfo(){ResPath = "Prefabs/UI/FirstPurchaseUI",TouchEmptyClose = true,AddRoot = true,Order = 40}},

            //{TaskDetailsUi,new UIInfo(){ResPath = "Prefabs/UI/TaskDetailsUi",AddRoot = true,TouchEmptyClose = true,Order = 40}},
            //{OpenMixBoxUI,new UIInfo(){ResPath = "Prefabs/UI/OpenMixBoxUI",TouchEmptyClose = true,AddRoot = true,Order = 40}},

            //{FightNpcPlotUi,new UIInfo(){ResPath = "Prefabs/UI/FightNpcPlotUi",AddRoot = true,NoTouchMask = true}},
            {LoadingUI,new UIInfo(){ResPath = "Prefabs/UI/LoadingUI",TouchEmptyClose = false,AddRoot = true,Order = 40}},

            //{LanguageConfigUI,new UIInfo(){ResPath = "Prefabs/UI/LanguageConfigUI",TouchEmptyClose = true,AddRoot = true,Order = 42}},
            //{TimeGiftUI1,new UIInfo(){ResPath = "Prefabs/UI/TimeGiftUI1",AddRoot = true,TouchEmptyClose = true,Order = 38,TouchMaskName = "Canvas1"}},
            //{TimeGiftUI2,new UIInfo(){ResPath = "Prefabs/UI/TimeGiftUI2",TouchEmptyClose = true,AddRoot = true,Order = 40}},
            //{CheckNetUI,new UIInfo(){ResPath = "Prefabs/UI/CheckNetUI",TouchEmptyClose = false,AddRoot = false,Order = 50}},
            //{CampRecycleUi,new UIInfo(){ResPath = "Prefabs/UI/CampRecycleUi",TouchEmptyClose = true,AddRoot = true,Order = 50}},
            //{ExchangeConfirmUI,new UIInfo(){ResPath = "Prefabs/UI/ExchangeConfirmUI",TouchEmptyClose = true,AddRoot = true,Order = 50}},
            //{CampMatBag,new UIInfo(){ResPath = "Prefabs/UI/CampMatBag",TouchEmptyClose = true,AddRoot = true,Order = 50}},
            //{CampMatRecycleDetailUi,new UIInfo(){ResPath = "Prefabs/UI/CampMatRecycleDetailUi",TouchEmptyClose = true,AddRoot = true,Order = 51}},
            //{CampDetailUI,new UIInfo(){ResPath = "Prefabs/UI/CampDetailUI",TouchEmptyClose = true,AddRoot = true,Order = 51}},
            //{WeaponRaiseSucess_UI,new UIInfo(){ResPath = "Prefabs/UI/WeaponRaiseSucess_UI",AddRoot = true,Order = 40}},
            //{CampResourcesDetailUI,new UIInfo(){ResPath = "Prefabs/UI/CampResourcesDetailUI",TouchEmptyClose = true,AddRoot = true,Order = 51}},
            //{ExchangeSuccessUI,new UIInfo(){ResPath = "Prefabs/UI/ExchangeSuccessUI",TouchEmptyClose = true,AddRoot = true,Order = 51}},
            //{ConfirmEnterPassUI,new UIInfo(){ResPath = "Prefabs/UI/ConfirmEnterPassUI",TouchEmptyClose = true,AddRoot = true,Order = 42}},
            //{CampTaskDetails,new UIInfo(){ResPath = "Prefabs/UI/CampTaskDetails",TouchEmptyClose = true,AddRoot = true,Order = 51}},
            //{ConfirmBranchPassUI,new UIInfo(){ResPath = "Prefabs/UI/ConfirmBranchPassUI",TouchEmptyClose = true,AddRoot = true,Order = 38}},
            //{DialogueUI,new UIInfo(){ResPath = "Prefabs/UI/DialogueUI",TouchEmptyClose = false,AddRoot = true,Order = 42}},
            //{ConfirmLookAdUI,new UIInfo() { ResPath = "Prefabs/UI/ConfirmLookAdUI",TouchEmptyClose = false,AddRoot = true,Order = 42}},
            //{CampBUFF,new UIInfo(){ResPath = "Prefabs/UI/CampBUFF",TouchEmptyClose = true,AddRoot = true,Order = 50}},
            //{Bubble,new UIInfo(){ResPath = "Prefabs/UI/Bubble",TouchEmptyClose = false, NoTouchMask = true, AddRoot = true, Order = 60}},
            //{CampToUpgrade,new UIInfo(){ResPath = "Prefabs/UI/CampToUpgrade",TouchEmptyClose = true,AddRoot = true,Order = 50}},
            //{CampBadgeSuccessUI,new UIInfo(){ResPath = "Prefabs/UI/CampBadgeSuccessUI",AddRoot = true,Order = 50}},
            //{CampBadgeUI,new UIInfo(){ResPath = "Prefabs/UI/CampBadgeUI",AddRoot = true,TouchEmptyClose = true,Order = 50}},

            {HomeUI,new UIInfo(){ResPath = "Prefabs/UI/HomeUI",AddRoot = true,Order = -10,NoTouchMask = true, Layer = 1 }},
            {GunUI,new UIInfo(){ResPath = "Prefabs/UI/GunUI",Order = -10,AddRoot = true,TouchMaskName = "Canvas", Layer = 1 }},
            {GunInfoUI,new UIInfo(){ResPath = "Prefabs/UI/GunInfoUI",TouchEmptyClose = true,AddRoot = true,Order = 40, Layer = 4}},
            {GunListUI,new UIInfo(){ResPath = "Prefabs/UI/GunListUI",TouchEmptyClose = true,AddRoot = true,Order = 40, Layer = 3 }},
            {GunStarUpUI,new UIInfo(){ResPath = "Prefabs/UI/GunStarUpUI",TouchEmptyClose = true,AddRoot = true,Order = 41, Layer = 5 }},
            {GunSkillLvUpUI,new UIInfo(){ResPath = "Prefabs/UI/GunSkillLvUpUI",TouchEmptyClose = true,AddRoot = true,Order = 41, Layer = 5}},
            {GunSkillInfoUI,new UIInfo(){ResPath = "Prefabs/UI/GunSkillInfoUI",TouchEmptyClose = true,AddRoot = true,Order = 41, Layer = 5}},
            {GunStarUpShowUI,new UIInfo(){ResPath = "Prefabs/UI/GunStarUpShowUI",TouchEmptyClose = true,AddRoot = true,Order = 41, Layer = 5}},
            {GunChipUI,new UIInfo(){ResPath = "Prefabs/UI/GunChipUI",TouchEmptyClose = true,AddRoot = true,Order = 40, Layer = 2}},
            {PlayerInfoUI,new UIInfo(){ResPath = "Prefabs/UI/PlayerInfoUI",TouchEmptyClose = true,AddRoot = true,Order = 40, Layer = 3}},
            {GunResetUI,new UIInfo(){ResPath = "Prefabs/UI/GunResetUI",TouchEmptyClose = true,AddRoot = true,Order = 41, Layer = 3}},
            {GunCostUI,new UIInfo(){ResPath = "Prefabs/UI/GunCostUI",TouchEmptyClose = true,AddRoot = true,Order = 41, Layer = 3}},

            {IdleRewardUI,new UIInfo(){ResPath = "Prefabs/UI/IdleRewardUI",TouchEmptyClose = true,TouchMaskName = "Canvas", AddRoot = true,Order = 40, Layer = 3} },
            {QuickIdleRewardUI,new UIInfo(){ResPath = "Prefabs/UI/QuickIdleRewardUI",TouchEmptyClose = true,TouchMaskName = "Canvas", AddRoot = true,Order = 41, NetOpen = true, Layer = 3} },

            {CampsitePointUI,new UIInfo(){ResPath = "Prefabs/UI/CampsitePointUI",NoTouchMask = true,TouchEmptyClose = false,TouchMaskName = "Canvas", AddRoot = true,Order = 10, Layer = 2} },
            {CampsitePointSetGunUI,new UIInfo(){ResPath = "Prefabs/UI/CampsitePointSetGunUI", AddRoot = true,Order = 15, Layer = 3} },
            {CampsiteOfflineRewardUI,new UIInfo(){ResPath = "Prefabs/UI/CampsiteOfflineRewardUI", AddRoot = true,Order = 50, Layer = 2} },
            {CampsiteUnlockUI,new UIInfo(){ResPath = "Prefabs/UI/CampsiteUnlockUI",TouchEmptyClose = true,AddRoot = true,Order = 40, Layer = 2}},
            {CampsitePointDetailUI,new UIInfo(){ResPath = "Prefabs/UI/CampsitePointDetailUI",TouchEmptyClose = true,TouchMaskName = "Canvas",AddRoot = true,Order = 40, Layer = 3}},
            {ExpeditionUI,new UIInfo(){ResPath = "Prefabs/UI/ExpeditionUI",AddRoot = true,Order = 15,TouchMaskName = "Canvas1", Layer = 2}},
            {StarRewardUI,new UIInfo(){ResPath = "Prefabs/UI/StarRewardUI",AddRoot = true,Order = 16,TouchEmptyClose = true, Layer = 3}},
            {LevelInfoUI,new UIInfo(){ResPath = "Prefabs/UI/LevelInfoUI",AddRoot = true,Order = 16,TouchEmptyClose = true, Layer = 3}},
            {ChapterUI,new UIInfo(){ResPath = "Prefabs/UI/ChapterUI",AddRoot = true,Order = 16,TouchEmptyClose = true, Layer = 3}},

            {TokenUI,new UIInfo(){ResPath = "Prefabs/UI/TokenUI",NoTouchMask = true,AddRoot = true,Order = 35}},

            {TaskUI,new UIInfo(){ResPath = "Prefabs/UI/TaskUI",NoTouchMask = true,AddRoot = true,Order = 10, Layer = 1 }},
            {TaskInfoUI,new UIInfo(){ResPath = "Prefabs/UI/TaskInfoUI",TouchEmptyClose = true,AddRoot = true,Order = 41, Layer = 2}},
            {TaskTipsUI,new UIInfo(){ResPath = "Prefabs/UI/TaskTipsUI",TouchEmptyClose = true,AddRoot = true,Order = 51}},

            {BoxOpenUI,new UIInfo(){ResPath = "Prefabs/UI/BoxOpenUI", AddRoot = true,Order = 50, Layer = 2} },
            {NoDiamondUI,new UIInfo(){ResPath = "Prefabs/UI/NoDiamondUI",AddRoot = true,Order = 50, Layer = 4}},
            {DiamondBuyUI,new UIInfo(){ResPath = "Prefabs/UI/DiamondBuyUI", Layer = 4}},

            {DialogueUI,new UIInfo(){ResPath = "Prefabs/UI/DialogueUI",TouchEmptyClose = false,AddRoot = true,Order = 42}},
            {CampSwitchUI,new UIInfo(){ResPath = "Prefabs/UI/CampSwitchUI",TouchEmptyClose = false,AddRoot = true,Order = 42}},
            {OpeningUI,new UIInfo(){ResPath = "Prefabs/UI/OpeningUI",TouchEmptyClose = false,AddRoot = true,Order = 42}},

        };
    }
}
