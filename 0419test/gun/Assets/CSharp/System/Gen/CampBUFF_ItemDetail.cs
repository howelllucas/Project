using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampBUFF_ItemDetail : MonoBehaviour {

		[SerializeField]
		private RectTransform_Image_Container m_Box;
		public RectTransform_Image_Container Box { get { return m_Box; } }

		[SerializeField]
		private RectTransform_Image_Container m_arrow;
		public RectTransform_Image_Container arrow { get { return m_arrow; } }

		[SerializeField]
		private RectTransform_Text_Container m_name;
		public RectTransform_Text_Container name { get { return m_name; } }

		[SerializeField]
		private RectTransform_Image_Container m_up;
		public RectTransform_Image_Container up { get { return m_up; } }

		[SerializeField]
		private RectTransform_Image_Container m_icon;
		public RectTransform_Image_Container icon { get { return m_icon; } }

		[SerializeField]
		private RectTransform_Image_Container m_lvBg;
		public RectTransform_Image_Container lvBg { get { return m_lvBg; } }

		[SerializeField]
		private RectTransform_Text_Container m_level;
		public RectTransform_Text_Container level { get { return m_level; } }

		[SerializeField]
		private RectTransform_Image_Container m_lockIcon;
		public RectTransform_Image_Container lockIcon { get { return m_lockIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_desc;
		public RectTransform_Text_Container desc { get { return m_desc; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Button;
		public RectTransform_Button_Image_Container Button { get { return m_Button; } }

		[SerializeField]
		private RectTransform_Text_Container m_cost;
		public RectTransform_Text_Container cost { get { return m_cost; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_upgrade;
		public RectTransform_Text_LanguageTip_Container upgrade { get { return m_upgrade; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_unlock;
		public RectTransform_Text_LanguageTip_Container unlock { get { return m_unlock; } }

		[SerializeField]
		private RectTransform_Text_Container m_unlockTips;
		public RectTransform_Text_Container unlockTips { get { return m_unlockTips; } }

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

		[System.Serializable]
		public class RectTransform_Text_LanguageTip_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private Text m_text;
			public Text text { get { return m_text; } }

			[SerializeField]
			private LanguageTip m_languageTip;
			public LanguageTip languageTip { get { return m_languageTip; } }

		}

	}

}
