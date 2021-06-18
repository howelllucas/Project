using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GunCardIcon : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_Frame;
		public RectTransform_Image_Container Frame { get { return m_Frame; } }

		[SerializeField]
		private RectTransform_Image_Container m_IconBtn;
		public RectTransform_Image_Container IconBtn { get { return m_IconBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_TipsTitle;
		public RectTransform_Text_Container TipsTitle { get { return m_TipsTitle; } }

		[SerializeField]
		private RectTransform_Text_Container m_CardCount;
		public RectTransform_Text_Container CardCount { get { return m_CardCount; } }

		[SerializeField]
		private RectTransform_Image_Container m_ChipIcon;
		public RectTransform_Image_Container ChipIcon { get { return m_ChipIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_ChipCount;
		public RectTransform_Text_Container ChipCount { get { return m_ChipCount; } }

		[SerializeField]
		private RectTransform_Image_Container m_TypeIcon;
		public RectTransform_Image_Container TypeIcon { get { return m_TypeIcon; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewIcon;
		public RectTransform_Image_Container NewIcon { get { return m_NewIcon; } }

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
