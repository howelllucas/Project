using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class FightResult : BaseUi {

		[SerializeField]
		private RectTransform_Image_Container NormalIcon;

		[SerializeField]
		private RectTransform_Button_Image_Container Btn2;

		[SerializeField]
		private RectTransform_Image_Container CmIcon;

		[SerializeField]
		private RectTransform_Text_Container CmNum;

		[SerializeField]
		private RectTransform_Image_Container AdIcon;

		[SerializeField]
		private RectTransform_Container waitAdDelay;

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container waitAdTxt;

		[SerializeField]
		private RectTransform_Button_Image_Container Btn1;

		[SerializeField]
		private RectTransform_Image_Container CmIcon3;

		[SerializeField]
		private RectTransform_Text_Container CmNum3;

		[SerializeField]
		private RectTransform_Image_Container Star1;

		[SerializeField]
		private RectTransform_Text_Container StarText1;

		[SerializeField]
		private RectTransform_Image_Container Star2;

		[SerializeField]
		private RectTransform_Text_Container StarText2;

		[SerializeField]
		private RectTransform_Image_Container Star3;

		[SerializeField]
		private RectTransform_Text_Container StarText3;

		[System.Serializable]
		private class RectTransform_Button_Image_Container {

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
		private class RectTransform_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

		}

		[System.Serializable]
		private class RectTransform_Image_Container {

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
		private class RectTransform_Text_Container {

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
		private class RectTransform_Text_LanguageTip_Container {

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
