using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampBadgeUI_BadgeItemUI : MonoBehaviour {

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_Title;
		public RectTransform_Text_LanguageTip_Container Title { get { return m_Title; } }

		[SerializeField]
		private RectTransform_Text_Container m_Detail;
		public RectTransform_Text_Container Detail { get { return m_Detail; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_NoActiveTip;
		public RectTransform_Text_LanguageTip_Container NoActiveTip { get { return m_NoActiveTip; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon;
		public RectTransform_Image_Container Icon { get { return m_Icon; } }

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
