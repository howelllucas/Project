using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class FightPause : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container BtnvolOff;

		[SerializeField]
		private RectTransform_Button_Image_Container BtnvolOn;

		[SerializeField]
		private RectTransform_Button_Image_Container BtnmusicOff;

		[SerializeField]
		private RectTransform_Button_Image_Container BtnmusicOn;

		[SerializeField]
		private RectTransform_Button_Image_Container Btn1;

		[SerializeField]
		private RectTransform_Button_Image_Container Btn2;

		[SerializeField]
		private RectTransform_Button_Image_Container BtnC;

		[SerializeField]
		private RectTransform_Button_Image_Container BtnVibeOff;

		[SerializeField]
		private RectTransform_Button_Image_Container BtnVibeOn;

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

	}

}
