using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class FightUI_Itemtime : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_Timepro;
		public RectTransform_Image_Container Timepro { get { return m_Timepro; } }

		[SerializeField]
		private RectTransform_Image_Container m_Itemicon;
		public RectTransform_Image_Container Itemicon { get { return m_Itemicon; } }

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
