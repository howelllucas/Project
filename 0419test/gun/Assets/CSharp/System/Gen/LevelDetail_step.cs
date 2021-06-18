using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class LevelDetail_step : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_StepIcon;
		public RectTransform_Image_Container StepIcon { get { return m_StepIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_StepName;
		public RectTransform_Text_Container StepName { get { return m_StepName; } }

		[SerializeField]
		private RectTransform_Image_Container m_GunItem;
		public RectTransform_Image_Container GunItem { get { return m_GunItem; } }

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
