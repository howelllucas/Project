using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampsitePointSetGunUI : BaseUi {

		[SerializeField]
		private RectTransform_Text_Container m_NameTxt;
		public RectTransform_Text_Container NameTxt { get { return m_NameTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_TypeImg;
		public RectTransform_Image_Container TypeImg { get { return m_TypeImg; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Container m_EquipCardRoot;
		public RectTransform_Container EquipCardRoot { get { return m_EquipCardRoot; } }

		[SerializeField]
		private RectTransform_CampsiteCardData_Container m_CardData;
		public RectTransform_CampsiteCardData_Container CardData { get { return m_CardData; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_RemoveBtn;
		public RectTransform_Button_Image_Container RemoveBtn { get { return m_RemoveBtn; } }

		[SerializeField]
		private RectTransform_Container m_NoneCardRoot;
		public RectTransform_Container NoneCardRoot { get { return m_NoneCardRoot; } }

		[SerializeField]
		private RectTransform_Container m_SelectContent;
		public RectTransform_Container SelectContent { get { return m_SelectContent; } }

		[SerializeField]
		private RectTransform_CampsitePointSetGunUI_SetItem_Container m_SetItem;
		public RectTransform_CampsitePointSetGunUI_SetItem_Container SetItem { get { return m_SetItem; } }

		[SerializeField]
		private RectTransform_Container m_SkillDescRoot;
		public RectTransform_Container SkillDescRoot { get { return m_SkillDescRoot; } }

		[SerializeField]
		private RectTransform_Text_Container m_SkillDescTxt;
		public RectTransform_Text_Container SkillDescTxt { get { return m_SkillDescTxt; } }

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
		public class RectTransform_CampsiteCardData_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampsiteCardData m_CampsiteCardData;
			public CampsiteCardData CampsiteCardData { get { return m_CampsiteCardData; } }

			private Queue<CampsiteCardData> mCachedInstances;
			public CampsiteCardData GetInstance() {
				CampsiteCardData instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampsiteCardData>(m_CampsiteCardData);
				}
				Transform t0 = m_CampsiteCardData.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampsiteCardData instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampsiteCardData>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_CampsitePointSetGunUI_SetItem_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CampsitePointSetGunUI_SetItem m_SetItem;
			public CampsitePointSetGunUI_SetItem SetItem { get { return m_SetItem; } }

			private Queue<CampsitePointSetGunUI_SetItem> mCachedInstances;
			public CampsitePointSetGunUI_SetItem GetInstance() {
				CampsitePointSetGunUI_SetItem instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CampsitePointSetGunUI_SetItem>(m_SetItem);
				}
				Transform t0 = m_SetItem.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CampsitePointSetGunUI_SetItem instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CampsitePointSetGunUI_SetItem>(); }
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
