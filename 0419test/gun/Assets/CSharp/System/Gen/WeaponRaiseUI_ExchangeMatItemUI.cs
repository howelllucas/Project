using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class WeaponRaiseUI_ExchangeMatItemUI : MonoBehaviour {

		[SerializeField]
		private RectTransform_Button_Image_NewbieGuideButton_Container m_MatItemBtn;
		public RectTransform_Button_Image_NewbieGuideButton_Container MatItemBtn { get { return m_MatItemBtn; } }

		[SerializeField]
		private RectTransform_Container m_UpNode;
		public RectTransform_Container UpNode { get { return m_UpNode; } }

		[SerializeField]
		private RectTransform_Text_Container m_ExchangeCount;
		public RectTransform_Text_Container ExchangeCount { get { return m_ExchangeCount; } }

		[SerializeField]
		private RectTransform_Image_Container m_MatIcon;
		public RectTransform_Image_Container MatIcon { get { return m_MatIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_MatName;
		public RectTransform_Text_Container MatName { get { return m_MatName; } }

		[SerializeField]
		private RectTransform_Container m_ExchangeNode;
		public RectTransform_Container ExchangeNode { get { return m_ExchangeNode; } }

		[SerializeField]
		private RectTransform_Image_Container m_HeartIcon;
		public RectTransform_Image_Container HeartIcon { get { return m_HeartIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_HeartCount;
		public RectTransform_Text_Container HeartCount { get { return m_HeartCount; } }

		[SerializeField]
		private RectTransform_Image_Container m_SellOut;
		public RectTransform_Image_Container SellOut { get { return m_SellOut; } }

		[System.Serializable]
		public class RectTransform_Button_Image_NewbieGuideButton_Container {

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

			[SerializeField]
			private NewbieGuideButton m_newbieGuideButton;
			public NewbieGuideButton newbieGuideButton { get { return m_newbieGuideButton; } }

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
