using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class SkillUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_BgBtn;
		public RectTransform_Button_Image_Container BgBtn { get { return m_BgBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_skill_item0;
		public RectTransform_Button_Image_Container skill_item0 { get { return m_skill_item0; } }

		[SerializeField]
		private RectTransform_Text_Container m_lv;
		public RectTransform_Text_Container lv { get { return m_lv; } }

		[SerializeField]
		private RectTransform_Image_Container m_choose;
		public RectTransform_Image_Container choose { get { return m_choose; } }

		[SerializeField]
		private RectTransform_Image_Container m_lockIcon;
		public RectTransform_Image_Container lockIcon { get { return m_lockIcon; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_skill_item1;
		public RectTransform_Button_Image_Container skill_item1 { get { return m_skill_item1; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_skill_item2;
		public RectTransform_Button_Image_Container skill_item2 { get { return m_skill_item2; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_skill_item3;
		public RectTransform_Button_Image_Container skill_item3 { get { return m_skill_item3; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_skill_item4;
		public RectTransform_Button_Image_Container skill_item4 { get { return m_skill_item4; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_skill_item5;
		public RectTransform_Button_Image_Container skill_item5 { get { return m_skill_item5; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_skill_item6;
		public RectTransform_Button_Image_Container skill_item6 { get { return m_skill_item6; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_skill_item7;
		public RectTransform_Button_Image_Container skill_item7 { get { return m_skill_item7; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_skill_item8;
		public RectTransform_Button_Image_Container skill_item8 { get { return m_skill_item8; } }

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
		private RectTransform_Text_Container m_Maxtxt;
		public RectTransform_Text_Container Maxtxt { get { return m_Maxtxt; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_dotxt;
		public RectTransform_Text_LanguageTip_Container dotxt { get { return m_dotxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_TimesText;
		public RectTransform_Text_Container TimesText { get { return m_TimesText; } }

		[SerializeField]
		private RectTransform_Text_Container m_TotalTimes;
		public RectTransform_Text_Container TotalTimes { get { return m_TotalTimes; } }

		[SerializeField]
		private RectTransform_Image_Container m_tip0;
		public RectTransform_Image_Container tip0 { get { return m_tip0; } }

		[SerializeField]
		private RectTransform_Image_Container m_Box;
		public RectTransform_Image_Container Box { get { return m_Box; } }

		[SerializeField]
		private RectTransform_Text_Container m_tip_text;
		public RectTransform_Text_Container tip_text { get { return m_tip_text; } }

		[SerializeField]
		private RectTransform_Image_Container m_arrow;
		public RectTransform_Image_Container arrow { get { return m_arrow; } }

		[SerializeField]
		private RectTransform_Image_Container m_tip1;
		public RectTransform_Image_Container tip1 { get { return m_tip1; } }

		[SerializeField]
		private RectTransform_Image_Container m_tip2;
		public RectTransform_Image_Container tip2 { get { return m_tip2; } }

		[SerializeField]
		private RectTransform_Image_Container m_tip3;
		public RectTransform_Image_Container tip3 { get { return m_tip3; } }

		[SerializeField]
		private RectTransform_Image_Container m_tip4;
		public RectTransform_Image_Container tip4 { get { return m_tip4; } }

		[SerializeField]
		private RectTransform_Image_Container m_tip5;
		public RectTransform_Image_Container tip5 { get { return m_tip5; } }

		[SerializeField]
		private RectTransform_Image_Container m_tip6;
		public RectTransform_Image_Container tip6 { get { return m_tip6; } }

		[SerializeField]
		private RectTransform_Image_Container m_tip7;
		public RectTransform_Image_Container tip7 { get { return m_tip7; } }

		[SerializeField]
		private RectTransform_Image_Container m_tip8;
		public RectTransform_Image_Container tip8 { get { return m_tip8; } }

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
