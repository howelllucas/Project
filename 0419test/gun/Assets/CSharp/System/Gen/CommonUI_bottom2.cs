using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public class CommonUI_bottom2 : MonoBehaviour {

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btnshop;
		public RectTransform_Button_Image_Container Btnshop { get { return m_Btnshop; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btnrole;
		public RectTransform_Button_Image_Container Btnrole { get { return m_Btnrole; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btncombat;
		public RectTransform_Button_Image_Container Btncombat { get { return m_Btncombat; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btnweapon;
		public RectTransform_Button_Image_Container Btnweapon { get { return m_Btnweapon; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btnsupport;
		public RectTransform_Button_Image_Container Btnsupport { get { return m_Btnsupport; } }

		[SerializeField]
		private RectTransform_Text_Container m_Pagename;
		public RectTransform_Text_Container Pagename { get { return m_Pagename; } }

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
