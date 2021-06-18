using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GunUI_GunCard : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_Frame;
		public RectTransform_Image_Container Frame { get { return m_Frame; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_IconBtn;
		public RectTransform_Button_Image_Container IconBtn { get { return m_IconBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_TipsTitle;
		public RectTransform_Text_Container TipsTitle { get { return m_TipsTitle; } }

		[SerializeField]
		private RectTransform_Text_Container m_Level;
		public RectTransform_Text_Container Level { get { return m_Level; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon1;
		public RectTransform_Image_Container Icon1 { get { return m_Icon1; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon2;
		public RectTransform_Image_Container Icon2 { get { return m_Icon2; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon3;
		public RectTransform_Image_Container Icon3 { get { return m_Icon3; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon4;
		public RectTransform_Image_Container Icon4 { get { return m_Icon4; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon5;
		public RectTransform_Image_Container Icon5 { get { return m_Icon5; } }

		[SerializeField]
		private RectTransform_Image_Container m_TypeIcon;
		public RectTransform_Image_Container TypeIcon { get { return m_TypeIcon; } }

		[SerializeField]
		private RectTransform_Image_Container m_ChipIcon;
		public RectTransform_Image_Container ChipIcon { get { return m_ChipIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_ChipCount;
		public RectTransform_Text_Container ChipCount { get { return m_ChipCount; } }

		[SerializeField]
		private RectTransform_Container m_LvupIcon;
		public RectTransform_Container LvupIcon { get { return m_LvupIcon; } }

		[SerializeField]
		private RectTransform_Container m_StarupIcon;
		public RectTransform_Container StarupIcon { get { return m_StarupIcon; } }

		[SerializeField]
		private RectTransform_Image_Container m_OccupiedFlag;
		public RectTransform_Image_Container OccupiedFlag { get { return m_OccupiedFlag; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewIcon;
		public RectTransform_Image_Container NewIcon { get { return m_NewIcon; } }

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

	}

}
