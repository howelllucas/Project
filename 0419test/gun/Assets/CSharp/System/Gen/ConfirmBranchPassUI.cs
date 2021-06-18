using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class ConfirmBranchPassUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_Close;
		public RectTransform_Button_Image_Container Close { get { return m_Close; } }

		[SerializeField]
		private RectTransform_Text_Container m_confirmtxt;
		public RectTransform_Text_Container confirmtxt { get { return m_confirmtxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_DelayBtn;
		public RectTransform_Button_Image_Container DelayBtn { get { return m_DelayBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Confirm;
		public RectTransform_Button_Image_Container Confirm { get { return m_Confirm; } }

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
