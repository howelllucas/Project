using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GunChipUI_GunChip : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_Frame;
		public RectTransform_Image_Container Frame { get { return m_Frame; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_IconBtn;
		public RectTransform_Button_Image_Container IconBtn { get { return m_IconBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_TipsTitle;
		public RectTransform_Text_Container TipsTitle { get { return m_TipsTitle; } }

		[SerializeField]
		private RectTransform_Image_Container m_TypeIcon;
		public RectTransform_Image_Container TypeIcon { get { return m_TypeIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_TypeTxt;
		public RectTransform_Text_Container TypeTxt { get { return m_TypeTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_ChipCount;
		public RectTransform_Text_Container ChipCount { get { return m_ChipCount; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_SubBtn;
		public RectTransform_Button_Image_Container SubBtn { get { return m_SubBtn; } }

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
