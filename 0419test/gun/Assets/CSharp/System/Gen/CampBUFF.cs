using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampBUFF : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Container m_Content;
		public RectTransform_Container Content { get { return m_Content; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_maskBtn;
		public RectTransform_Button_Image_Container maskBtn { get { return m_maskBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_TopName;
		public RectTransform_Text_Container TopName { get { return m_TopName; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_buff_atkNpc;
		public RectTransform_Button_Image_Container buff_atkNpc { get { return m_buff_atkNpc; } }

		[SerializeField]
		private RectTransform_CampBUFF_ItemBig_Container m_ItemBig;
		public RectTransform_CampBUFF_ItemBig_Container ItemBig { get { return m_ItemBig; } }

		[SerializeField]
		private RectTransform_Image_Container m_lock1;
		public RectTransform_Image_Container lock1 { get { return m_lock1; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_buff_atk;
		public RectTransform_Button_Image_Container buff_atk { get { return m_buff_atk; } }

		[SerializeField]
		private RectTransform_CampBUFF_ItemSmall_Container m_ItemSmall;
		public RectTransform_CampBUFF_ItemSmall_Container ItemSmall { get { return m_ItemSmall; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_buff_fireSpeed;
		public RectTransform_Button_Image_Container buff_fireSpeed { get { return m_buff_fireSpeed; } }

		[SerializeField]
		private RectTransform_Image_Container m_lock2;
		public RectTransform_Image_Container lock2 { get { return m_lock2; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_buff_Beheading;
		public RectTransform_Button_Image_Container buff_Beheading { get { return m_buff_Beheading; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_buff_second;
		public RectTransform_Button_Image_Container buff_second { get { return m_buff_second; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_buff_robot;
		public RectTransform_Button_Image_Container buff_robot { get { return m_buff_robot; } }

		[SerializeField]
		private RectTransform_Image_Container m_lock3;
		public RectTransform_Image_Container lock3 { get { return m_lock3; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_buff_seckill;
		public RectTransform_Button_Image_Container buff_seckill { get { return m_buff_seckill; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_buff_critBoom;
		public RectTransform_Button_Image_Container buff_critBoom { get { return m_buff_critBoom; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_buff_critIgnition;
		public RectTransform_Button_Image_Container buff_critIgnition { get { return m_buff_critIgnition; } }

		[SerializeField]
		private RectTransform_CampBUFF_TopItemDetail_Container m_TopItemDetail;
		public RectTransform_CampBUFF_TopItemDetail_Container TopItemDetail { get { return m_TopItemDetail; } }

		[SerializeField]
		private RectTransform_CampBUFF_ItemDetail_Container m_ItemDetail;
		public RectTransform_CampBUFF_ItemDetail_Container ItemDetail { get { return m_ItemDetail; } }

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
		public class RectTransform_CampBUFF_ItemBig_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampBUFF_ItemBig m_ItemBig;
			public CampBUFF_ItemBig ItemBig { get { return m_ItemBig; } }

			private Queue<CampBUFF_ItemBig> mCachedInstances;
			public CampBUFF_ItemBig GetInstance() {
				CampBUFF_ItemBig instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampBUFF_ItemBig>(m_ItemBig);
				}
				Transform t0 = m_ItemBig.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampBUFF_ItemBig instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampBUFF_ItemBig>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_CampBUFF_ItemDetail_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampBUFF_ItemDetail m_ItemDetail;
			public CampBUFF_ItemDetail ItemDetail { get { return m_ItemDetail; } }

			private Queue<CampBUFF_ItemDetail> mCachedInstances;
			public CampBUFF_ItemDetail GetInstance() {
				CampBUFF_ItemDetail instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampBUFF_ItemDetail>(m_ItemDetail);
				}
				Transform t0 = m_ItemDetail.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampBUFF_ItemDetail instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampBUFF_ItemDetail>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_CampBUFF_ItemSmall_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampBUFF_ItemSmall m_ItemSmall;
			public CampBUFF_ItemSmall ItemSmall { get { return m_ItemSmall; } }

			private Queue<CampBUFF_ItemSmall> mCachedInstances;
			public CampBUFF_ItemSmall GetInstance() {
				CampBUFF_ItemSmall instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampBUFF_ItemSmall>(m_ItemSmall);
				}
				Transform t0 = m_ItemSmall.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampBUFF_ItemSmall instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampBUFF_ItemSmall>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_CampBUFF_TopItemDetail_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampBUFF_TopItemDetail m_TopItemDetail;
			public CampBUFF_TopItemDetail TopItemDetail { get { return m_TopItemDetail; } }

			private Queue<CampBUFF_TopItemDetail> mCachedInstances;
			public CampBUFF_TopItemDetail GetInstance() {
				CampBUFF_TopItemDetail instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampBUFF_TopItemDetail>(m_TopItemDetail);
				}
				Transform t0 = m_TopItemDetail.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampBUFF_TopItemDetail instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampBUFF_TopItemDetail>(); }
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
