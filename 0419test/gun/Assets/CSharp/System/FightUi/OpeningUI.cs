using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class OpeningUI : BaseUi {

		[SerializeField]
		private RectTransform_Image_Container m_BackImg;
		public RectTransform_Image_Container BackImg { get { return m_BackImg; } }

		[SerializeField]
		private RectTransform_Text_Container m_Text1;
		public RectTransform_Text_Container Text1 { get { return m_Text1; } }

		[SerializeField]
		private RectTransform_Text_Container m_Text2;
		public RectTransform_Text_Container Text2 { get { return m_Text2; } }

		[SerializeField]
		private RectTransform_Text_Container m_Text3;
		public RectTransform_Text_Container Text3 { get { return m_Text3; } }

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
