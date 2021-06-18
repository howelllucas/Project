using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class ShopUI_ShopMdtItemUI : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_Cmbglight;
		public RectTransform_Image_Container Cmbglight { get { return m_Cmbglight; } }

		[SerializeField]
		private RectTransform_Image_Container m_ItemIcon;
		public RectTransform_Image_Container ItemIcon { get { return m_ItemIcon; } }

		[SerializeField]
		private RectTransform_Image_Container m_CmIcon;
		public RectTransform_Image_Container CmIcon { get { return m_CmIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_Valuetxt;
		public RectTransform_Text_Container Valuetxt { get { return m_Valuetxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BuyBtn;
		public RectTransform_Button_Image_Container BuyBtn { get { return m_BuyBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_ConsumeIcon;
		public RectTransform_Image_Container ConsumeIcon { get { return m_ConsumeIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_ConsumeValue;
		public RectTransform_Text_Container ConsumeValue { get { return m_ConsumeValue; } }

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
