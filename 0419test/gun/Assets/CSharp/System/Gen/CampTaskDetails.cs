using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampTaskDetails : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_Level;
		public RectTransform_Text_LanguageTip_Container Level { get { return m_Level; } }

		[SerializeField]
		private RectTransform_Text_Container m_none;
		public RectTransform_Text_Container none { get { return m_none; } }

		[SerializeField]
		private RectTransform_CampTaskDetails_TotalTaskItem_Container m_TotalTaskItem;
		public RectTransform_CampTaskDetails_TotalTaskItem_Container TotalTaskItem { get { return m_TotalTaskItem; } }

		[SerializeField]
		private RectTransform_CampTaskDetails_DayTaskItemUI_Container m_DayTaskItemUI;
		public RectTransform_CampTaskDetails_DayTaskItemUI_Container DayTaskItemUI { get { return m_DayTaskItemUI; } }

		[SerializeField]
		private RectTransform_Container m_ShowNode;
		public RectTransform_Container ShowNode { get { return m_ShowNode; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BgBtn;
		public RectTransform_Button_Image_Container BgBtn { get { return m_BgBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_person1;
		public RectTransform_Image_Container person1 { get { return m_person1; } }

		[SerializeField]
		private RectTransform_Image_Container m_person2;
		public RectTransform_Image_Container person2 { get { return m_person2; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_tip;
		public RectTransform_Text_LanguageTip_Container tip { get { return m_tip; } }

		[SerializeField]
		private RectTransform_Image_Container m_nextIcon;
		public RectTransform_Image_Container nextIcon { get { return m_nextIcon; } }

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
		public class RectTransform_CampTaskDetails_DayTaskItemUI_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampTaskDetails_DayTaskItemUI m_DayTaskItemUI;
			public CampTaskDetails_DayTaskItemUI DayTaskItemUI { get { return m_DayTaskItemUI; } }

			private Queue<CampTaskDetails_DayTaskItemUI> mCachedInstances;
			public CampTaskDetails_DayTaskItemUI GetInstance() {
				CampTaskDetails_DayTaskItemUI instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampTaskDetails_DayTaskItemUI>(m_DayTaskItemUI);
				}
				Transform t0 = m_DayTaskItemUI.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampTaskDetails_DayTaskItemUI instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampTaskDetails_DayTaskItemUI>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_CampTaskDetails_TotalTaskItem_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampTaskDetails_TotalTaskItem m_TotalTaskItem;
			public CampTaskDetails_TotalTaskItem TotalTaskItem { get { return m_TotalTaskItem; } }

			private Queue<CampTaskDetails_TotalTaskItem> mCachedInstances;
			public CampTaskDetails_TotalTaskItem GetInstance() {
				CampTaskDetails_TotalTaskItem instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampTaskDetails_TotalTaskItem>(m_TotalTaskItem);
				}
				Transform t0 = m_TotalTaskItem.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampTaskDetails_TotalTaskItem instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampTaskDetails_TotalTaskItem>(); }
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
