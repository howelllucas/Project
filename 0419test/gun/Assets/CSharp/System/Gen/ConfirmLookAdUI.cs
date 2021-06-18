using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class ConfirmLookAdUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn1;
		public RectTransform_Button_Image_Container Btn1 { get { return m_Btn1; } }

		[SerializeField]
		private RectTransform_Text_Container m_confirmtxt;
		public RectTransform_Text_Container confirmtxt { get { return m_confirmtxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_num;
		public RectTransform_Text_Container num { get { return m_num; } }

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
