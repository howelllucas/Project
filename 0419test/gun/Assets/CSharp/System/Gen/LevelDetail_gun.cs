using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class LevelDetail_gun : MonoBehaviour {

		[SerializeField]
		private RectTransform_Button_Image_Container m_Btn;
		public RectTransform_Button_Image_Container Btn { get { return m_Btn; } }

		[SerializeField]
		private RectTransform_Image_Container m_gunbg;
		public RectTransform_Image_Container gunbg { get { return m_gunbg; } }

		[SerializeField]
		private RectTransform_Container m_level;
		public RectTransform_Container level { get { return m_level; } }

		[SerializeField]
		private RectTransform_Text_Container m_GunName;
		public RectTransform_Text_Container GunName { get { return m_GunName; } }

		[SerializeField]
		private RectTransform_Text_Container m_num;
		public RectTransform_Text_Container num { get { return m_num; } }

		[SerializeField]
		private RectTransform_Image_Container m_GunIcon;
		public RectTransform_Image_Container GunIcon { get { return m_GunIcon; } }

		[SerializeField]
		private RectTransform_Image_Container m_GunItem;
		public RectTransform_Image_Container GunItem { get { return m_GunItem; } }

		[SerializeField]
		private RectTransform_Container m_EffectNode;
		public RectTransform_Container EffectNode { get { return m_EffectNode; } }

		[SerializeField]
		private RectTransform_Image_Container m_GunBottom;
		public RectTransform_Image_Container GunBottom { get { return m_GunBottom; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_GunRetry;
		public RectTransform_Button_Image_Container GunRetry { get { return m_GunRetry; } }

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
