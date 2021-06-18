using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class SuperGunUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_fromGun;
		public RectTransform_Image_Container fromGun { get { return m_fromGun; } }

		[SerializeField]
		private RectTransform_Image_Container m_toGun;
		public RectTransform_Image_Container toGun { get { return m_toGun; } }

		[SerializeField]
		private RectTransform_Text_Container m_Confirmtxt3;
		public RectTransform_Text_Container Confirmtxt3 { get { return m_Confirmtxt3; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn1;
		public RectTransform_Button_Image_Container Btn1 { get { return m_Btn1; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn2;
		public RectTransform_Button_Image_Container Btn2 { get { return m_Btn2; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon1;
		public RectTransform_Image_Container Icon1 { get { return m_Icon1; } }

		[SerializeField]
		private RectTransform_Text_Container m_Cost1;
		public RectTransform_Text_Container Cost1 { get { return m_Cost1; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon2;
		public RectTransform_Image_Container Icon2 { get { return m_Icon2; } }

		[SerializeField]
		private RectTransform_Text_Container m_Cost2;
		public RectTransform_Text_Container Cost2 { get { return m_Cost2; } }

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

	}

}
