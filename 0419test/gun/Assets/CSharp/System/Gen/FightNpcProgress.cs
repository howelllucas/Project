using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class FightNpcProgress : MonoBehaviour {

		[SerializeField]
		private RectTransform_Container m_TaskNode;
		public RectTransform_Container TaskNode { get { return m_TaskNode; } }

		[SerializeField]
		private RectTransform_Image_Container m_UnReceive;
		public RectTransform_Image_Container UnReceive { get { return m_UnReceive; } }

		[SerializeField]
		private RectTransform_Image_Container m_Received;
		public RectTransform_Image_Container Received { get { return m_Received; } }

		[System.Serializable]
		public class RectTransform_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

		}

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
