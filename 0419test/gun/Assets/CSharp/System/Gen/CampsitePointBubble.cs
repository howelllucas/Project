using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampsitePointBubble : MonoBehaviour {

		[SerializeField]
		private RectTransform_Container m_LockRoot;
		public RectTransform_Container LockRoot { get { return m_LockRoot; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_UnlockBtn;
		public RectTransform_Button_Image_Container UnlockBtn { get { return m_UnlockBtn; } }

		[SerializeField]
		private RectTransform_Container m_UnlockRoot;
		public RectTransform_Container UnlockRoot { get { return m_UnlockRoot; } }

		[SerializeField]
		private RectTransform_Container m_RewardNode;
		public RectTransform_Container RewardNode { get { return m_RewardNode; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_RewardBtn;
		public RectTransform_Button_Image_Container RewardBtn { get { return m_RewardBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_RewardFlag;
		public RectTransform_Image_Container RewardFlag { get { return m_RewardFlag; } }

		[SerializeField]
		private RectTransform_Text_Container m_RewardCount;
		public RectTransform_Text_Container RewardCount { get { return m_RewardCount; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CardBtn;
		public RectTransform_Button_Image_Container CardBtn { get { return m_CardBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_ProgressImg;
		public RectTransform_Image_Container ProgressImg { get { return m_ProgressImg; } }

		[SerializeField]
		private RectTransform_Image_Container m_CardIconImg;
		public RectTransform_Image_Container CardIconImg { get { return m_CardIconImg; } }

		[SerializeField]
		private RectTransform_Container m_RedPoint;
		public RectTransform_Container RedPoint { get { return m_RedPoint; } }

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

		[System.Serializable]
		public class RectTransform_Text_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private Text m_text;
			public Text text { get { return m_text; } }

		}

	}

}
