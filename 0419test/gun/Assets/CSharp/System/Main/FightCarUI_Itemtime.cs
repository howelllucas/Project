using UnityEngine;
using UnityEngine.UI;

public partial class FightCarUI_Itemtime : MonoBehaviour {

	[SerializeField]
	private RectTransform_Image_Container Timepro;

	[SerializeField]
	private RectTransform_Image_Container Itemicon;

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
