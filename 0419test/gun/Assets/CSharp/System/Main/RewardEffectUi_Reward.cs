using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class RewardEffectUi_Reward : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container M1;

		[SerializeField]
		private RectTransform_Image_Container M2;

		[SerializeField]
		private RectTransform_Image_Container M3;

		[SerializeField]
		private RectTransform_Image_Container M4;

		[SerializeField]
		private RectTransform_Image_Container M5;

		[SerializeField]
		private RectTransform_Image_Container M6;

		[SerializeField]
		private RectTransform_Image_Container M7;

		[SerializeField]
		private RectTransform_Image_Container M8;

		[SerializeField]
		private RectTransform_Image_Container M9;

		[SerializeField]
		private RectTransform_Image_Container M10;

		[SerializeField]
		private RectTransform_Image_Container M11;

		[SerializeField]
		private RectTransform_Image_Container M12;

		[SerializeField]
		private RectTransform_Image_Container M13;

		[SerializeField]
		private RectTransform_Image_Container M14;

		[SerializeField]
		private RectTransform_Image_Container M15;

		[System.Serializable]
		private class RectTransform_Image_Container {

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
