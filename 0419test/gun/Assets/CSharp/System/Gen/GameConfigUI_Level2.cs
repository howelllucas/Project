using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GameConfigUI_Level2 : MonoBehaviour {

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnTS;
		public RectTransform_Button_Image_Container BtnTS { get { return m_BtnTS; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnPP;
		public RectTransform_Button_Image_Container BtnPP { get { return m_BtnPP; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnAC;
		public RectTransform_Button_Image_Container BtnAC { get { return m_BtnAC; } }

		[SerializeField]
		private RectTransform_Container m_DM;
		public RectTransform_Container DM { get { return m_DM; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnDM;
		public RectTransform_Button_Image_Container BtnDM { get { return m_BtnDM; } }

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
