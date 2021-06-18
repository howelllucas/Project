using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class DialogueUI : BaseUi {

		[SerializeField]
		private RectTransform_Container m_ShowNode;
		public RectTransform_Container ShowNode { get { return m_ShowNode; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BgBtn;
		public RectTransform_Button_Image_Container BgBtn { get { return m_BgBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_person1;
		public RectTransform_Image_Container person1 { get { return m_person1; } }

		[SerializeField]
		private RectTransform_Image_Container m_person2;
		public RectTransform_Image_Container person2 { get { return m_person2; } }

		[SerializeField]
		private RectTransform_Text_Container m_tip;
		public RectTransform_Text_Container tip { get { return m_tip; } }

		[SerializeField]
		private RectTransform_Image_Container m_nextIcon;
		public RectTransform_Image_Container nextIcon { get { return m_nextIcon; } }

		[SerializeField]
		private RectTransform_Image_Container m_namebg;
		public RectTransform_Image_Container namebg { get { return m_namebg; } }

		[SerializeField]
		private RectTransform_Text_Container m_nameText;
		public RectTransform_Text_Container nameText { get { return m_nameText; } }

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
