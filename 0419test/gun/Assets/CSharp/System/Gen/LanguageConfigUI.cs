using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class LanguageConfigUI : BaseUi {

		[SerializeField]
		private RectTransform_Button_Image_Container m_BtnC;
		public RectTransform_Button_Image_Container BtnC { get { return m_BtnC; } }

		[SerializeField]
		private RectTransform_LanguageConfigUI_BtnLanguage_Container m_BtnLanguage;
		public RectTransform_LanguageConfigUI_BtnLanguage_Container BtnLanguage { get { return m_BtnLanguage; } }

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
		public class RectTransform_LanguageConfigUI_BtnLanguage_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private LanguageConfigUI_BtnLanguage m_BtnLanguage;
			public LanguageConfigUI_BtnLanguage BtnLanguage { get { return m_BtnLanguage; } }

			private Queue<LanguageConfigUI_BtnLanguage> mCachedInstances;
			public LanguageConfigUI_BtnLanguage GetInstance() {
				LanguageConfigUI_BtnLanguage instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<LanguageConfigUI_BtnLanguage>(m_BtnLanguage);
				}
				Transform t0 = m_BtnLanguage.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(LanguageConfigUI_BtnLanguage instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<LanguageConfigUI_BtnLanguage>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

	}

}
