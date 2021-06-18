using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CharUI_level_ItemUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_Itembtn;
		public RectTransform_Button_Image_Container Itembtn { get { return m_Itembtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_ItemIcon;
		public RectTransform_Image_Container ItemIcon { get { return m_ItemIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_ItemNameTxt;
		public RectTransform_Text_Container ItemNameTxt { get { return m_ItemNameTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_ItemExpTxt;
		public RectTransform_Text_Container ItemExpTxt { get { return m_ItemExpTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_CmIcon;
		public RectTransform_Image_Container CmIcon { get { return m_CmIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_CmNum;
		public RectTransform_Text_Container CmNum { get { return m_CmNum; } }

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
