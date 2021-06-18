using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampDetailUI_DetailItemUI : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_Icon;
		public RectTransform_Image_Container Icon { get { return m_Icon; } }

		[SerializeField]
		private RectTransform_Text_Container m_LockText;
		public RectTransform_Text_Container LockText { get { return m_LockText; } }

		[SerializeField]
		private RectTransform_Text_Container m_DName;
		public RectTransform_Text_Container DName { get { return m_DName; } }

		[SerializeField]
		private RectTransform_Image_Container m_Lock;
		public RectTransform_Image_Container Lock { get { return m_Lock; } }

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
