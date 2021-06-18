using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampSkill : MonoBehaviour {

		[SerializeField]
		private RectTransform_Button_Image_Container m_IconBtn;
		public RectTransform_Button_Image_Container IconBtn { get { return m_IconBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_LvTxt;
		public RectTransform_Text_Container LvTxt { get { return m_LvTxt; } }

		[SerializeField]
		private RectTransform_Container m_DescNode;
		public RectTransform_Container DescNode { get { return m_DescNode; } }

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
