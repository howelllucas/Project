using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class TaskStateNode : MonoBehaviour {

		[SerializeField]
		private RectTransform_Container m_TaskNode;
		public RectTransform_Container TaskNode { get { return m_TaskNode; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_TaskStateBg;
		public RectTransform_Button_Image_Container TaskStateBg { get { return m_TaskStateBg; } }

		[SerializeField]
		private RectTransform_Image_Container m_Received;
		public RectTransform_Image_Container Received { get { return m_Received; } }

		[SerializeField]
		private RectTransform_Image_Container m_UnReceive;
		public RectTransform_Image_Container UnReceive { get { return m_UnReceive; } }

		[SerializeField]
		private RectTransform_Image_Container m_Complet;
		public RectTransform_Image_Container Complet { get { return m_Complet; } }

		[SerializeField]
		private RectTransform_Container m_RewardNode;
		public RectTransform_Container RewardNode { get { return m_RewardNode; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_RewardStateBg;
		public RectTransform_Button_Image_Container RewardStateBg { get { return m_RewardStateBg; } }

		[SerializeField]
		private RectTransform_Image_Container m_Reward;
		public RectTransform_Image_Container Reward { get { return m_Reward; } }

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
