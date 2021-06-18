using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public class FightUI_HeartItem : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_heartbreak;
		public RectTransform_Image_Container heartbreak { get { return m_heartbreak; } }

		[SerializeField]
		private RectTransform_Image_Container m_heart;
		public RectTransform_Image_Container heart { get { return m_heart; } }

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
