
using EZ.Data;
using UnityEngine;

namespace EZ
{
    public partial class WeaponRaiseSucess_UI
    {
        ItemItem m_ItemItem;
        public override void Init<T>(string name, UIInfo info, T arg)
        {
            base.Init(name, info, arg);
            m_ItemItem = arg as ItemItem;
            InitNode();
            base.ChangeLanguage();
        }
        private void InitNode()
        {
            m_CloseBtn.button.onClick.AddListener(TouchClose);

            m_CurWpnIcon.image.sprite = Resources.Load(m_ItemItem.image_grow, typeof(Sprite)) as Sprite;
            m_NextWpnIcon.image.sprite = Resources.Load(m_ItemItem.image_grow + "_s", typeof(Sprite)) as Sprite;



            int gunLevel = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponLevel(m_ItemItem.name);
            int gunShowLevel = Global.gApp.gSystemMgr.GetWeaponMgr().GetWeaponShowLevel(m_ItemItem.name);
            Guns_dataItem weaponLevelData = Global.gApp.gGameData.GunDataConfig.Get(gunLevel);


            double[] paramValuePpre = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(m_ItemItem.name, weaponLevelData);
            double[] paramValueSuper = ReflectionUtil.GetValueByProperty<Guns_dataItem, double[]>(m_ItemItem.name + "_super", weaponLevelData);
            double showVal1 = m_ItemItem.showParam / m_ItemItem.dtime * paramValuePpre[0];
            double nextVal = m_ItemItem.showParam / m_ItemItem.dtime * paramValueSuper[0];
            CurVal.text.text = UiTools.FormateMoney(showVal1);
            IncreaseVal.text.text = "+" + UiTools.FormateMoney(nextVal - showVal1);
        }
    }
}
