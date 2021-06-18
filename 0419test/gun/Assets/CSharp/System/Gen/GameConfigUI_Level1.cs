using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GameConfigUI_Level1 : MonoBehaviour {

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnLanguage;
		public RectTransform_Button_Image_Container BtnLanguage { get { return m_BtnLanguage; } }

		[SerializeField]
		private RectTransform_Text_Container m_languageTxt;
		public RectTransform_Text_Container languageTxt { get { return m_languageTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnvolOn;
		public RectTransform_Button_Image_Container BtnvolOn { get { return m_BtnvolOn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnvolOff;
		public RectTransform_Button_Image_Container BtnvolOff { get { return m_BtnvolOff; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnmusicOn;
		public RectTransform_Button_Image_Container BtnmusicOn { get { return m_BtnmusicOn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnmusicOff;
		public RectTransform_Button_Image_Container BtnmusicOff { get { return m_BtnmusicOff; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnVibeOn;
		public RectTransform_Button_Image_Container BtnVibeOn { get { return m_BtnVibeOn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnVibeOff;
		public RectTransform_Button_Image_Container BtnVibeOff { get { return m_BtnVibeOff; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_about;
		public RectTransform_Button_Image_Container about { get { return m_about; } }

		[SerializeField]
		private RectTransform_Text_Button_Container m_AboatTxt;
		public RectTransform_Text_Button_Container AboatTxt { get { return m_AboatTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_AboatArrow;
		public RectTransform_Button_Image_Container AboatArrow { get { return m_AboatArrow; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn2;
		public RectTransform_Button_Image_Container Btn2 { get { return m_Btn2; } }

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
		public class RectTransform_Text_Button_Container {

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
			private Button m_button;
			public Button button { get { return m_button; } }

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

	}

}
