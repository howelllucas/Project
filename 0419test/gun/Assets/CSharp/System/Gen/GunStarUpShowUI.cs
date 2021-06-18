using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GunStarUpShowUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseImg;
		public RectTransform_Button_Image_Container CloseImg { get { return m_CloseImg; } }

		[SerializeField]
		private RectTransform_Text_Container m_GunName;
		public RectTransform_Text_Container GunName { get { return m_GunName; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_IconBtn;
		public RectTransform_Button_Image_Container IconBtn { get { return m_IconBtn; } }

		[SerializeField]
		private RectTransform_Slider_Container m_CountSlider;
		public RectTransform_Slider_Container CountSlider { get { return m_CountSlider; } }

		[SerializeField]
		private RectTransform_Text_Container m_Count;
		public RectTransform_Text_Container Count { get { return m_Count; } }

		[SerializeField]
		private RectTransform_Text_Container m_StarTxt;
		public RectTransform_Text_Container StarTxt { get { return m_StarTxt; } }

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
		public class RectTransform_Slider_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private Slider m_slider;
			public Slider slider { get { return m_slider; } }

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
