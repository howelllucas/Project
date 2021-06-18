using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class WeaponUI_WeaponItemUI : MonoBehaviour {

		[SerializeField]
		private RectTransform_Container m_Item;
		public RectTransform_Container Item { get { return m_Item; } }

		[SerializeField]
		private RectTransform_Image_Container m_Mask1;
		public RectTransform_Image_Container Mask1 { get { return m_Mask1; } }

		[SerializeField]
		private RectTransform_Text_Container m_MaskTxt;
		public RectTransform_Text_Container MaskTxt { get { return m_MaskTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewWeapon;
		public RectTransform_Image_Container NewWeapon { get { return m_NewWeapon; } }

		[SerializeField]
		private RectTransform_Image_Container m_EquipBg;
		public RectTransform_Image_Container EquipBg { get { return m_EquipBg; } }

		[SerializeField]
		private RectTransform_Button_Image_NewbieGuideButton_Container m_EquipBtn;
		public RectTransform_Button_Image_NewbieGuideButton_Container EquipBtn { get { return m_EquipBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_WeaponBg;
		public RectTransform_Image_Container WeaponBg { get { return m_WeaponBg; } }

		[SerializeField]
		private RectTransform_Image_Container m_Equip;
		public RectTransform_Image_Container Equip { get { return m_Equip; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_WeaponIcon;
		public RectTransform_Button_Image_Container WeaponIcon { get { return m_WeaponIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_WeaponNameTxt;
		public RectTransform_Text_Container WeaponNameTxt { get { return m_WeaponNameTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewIMG;
		public RectTransform_Image_Container NewIMG { get { return m_NewIMG; } }

		[SerializeField]
		private RectTransform_Text_Container m_WeaponLvTxt;
		public RectTransform_Text_Container WeaponLvTxt { get { return m_WeaponLvTxt; } }

		[SerializeField]
		private RectTransform_Container m_WeaponUp;
		public RectTransform_Container WeaponUp { get { return m_WeaponUp; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_Param1NameTxt;
		public RectTransform_Text_LanguageTip_Container Param1NameTxt { get { return m_Param1NameTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_Param1Txt;
		public RectTransform_Text_Container Param1Txt { get { return m_Param1Txt; } }

		[SerializeField]
		private RectTransform_Text_Container m_Param2NameTxt;
		public RectTransform_Text_Container Param2NameTxt { get { return m_Param2NameTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_Param2Txt;
		public RectTransform_Text_Container Param2Txt { get { return m_Param2Txt; } }

		[SerializeField]
		private RectTransform_Text_Container m_Param3NameTxt;
		public RectTransform_Text_Container Param3NameTxt { get { return m_Param3NameTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_Param3Txt;
		public RectTransform_Text_Container Param3Txt { get { return m_Param3Txt; } }

		[SerializeField]
		private RectTransform_Button_Image_NewbieGuideButton_Container m_UpBtn;
		public RectTransform_Button_Image_NewbieGuideButton_Container UpBtn { get { return m_UpBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_CmIcon;
		public RectTransform_Image_Container CmIcon { get { return m_CmIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_CmNum;
		public RectTransform_Text_Container CmNum { get { return m_CmNum; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_dotxt;
		public RectTransform_Text_LanguageTip_Container dotxt { get { return m_dotxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_MaxTxt;
		public RectTransform_Text_Container MaxTxt { get { return m_MaxTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_NewbieGuideButton_Container m_UnlockBtn;
		public RectTransform_Button_Image_NewbieGuideButton_Container UnlockBtn { get { return m_UnlockBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_u_CmIcon;
		public RectTransform_Image_Container u_CmIcon { get { return m_u_CmIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_u_CmNum;
		public RectTransform_Text_Container u_CmNum { get { return m_u_CmNum; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_u_dotxt;
		public RectTransform_Text_LanguageTip_Container u_dotxt { get { return m_u_dotxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_AskBg;
		public RectTransform_Image_Container AskBg { get { return m_AskBg; } }

		[SerializeField]
		private RectTransform_Image_Container m_AskIcon;
		public RectTransform_Image_Container AskIcon { get { return m_AskIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_AskTxt;
		public RectTransform_Text_Container AskTxt { get { return m_AskTxt; } }

		[System.Serializable]
		public class RectTransform_Button_Image_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private Button m_button;
			public Button button { get { return m_button; } }

			[SerializeField]
			private Image m_image;
			public Image image { get { return m_image; } }

		}

		[System.Serializable]
		public class RectTransform_Button_Image_NewbieGuideButton_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private Button m_button;
			public Button button { get { return m_button; } }

			[SerializeField]
			private Image m_image;
			public Image image { get { return m_image; } }

			[SerializeField]
			private NewbieGuideButton m_newbieGuideButton;
			public NewbieGuideButton newbieGuideButton { get { return m_newbieGuideButton; } }

		}

		[System.Serializable]
		public class RectTransform_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

		}

		[System.Serializable]
		public class RectTransform_Image_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private Image m_image;
			public Image image { get { return m_image; } }

		}

		[System.Serializable]
		public class RectTransform_Text_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private Text m_text;
			public Text text { get { return m_text; } }

		}

		[System.Serializable]
		public class RectTransform_Text_LanguageTip_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private Text m_text;
			public Text text { get { return m_text; } }

			[SerializeField]
			private LanguageTip m_languageTip;
			public LanguageTip languageTip { get { return m_languageTip; } }

		}

	}

}
