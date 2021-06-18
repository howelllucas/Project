using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class LevelDetail_progress : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_mask;
		public RectTransform_Image_Container mask { get { return m_mask; } }

		[SerializeField]
		private RectTransform_Image_Container m_bg;
		public RectTransform_Image_Container bg { get { return m_bg; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_RightGiftBg;
		public RectTransform_Button_Image_Container RightGiftBg { get { return m_RightGiftBg; } }

		[SerializeField]
		private RectTransform_Image_Container m_RightBgImg;
		public RectTransform_Image_Container RightBgImg { get { return m_RightBgImg; } }

		[SerializeField]
		private RectTransform_Image_Container m_RightAwardBg;
		public RectTransform_Image_Container RightAwardBg { get { return m_RightAwardBg; } }

		[SerializeField]
		private RectTransform_Image_Container m_RightGift;
		public RectTransform_Image_Container RightGift { get { return m_RightGift; } }

		[SerializeField]
		private RectTransform_Image_Container m_RightLevel;
		public RectTransform_Image_Container RightLevel { get { return m_RightLevel; } }

		[SerializeField]
		private RectTransform_Text_Container m_RightNum;
		public RectTransform_Text_Container RightNum { get { return m_RightNum; } }

		[SerializeField]
		private RectTransform_Image_Container m_RightOpened;
		public RectTransform_Image_Container RightOpened { get { return m_RightOpened; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_RightRetry;
		public RectTransform_Button_Image_Container RightRetry { get { return m_RightRetry; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_LeftGiftBg;
		public RectTransform_Button_Image_Container LeftGiftBg { get { return m_LeftGiftBg; } }

		[SerializeField]
		private RectTransform_Image_Container m_LeftBgImg;
		public RectTransform_Image_Container LeftBgImg { get { return m_LeftBgImg; } }

		[SerializeField]
		private RectTransform_Image_Container m_LeftAwardBg;
		public RectTransform_Image_Container LeftAwardBg { get { return m_LeftAwardBg; } }

		[SerializeField]
		private RectTransform_Image_Container m_LeftGift;
		public RectTransform_Image_Container LeftGift { get { return m_LeftGift; } }

		[SerializeField]
		private RectTransform_Image_Container m_LeftLevel;
		public RectTransform_Image_Container LeftLevel { get { return m_LeftLevel; } }

		[SerializeField]
		private RectTransform_Text_Container m_LeftNum;
		public RectTransform_Text_Container LeftNum { get { return m_LeftNum; } }

		[SerializeField]
		private RectTransform_Image_Container m_LeftOpened;
		public RectTransform_Image_Container LeftOpened { get { return m_LeftOpened; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_LeftRetry;
		public RectTransform_Button_Image_Container LeftRetry { get { return m_LeftRetry; } }

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
