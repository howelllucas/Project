using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class HomeUI : BaseUi {

		[SerializeField]
		private RectTransform_Container m_FireAdapter;
		public RectTransform_Container FireAdapter { get { return m_FireAdapter; } }

		[SerializeField]
		private RectTransform_Container m_CampsiteBubbleRoot;
		public RectTransform_Container CampsiteBubbleRoot { get { return m_CampsiteBubbleRoot; } }

		[SerializeField]
		private RectTransform_Container m_BtnsRoot;
		public RectTransform_Container BtnsRoot { get { return m_BtnsRoot; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_DebugBtn;
		public RectTransform_Button_Image_Container DebugBtn { get { return m_DebugBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CampsiteClaimBtn;
		public RectTransform_Button_Image_Container CampsiteClaimBtn { get { return m_CampsiteClaimBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ExpeditionBtn;
		public RectTransform_Button_Image_Container ExpeditionBtn { get { return m_ExpeditionBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ClearDataBtn;
		public RectTransform_Button_Image_Container ClearDataBtn { get { return m_ClearDataBtn; } }

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

	}

}
