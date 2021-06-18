using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class LanguageConfigUI_BtnLanguage : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_active;
		public RectTransform_Image_Container active { get { return m_active; } }

		[SerializeField]
		private RectTransform_Text_Container m_Ontxt;
		public RectTransform_Text_Container Ontxt { get { return m_Ontxt; } }

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
