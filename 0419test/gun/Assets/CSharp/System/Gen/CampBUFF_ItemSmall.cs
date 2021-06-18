using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampBUFF_ItemSmall : MonoBehaviour {

		[SerializeField]
		private RectTransform_Text_Container m_name;
		public RectTransform_Text_Container name { get { return m_name; } }

		[SerializeField]
		private RectTransform_Image_Container m_goods;
		public RectTransform_Image_Container goods { get { return m_goods; } }

		[SerializeField]
		private RectTransform_Image_Container m_numberBg;
		public RectTransform_Image_Container numberBg { get { return m_numberBg; } }

		[SerializeField]
		private RectTransform_Text_Container m_text;
		public RectTransform_Text_Container text { get { return m_text; } }

		[SerializeField]
		private RectTransform_Image_Container m_lvBg;
		public RectTransform_Image_Container lvBg { get { return m_lvBg; } }

		[SerializeField]
		private RectTransform_Text_Container m_level;
		public RectTransform_Text_Container level { get { return m_level; } }

		[SerializeField]
		private RectTransform_Image_Container m_lockIcon;
		public RectTransform_Image_Container lockIcon { get { return m_lockIcon; } }

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
