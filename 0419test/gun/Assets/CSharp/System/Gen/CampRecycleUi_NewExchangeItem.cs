using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampRecycleUi_NewExchangeItem : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_Icon;
		public RectTransform_Image_Container Icon { get { return m_Icon; } }

		[SerializeField]
		private RectTransform_Text_Container m_Count;
		public RectTransform_Text_Container Count { get { return m_Count; } }

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
