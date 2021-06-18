using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class WeaponUI_SubWeaponItemUI : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_SubWeaponBg;
		public RectTransform_Image_Container SubWeaponBg { get { return m_SubWeaponBg; } }

		[SerializeField]
		private RectTransform_Container m_Item;
		public RectTransform_Container Item { get { return m_Item; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewSubWeapon;
		public RectTransform_Image_Container NewSubWeapon { get { return m_NewSubWeapon; } }

		[SerializeField]
		private RectTransform_Image_Container m_Mask;
		public RectTransform_Image_Container Mask { get { return m_Mask; } }

		[SerializeField]
		private RectTransform_Image_Container m_bottom;
		public RectTransform_Image_Container bottom { get { return m_bottom; } }

		[SerializeField]
		private RectTransform_Button_Image_NewbieGuideButton_Container m_SubEquipBtn;
		public RectTransform_Button_Image_NewbieGuideButton_Container SubEquipBtn { get { return m_SubEquipBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_SubWeaponNameTxt;
		public RectTransform_Text_Container SubWeaponNameTxt { get { return m_SubWeaponNameTxt; } }

		[SerializeField]
		private RectTransform_Container m_UpNode;
		public RectTransform_Container UpNode { get { return m_UpNode; } }

		[SerializeField]
		private RectTransform_Image_Container m_SubWeaponIcon;
		public RectTransform_Image_Container SubWeaponIcon { get { return m_SubWeaponIcon; } }

		[SerializeField]
		private RectTransform_Image_Container m_Lock;
		public RectTransform_Image_Container Lock { get { return m_Lock; } }

		[SerializeField]
		private RectTransform_Image_Container m_SubEquip;
		public RectTransform_Image_Container SubEquip { get { return m_SubEquip; } }

		[SerializeField]
		private RectTransform_Text_Container m_SubLvtxt;
		public RectTransform_Text_Container SubLvtxt { get { return m_SubLvtxt; } }

		[SerializeField]
		private RectTransform_Container m_SubWeaponUp;
		public RectTransform_Container SubWeaponUp { get { return m_SubWeaponUp; } }

		[SerializeField]
		private RectTransform_Container m_AtkMsg;
		public RectTransform_Container AtkMsg { get { return m_AtkMsg; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_AtkTitle;
		public RectTransform_Text_LanguageTip_Container AtkTitle { get { return m_AtkTitle; } }

		[SerializeField]
		private RectTransform_Text_Container m_Atk;
		public RectTransform_Text_Container Atk { get { return m_Atk; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewIMG_sub;
		public RectTransform_Image_Container NewIMG_sub { get { return m_NewIMG_sub; } }

		[SerializeField]
		private RectTransform_Container m_Effect;
		public RectTransform_Container Effect { get { return m_Effect; } }

		[SerializeField]
		private RectTransform_Image_Container m_progress_bg;
		public RectTransform_Image_Container progress_bg { get { return m_progress_bg; } }

		[SerializeField]
		private RectTransform_Image_Container m_passProgress;
		public RectTransform_Image_Container passProgress { get { return m_passProgress; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnUnlock;
		public RectTransform_Button_Image_Container BtnUnlock { get { return m_BtnUnlock; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnUpgrade;
		public RectTransform_Button_Image_Container BtnUpgrade { get { return m_BtnUpgrade; } }

		[SerializeField]
		private RectTransform_Image_Container m_Moneyiconbtn;
		public RectTransform_Image_Container Moneyiconbtn { get { return m_Moneyiconbtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_Moneycostbtn;
		public RectTransform_Text_Container Moneycostbtn { get { return m_Moneycostbtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_Unlocktxt;
		public RectTransform_Text_Container Unlocktxt { get { return m_Unlocktxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_AskIcon;
		public RectTransform_Image_Container AskIcon { get { return m_AskIcon; } }

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
