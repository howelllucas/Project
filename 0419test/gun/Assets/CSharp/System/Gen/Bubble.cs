using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class Bubble : BaseUi {

		[SerializeField]
		private RectTransform_Image_Container m_Arrow;
		public RectTransform_Image_Container Arrow { get { return m_Arrow; } }

		[SerializeField]
		private RectTransform_Image_Container m_Content;
		public RectTransform_Image_Container Content { get { return m_Content; } }

		[SerializeField]
		private RectTransform_Text_Container m_desc;
		public RectTransform_Text_Container desc { get { return m_desc; } }

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
