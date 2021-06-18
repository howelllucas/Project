using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampRecycleUi : BaseUi {

		[SerializeField]
		private RectTransform_Container m_AdaptNode;
		public RectTransform_Container AdaptNode { get { return m_AdaptNode; } }

		[SerializeField]
		private RectTransform_Text_Container m_TitleText;
		public RectTransform_Text_Container TitleText { get { return m_TitleText; } }

		[SerializeField]
		private RectTransform_Text_Container m_DesText;
		public RectTransform_Text_Container DesText { get { return m_DesText; } }

		[SerializeField]
		private RectTransform_Image_Container m_icon;
		public RectTransform_Image_Container icon { get { return m_icon; } }

		[SerializeField]
		private RectTransform_Text_Container m_Num;
		public RectTransform_Text_Container Num { get { return m_Num; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ExchangeAllBtn;
		public RectTransform_Button_Image_Container ExchangeAllBtn { get { return m_ExchangeAllBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ExpansionBtn;
		public RectTransform_Button_Image_Container ExpansionBtn { get { return m_ExpansionBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_UnExpansionImage;
		public RectTransform_Image_Container UnExpansionImage { get { return m_UnExpansionImage; } }

		[SerializeField]
		private RectTransform_CampRecycleUi_ExchangeItem_Container m_ExchangeItem;
		public RectTransform_CampRecycleUi_ExchangeItem_Container ExchangeItem { get { return m_ExchangeItem; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_AllExchangeMatBg;
		public RectTransform_Button_Image_Container AllExchangeMatBg { get { return m_AllExchangeMatBg; } }

		[SerializeField]
		private RectTransform_CampRecycleUi_NewExchangeItem_Container m_NewExchangeItem;
		public RectTransform_CampRecycleUi_NewExchangeItem_Container NewExchangeItem { get { return m_NewExchangeItem; } }

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
		public class RectTransform_CampRecycleUi_ExchangeItem_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampRecycleUi_ExchangeItem m_ExchangeItem;
			public CampRecycleUi_ExchangeItem ExchangeItem { get { return m_ExchangeItem; } }

			private Queue<CampRecycleUi_ExchangeItem> mCachedInstances;
			public CampRecycleUi_ExchangeItem GetInstance() {
				CampRecycleUi_ExchangeItem instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampRecycleUi_ExchangeItem>(m_ExchangeItem);
				}
				Transform t0 = m_ExchangeItem.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampRecycleUi_ExchangeItem instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampRecycleUi_ExchangeItem>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_CampRecycleUi_NewExchangeItem_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampRecycleUi_NewExchangeItem m_NewExchangeItem;
			public CampRecycleUi_NewExchangeItem NewExchangeItem { get { return m_NewExchangeItem; } }

			private Queue<CampRecycleUi_NewExchangeItem> mCachedInstances;
			public CampRecycleUi_NewExchangeItem GetInstance() {
				CampRecycleUi_NewExchangeItem instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampRecycleUi_NewExchangeItem>(m_NewExchangeItem);
				}
				Transform t0 = m_NewExchangeItem.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampRecycleUi_NewExchangeItem instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampRecycleUi_NewExchangeItem>(); }
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
