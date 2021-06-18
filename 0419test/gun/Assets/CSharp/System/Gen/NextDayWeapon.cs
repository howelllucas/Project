using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class NextDayWeapon : BaseUi {

		[SerializeField]
		private RectTransform_Text_Container TitleTxt;

		[SerializeField]
		private RectTransform_Container NextDay;

		[SerializeField]
		private RectTransform_Text_Container NextGunName;

		[SerializeField]
		private RectTransform_Image_Container NextGunDown;

		[SerializeField]
		private RectTransform_Image_Container NextGunIcon;

		[SerializeField]
		private RectTransform_Image_Container NextAwardIcon;

		[SerializeField]
		private RectTransform_Button_Image_Container Btn1;

		[SerializeField]
		private RectTransform_Text_Container Destxt;

		[SerializeField]
		private RectTransform_Text_Container LeftTxt;

		[SerializeField]
		private RectTransform_Button_Image_Container Btn2;

		[SerializeField]
		private RectTransform_Button_Image_Container BtnC;

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

	}

}
