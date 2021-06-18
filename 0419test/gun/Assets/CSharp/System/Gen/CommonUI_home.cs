using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public class CommonUI_home : MonoBehaviour {

		[SerializeField]
		private RectTransform_Button_Image_Container m_icon;
		public RectTransform_Button_Image_Container icon { get { return m_icon; } }

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

	}

}
