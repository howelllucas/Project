using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampsiteOfflineRewardUI : BaseUi {

		[SerializeField]
		private RectTransform_Text_Container m_RewardValTxt;
		public RectTransform_Text_Container RewardValTxt { get { return m_RewardValTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_OfflineTimeTxt;
		public RectTransform_Text_Container OfflineTimeTxt { get { return m_OfflineTimeTxt; } }

		[SerializeField]
		private RectTransform_Slider_Container m_OfflineTimeBar;
		public RectTransform_Slider_Container OfflineTimeBar { get { return m_OfflineTimeBar; } }

		[SerializeField]
		private RectTransform_Text_Container m_MinHourTxt;
		public RectTransform_Text_Container MinHourTxt { get { return m_MinHourTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_MaxHourTxt;
		public RectTransform_Text_Container MaxHourTxt { get { return m_MaxHourTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_MaxDurationDocTxt;
		public RectTransform_Text_Container MaxDurationDocTxt { get { return m_MaxDurationDocTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_CollectCardCountTxt;
		public RectTransform_Text_Container CollectCardCountTxt { get { return m_CollectCardCountTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ClaimBtn;
		public RectTransform_Button_Image_Container ClaimBtn { get { return m_ClaimBtn; } }

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
