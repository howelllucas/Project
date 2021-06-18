using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class WeaponRaiseSucess_UI : BaseUi {

		[SerializeField]
		private RectTransform_Container m_AllNode;
		public RectTransform_Container AllNode { get { return m_AllNode; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_titletxt;
		public RectTransform_Text_LanguageTip_Container titletxt { get { return m_titletxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_CurVal;
		public RectTransform_Text_Container CurVal { get { return m_CurVal; } }

		[SerializeField]
		private RectTransform_Text_Container m_IncreaseVal;
		public RectTransform_Text_Container IncreaseVal { get { return m_IncreaseVal; } }

		[SerializeField]
		private RectTransform_Image_Container m_CurWpnIcon;
		public RectTransform_Image_Container CurWpnIcon { get { return m_CurWpnIcon; } }

		[SerializeField]
		private RectTransform_Image_Container m_NextWpnIcon;
		public RectTransform_Image_Container NextWpnIcon { get { return m_NextWpnIcon; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

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
