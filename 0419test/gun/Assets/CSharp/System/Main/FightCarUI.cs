using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class FightCarUI : MonoBehaviour {

	[SerializeField]
	private RectTransform_Container BossHpNode;

	[SerializeField]
	private RectTransform_Text_Container cointxt;

	[SerializeField]
	private RectTransform_Image_Container exit;

	[SerializeField]
	private RectTransform_Button_Image_Container pause;

	[SerializeField]
	private RectTransform_Image_Container isdone;

	[SerializeField]
	private RectTransform_Text_Container wavetxt;

	[SerializeField]
	private RectTransform_Text_Container countdownname;

	[SerializeField]
	private RectTransform_Text_Container countdowntime;

	[SerializeField]
	private RectTransform_Text_Container timetxt;

	[SerializeField]
	private RectTransform_FightCarUI_HeartItem_Container HeartItem;

	[SerializeField]
	private RectTransform_Container RightTop;

	[SerializeField]
	private RectTransform_Container LeftTop;

	[SerializeField]
	private RectTransform_Container LeftBottom;

	[SerializeField]
	private RectTransform_Container RightBottom;

	[SerializeField]
	private RectTransform_Image_Container ArrowBg;

	[SerializeField]
	private RectTransform_Container RotateNode;

	[SerializeField]
	private RectTransform_Text_Container Distance;

	[SerializeField]
	private RectTransform_FightCarUI_Itemtime_Container Itemtime;

	[System.Serializable]
	private class RectTransform_Button_Image_Container {

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
	private class RectTransform_Container {

		[SerializeField]
		private GameObject m_GameObject;
		public GameObject gameObject { get { return m_GameObject; } }

		[SerializeField]
		private RectTransform m_rectTransform;
		public RectTransform rectTransform { get { return m_rectTransform; } }

	}

	[System.Serializable]
	private class RectTransform_FightCarUI_HeartItem_Container {

		[SerializeField]
		private GameObject m_GameObject;
		public GameObject gameObject { get { return m_GameObject; } }

		[SerializeField]
		private RectTransform m_rectTransform;
		public RectTransform rectTransform { get { return m_rectTransform; } }

		[SerializeField]
		private FightCarUI_HeartItem m_HeartItem;
		public FightCarUI_HeartItem HeartItem { get { return m_HeartItem; } }

		private Queue<FightCarUI_HeartItem> mCachedInstances;
		public FightCarUI_HeartItem GetInstance() {
			FightCarUI_HeartItem instance = null;
			if (mCachedInstances != null) {
				while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
					instance = mCachedInstances.Dequeue();
				}
			}
			if (instance == null || instance.Equals(null)) {
				instance = Instantiate<FightCarUI_HeartItem>(m_HeartItem);
			}
			Transform t0 = m_HeartItem.transform;
			Transform t1 = instance.transform;
			t1.SetParent(t0.parent);
			t1.localPosition = t0.localPosition;
			t1.localRotation = t0.localRotation;
			t1.localScale = t0.localScale;
			t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
			return instance;
		}
		public bool CacheInstance(FightCarUI_HeartItem instance) {
			if (instance == null || instance.Equals(null)) { return false; }
			if (mCachedInstances == null) { mCachedInstances = new Queue<FightCarUI_HeartItem>(); }
			if (mCachedInstances.Contains(instance)) { return false; }
			instance.gameObject.SetActive(false);
			mCachedInstances.Enqueue(instance);
			return true;
		}

	}

	[System.Serializable]
	private class RectTransform_FightCarUI_Itemtime_Container {

		[SerializeField]
		private GameObject m_GameObject;
		public GameObject gameObject { get { return m_GameObject; } }

		[SerializeField]
		private RectTransform m_rectTransform;
		public RectTransform rectTransform { get { return m_rectTransform; } }

		[SerializeField]
		private FightCarUI_Itemtime m_Itemtime;
		public FightCarUI_Itemtime Itemtime { get { return m_Itemtime; } }

		private Queue<FightCarUI_Itemtime> mCachedInstances;
		public FightCarUI_Itemtime GetInstance() {
			FightCarUI_Itemtime instance = null;
			if (mCachedInstances != null) {
				while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
					instance = mCachedInstances.Dequeue();
				}
			}
			if (instance == null || instance.Equals(null)) {
				instance = Instantiate<FightCarUI_Itemtime>(m_Itemtime);
			}
			Transform t0 = m_Itemtime.transform;
			Transform t1 = instance.transform;
			t1.SetParent(t0.parent);
			t1.localPosition = t0.localPosition;
			t1.localRotation = t0.localRotation;
			t1.localScale = t0.localScale;
			t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
			return instance;
		}
		public bool CacheInstance(FightCarUI_Itemtime instance) {
			if (instance == null || instance.Equals(null)) { return false; }
			if (mCachedInstances == null) { mCachedInstances = new Queue<FightCarUI_Itemtime>(); }
			if (mCachedInstances.Contains(instance)) { return false; }
			instance.gameObject.SetActive(false);
			mCachedInstances.Enqueue(instance);
			return true;
		}

	}

	[System.Serializable]
	private class RectTransform_Image_Container {

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
	private class RectTransform_Text_Container {

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
