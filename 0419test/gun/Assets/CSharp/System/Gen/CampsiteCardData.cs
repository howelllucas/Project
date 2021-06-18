using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampsiteCardData : MonoBehaviour {

		[SerializeField]
		private RectTransform_GunUI_GunCard_Container m_CardData;
		public RectTransform_GunUI_GunCard_Container CardData { get { return m_CardData; } }

		[SerializeField]
		private RectTransform_Container m_RewardFactorNode;
		public RectTransform_Container RewardFactorNode { get { return m_RewardFactorNode; } }

		[SerializeField]
		private RectTransform_Text_Container m_RewardFactorTxt;
		public RectTransform_Text_Container RewardFactorTxt { get { return m_RewardFactorTxt; } }

		[SerializeField]
		private RectTransform_Container m_IntervalFactorNode;
		public RectTransform_Container IntervalFactorNode { get { return m_IntervalFactorNode; } }

		[SerializeField]
		private RectTransform_Text_Container m_IntervalFactorTxt;
		public RectTransform_Text_Container IntervalFactorTxt { get { return m_IntervalFactorTxt; } }

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
		private RectTransform_Image_Container m_AutoTip;
		public RectTransform_Image_Container AutoTip { get { return m_AutoTip; } }

		[SerializeField]
		private RectTransform_Text_Container m_AutoTipTxt;
		public RectTransform_Text_Container AutoTipTxt { get { return m_AutoTipTxt; } }

		[SerializeField]
		private RectTransform_CampSkill_Container m_CampSkill;
		public RectTransform_CampSkill_Container CampSkill { get { return m_CampSkill; } }

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
		public class RectTransform_CampSkill_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampSkill m_CampSkill;
			public CampSkill CampSkill { get { return m_CampSkill; } }

			private Queue<CampSkill> mCachedInstances;
			public CampSkill GetInstance() {
				CampSkill instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampSkill>(m_CampSkill);
				}
				Transform t0 = m_CampSkill.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampSkill instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampSkill>(); }
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
