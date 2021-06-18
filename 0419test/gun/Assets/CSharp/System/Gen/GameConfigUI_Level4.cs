using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GameConfigUI_Level4 : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_ScrollView;
		public RectTransform_Image_Container ScrollView { get { return m_ScrollView; } }

		[SerializeField]
		private RectTransform_Container m_Content;
		public RectTransform_Container Content { get { return m_Content; } }

		[SerializeField]
		private RectTransform_Text_Container m_desc;
		public RectTransform_Text_Container desc { get { return m_desc; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ConfirmBtn;
		public RectTransform_Button_Image_Container ConfirmBtn { get { return m_ConfirmBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CancelBtn;
		public RectTransform_Button_Image_Container CancelBtn { get { return m_CancelBtn; } }

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
