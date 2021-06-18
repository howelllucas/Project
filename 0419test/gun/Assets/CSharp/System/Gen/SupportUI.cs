using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class SupportUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_ExitBtn;
		public RectTransform_Button_Image_Container ExitBtn { get { return m_ExitBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_CoinTxt;
		public RectTransform_Text_Container CoinTxt { get { return m_CoinTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_MoneyTxt;
		public RectTransform_Text_Container MoneyTxt { get { return m_MoneyTxt; } }

		[SerializeField]
		private RectTransform_Text_Button_Container m_MainTxtBtn;
		public RectTransform_Text_Button_Container MainTxtBtn { get { return m_MainTxtBtn; } }

		[SerializeField]
		private RectTransform_Text_Button_Container m_SubTxtBtn;
		public RectTransform_Text_Button_Container SubTxtBtn { get { return m_SubTxtBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_MainWp;
		public RectTransform_Image_Container MainWp { get { return m_MainWp; } }

		[SerializeField]
		private RectTransform_Image_Container m_SubWp;
		public RectTransform_Image_Container SubWp { get { return m_SubWp; } }

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
		private RectTransform_SupportUI_SupportItemUI_Container m_SupportItemUI;
		public RectTransform_SupportUI_SupportItemUI_Container SupportItemUI { get { return m_SupportItemUI; } }

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
		public class RectTransform_SupportUI_SupportItemUI_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private SupportUI_SupportItemUI m_SupportItemUI;
			public SupportUI_SupportItemUI SupportItemUI { get { return m_SupportItemUI; } }

			private Queue<SupportUI_SupportItemUI> mCachedInstances;
			public SupportUI_SupportItemUI GetInstance() {
				SupportUI_SupportItemUI instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<SupportUI_SupportItemUI>(m_SupportItemUI);
				}
				Transform t0 = m_SupportItemUI.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(SupportUI_SupportItemUI instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<SupportUI_SupportItemUI>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_Text_Button_Container {

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
			private Button m_button;
			public Button button { get { return m_button; } }

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
