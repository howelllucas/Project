using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GetRewardUI : BaseUi {

		[SerializeField]
		private RectTransform_Image_Container Bg;

		[SerializeField]
		private RectTransform_Image_Container airdropicon_close;

		[SerializeField]
		private RectTransform_Image_Container airdropicon_open;

		[SerializeField]
		private RectTransform_Button_Image_Container Btn1;

		[SerializeField]
		private RectTransform_Image_Container CmIcon;

		[SerializeField]
		private RectTransform_Text_Container CmNum;

		[SerializeField]
		private RectTransform_Button_Image_Container Btn2;

		[SerializeField]
		private RectTransform_Image_Container CmIconAd;

		[SerializeField]
		private RectTransform_Text_Container CmNumAd;

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

	}

}
