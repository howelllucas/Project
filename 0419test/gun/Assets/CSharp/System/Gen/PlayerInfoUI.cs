using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class PlayerInfoUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_RateValue;
		public RectTransform_Text_Container RateValue { get { return m_RateValue; } }

		[SerializeField]
		private RectTransform_Text_Container m_CampValue;
		public RectTransform_Text_Container CampValue { get { return m_CampValue; } }

		[SerializeField]
		private RectTransform_Text_Container m_LevelValue;
		public RectTransform_Text_Container LevelValue { get { return m_LevelValue; } }

		[SerializeField]
		private RectTransform_Text_Container m_DPSValue;
		public RectTransform_Text_Container DPSValue { get { return m_DPSValue; } }

		[SerializeField]
		private RectTransform_Text_Container m_HpValue;
		public RectTransform_Text_Container HpValue { get { return m_HpValue; } }

		[SerializeField]
		private RectTransform_Text_Container m_AtkValue;
		public RectTransform_Text_Container AtkValue { get { return m_AtkValue; } }

		[SerializeField]
		private RectTransform_Text_Container m_SpeedValue;
		public RectTransform_Text_Container SpeedValue { get { return m_SpeedValue; } }

		[SerializeField]
		private RectTransform_Text_Container m_CritValue;
		public RectTransform_Text_Container CritValue { get { return m_CritValue; } }

		[SerializeField]
		private RectTransform_Text_Container m_CritDamageValue;
		public RectTransform_Text_Container CritDamageValue { get { return m_CritDamageValue; } }

		[SerializeField]
		private RectTransform_Text_Container m_DodgeValue;
		public RectTransform_Text_Container DodgeValue { get { return m_DodgeValue; } }

		[SerializeField]
		private RectTransform_Text_Container m_AtkSpeedValue;
		public RectTransform_Text_Container AtkSpeedValue { get { return m_AtkSpeedValue; } }

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
