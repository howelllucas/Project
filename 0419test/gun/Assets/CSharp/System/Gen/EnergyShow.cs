using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class EnergyShow : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn2;
		public RectTransform_Button_Image_Container Btn2 { get { return m_Btn2; } }

		[SerializeField]
		private RectTransform_Text_Container m_Titletxt;
		public RectTransform_Text_Container Titletxt { get { return m_Titletxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_EnergyNum;
		public RectTransform_Text_Container EnergyNum { get { return m_EnergyNum; } }

		[SerializeField]
		private RectTransform_Image_Container m_Pronum;
		public RectTransform_Image_Container Pronum { get { return m_Pronum; } }

		[SerializeField]
		private RectTransform_Text_Container m_Energytxt;
		public RectTransform_Text_Container Energytxt { get { return m_Energytxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_AdEnergyNum;
		public RectTransform_Text_Container AdEnergyNum { get { return m_AdEnergyNum; } }

		[SerializeField]
		private RectTransform_Text_Container m_Timeneed;
		public RectTransform_Text_Container Timeneed { get { return m_Timeneed; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn1;
		public RectTransform_Button_Image_Container Btn1 { get { return m_Btn1; } }

		[SerializeField]
		private RectTransform_Text_Container m_Btn1txt;
		public RectTransform_Text_Container Btn1txt { get { return m_Btn1txt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn3;
		public RectTransform_Button_Image_Container Btn3 { get { return m_Btn3; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_AdBtn;
		public RectTransform_Button_Image_Container AdBtn { get { return m_AdBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_AdText;
		public RectTransform_Text_Container AdText { get { return m_AdText; } }

		[SerializeField]
		private RectTransform_Text_Container m_AdBtntxt;
		public RectTransform_Text_Container AdBtntxt { get { return m_AdBtntxt; } }

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
