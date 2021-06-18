using EZ.Util;

namespace EZ.DataMgr {
    
    public class SystemMgr {
        private WeaponMgr m_WeaponMgr;
        private BaseAttrMgr m_AttrMgr;
        private PassMgr m_PassMgr;
        private MiscMgr m_MiscMgr;
        private SkillMgr m_SkillMgr;
        private QuestMgr m_QuestMgr;
        private NewbieGuideMgr m_NewbieGuideMgr;
        private NpcMgr m_NpcMgr;
        private CampGuidMgr m_CampGuidMgr;

        public SystemMgr()
        {
            m_WeaponMgr = new WeaponMgr();
            m_AttrMgr = new BaseAttrMgr();
            m_PassMgr = new PassMgr();
            m_QuestMgr = new QuestMgr();
            m_MiscMgr = new MiscMgr();
            m_SkillMgr = new SkillMgr();
            m_NewbieGuideMgr = new NewbieGuideMgr();
            m_NpcMgr = new NpcMgr();
            m_CampGuidMgr = new CampGuidMgr();
        }

        public void ClearData()
        {
            m_WeaponMgr.ClearData();
            m_AttrMgr.ClearData();
            m_PassMgr.ClearData();
            m_QuestMgr.ClearData();
            m_MiscMgr.ClearData();
            m_SkillMgr.ClearData();
            m_NewbieGuideMgr.ClearData();
            m_NpcMgr.ClearData();
            m_CampGuidMgr.ClearData();
        }

        public void ResetData()
        {
            m_WeaponMgr.OnInit();
            m_AttrMgr.OnInit();
            m_PassMgr.OnInit();
            m_QuestMgr.OnInit();
            m_MiscMgr.OnInit();
            m_SkillMgr.OnInit();
            m_NewbieGuideMgr.OnInit();
            m_NpcMgr.OnInit();
            m_CampGuidMgr.OnInit();
        }

        public void AfterInit()
        {
            //任务需要在杂项初始化之前初始化，进行时效任务的刷新
            m_QuestMgr.AfterInit();
            //杂项会处理一些登录记录，触发任务
            m_MiscMgr.AfterInit();

            m_AttrMgr.AfterInit();;
            m_WeaponMgr.AfterInit();
            m_SkillMgr.AfterInit();
            m_PassMgr.AfterInit();

            m_NpcMgr.AfterInit();

            // 最后
            m_CampGuidMgr.AfterInit();
        }
        public PassMgr GetPassMgr()
        {
            return m_PassMgr;
        }
        public BaseAttrMgr GetBaseAttrMgr()
        {
            return m_AttrMgr;
        }
        public WeaponMgr GetWeaponMgr()
        {
            return m_WeaponMgr;
        }
        
        public MiscMgr GetMiscMgr()
        {
            return m_MiscMgr;
        }

        public SkillMgr GetSkillMgr()
        {
            return m_SkillMgr;
        }

        public QuestMgr GetQuestMgr()
        {
            return m_QuestMgr;
        }

        public NewbieGuideMgr GetNewbieGuideMgr()
        {
            return m_NewbieGuideMgr;
        }

        public NpcMgr GetNpcMgr()
        {
            return m_NpcMgr;
        }

        public CampGuidMgr GetCampGuidMgr()
        {
            return m_CampGuidMgr;
        }
    }
}
