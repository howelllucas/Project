using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class OfflineUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn1;
		public RectTransform_Button_Image_Container Btn1 { get { return m_Btn1; } }

		[SerializeField]
		private RectTransform_Image_Container m_Itemicon;
		public RectTransform_Image_Container Itemicon { get { return m_Itemicon; } }

		[SerializeField]
		private RectTransform_Image_Container m_Itemvalueicon;
		public RectTransform_Image_Container Itemvalueicon { get { return m_Itemvalueicon; } }

		[SerializeField]
		private RectTransform_Text_Container m_Itemvaluetxt;
		public RectTransform_Text_Container Itemvaluetxt { get { return m_Itemvaluetxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_AdBtn;
		public RectTransform_Button_Image_Container AdBtn { get { return m_AdBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_AdIcon;
		public RectTransform_Image_Container AdIcon { get { return m_AdIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_AdText;
		public RectTransform_Text_Container AdText { get { return m_AdText; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_MoneyBtn;
		public RectTransform_Button_Image_Container MoneyBtn { get { return m_MoneyBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_MoneyIcon;
		public RectTransform_Image_Container MoneyIcon { get { return m_MoneyIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_MoneyText;
		public RectTransform_Text_Container MoneyText { get { return m_MoneyText; } }

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
