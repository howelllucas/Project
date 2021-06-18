using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public class RewardEffectUi_RewardEffect : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_M1;
		public RectTransform_Image_Container M1 { get { return m_M1; } }

		[SerializeField]
		private RectTransform_Image_Container m_M2;
		public RectTransform_Image_Container M2 { get { return m_M2; } }

		[SerializeField]
		private RectTransform_Image_Container m_M3;
		public RectTransform_Image_Container M3 { get { return m_M3; } }

		[SerializeField]
		private RectTransform_Image_Container m_M4;
		public RectTransform_Image_Container M4 { get { return m_M4; } }

		[SerializeField]
		private RectTransform_Image_Container m_M5;
		public RectTransform_Image_Container M5 { get { return m_M5; } }

		[SerializeField]
		private RectTransform_Image_Container m_M6;
		public RectTransform_Image_Container M6 { get { return m_M6; } }

		[SerializeField]
		private RectTransform_Image_Container m_M7;
		public RectTransform_Image_Container M7 { get { return m_M7; } }

		[SerializeField]
		private RectTransform_Image_Container m_M8;
		public RectTransform_Image_Container M8 { get { return m_M8; } }

		[SerializeField]
		private RectTransform_Image_Container m_M9;
		public RectTransform_Image_Container M9 { get { return m_M9; } }

		[SerializeField]
		private RectTransform_Image_Container m_M10;
		public RectTransform_Image_Container M10 { get { return m_M10; } }

		[SerializeField]
		private RectTransform_Image_Container m_M11;
		public RectTransform_Image_Container M11 { get { return m_M11; } }

		[SerializeField]
		private RectTransform_Image_Container m_M12;
		public RectTransform_Image_Container M12 { get { return m_M12; } }

		[SerializeField]
		private RectTransform_Image_Container m_M13;
		public RectTransform_Image_Container M13 { get { return m_M13; } }

		[SerializeField]
		private RectTransform_Image_Container m_M14;
		public RectTransform_Image_Container M14 { get { return m_M14; } }

		[SerializeField]
		private RectTransform_Image_Container m_M15;
		public RectTransform_Image_Container M15 { get { return m_M15; } }

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

	}

}
