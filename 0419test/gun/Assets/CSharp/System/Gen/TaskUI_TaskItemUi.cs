using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class TaskUI_TaskItemUi : MonoBehaviour {

		[SerializeField]
		private RectTransform_Button_Image_Container m_TaskBtn;
		public RectTransform_Button_Image_Container TaskBtn { get { return m_TaskBtn; } }

		[SerializeField]
		private RectTransform_Container m_taskProgress;
		public RectTransform_Container taskProgress { get { return m_taskProgress; } }

		[SerializeField]
		private RectTransform_Image_Container m_passProgress;
		public RectTransform_Image_Container passProgress { get { return m_passProgress; } }

		[SerializeField]
		private RectTransform_Text_Container m_prNum;
		public RectTransform_Text_Container prNum { get { return m_prNum; } }

		[SerializeField]
		private RectTransform_Text_Container m_TitleText;
		public RectTransform_Text_Container TitleText { get { return m_TitleText; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_FinishTaskBtn;
		public RectTransform_Button_Image_Container FinishTaskBtn { get { return m_FinishTaskBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_FinishText;
		public RectTransform_Text_Container FinishText { get { return m_FinishText; } }

		[SerializeField]
		private RectTransform_Image_Container m_icon;
		public RectTransform_Image_Container icon { get { return m_icon; } }

		[SerializeField]
		private RectTransform_Text_Container m_Num;
		public RectTransform_Text_Container Num { get { return m_Num; } }

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
