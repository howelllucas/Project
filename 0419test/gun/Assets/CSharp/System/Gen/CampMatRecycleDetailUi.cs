using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampMatRecycleDetailUi : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_MatName;
		public RectTransform_Text_Container MatName { get { return m_MatName; } }

		[SerializeField]
		private RectTransform_Image_Container m_MatIcon;
		public RectTransform_Image_Container MatIcon { get { return m_MatIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_MatPrice;
		public RectTransform_Text_Container MatPrice { get { return m_MatPrice; } }

		[SerializeField]
		private RectTransform_Text_Container m_MatDes;
		public RectTransform_Text_Container MatDes { get { return m_MatDes; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Reduce10;
		public RectTransform_Button_Image_Container Reduce10 { get { return m_Reduce10; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Reduce1;
		public RectTransform_Button_Image_Container Reduce1 { get { return m_Reduce1; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Add1;
		public RectTransform_Button_Image_Container Add1 { get { return m_Add1; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Max;
		public RectTransform_Button_Image_Container Max { get { return m_Max; } }

		[SerializeField]
		private RectTransform_Text_Container m_CountText;
		public RectTransform_Text_Container CountText { get { return m_CountText; } }

		[SerializeField]
		private RectTransform_Text_Container m_GoldCount;
		public RectTransform_Text_Container GoldCount { get { return m_GoldCount; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ConfirmBtn;
		public RectTransform_Button_Image_Container ConfirmBtn { get { return m_ConfirmBtn; } }

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
