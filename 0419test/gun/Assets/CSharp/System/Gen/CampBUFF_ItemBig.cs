using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampBUFF_ItemBig : MonoBehaviour {

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_desc;
		public RectTransform_Text_LanguageTip_Container desc { get { return m_desc; } }

		[SerializeField]
		private RectTransform_Image_Container m_goods;
		public RectTransform_Image_Container goods { get { return m_goods; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_MaxLevel;
		public RectTransform_Text_LanguageTip_Container MaxLevel { get { return m_MaxLevel; } }

		[SerializeField]
		private RectTransform_Container m_NormalLevel;
		public RectTransform_Container NormalLevel { get { return m_NormalLevel; } }

		[SerializeField]
		private RectTransform_Text_Container m_CurText;
		public RectTransform_Text_Container CurText { get { return m_CurText; } }

		[SerializeField]
		private RectTransform_Text_Container m_MaxText;
		public RectTransform_Text_Container MaxText { get { return m_MaxText; } }

		[SerializeField]
		private RectTransform_Image_Container m_Progress;
		public RectTransform_Image_Container Progress { get { return m_Progress; } }

		[SerializeField]
		private RectTransform_Text_Container m_level;
		public RectTransform_Text_Container level { get { return m_level; } }

		[SerializeField]
		private RectTransform_Image_Container m_lockIcon;
		public RectTransform_Image_Container lockIcon { get { return m_lockIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_AddText;
		public RectTransform_Text_Container AddText { get { return m_AddText; } }

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
