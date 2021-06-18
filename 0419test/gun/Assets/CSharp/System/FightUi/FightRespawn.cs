using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class FightRespawn : BaseUi {

		[SerializeField]
		private RectTransform_Text_Container RemainSec;

		[SerializeField]
		private RectTransform_Button_Image_Container Btn1;

		[SerializeField]
		private RectTransform_Text_Container Btn1txt;

		[SerializeField]
		private RectTransform_Button_Image_Container ADBtn;

		[SerializeField]
		private RectTransform_Text_Button_Container CloseBtn;

		[System.Serializable]
		private class RectTransform_Button_Image_Container {

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
		private class RectTransform_Text_Button_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private Text m_text;
			public Text text { get { return m_text; } }

			[SerializeField]
			private Button m_button;
			public Button button { get { return m_button; } }

		}

		[System.Serializable]
		private class RectTransform_Text_Container {

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
