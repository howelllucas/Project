using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampBUFF_TopItemDetail : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_Box;
		public RectTransform_Image_Container Box { get { return m_Box; } }

		[SerializeField]
		private RectTransform_Image_Container m_arrow;
		public RectTransform_Image_Container arrow { get { return m_arrow; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon;
		public RectTransform_Image_Container Icon { get { return m_Icon; } }

		[SerializeField]
		private RectTransform_Text_Container m_level;
		public RectTransform_Text_Container level { get { return m_level; } }

		[SerializeField]
		private RectTransform_Text_Container m_name;
		public RectTransform_Text_Container name { get { return m_name; } }

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
