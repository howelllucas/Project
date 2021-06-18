using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class BoxOpenUI : BaseUi {

		[SerializeField]
		private RectTransform_Container m_BoxRoot;
		public RectTransform_Container BoxRoot { get { return m_BoxRoot; } }

		[SerializeField]
		private RectTransform_Image_Container m_CardOpenImg;
		public RectTransform_Image_Container CardOpenImg { get { return m_CardOpenImg; } }

		[SerializeField]
		private RectTransform_Image_Container m_BoxDownImg;
		public RectTransform_Image_Container BoxDownImg { get { return m_BoxDownImg; } }

		[SerializeField]
		private RectTransform_Image_Container m_CardTopImg;
		public RectTransform_Image_Container CardTopImg { get { return m_CardTopImg; } }

		[SerializeField]
		private RectTransform_Container m_CardRoot;
		public RectTransform_Container CardRoot { get { return m_CardRoot; } }

		[SerializeField]
		private RectTransform_Image_Container m_ClickObj;
		public RectTransform_Image_Container ClickObj { get { return m_ClickObj; } }

		[SerializeField]
		private RectTransform_Text_Container m_BoxCountTxt;
		public RectTransform_Text_Container BoxCountTxt { get { return m_BoxCountTxt; } }

		[SerializeField]
		private RectTransform_GunCardIcon_Container m_NewCardIcon;
		public RectTransform_GunCardIcon_Container NewCardIcon { get { return m_NewCardIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_CardName;
		public RectTransform_Text_Container CardName { get { return m_CardName; } }

		[SerializeField]
		private RectTransform_Container m_Card2Chip;
		public RectTransform_Container Card2Chip { get { return m_Card2Chip; } }

		[SerializeField]
		private RectTransform_Text_Container m_CardChipCount;
		public RectTransform_Text_Container CardChipCount { get { return m_CardChipCount; } }

		[SerializeField]
		private RectTransform_Container m_CloseNode;
		public RectTransform_Container CloseNode { get { return m_CloseNode; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_OpenBtn;
		public RectTransform_Button_Image_Container OpenBtn { get { return m_OpenBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_OpenCostIcon;
		public RectTransform_Image_Container OpenCostIcon { get { return m_OpenCostIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_OpenCostTxt;
		public RectTransform_Text_Container OpenCostTxt { get { return m_OpenCostTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Container m_ShowRoot;
		public RectTransform_Container ShowRoot { get { return m_ShowRoot; } }

		[SerializeField]
		private RectTransform_GunCardIcon_Container m_ListCardIcon;
		public RectTransform_GunCardIcon_Container ListCardIcon { get { return m_ListCardIcon; } }

		[SerializeField]
		private RectTransform_Container m_NewCardInfo;
		public RectTransform_Container NewCardInfo { get { return m_NewCardInfo; } }

		[SerializeField]
		private RectTransform_Text_Container m_IdleBeforeTxt;
		public RectTransform_Text_Container IdleBeforeTxt { get { return m_IdleBeforeTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_IdleAfterTxt;
		public RectTransform_Text_Container IdleAfterTxt { get { return m_IdleAfterTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_CampOfflineBeforeTxt;
		public RectTransform_Text_Container CampOfflineBeforeTxt { get { return m_CampOfflineBeforeTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_CampOfflineAfterTxt;
		public RectTransform_Text_Container CampOfflineAfterTxt { get { return m_CampOfflineAfterTxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_TipsTxt;
		public RectTransform_Text_Container TipsTxt { get { return m_TipsTxt; } }

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
		public class RectTransform_GunCardIcon_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private GunCardIcon m_GunCardIcon;
			public GunCardIcon GunCardIcon { get { return m_GunCardIcon; } }

			private Queue<GunCardIcon> mCachedInstances;
			public GunCardIcon GetInstance() {
				GunCardIcon instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<GunCardIcon>(m_GunCardIcon);
				}
				Transform t0 = m_GunCardIcon.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(GunCardIcon instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<GunCardIcon>(); }
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
