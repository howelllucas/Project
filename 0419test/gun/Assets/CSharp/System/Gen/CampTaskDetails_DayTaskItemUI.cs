using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampTaskDetails_DayTaskItemUI : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_Award;
		public RectTransform_Image_Container Award { get { return m_Award; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn;
		public RectTransform_Button_Image_Container Btn { get { return m_Btn; } }

		[SerializeField]
		private RectTransform_Image_Container m_OnGoing;
		public RectTransform_Image_Container OnGoing { get { return m_OnGoing; } }

		[SerializeField]
		private RectTransform_Image_Container m_AwardIcon;
		public RectTransform_Image_Container AwardIcon { get { return m_AwardIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_AwardAmount;
		public RectTransform_Text_Container AwardAmount { get { return m_AwardAmount; } }

		[SerializeField]
		private RectTransform_Image_Container m_Received;
		public RectTransform_Image_Container Received { get { return m_Received; } }

		[SerializeField]
		private RectTransform_Image_Container m_Progress;
		public RectTransform_Image_Container Progress { get { return m_Progress; } }

		[SerializeField]
		private RectTransform_Text_Container m_Amount;
		public RectTransform_Text_Container Amount { get { return m_Amount; } }

		[SerializeField]
		private RectTransform_Text_Container m_IName;
		public RectTransform_Text_Container IName { get { return m_IName; } }

		[SerializeField]
		private RectTransform_Image_Container m_TargetIcon;
		public RectTransform_Image_Container TargetIcon { get { return m_TargetIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_TaskTargetText;
		public RectTransform_Text_Container TaskTargetText { get { return m_TaskTargetText; } }

		[SerializeField]
		private RectTransform_Text_Container m_TaskTargetAmount;
		public RectTransform_Text_Container TaskTargetAmount { get { return m_TaskTargetAmount; } }

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
