using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CheckNetUI : BaseUi {

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_titletxt;
		public RectTransform_Text_LanguageTip_Container titletxt { get { return m_titletxt; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_Tips;
		public RectTransform_Text_LanguageTip_Container Tips { get { return m_Tips; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn1;
		public RectTransform_Button_Image_Container Btn1 { get { return m_Btn1; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_Btn1txt;
		public RectTransform_Text_LanguageTip_Container Btn1txt { get { return m_Btn1txt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn2;
		public RectTransform_Button_Image_Container Btn2 { get { return m_Btn2; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_Btn2txt;
		public RectTransform_Text_LanguageTip_Container Btn2txt { get { return m_Btn2txt; } }

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
