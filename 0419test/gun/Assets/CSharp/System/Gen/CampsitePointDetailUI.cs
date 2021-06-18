using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampsitePointDetailUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_Title;
		public RectTransform_Text_Container Title { get { return m_Title; } }

		[SerializeField]
		private RectTransform_CampDetailItem_Container m_RewardBase;
		public RectTransform_CampDetailItem_Container RewardBase { get { return m_RewardBase; } }

		[SerializeField]
		private RectTransform_CampDetailItem_Container m_RewardCardBase;
		public RectTransform_CampDetailItem_Container RewardCardBase { get { return m_RewardCardBase; } }

		[SerializeField]
		private RectTransform_CampDetailItem_Container m_RewardCardSkill;
		public RectTransform_CampDetailItem_Container RewardCardSkill { get { return m_RewardCardSkill; } }

		[SerializeField]
		private RectTransform_CampDetailItem_Container m_RewardPlayer;
		public RectTransform_CampDetailItem_Container RewardPlayer { get { return m_RewardPlayer; } }

		[SerializeField]
		private RectTransform_Text_Container m_RewardSettleTxt;
		public RectTransform_Text_Container RewardSettleTxt { get { return m_RewardSettleTxt; } }

		[SerializeField]
		private RectTransform_CampDetailItem_Container m_IntervalBase;
		public RectTransform_CampDetailItem_Container IntervalBase { get { return m_IntervalBase; } }

		[SerializeField]
		private RectTransform_CampDetailItem_Container m_IntervalCardSkill;
		public RectTransform_CampDetailItem_Container IntervalCardSkill { get { return m_IntervalCardSkill; } }

		[SerializeField]
		private RectTransform_Text_Container m_IntervalSettleTxt;
		public RectTransform_Text_Container IntervalSettleTxt { get { return m_IntervalSettleTxt; } }

		[SerializeField]
		private RectTransform_CampDetailItem_Container m_CostBase;
		public RectTransform_CampDetailItem_Container CostBase { get { return m_CostBase; } }

		[SerializeField]
		private RectTransform_Text_Container m_CostSettleTxt;
		public RectTransform_Text_Container CostSettleTxt { get { return m_CostSettleTxt; } }

		[SerializeField]
		private RectTransform_CampDetailItem_Container m_AutoCardLv;
		public RectTransform_CampDetailItem_Container AutoCardLv { get { return m_AutoCardLv; } }

		[SerializeField]
		private RectTransform_Text_Container m_AutoSettleTxt;
		public RectTransform_Text_Container AutoSettleTxt { get { return m_AutoSettleTxt; } }

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
		public class RectTransform_CampDetailItem_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampDetailItem m_CampDetailItem;
			public CampDetailItem CampDetailItem { get { return m_CampDetailItem; } }

			private Queue<CampDetailItem> mCachedInstances;
			public CampDetailItem GetInstance() {
				CampDetailItem instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampDetailItem>(m_CampDetailItem);
				}
				Transform t0 = m_CampDetailItem.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampDetailItem instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampDetailItem>(); }
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
