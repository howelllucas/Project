using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampsitePointUI : BaseUi {

		[SerializeField]
		private RectTransform_Text_Container m_NameTxt;
		public RectTransform_Text_Container NameTxt { get { return m_NameTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_TypeImg;
		public RectTransform_Image_Container TypeImg { get { return m_TypeImg; } }

		[SerializeField]
		private RectTransform_Text_Container m_TypeTxt;
		public RectTransform_Text_Container TypeTxt { get { return m_TypeTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_DetailBtn;
		public RectTransform_Button_Image_Container DetailBtn { get { return m_DetailBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_LvTxt;
		public RectTransform_Text_Container LvTxt { get { return m_LvTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_DescTxt;
		public RectTransform_Text_Container DescTxt { get { return m_DescTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_RewardTxt;
		public RectTransform_Text_Container RewardTxt { get { return m_RewardTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_IntervalTxt;
		public RectTransform_Text_Container IntervalTxt { get { return m_IntervalTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_AutoBtn;
		public RectTransform_Button_Image_Container AutoBtn { get { return m_AutoBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_AutoIcon;
		public RectTransform_Image_Container AutoIcon { get { return m_AutoIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_AutoTxt;
		public RectTransform_Text_Container AutoTxt { get { return m_AutoTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_CardFrame;
		public RectTransform_Image_Container CardFrame { get { return m_CardFrame; } }

		[SerializeField]
		private RectTransform_GunUI_GunCard_Container m_CardData;
		public RectTransform_GunUI_GunCard_Container CardData { get { return m_CardData; } }

		[SerializeField]
		private RectTransform_ButtonEx_Image_Container m_LvUpBtn;
		public RectTransform_ButtonEx_Image_Container LvUpBtn { get { return m_LvUpBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_CostTxt;
		public RectTransform_Text_Container CostTxt { get { return m_CostTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_SetGunBtn;
		public RectTransform_Button_Image_Container SetGunBtn { get { return m_SetGunBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_SetGunTxt;
		public RectTransform_Text_Container SetGunTxt { get { return m_SetGunTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_AutoTip;
		public RectTransform_Image_Container AutoTip { get { return m_AutoTip; } }

		[SerializeField]
		private RectTransform_Text_Container m_AutoTipTxt;
		public RectTransform_Text_Container AutoTipTxt { get { return m_AutoTipTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_LastBtn;
		public RectTransform_Button_Image_Container LastBtn { get { return m_LastBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_NextBtn;
		public RectTransform_Button_Image_Container NextBtn { get { return m_NextBtn; } }

		[SerializeField]
		private RectTransform_Container m_DataChangePanel;
		public RectTransform_Container DataChangePanel { get { return m_DataChangePanel; } }

		[SerializeField]
		private RectTransform_CampsitePointUI_DataChangeItem_Container m_RewardChangeItem;
		public RectTransform_CampsitePointUI_DataChangeItem_Container RewardChangeItem { get { return m_RewardChangeItem; } }

		[SerializeField]
		private RectTransform_CampsitePointUI_DataChangeItem_Container m_IntervalChangeItem;
		public RectTransform_CampsitePointUI_DataChangeItem_Container IntervalChangeItem { get { return m_IntervalChangeItem; } }

		[SerializeField]
		private RectTransform_CampsitePointUI_DataChangeItem_Container m_AutoChangeItem;
		public RectTransform_CampsitePointUI_DataChangeItem_Container AutoChangeItem { get { return m_AutoChangeItem; } }

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
		public class RectTransform_ButtonEx_Image_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private ButtonEx m_buttonEx;
			public ButtonEx buttonEx { get { return m_buttonEx; } }

			[SerializeField]
			private Image m_image;
			public Image image { get { return m_image; } }

		}

		[System.Serializable]
		public class RectTransform_CampsitePointUI_DataChangeItem_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampsitePointUI_DataChangeItem m_CampsitePointUI_DataChangeItem;
			public CampsitePointUI_DataChangeItem CampsitePointUI_DataChangeItem { get { return m_CampsitePointUI_DataChangeItem; } }

			private Queue<CampsitePointUI_DataChangeItem> mCachedInstances;
			public CampsitePointUI_DataChangeItem GetInstance() {
				CampsitePointUI_DataChangeItem instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampsitePointUI_DataChangeItem>(m_CampsitePointUI_DataChangeItem);
				}
				Transform t0 = m_CampsitePointUI_DataChangeItem.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampsitePointUI_DataChangeItem instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampsitePointUI_DataChangeItem>(); }
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
		public class RectTransform_GunUI_GunCard_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private GunUI_GunCard m_GunUI_GunCard;
			public GunUI_GunCard GunUI_GunCard { get { return m_GunUI_GunCard; } }

			private Queue<GunUI_GunCard> mCachedInstances;
			public GunUI_GunCard GetInstance() {
				GunUI_GunCard instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<GunUI_GunCard>(m_GunUI_GunCard);
				}
				Transform t0 = m_GunUI_GunCard.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(GunUI_GunCard instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<GunUI_GunCard>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

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
