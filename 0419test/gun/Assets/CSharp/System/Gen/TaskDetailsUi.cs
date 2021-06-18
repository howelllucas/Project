using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class TaskDetailsUi : BaseUi {

		[SerializeField]
		private RectTransform_Container m_TaskAdaptNode;
		public RectTransform_Container TaskAdaptNode { get { return m_TaskAdaptNode; } }

		[SerializeField]
		private RectTransform_Text_Container m_TitleText;
		public RectTransform_Text_Container TitleText { get { return m_TitleText; } }

		[SerializeField]
		private RectTransform_Text_Container m_TaskText;
		public RectTransform_Text_Container TaskText { get { return m_TaskText; } }

		[SerializeField]
		private RectTransform_Image_Container m_taskIcon;
		public RectTransform_Image_Container taskIcon { get { return m_taskIcon; } }

		[SerializeField]
		private RectTransform_Image_Container m_passProgress;
		public RectTransform_Image_Container passProgress { get { return m_passProgress; } }

		[SerializeField]
		private RectTransform_Text_Container m_prNum;
		public RectTransform_Text_Container prNum { get { return m_prNum; } }

		[SerializeField]
		private RectTransform_Image_Container m_icon;
		public RectTransform_Image_Container icon { get { return m_icon; } }

		[SerializeField]
		private RectTransform_Text_Container m_Num;
		public RectTransform_Text_Container Num { get { return m_Num; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_WaitRewardBtn;
		public RectTransform_Button_Image_Container WaitRewardBtn { get { return m_WaitRewardBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_RewardBtn;
		public RectTransform_Button_Image_Container RewardBtn { get { return m_RewardBtn; } }

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
