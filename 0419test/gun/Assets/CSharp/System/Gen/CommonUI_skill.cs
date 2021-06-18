using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public class CommonUI_skill : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_Newbg;
		public RectTransform_Image_Container Newbg { get { return m_Newbg; } }

		[SerializeField]
		private RectTransform_Button_Image_NewbieGuideButton_Container m_icon;
		public RectTransform_Button_Image_NewbieGuideButton_Container icon { get { return m_icon; } }

		[SerializeField]
		private RectTransform_Image_Container m_Lockimg;
		public RectTransform_Image_Container Lockimg { get { return m_Lockimg; } }

		[SerializeField]
		private RectTransform_Container m_CanUpgrade;
		public RectTransform_Container CanUpgrade { get { return m_CanUpgrade; } }

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

	}

}
