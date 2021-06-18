using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class EvaluateUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnC;
		public RectTransform_Button_Image_Container BtnC { get { return m_BtnC; } }

		[SerializeField]
		private RectTransform_Text_Container m_desc;
		public RectTransform_Text_Container desc { get { return m_desc; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CancelBtn;
		public RectTransform_Button_Image_Container CancelBtn { get { return m_CancelBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ConfirmBtn;
		public RectTransform_Button_Image_Container ConfirmBtn { get { return m_ConfirmBtn; } }

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
