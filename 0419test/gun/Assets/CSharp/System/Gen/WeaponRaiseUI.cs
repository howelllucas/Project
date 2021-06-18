using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class WeaponRaiseUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_WeaponViewport;
		public RectTransform_Image_Container WeaponViewport { get { return m_WeaponViewport; } }

		[SerializeField]
		private RectTransform_Container m_WeaponContent;
		public RectTransform_Container WeaponContent { get { return m_WeaponContent; } }

		[SerializeField]
		private RectTransform_WeaponRaiseUI_WeaponItemUI_Container m_WeaponItemUI;
		public RectTransform_WeaponRaiseUI_WeaponItemUI_Container WeaponItemUI { get { return m_WeaponItemUI; } }

		[SerializeField]
		private RectTransform_Image_Container m_ExChangeMatViewport;
		public RectTransform_Image_Container ExChangeMatViewport { get { return m_ExChangeMatViewport; } }

		[SerializeField]
		private RectTransform_Container m_ExChangeMatContent;
		public RectTransform_Container ExChangeMatContent { get { return m_ExChangeMatContent; } }

		[SerializeField]
		private RectTransform_WeaponRaiseUI_ExchangeMatItemUI_Container m_ExchangeMatItemUI;
		public RectTransform_WeaponRaiseUI_ExchangeMatItemUI_Container ExchangeMatItemUI { get { return m_ExchangeMatItemUI; } }

		[SerializeField]
		private RectTransform_Container m_Tabs;
		public RectTransform_Container Tabs { get { return m_Tabs; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_WeaponTab;
		public RectTransform_Button_Image_Container WeaponTab { get { return m_WeaponTab; } }

		[SerializeField]
		private RectTransform_Image_Container m_weapon_img_active;
		public RectTransform_Image_Container weapon_img_active { get { return m_weapon_img_active; } }

		[SerializeField]
		private RectTransform_Container m_TabWeaponUp;
		public RectTransform_Container TabWeaponUp { get { return m_TabWeaponUp; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_weapon_txt;
		public RectTransform_Text_LanguageTip_Container weapon_txt { get { return m_weapon_txt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_MatExchangeTab;
		public RectTransform_Button_Image_Container MatExchangeTab { get { return m_MatExchangeTab; } }

		[SerializeField]
		private RectTransform_Image_Container m_matexchange_img_active;
		public RectTransform_Image_Container matexchange_img_active { get { return m_matexchange_img_active; } }

		[SerializeField]
		private RectTransform_Container m_TabMatExchangeUp;
		public RectTransform_Container TabMatExchangeUp { get { return m_TabMatExchangeUp; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_ExchangeMat_txt;
		public RectTransform_Text_LanguageTip_Container ExchangeMat_txt { get { return m_ExchangeMat_txt; } }

		[SerializeField]
		private RectTransform_Image_Container m_Viewport2;
		public RectTransform_Image_Container Viewport2 { get { return m_Viewport2; } }

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

		[System.Serializable]
		public class RectTransform_WeaponRaiseUI_ExchangeMatItemUI_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private WeaponRaiseUI_ExchangeMatItemUI m_ExchangeMatItemUI;
			public WeaponRaiseUI_ExchangeMatItemUI ExchangeMatItemUI { get { return m_ExchangeMatItemUI; } }

			private Queue<WeaponRaiseUI_ExchangeMatItemUI> mCachedInstances;
			public WeaponRaiseUI_ExchangeMatItemUI GetInstance() {
				WeaponRaiseUI_ExchangeMatItemUI instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<WeaponRaiseUI_ExchangeMatItemUI>(m_ExchangeMatItemUI);
				}
				Transform t0 = m_ExchangeMatItemUI.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(WeaponRaiseUI_ExchangeMatItemUI instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<WeaponRaiseUI_ExchangeMatItemUI>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_WeaponRaiseUI_WeaponItemUI_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private WeaponRaiseUI_WeaponItemUI m_WeaponItemUI;
			public WeaponRaiseUI_WeaponItemUI WeaponItemUI { get { return m_WeaponItemUI; } }

			private Queue<WeaponRaiseUI_WeaponItemUI> mCachedInstances;
			public WeaponRaiseUI_WeaponItemUI GetInstance() {
				WeaponRaiseUI_WeaponItemUI instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<WeaponRaiseUI_WeaponItemUI>(m_WeaponItemUI);
				}
				Transform t0 = m_WeaponItemUI.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(WeaponRaiseUI_WeaponItemUI instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<WeaponRaiseUI_WeaponItemUI>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

	}

}
