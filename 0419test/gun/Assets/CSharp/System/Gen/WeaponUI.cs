using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class WeaponUI : BaseUi {

		[SerializeField]
		private RectTransform_Image_Container m_WeaponViewport;
		public RectTransform_Image_Container WeaponViewport { get { return m_WeaponViewport; } }

		[SerializeField]
		private RectTransform_Container m_WeaponContent;
		public RectTransform_Container WeaponContent { get { return m_WeaponContent; } }

		[SerializeField]
		private RectTransform_WeaponUI_WeaponItemUI_Container m_WeaponItemUI;
		public RectTransform_WeaponUI_WeaponItemUI_Container WeaponItemUI { get { return m_WeaponItemUI; } }

		[SerializeField]
		private RectTransform_Image_Container m_SubWeaponViewport;
		public RectTransform_Image_Container SubWeaponViewport { get { return m_SubWeaponViewport; } }

		[SerializeField]
		private RectTransform_Container m_SubWeaponContent;
		public RectTransform_Container SubWeaponContent { get { return m_SubWeaponContent; } }

		[SerializeField]
		private RectTransform_WeaponUI_SubWeaponItemUI_Container m_SubWeaponItemUI;
		public RectTransform_WeaponUI_SubWeaponItemUI_Container SubWeaponItemUI { get { return m_SubWeaponItemUI; } }

		[SerializeField]
		private RectTransform_Image_Container m_PetViewport;
		public RectTransform_Image_Container PetViewport { get { return m_PetViewport; } }

		[SerializeField]
		private RectTransform_Container m_PetContent;
		public RectTransform_Container PetContent { get { return m_PetContent; } }

		[SerializeField]
		private RectTransform_WeaponUI_PetItemUI_Container m_PetItemUI;
		public RectTransform_WeaponUI_PetItemUI_Container PetItemUI { get { return m_PetItemUI; } }

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
		private RectTransform_Image_Container m_NewWp;
		public RectTransform_Image_Container NewWp { get { return m_NewWp; } }

		[SerializeField]
		private RectTransform_Container m_TabWeaponUp;
		public RectTransform_Container TabWeaponUp { get { return m_TabWeaponUp; } }

		[SerializeField]
		private RectTransform_Image_Container m_weapon_lock;
		public RectTransform_Image_Container weapon_lock { get { return m_weapon_lock; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_weapon_txt;
		public RectTransform_Text_LanguageTip_Container weapon_txt { get { return m_weapon_txt; } }

		[SerializeField]
		private RectTransform_Button_Image_NewbieGuideButton_Container m_SubWeaponTab;
		public RectTransform_Button_Image_NewbieGuideButton_Container SubWeaponTab { get { return m_SubWeaponTab; } }

		[SerializeField]
		private RectTransform_Image_Container m_subweapon_img_active;
		public RectTransform_Image_Container subweapon_img_active { get { return m_subweapon_img_active; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewSubWp;
		public RectTransform_Image_Container NewSubWp { get { return m_NewSubWp; } }

		[SerializeField]
		private RectTransform_Image_Container m_subweapon_lock;
		public RectTransform_Image_Container subweapon_lock { get { return m_subweapon_lock; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_subweapon_txt;
		public RectTransform_Text_LanguageTip_Container subweapon_txt { get { return m_subweapon_txt; } }

		[SerializeField]
		private RectTransform_Container m_TabSubWeaponUp;
		public RectTransform_Container TabSubWeaponUp { get { return m_TabSubWeaponUp; } }

		[SerializeField]
		private RectTransform_Button_Image_NewbieGuideButton_Container m_PetTab;
		public RectTransform_Button_Image_NewbieGuideButton_Container PetTab { get { return m_PetTab; } }

		[SerializeField]
		private RectTransform_Image_Container m_pet_img_active;
		public RectTransform_Image_Container pet_img_active { get { return m_pet_img_active; } }

		[SerializeField]
		private RectTransform_Container m_TabPetUp;
		public RectTransform_Container TabPetUp { get { return m_TabPetUp; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewPet;
		public RectTransform_Image_Container NewPet { get { return m_NewPet; } }

		[SerializeField]
		private RectTransform_Image_Container m_pet_lock;
		public RectTransform_Image_Container pet_lock { get { return m_pet_lock; } }

		[SerializeField]
		private RectTransform_Text_LanguageTip_Container m_pet_txt;
		public RectTransform_Text_LanguageTip_Container pet_txt { get { return m_pet_txt; } }

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
		public class RectTransform_Button_Image_NewbieGuideButton_Container {

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
			private NewbieGuideButton m_newbieGuideButton;
			public NewbieGuideButton newbieGuideButton { get { return m_newbieGuideButton; } }

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
		public class RectTransform_WeaponUI_PetItemUI_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private WeaponUI_PetItemUI m_PetItemUI;
			public WeaponUI_PetItemUI PetItemUI { get { return m_PetItemUI; } }

			private Queue<WeaponUI_PetItemUI> mCachedInstances;
			public WeaponUI_PetItemUI GetInstance() {
				WeaponUI_PetItemUI instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<WeaponUI_PetItemUI>(m_PetItemUI);
				}
				Transform t0 = m_PetItemUI.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(WeaponUI_PetItemUI instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<WeaponUI_PetItemUI>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_WeaponUI_SubWeaponItemUI_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private WeaponUI_SubWeaponItemUI m_SubWeaponItemUI;
			public WeaponUI_SubWeaponItemUI SubWeaponItemUI { get { return m_SubWeaponItemUI; } }

			private Queue<WeaponUI_SubWeaponItemUI> mCachedInstances;
			public WeaponUI_SubWeaponItemUI GetInstance() {
				WeaponUI_SubWeaponItemUI instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<WeaponUI_SubWeaponItemUI>(m_SubWeaponItemUI);
				}
				Transform t0 = m_SubWeaponItemUI.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(WeaponUI_SubWeaponItemUI instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<WeaponUI_SubWeaponItemUI>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_WeaponUI_WeaponItemUI_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private WeaponUI_WeaponItemUI m_WeaponItemUI;
			public WeaponUI_WeaponItemUI WeaponItemUI { get { return m_WeaponItemUI; } }

			private Queue<WeaponUI_WeaponItemUI> mCachedInstances;
			public WeaponUI_WeaponItemUI GetInstance() {
				WeaponUI_WeaponItemUI instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<WeaponUI_WeaponItemUI>(m_WeaponItemUI);
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
			public bool CacheInstance(WeaponUI_WeaponItemUI instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<WeaponUI_WeaponItemUI>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

	}

}
