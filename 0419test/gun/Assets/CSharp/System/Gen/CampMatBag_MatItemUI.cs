using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampMatBag_MatItemUI : MonoBehaviour {

		[SerializeField]
		private RectTransform_Button_Image_Container m_MatItemBtn;
		public RectTransform_Button_Image_Container MatItemBtn { get { return m_MatItemBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_MatName;
		public RectTransform_Text_Container MatName { get { return m_MatName; } }

		[SerializeField]
		private RectTransform_Image_Container m_MatIcon;
		public RectTransform_Image_Container MatIcon { get { return m_MatIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_CurCount;
		public RectTransform_Text_Container CurCount { get { return m_CurCount; } }

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
