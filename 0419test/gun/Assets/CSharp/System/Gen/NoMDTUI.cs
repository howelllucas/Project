using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class NoMDTUI : BaseUi {

		[SerializeField]
		private RectTransform_Container m_mdt;
		public RectTransform_Container mdt { get { return m_mdt; } }

		[SerializeField]
		private RectTransform_Container m_mdtItem1;
		public RectTransform_Container mdtItem1 { get { return m_mdtItem1; } }

		[SerializeField]
		private RectTransform_Image_Container m_curBg;
		public RectTransform_Image_Container curBg { get { return m_curBg; } }

		[SerializeField]
		private RectTransform_Text_Container m_num;
		public RectTransform_Text_Container num { get { return m_num; } }

		[SerializeField]
		private RectTransform_Image_Container m_mask;
		public RectTransform_Image_Container mask { get { return m_mask; } }

		[SerializeField]
		private RectTransform_Container m_mdtItem2;
		public RectTransform_Container mdtItem2 { get { return m_mdtItem2; } }

		[SerializeField]
		private RectTransform_Container m_mdtItem3;
		public RectTransform_Container mdtItem3 { get { return m_mdtItem3; } }

		[SerializeField]
		private RectTransform_Container m_mdtItem4;
		public RectTransform_Container mdtItem4 { get { return m_mdtItem4; } }

		[SerializeField]
		private RectTransform_Container m_mdtItem5;
		public RectTransform_Container mdtItem5 { get { return m_mdtItem5; } }

		[SerializeField]
		private RectTransform_Image_Container m_progress;
		public RectTransform_Image_Container progress { get { return m_progress; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_GetMDTBtn;
		public RectTransform_Button_Image_Container GetMDTBtn { get { return m_GetMDTBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Ad4Mdt;
		public RectTransform_Button_Image_Container Ad4Mdt { get { return m_Ad4Mdt; } }

		[SerializeField]
		private RectTransform_Image_Container m_content;
		public RectTransform_Image_Container content { get { return m_content; } }

		[SerializeField]
		private RectTransform_Image_Container m_mdttBox;
		public RectTransform_Image_Container mdttBox { get { return m_mdttBox; } }

		[SerializeField]
		private RectTransform_Image_Container m_newHas;
		public RectTransform_Image_Container newHas { get { return m_newHas; } }

		[SerializeField]
		private RectTransform_Container m_MDTGetText;
		public RectTransform_Container MDTGetText { get { return m_MDTGetText; } }

		[SerializeField]
		private RectTransform_Text_Container m_mdtNum;
		public RectTransform_Text_Container mdtNum { get { return m_mdtNum; } }

		[SerializeField]
		private RectTransform_Container m_MDTTimes;
		public RectTransform_Container MDTTimes { get { return m_MDTTimes; } }

		[SerializeField]
		private RectTransform_Text_Container m_leftChance;
		public RectTransform_Text_Container leftChance { get { return m_leftChance; } }

		[SerializeField]
		private RectTransform_Text_Container m_totalChance;
		public RectTransform_Text_Container totalChance { get { return m_totalChance; } }

		[SerializeField]
		private RectTransform_Image_Container m_m1;
		public RectTransform_Image_Container m1 { get { return m_m1; } }

		[SerializeField]
		private RectTransform_Image_Container m_m2;
		public RectTransform_Image_Container m2 { get { return m_m2; } }

		[SerializeField]
		private RectTransform_Image_Container m_m3;
		public RectTransform_Image_Container m3 { get { return m_m3; } }

		[SerializeField]
		private RectTransform_Image_Container m_m4;
		public RectTransform_Image_Container m4 { get { return m_m4; } }

		[SerializeField]
		private RectTransform_Image_Container m_m5;
		public RectTransform_Image_Container m5 { get { return m_m5; } }

		[SerializeField]
		private RectTransform_Image_Container m_RBADIcon;
		public RectTransform_Image_Container RBADIcon { get { return m_RBADIcon; } }

		[SerializeField]
		private Transform_Container m_UI_shop_effect;
		public Transform_Container UI_shop_effect { get { return m_UI_shop_effect; } }

		[SerializeField]
		private RectTransform_Image_Container m_MdtCDbg;
		public RectTransform_Image_Container MdtCDbg { get { return m_MdtCDbg; } }

		[SerializeField]
		private RectTransform_Text_Container m_MdtCDText;
		public RectTransform_Text_Container MdtCDText { get { return m_MdtCDText; } }

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

		[System.Serializable]
		public class Transform_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private Transform m_transform;
			public Transform transform { get { return m_transform; } }

		}

	}

}
