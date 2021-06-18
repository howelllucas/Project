using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class OpenMixBoxUI_card : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_Show;
		public RectTransform_Image_Container Show { get { return m_Show; } }

		[SerializeField]
		private RectTransform_Image_Container m_debris;
		public RectTransform_Image_Container debris { get { return m_debris; } }

		[SerializeField]
		private RectTransform_Image_Container m_icon;
		public RectTransform_Image_Container icon { get { return m_icon; } }

		[SerializeField]
		private RectTransform_Text_Container m_num;
		public RectTransform_Text_Container num { get { return m_num; } }

		[SerializeField]
		private RectTransform_Text_Container m_title;
		public RectTransform_Text_Container title { get { return m_title; } }

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
