using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class OpenBoxUI : BaseUi {

		[SerializeField]
		private RectTransform_Container m_AllNode;
		public RectTransform_Container AllNode { get { return m_AllNode; } }

		[SerializeField]
		private RectTransform_Image_Container m_Bg;
		public RectTransform_Image_Container Bg { get { return m_Bg; } }

		[SerializeField]
		private RectTransform_Image_Container m_ConsumeIcon;
		public RectTransform_Image_Container ConsumeIcon { get { return m_ConsumeIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_ConsumeValue;
		public RectTransform_Text_Container ConsumeValue { get { return m_ConsumeValue; } }

		[SerializeField]
		private RectTransform_Image_Container m_box_close;
		public RectTransform_Image_Container box_close { get { return m_box_close; } }

		[SerializeField]
		private RectTransform_Image_Container m_box_open;
		public RectTransform_Image_Container box_open { get { return m_box_open; } }

		[SerializeField]
		private Transform_Container m_UI_openbox;
		public Transform_Container UI_openbox { get { return m_UI_openbox; } }

		[SerializeField]
		private Transform_Container m_UI_openbox_white;
		public Transform_Container UI_openbox_white { get { return m_UI_openbox_white; } }

		[SerializeField]
		private Transform_Container m_UI_openbox_purple;
		public Transform_Container UI_openbox_purple { get { return m_UI_openbox_purple; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_OpenBtn;
		public RectTransform_Button_Image_Container OpenBtn { get { return m_OpenBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn1;
		public RectTransform_Button_Image_Container Btn1 { get { return m_Btn1; } }

		[SerializeField]
		private RectTransform_Image_Container m_CmIcon;
		public RectTransform_Image_Container CmIcon { get { return m_CmIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_CmNum;
		public RectTransform_Text_Container CmNum { get { return m_CmNum; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn2;
		public RectTransform_Button_Image_Container Btn2 { get { return m_Btn2; } }

		[SerializeField]
		private RectTransform_Image_Container m_CmIconAd;
		public RectTransform_Image_Container CmIconAd { get { return m_CmIconAd; } }

		[SerializeField]
		private RectTransform_Text_Container m_CmNumAd;
		public RectTransform_Text_Container CmNumAd { get { return m_CmNumAd; } }

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
		public class RectTransform_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

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

		[System.Serializable]
		public class Transform_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private Transform m_transform;
			public Transform transform { get { return m_transform; } }

		}

	}

}
