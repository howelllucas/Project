using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class RewardEffectUi_RewardItem : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container Icon;

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
