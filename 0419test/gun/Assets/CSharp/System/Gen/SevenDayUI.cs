using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class SevenDayUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn1;
		public RectTransform_Button_Image_Container Btn1 { get { return m_Btn1; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_NormalBtn;
		public RectTransform_Button_Image_Container NormalBtn { get { return m_NormalBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_AdText;
		public RectTransform_Text_Container AdText { get { return m_AdText; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_AdBtn;
		public RectTransform_Button_Image_Container AdBtn { get { return m_AdBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_AdIcon;
		public RectTransform_Image_Container AdIcon { get { return m_AdIcon; } }

		[SerializeField]
		private RectTransform_Image_Container m_day1;
		public RectTransform_Image_Container day1 { get { return m_day1; } }

		[SerializeField]
		private RectTransform_Image_Container m_current;
		public RectTransform_Image_Container current { get { return m_current; } }

		[SerializeField]
		private RectTransform_Image_Container m_reward;
		public RectTransform_Image_Container reward { get { return m_reward; } }

		[SerializeField]
		private RectTransform_Image_Container m_rewardGun;
		public RectTransform_Image_Container rewardGun { get { return m_rewardGun; } }

		[SerializeField]
		private RectTransform_Text_Container m_Num;
		public RectTransform_Text_Container Num { get { return m_Num; } }

		[SerializeField]
		private RectTransform_Image_Container m_getMask;
		public RectTransform_Image_Container getMask { get { return m_getMask; } }

		[SerializeField]
		private RectTransform_Image_Container m_day2;
		public RectTransform_Image_Container day2 { get { return m_day2; } }

		[SerializeField]
		private RectTransform_Image_Container m_day3;
		public RectTransform_Image_Container day3 { get { return m_day3; } }

		[SerializeField]
		private RectTransform_Image_Container m_day4;
		public RectTransform_Image_Container day4 { get { return m_day4; } }

		[SerializeField]
		private RectTransform_Image_Container m_day5;
		public RectTransform_Image_Container day5 { get { return m_day5; } }

		[SerializeField]
		private RectTransform_Image_Container m_day6;
		public RectTransform_Image_Container day6 { get { return m_day6; } }

		[SerializeField]
		private RectTransform_Image_Container m_day7;
		public RectTransform_Image_Container day7 { get { return m_day7; } }

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
