using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class ExpeditionUI_LevelItem : MonoBehaviour {

		[SerializeField]
		private RectTransform_Button_Image_Container m_PassFlag;
		public RectTransform_Button_Image_Container PassFlag { get { return m_PassFlag; } }

		[SerializeField]
		private RectTransform_Image_Container m_Star1;
		public RectTransform_Image_Container Star1 { get { return m_Star1; } }

		[SerializeField]
		private RectTransform_Image_Container m_Star2;
		public RectTransform_Image_Container Star2 { get { return m_Star2; } }

		[SerializeField]
		private RectTransform_Image_Container m_Star3;
		public RectTransform_Image_Container Star3 { get { return m_Star3; } }

		[SerializeField]
		private RectTransform_Image_Container m_CurFlag;
		public RectTransform_Image_Container CurFlag { get { return m_CurFlag; } }

		[SerializeField]
		private RectTransform_Image_Container m_WillFlag;
		public RectTransform_Image_Container WillFlag { get { return m_WillFlag; } }

		[SerializeField]
		private RectTransform_Text_Container m_LvTxt;
		public RectTransform_Text_Container LvTxt { get { return m_LvTxt; } }

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
