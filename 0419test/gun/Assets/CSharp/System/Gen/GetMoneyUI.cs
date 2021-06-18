using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GetMoneyUI : BaseUi {

		[SerializeField]
		private RectTransform_Text_Container m_Tips;
		public RectTransform_Text_Container Tips { get { return m_Tips; } }

		[SerializeField]
		private RectTransform_Image_Container m_Airdropiconbg;
		public RectTransform_Image_Container Airdropiconbg { get { return m_Airdropiconbg; } }

		[SerializeField]
		private RectTransform_Image_Container m_airdropicon;
		public RectTransform_Image_Container airdropicon { get { return m_airdropicon; } }

		[SerializeField]
		private RectTransform_Image_Container m_CmIcon;
		public RectTransform_Image_Container CmIcon { get { return m_CmIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_CmNum;
		public RectTransform_Text_Container CmNum { get { return m_CmNum; } }

		[SerializeField]
		private RectTransform_Image_Container m_MDTIcon;
		public RectTransform_Image_Container MDTIcon { get { return m_MDTIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_MDTNum;
		public RectTransform_Text_Container MDTNum { get { return m_MDTNum; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn1;
		public RectTransform_Button_Image_Container Btn1 { get { return m_Btn1; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn2;
		public RectTransform_Button_Image_Container Btn2 { get { return m_Btn2; } }

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
