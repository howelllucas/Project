using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampResourcesDetailUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_Level;
		public RectTransform_Text_Container Level { get { return m_Level; } }

		[SerializeField]
		private RectTransform_Text_Container m_DName;
		public RectTransform_Text_Container DName { get { return m_DName; } }

		[SerializeField]
		private RectTransform_Text_Container m_Text;
		public RectTransform_Text_Container Text { get { return m_Text; } }

		[SerializeField]
		private RectTransform_CampResourcesDetailUI_ResourceItemUI_Container m_ResourceItemUI;
		public RectTransform_CampResourcesDetailUI_ResourceItemUI_Container ResourceItemUI { get { return m_ResourceItemUI; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_KnowBtn;
		public RectTransform_Button_Image_Container KnowBtn { get { return m_KnowBtn; } }

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
		public class RectTransform_CampResourcesDetailUI_ResourceItemUI_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampResourcesDetailUI_ResourceItemUI m_ResourceItemUI;
			public CampResourcesDetailUI_ResourceItemUI ResourceItemUI { get { return m_ResourceItemUI; } }

			private Queue<CampResourcesDetailUI_ResourceItemUI> mCachedInstances;
			public CampResourcesDetailUI_ResourceItemUI GetInstance() {
				CampResourcesDetailUI_ResourceItemUI instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampResourcesDetailUI_ResourceItemUI>(m_ResourceItemUI);
				}
				Transform t0 = m_ResourceItemUI.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampResourcesDetailUI_ResourceItemUI instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampResourcesDetailUI_ResourceItemUI>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

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
