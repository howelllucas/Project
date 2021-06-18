using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class ExchangeConfirmUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_DesTips;
		public RectTransform_Text_LanguageTip_Container DesTips { get { return m_DesTips; } }

		[SerializeField]
		private RectTransform_Text_Container m_MatName;
		public RectTransform_Text_Container MatName { get { return m_MatName; } }

		[SerializeField]
		private RectTransform_Image_Container m_MatIcon;
		public RectTransform_Image_Container MatIcon { get { return m_MatIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_ExchangeCount;
		public RectTransform_Text_Container ExchangeCount { get { return m_ExchangeCount; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ConfirmBtn;
		public RectTransform_Button_Image_Container ConfirmBtn { get { return m_ConfirmBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_HeartCount;
		public RectTransform_Text_Container HeartCount { get { return m_HeartCount; } }

		[SerializeField]
		private RectTransform_Image_Container m_HeartIcon;
		public RectTransform_Image_Container HeartIcon { get { return m_HeartIcon; } }

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
