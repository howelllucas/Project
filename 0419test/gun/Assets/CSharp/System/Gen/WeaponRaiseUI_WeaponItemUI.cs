using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class WeaponRaiseUI_WeaponItemUI : MonoBehaviour {

		[SerializeField]
		private RectTransform_Container m_Item;
		public RectTransform_Container Item { get { return m_Item; } }

		[SerializeField]
		private RectTransform_Image_Container m_EquipBg;
		public RectTransform_Image_Container EquipBg { get { return m_EquipBg; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_Param1NameTxt;
		public RectTransform_Text_LanguageTip_Container Param1NameTxt { get { return m_Param1NameTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_Param1Txt;
		public RectTransform_Text_Container Param1Txt { get { return m_Param1Txt; } }

		[SerializeField]
		private RectTransform_Image_Container m_WeaponBg;
		public RectTransform_Image_Container WeaponBg { get { return m_WeaponBg; } }

		[SerializeField]
		private RectTransform_Image_Container m_WeaponIcon;
		public RectTransform_Image_Container WeaponIcon { get { return m_WeaponIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_WeaponNameTxt;
		public RectTransform_Text_Container WeaponNameTxt { get { return m_WeaponNameTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_WeaponLvTxt;
		public RectTransform_Text_Container WeaponLvTxt { get { return m_WeaponLvTxt; } }

		[SerializeField]
		private RectTransform_Container m_WeaponUp;
		public RectTransform_Container WeaponUp { get { return m_WeaponUp; } }

		[SerializeField]
		private RectTransform_Button_Image_NewbieGuideButton_Container m_UpBtn;
		public RectTransform_Button_Image_NewbieGuideButton_Container UpBtn { get { return m_UpBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_CmIcon2;
		public RectTransform_Image_Container CmIcon2 { get { return m_CmIcon2; } }

		[SerializeField]
		private RectTransform_Text_Container m_CmNum2;
		public RectTransform_Text_Container CmNum2 { get { return m_CmNum2; } }

		[SerializeField]
		private RectTransform_Image_Container m_CmIcon1;
		public RectTransform_Image_Container CmIcon1 { get { return m_CmIcon1; } }

		[SerializeField]
		private RectTransform_Text_Container m_CmNum1;
		public RectTransform_Text_Container CmNum1 { get { return m_CmNum1; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_dotxt;
		public RectTransform_Text_LanguageTip_Container dotxt { get { return m_dotxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_RaiseIcon;
		public RectTransform_Image_Container RaiseIcon { get { return m_RaiseIcon; } }

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
