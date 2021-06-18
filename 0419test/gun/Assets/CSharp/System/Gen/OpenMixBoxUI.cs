using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class OpenMixBoxUI : BaseUi {

		[SerializeField]
		private RectTransform_Container m_AllNode;
		public RectTransform_Container AllNode { get { return m_AllNode; } }

		[SerializeField]
		private RectTransform_Image_Container m_Bg;
		public RectTransform_Image_Container Bg { get { return m_Bg; } }

		[SerializeField]
		private RectTransform_Image_Container m_box_close;
		public RectTransform_Image_Container box_close { get { return m_box_close; } }

		[SerializeField]
		private RectTransform_Image_Container m_box_open;
		public RectTransform_Image_Container box_open { get { return m_box_open; } }

		[SerializeField]
		private Transform_Container m_UI_openbox;
		public Transform_Container UI_openbox { get { return m_UI_openbox; } }

		[SerializeField]
		private Transform_Container m_UI_openbox_white;
		public Transform_Container UI_openbox_white { get { return m_UI_openbox_white; } }

		[SerializeField]
		private Transform_Container m_UI_openbox_purple;
		public Transform_Container UI_openbox_purple { get { return m_UI_openbox_purple; } }

		[SerializeField]
		private RectTransform_OpenMixBoxUI_card_Container m_card;
		public RectTransform_OpenMixBoxUI_card_Container card { get { return m_card; } }

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
		public class RectTransform_OpenMixBoxUI_card_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private OpenMixBoxUI_card m_card;
			public OpenMixBoxUI_card card { get { return m_card; } }

			private Queue<OpenMixBoxUI_card> mCachedInstances;
			public OpenMixBoxUI_card GetInstance() {
				OpenMixBoxUI_card instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<OpenMixBoxUI_card>(m_card);
				}
				Transform t0 = m_card.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(OpenMixBoxUI_card instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<OpenMixBoxUI_card>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class Transform_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private Transform m_transform;
			public Transform transform { get { return m_transform; } }

		}

	}

}
