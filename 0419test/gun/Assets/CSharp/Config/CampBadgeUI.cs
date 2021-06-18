using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampBadgeUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_CampBadgeUI_BadgeItemUI_Container m_BadgeItemUI;
		public RectTransform_CampBadgeUI_BadgeItemUI_Container BadgeItemUI { get { return m_BadgeItemUI; } }

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
		public class RectTransform_CampBadgeUI_BadgeItemUI_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampBadgeUI_BadgeItemUI m_BadgeItemUI;
			public CampBadgeUI_BadgeItemUI BadgeItemUI { get { return m_BadgeItemUI; } }

			private Queue<CampBadgeUI_BadgeItemUI> mCachedInstances;
			public CampBadgeUI_BadgeItemUI GetInstance() {
				CampBadgeUI_BadgeItemUI instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampBadgeUI_BadgeItemUI>(m_BadgeItemUI);
				}
				Transform t0 = m_BadgeItemUI.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampBadgeUI_BadgeItemUI instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampBadgeUI_BadgeItemUI>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

	}

}
