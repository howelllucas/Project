using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class ConfirmUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn1;
		public RectTransform_Button_Image_Container Btn1 { get { return m_Btn1; } }

		[SerializeField]
		private RectTransform_Image_Container m_Itemicon;
		public RectTransform_Image_Container Itemicon { get { return m_Itemicon; } }

		[SerializeField]
		private RectTransform_Image_Container m_Moneyicon;
		public RectTransform_Image_Container Moneyicon { get { return m_Moneyicon; } }

		[SerializeField]
		private RectTransform_Text_Container m_Moneycost;
		public RectTransform_Text_Container Moneycost { get { return m_Moneycost; } }

		[SerializeField]
		private RectTransform_Image_Container m_Itemvalueicon;
		public RectTransform_Image_Container Itemvalueicon { get { return m_Itemvalueicon; } }

		[SerializeField]
		private RectTransform_Text_Container m_Itemvaluetxt;
		public RectTransform_Text_Container Itemvaluetxt { get { return m_Itemvaluetxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_Confirmtxt3;
		public RectTransform_Text_Container Confirmtxt3 { get { return m_Confirmtxt3; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn2;
		public RectTransform_Button_Image_Container Btn2 { get { return m_Btn2; } }

		[SerializeField]
		private RectTransform_Image_Container m_Moneyiconbtn;
		public RectTransform_Image_Container Moneyiconbtn { get { return m_Moneyiconbtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_Moneycostbtn;
		public RectTransform_Text_Container Moneycostbtn { get { return m_Moneycostbtn; } }

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
