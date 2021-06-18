using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GameConfigUI_Level3 : MonoBehaviour {

		[SerializeField]
		private RectTransform_Container m_OOIA;
		public RectTransform_Container OOIA { get { return m_OOIA; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnOOIA;
		public RectTransform_Button_Image_Container BtnOOIA { get { return m_BtnOOIA; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnDMD;
		public RectTransform_Button_Image_Container BtnDMD { get { return m_BtnDMD; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ReturnBtn;
		public RectTransform_Button_Image_Container ReturnBtn { get { return m_ReturnBtn; } }

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

	}

}
