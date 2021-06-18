using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class LevelInfoUI : BaseUi {

		[SerializeField]
		private RectTransform_Text_Container m_LevelTitle;
		public RectTransform_Text_Container LevelTitle { get { return m_LevelTitle; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_Star1;
		public RectTransform_Image_Container Star1 { get { return m_Star1; } }

		[SerializeField]
		private RectTransform_Text_Container m_StarText1;
		public RectTransform_Text_Container StarText1 { get { return m_StarText1; } }

		[SerializeField]
		private RectTransform_Image_Container m_Star2;
		public RectTransform_Image_Container Star2 { get { return m_Star2; } }

		[SerializeField]
		private RectTransform_Text_Container m_StarText2;
		public RectTransform_Text_Container StarText2 { get { return m_StarText2; } }

		[SerializeField]
		private RectTransform_Image_Container m_Star3;
		public RectTransform_Image_Container Star3 { get { return m_Star3; } }

		[SerializeField]
		private RectTransform_Text_Container m_StarText3;
		public RectTransform_Text_Container StarText3 { get { return m_StarText3; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_StartBtn;
		public RectTransform_Button_Image_Container StartBtn { get { return m_StartBtn; } }

		[SerializeField]
		private RectTransform_Container m_GoldReward;
		public RectTransform_Container GoldReward { get { return m_GoldReward; } }

		[SerializeField]
		private RectTransform_Text_Container m_GoldCount;
		public RectTransform_Text_Container GoldCount { get { return m_GoldCount; } }

		[SerializeField]
		private RectTransform_Container m_LevelReward;
		public RectTransform_Container LevelReward { get { return m_LevelReward; } }

		[SerializeField]
		private RectTransform_Image_Container m_LevelRewardIcon;
		public RectTransform_Image_Container LevelRewardIcon { get { return m_LevelRewardIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_LevelRewardCount;
		public RectTransform_Text_Container LevelRewardCount { get { return m_LevelRewardCount; } }

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
