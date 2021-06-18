using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class SupportUI_SupportItemUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_EquipBtn;
		public RectTransform_Button_Image_Container EquipBtn { get { return m_EquipBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_WeaponIcon;
		public RectTransform_Image_Container WeaponIcon { get { return m_WeaponIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_WeaponNameTxt;
		public RectTransform_Text_Container WeaponNameTxt { get { return m_WeaponNameTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_WeaponLvTxt;
		public RectTransform_Text_Container WeaponLvTxt { get { return m_WeaponLvTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_LvPro;
		public RectTransform_Image_Container LvPro { get { return m_LvPro; } }

		[SerializeField]
		private RectTransform_Text_Container m_Param1NameTxt;
		public RectTransform_Text_Container Param1NameTxt { get { return m_Param1NameTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_Param1Txt;
		public RectTransform_Text_Container Param1Txt { get { return m_Param1Txt; } }

		[SerializeField]
		private RectTransform_Text_Container m_Param2NameTxt;
		public RectTransform_Text_Container Param2NameTxt { get { return m_Param2NameTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_Param2Txt;
		public RectTransform_Text_Container Param2Txt { get { return m_Param2Txt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_UpBtn;
		public RectTransform_Button_Image_Container UpBtn { get { return m_UpBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_CmIcon;
		public RectTransform_Image_Container CmIcon { get { return m_CmIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_CmNum;
		public RectTransform_Text_Container CmNum { get { return m_CmNum; } }

		[SerializeField]
		private RectTransform_Image_Container m_Mask1;
		public RectTransform_Image_Container Mask1 { get { return m_Mask1; } }

		[SerializeField]
		private RectTransform_Text_Container m_MaskTxt;
		public RectTransform_Text_Container MaskTxt { get { return m_MaskTxt; } }

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
