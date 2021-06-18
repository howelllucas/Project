using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GunResetUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_OkBtn;
		public RectTransform_Button_Image_Container OkBtn { get { return m_OkBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_ResetCost;
		public RectTransform_Text_Container ResetCost { get { return m_ResetCost; } }

		[SerializeField]
		private RectTransform_Text_Container m_GetGold;
		public RectTransform_Text_Container GetGold { get { return m_GetGold; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

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
