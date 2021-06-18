using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampsiteUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_NameBg;
		public RectTransform_Button_Image_Container NameBg { get { return m_NameBg; } }

		[SerializeField]
		private RectTransform_Container m_NameEffectNode;
		public RectTransform_Container NameEffectNode { get { return m_NameEffectNode; } }

		[SerializeField]
		private RectTransform_Text_Container m_CampName;
		public RectTransform_Text_Container CampName { get { return m_CampName; } }

		[SerializeField]
		private RectTransform_Text_Container m_CurNpcNum;
		public RectTransform_Text_Container CurNpcNum { get { return m_CurNpcNum; } }

		[SerializeField]
		private RectTransform_Text_Container m_MaxNpcNum;
		public RectTransform_Text_Container MaxNpcNum { get { return m_MaxNpcNum; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ResourceBtn;
		public RectTransform_Button_Image_Container ResourceBtn { get { return m_ResourceBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Buff;
		public RectTransform_Button_Image_Container Buff { get { return m_Buff; } }

		[SerializeField]
		private RectTransform_Container m_ResState;
		public RectTransform_Container ResState { get { return m_ResState; } }

		[SerializeField]
		private RectTransform_Text_Container m_CmpState;
		public RectTransform_Text_Container CmpState { get { return m_CmpState; } }

		[SerializeField]
		private RectTransform_Container m_TaskStateNode;
		public RectTransform_Container TaskStateNode { get { return m_TaskStateNode; } }

		[SerializeField]
		private RectTransform_Container m_AdIcon;
		public RectTransform_Container AdIcon { get { return m_AdIcon; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_AdIconBtn;
		public RectTransform_Button_Image_Container AdIconBtn { get { return m_AdIconBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_AdCountDown;
		public RectTransform_Text_Container AdCountDown { get { return m_AdCountDown; } }

		[SerializeField]
		private RectTransform_Image_Container m_ResourceNode;
		public RectTransform_Image_Container ResourceNode { get { return m_ResourceNode; } }

		[SerializeField]
		private RectTransform_CampsiteUI_ResourceItemUI_Container m_ResourceItemUI;
		public RectTransform_CampsiteUI_ResourceItemUI_Container ResourceItemUI { get { return m_ResourceItemUI; } }

		[SerializeField]
		private RectTransform_Container m_DrstrangeIcon;
		public RectTransform_Container DrstrangeIcon { get { return m_DrstrangeIcon; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_DrstrangeIconBtn;
		public RectTransform_Button_Image_Container DrstrangeIconBtn { get { return m_DrstrangeIconBtn; } }

		[SerializeField]
		private RectTransform_Container m_RecycleIcon;
		public RectTransform_Container RecycleIcon { get { return m_RecycleIcon; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_RecycleIconBtn;
		public RectTransform_Button_Image_Container RecycleIconBtn { get { return m_RecycleIconBtn; } }

		[SerializeField]
		private RectTransform_Container m_OldWamonIcon;
		public RectTransform_Container OldWamonIcon { get { return m_OldWamonIcon; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_OldWamonIconBtn;
		public RectTransform_Button_Image_Container OldWamonIconBtn { get { return m_OldWamonIconBtn; } }

		[SerializeField]
		private RectTransform_Container m_BadgeIcon;
		public RectTransform_Container BadgeIcon { get { return m_BadgeIcon; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_BadgeIconBtn;
		public RectTransform_Button_Image_Container BadgeIconBtn { get { return m_BadgeIconBtn; } }

		[SerializeField]
		private RectTransform_Container m_RedHeartNode;
		public RectTransform_Container RedHeartNode { get { return m_RedHeartNode; } }

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
		public class RectTransform_CampsiteUI_ResourceItemUI_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampsiteUI_ResourceItemUI m_ResourceItemUI;
			public CampsiteUI_ResourceItemUI ResourceItemUI { get { return m_ResourceItemUI; } }

			private Queue<CampsiteUI_ResourceItemUI> mCachedInstances;
			public CampsiteUI_ResourceItemUI GetInstance() {
				CampsiteUI_ResourceItemUI instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampsiteUI_ResourceItemUI>(m_ResourceItemUI);
				}
				Transform t0 = m_ResourceItemUI.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampsiteUI_ResourceItemUI instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampsiteUI_ResourceItemUI>(); }
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
