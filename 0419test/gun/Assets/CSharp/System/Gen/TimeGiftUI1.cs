using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class TimeGiftUI1 : BaseUi {

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_TitleTxt;
		public RectTransform_Text_LanguageTip_Container TitleTxt { get { return m_TitleTxt; } }

		[SerializeField]
		private RectTransform_Container m_Reward;
		public RectTransform_Container Reward { get { return m_Reward; } }

		[SerializeField]
		private RectTransform_Text_Container m_NextGunName;
		public RectTransform_Text_Container NextGunName { get { return m_NextGunName; } }

		[SerializeField]
		private RectTransform_Image_Container m_NextGunDown;
		public RectTransform_Image_Container NextGunDown { get { return m_NextGunDown; } }

		[SerializeField]
		private RectTransform_Image_Container m_NextGunIcon;
		public RectTransform_Image_Container NextGunIcon { get { return m_NextGunIcon; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_Destxt;
		public RectTransform_Text_LanguageTip_Container Destxt { get { return m_Destxt; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_TimeCount;
		public RectTransform_Text_LanguageTip_Container TimeCount { get { return m_TimeCount; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_ItemCount1;
		public RectTransform_Text_LanguageTip_Container ItemCount1 { get { return m_ItemCount1; } }

		[SerializeField]
		private RectTransform_Image_Container m_ItemIcon1;
		public RectTransform_Image_Container ItemIcon1 { get { return m_ItemIcon1; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_ItemCount2;
		public RectTransform_Text_LanguageTip_Container ItemCount2 { get { return m_ItemCount2; } }

		[SerializeField]
		private RectTransform_Image_Container m_ItemIcon2;
		public RectTransform_Image_Container ItemIcon2 { get { return m_ItemIcon2; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn2;
		public RectTransform_Button_Image_Container Btn2 { get { return m_Btn2; } }

		[SerializeField]
		private RectTransform_Text_Container m_moneyNum;
		public RectTransform_Text_Container moneyNum { get { return m_moneyNum; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnC;
		public RectTransform_Button_Image_Container BtnC { get { return m_BtnC; } }

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
