using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CharUI_level : BaseUi {

		[SerializeField]
		private RectTransform_Text_Container m_LevelTxt;
		public RectTransform_Text_Container LevelTxt { get { return m_LevelTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_Exp;
		public RectTransform_Image_Container Exp { get { return m_Exp; } }

		[SerializeField]
		private RectTransform_Text_Container m_ExpDetail;
		public RectTransform_Text_Container ExpDetail { get { return m_ExpDetail; } }

		[SerializeField]
		private RectTransform_Button_Image_LongPressEventTrigger_Container m_PageLeft;
		public RectTransform_Button_Image_LongPressEventTrigger_Container PageLeft { get { return m_PageLeft; } }

		[SerializeField]
		private RectTransform_Button_Image_LongPressEventTrigger_Container m_PageRight;
		public RectTransform_Button_Image_LongPressEventTrigger_Container PageRight { get { return m_PageRight; } }

		[SerializeField]
		private RectTransform_Image_Container m_ScrollView;
		public RectTransform_Image_Container ScrollView { get { return m_ScrollView; } }

		[SerializeField]
		private RectTransform_Image_Container m_Viewport;
		public RectTransform_Image_Container Viewport { get { return m_Viewport; } }

		[SerializeField]
		private RectTransform_Container m_Content;
		public RectTransform_Container Content { get { return m_Content; } }

		[SerializeField]
		private RectTransform_CharUI_level_ItemUI_Container m_ItemUI;
		public RectTransform_CharUI_level_ItemUI_Container ItemUI { get { return m_ItemUI; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Tableft;
		public RectTransform_Button_Image_Container Tableft { get { return m_Tableft; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Tabright;
		public RectTransform_Button_Image_Container Tabright { get { return m_Tabright; } }

		[SerializeField]
		private RectTransform_Image_Container m_Tableft_on;
		public RectTransform_Image_Container Tableft_on { get { return m_Tableft_on; } }

		[SerializeField]
		private RectTransform_Image_Container m_Tabright_on;
		public RectTransform_Image_Container Tabright_on { get { return m_Tabright_on; } }

		[SerializeField]
		private RectTransform_Text_Container m_Tableftname;
		public RectTransform_Text_Container Tableftname { get { return m_Tableftname; } }

		[SerializeField]
		private RectTransform_Text_Container m_Tabrightname;
		public RectTransform_Text_Container Tabrightname { get { return m_Tabrightname; } }

		[SerializeField]
		private RectTransform_Image_Container m_MainRoleAdapter;
		public RectTransform_Image_Container MainRoleAdapter { get { return m_MainRoleAdapter; } }

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
		public class RectTransform_Button_Image_LongPressEventTrigger_Container {

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

			[SerializeField]
			private LongPressEventTrigger m_longPressEventTrigger;
			public LongPressEventTrigger longPressEventTrigger { get { return m_longPressEventTrigger; } }

		}

		[System.Serializable]
		public class RectTransform_CharUI_level_ItemUI_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CharUI_level_ItemUI m_ItemUI;
			public CharUI_level_ItemUI ItemUI { get { return m_ItemUI; } }

			private Queue<CharUI_level_ItemUI> mCachedInstances;
			public CharUI_level_ItemUI GetInstance() {
				CharUI_level_ItemUI instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CharUI_level_ItemUI>(m_ItemUI);
				}
				Transform t0 = m_ItemUI.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CharUI_level_ItemUI instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CharUI_level_ItemUI>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

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
