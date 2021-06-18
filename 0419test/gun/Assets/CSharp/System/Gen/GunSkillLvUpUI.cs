using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GunSkillLvUpUI : BaseUi {

		[SerializeField]
		private RectTransform_Text_Container m_CurLv;
		public RectTransform_Text_Container CurLv { get { return m_CurLv; } }

		[SerializeField]
		private RectTransform_Text_Container m_CurDesc;
		public RectTransform_Text_Container CurDesc { get { return m_CurDesc; } }

		[SerializeField]
		private RectTransform_Text_Container m_NextLv;
		public RectTransform_Text_Container NextLv { get { return m_NextLv; } }

		[SerializeField]
		private RectTransform_Text_Container m_NextDesc;
		public RectTransform_Text_Container NextDesc { get { return m_NextDesc; } }

		[SerializeField]
		private RectTransform_Text_Container m_SkillName;
		public RectTransform_Text_Container SkillName { get { return m_SkillName; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_SkillIcon;
		public RectTransform_Button_Image_Container SkillIcon { get { return m_SkillIcon; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_OkBtn;
		public RectTransform_Button_Image_Container OkBtn { get { return m_OkBtn; } }

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
