using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class TokenUI : BaseUi {

		[SerializeField]
		private RectTransform_Image_Container m_Bg;
		public RectTransform_Image_Container Bg { get { return m_Bg; } }

		[SerializeField]
		private RectTransform_Container m_CurrencysNode;
		public RectTransform_Container CurrencysNode { get { return m_CurrencysNode; } }

		[SerializeField]
		private RectTransform_Image_Container m_CampRewardNode;
		public RectTransform_Image_Container CampRewardNode { get { return m_CampRewardNode; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CampRewardAddBtn;
		public RectTransform_Button_Image_Container CampRewardAddBtn { get { return m_CampRewardAddBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_CampRewardIcon;
		public RectTransform_Image_Container CampRewardIcon { get { return m_CampRewardIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_CampRewardTxt;
		public RectTransform_Text_Container CampRewardTxt { get { return m_CampRewardTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_GoldNode;
		public RectTransform_Image_Container GoldNode { get { return m_GoldNode; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_GoldAddBtn;
		public RectTransform_Button_Image_Container GoldAddBtn { get { return m_GoldAddBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_GoldIcon;
		public RectTransform_Image_Container GoldIcon { get { return m_GoldIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_GoldTxt;
		public RectTransform_Text_Container GoldTxt { get { return m_GoldTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_DiamondNode;
		public RectTransform_Image_Container DiamondNode { get { return m_DiamondNode; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_DiamondAddBtn;
		public RectTransform_Button_Image_Container DiamondAddBtn { get { return m_DiamondAddBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_DiamondIcon;
		public RectTransform_Image_Container DiamondIcon { get { return m_DiamondIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_DiamondTxt;
		public RectTransform_Text_Container DiamondTxt { get { return m_DiamondTxt; } }

		[SerializeField]
		private RectTransform_Container m_PlayerNode;
		public RectTransform_Container PlayerNode { get { return m_PlayerNode; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_PlayerIconBtn;
		public RectTransform_Button_Image_Container PlayerIconBtn { get { return m_PlayerIconBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_CampRateNode;
		public RectTransform_Image_Container CampRateNode { get { return m_CampRateNode; } }

		[SerializeField]
		private RectTransform_Text_Container m_CampRateTxt;
		public RectTransform_Text_Container CampRateTxt { get { return m_CampRateTxt; } }

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
