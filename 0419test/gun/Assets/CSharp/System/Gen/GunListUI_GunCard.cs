using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GunListUI_GunCard : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_Frame;
		public RectTransform_Image_Container Frame { get { return m_Frame; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_IconBtn;
		public RectTransform_Button_Image_Container IconBtn { get { return m_IconBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_TypeIcon;
		public RectTransform_Image_Container TypeIcon { get { return m_TypeIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_NameTxt;
		public RectTransform_Text_Container NameTxt { get { return m_NameTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_EffectImg;
		public RectTransform_Image_Container EffectImg { get { return m_EffectImg; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon;
		public RectTransform_Image_Container Icon { get { return m_Icon; } }

		[SerializeField]
		private RectTransform_Text_Container m_TipsTitle;
		public RectTransform_Text_Container TipsTitle { get { return m_TipsTitle; } }

		[SerializeField]
		private RectTransform_Text_Container m_LevelTxt;
		public RectTransform_Text_Container LevelTxt { get { return m_LevelTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon1;
		public RectTransform_Image_Container Icon1 { get { return m_Icon1; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon2;
		public RectTransform_Image_Container Icon2 { get { return m_Icon2; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon3;
		public RectTransform_Image_Container Icon3 { get { return m_Icon3; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon4;
		public RectTransform_Image_Container Icon4 { get { return m_Icon4; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon5;
		public RectTransform_Image_Container Icon5 { get { return m_Icon5; } }

		[SerializeField]
		private RectTransform_Text_Container m_FuseSkillName;
		public RectTransform_Text_Container FuseSkillName { get { return m_FuseSkillName; } }

		[SerializeField]
		private RectTransform_Text_Container m_FuseSkillLevel;
		public RectTransform_Text_Container FuseSkillLevel { get { return m_FuseSkillLevel; } }

		[SerializeField]
		private RectTransform_Text_Container m_EquipTxt;
		public RectTransform_Text_Container EquipTxt { get { return m_EquipTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_FuseTxt;
		public RectTransform_Text_Container FuseTxt { get { return m_FuseTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_CurAtk;
		public RectTransform_Text_Container CurAtk { get { return m_CurAtk; } }

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
