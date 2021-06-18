using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class DebugUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn1;
		public RectTransform_Button_Image_Container Btn1 { get { return m_Btn1; } }

		[SerializeField]
		private RectTransform_Image_Container m_ModuleDropdown;
		public RectTransform_Image_Container ModuleDropdown { get { return m_ModuleDropdown; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ModuleBtn;
		public RectTransform_Button_Image_Container ModuleBtn { get { return m_ModuleBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_AllModuleBtn;
		public RectTransform_Button_Image_Container AllModuleBtn { get { return m_AllModuleBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_DialogueDropdown;
		public RectTransform_Image_Container DialogueDropdown { get { return m_DialogueDropdown; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_DialogueBtn;
		public RectTransform_Button_Image_Container DialogueBtn { get { return m_DialogueBtn; } }

		[SerializeField]
		private RectTransform_Slider_InputSlider_Container m_VibeDurSlider;
		public RectTransform_Slider_InputSlider_Container VibeDurSlider { get { return m_VibeDurSlider; } }

		[SerializeField]
		private RectTransform_InputField_Image_Container m_VibeDur;
		public RectTransform_InputField_Image_Container VibeDur { get { return m_VibeDur; } }

		[SerializeField]
		private RectTransform_Slider_InputSlider_Container m_VibeAmpSlider;
		public RectTransform_Slider_InputSlider_Container VibeAmpSlider { get { return m_VibeAmpSlider; } }

		[SerializeField]
		private RectTransform_InputField_Image_Container m_VibeAmp;
		public RectTransform_InputField_Image_Container VibeAmp { get { return m_VibeAmp; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_VibeBtn;
		public RectTransform_Button_Image_Container VibeBtn { get { return m_VibeBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_ItemDropdown;
		public RectTransform_Image_Container ItemDropdown { get { return m_ItemDropdown; } }

		[SerializeField]
		private RectTransform_Slider_InputSlider_Container m_ItemNumSlider;
		public RectTransform_Slider_InputSlider_Container ItemNumSlider { get { return m_ItemNumSlider; } }

		[SerializeField]
		private RectTransform_InputField_Image_Container m_ItemNum;
		public RectTransform_InputField_Image_Container ItemNum { get { return m_ItemNum; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ItemBtn;
		public RectTransform_Button_Image_Container ItemBtn { get { return m_ItemBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ItemReduceBtn;
		public RectTransform_Button_Image_Container ItemReduceBtn { get { return m_ItemReduceBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_PassDropdown;
		public RectTransform_Image_Container PassDropdown { get { return m_PassDropdown; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_MonsterBtn;
		public RectTransform_Button_Image_Container MonsterBtn { get { return m_MonsterBtn; } }

		[SerializeField]
		private RectTransform_Slider_InputSlider_Container m_PassNumSlider;
		public RectTransform_Slider_InputSlider_Container PassNumSlider { get { return m_PassNumSlider; } }

		[SerializeField]
		private RectTransform_InputField_Image_Container m_PassNum;
		public RectTransform_InputField_Image_Container PassNum { get { return m_PassNum; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_PassBtn;
		public RectTransform_Button_Image_Container PassBtn { get { return m_PassBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_PassCarDropdown;
		public RectTransform_Image_Container PassCarDropdown { get { return m_PassCarDropdown; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_PassCarBtn;
		public RectTransform_Button_Image_Container PassCarBtn { get { return m_PassCarBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_PassBreakOutDropdown;
		public RectTransform_Image_Container PassBreakOutDropdown { get { return m_PassBreakOutDropdown; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_PassBreakOutBtn;
		public RectTransform_Button_Image_Container PassBreakOutBtn { get { return m_PassBreakOutBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_JS1;
		public RectTransform_Button_Image_Container JS1 { get { return m_JS1; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_JS3;
		public RectTransform_Button_Image_Container JS3 { get { return m_JS3; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_NextDay;
		public RectTransform_Button_Image_Container NextDay { get { return m_NextDay; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_OnLineReward1;
		public RectTransform_Button_Image_Container OnLineReward1 { get { return m_OnLineReward1; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_OnLineReward3;
		public RectTransform_Button_Image_Container OnLineReward3 { get { return m_OnLineReward3; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_PassCheck;
		public RectTransform_Button_Image_Container PassCheck { get { return m_PassCheck; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Skill;
		public RectTransform_Button_Image_Container Skill { get { return m_Skill; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_FirstPurchase;
		public RectTransform_Button_Image_Container FirstPurchase { get { return m_FirstPurchase; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_MCampStep1;
		public RectTransform_Button_Image_Container MCampStep1 { get { return m_MCampStep1; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_MCampStep2;
		public RectTransform_Button_Image_Container MCampStep2 { get { return m_MCampStep2; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_MCampStep3;
		public RectTransform_Button_Image_Container MCampStep3 { get { return m_MCampStep3; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_MCampStep4;
		public RectTransform_Button_Image_Container MCampStep4 { get { return m_MCampStep4; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_MCampStep5;
		public RectTransform_Button_Image_Container MCampStep5 { get { return m_MCampStep5; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_MResetCampGuid;
		public RectTransform_Button_Image_Container MResetCampGuid { get { return m_MResetCampGuid; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_MResetBranck;
		public RectTransform_Button_Image_Container MResetBranck { get { return m_MResetBranck; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_MPlotTest;
		public RectTransform_Button_Image_Container MPlotTest { get { return m_MPlotTest; } }

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
		public class RectTransform_InputField_Image_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private InputField m_inputField;
			public InputField inputField { get { return m_inputField; } }

			[SerializeField]
			private Image m_image;
			public Image image { get { return m_image; } }

		}

		[System.Serializable]
		public class RectTransform_Slider_InputSlider_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private Slider m_slider;
			public Slider slider { get { return m_slider; } }

			[SerializeField]
			private InputSlider m_inputSlider;
			public InputSlider inputSlider { get { return m_inputSlider; } }

		}

	}

}
