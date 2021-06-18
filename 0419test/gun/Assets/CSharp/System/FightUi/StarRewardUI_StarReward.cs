using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class StarRewardUI_StarReward : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_FinishImg;
		public RectTransform_Image_Container FinishImg { get { return m_FinishImg; } }

		[SerializeField]
		private RectTransform_Text_Container m_StarCount;
		public RectTransform_Text_Container StarCount { get { return m_StarCount; } }

		[SerializeField]
		private RectTransform_Image_Container m_RewardIcon;
		public RectTransform_Image_Container RewardIcon { get { return m_RewardIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_RewardNum;
		public RectTransform_Text_Container RewardNum { get { return m_RewardNum; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_RewardBtn;
		public RectTransform_Button_Image_Container RewardBtn { get { return m_RewardBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_UnfinishImg;
		public RectTransform_Image_Container UnfinishImg { get { return m_UnfinishImg; } }

		[SerializeField]
		private RectTransform_Container m_GetFrame;
		public RectTransform_Container GetFrame { get { return m_GetFrame; } }

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
