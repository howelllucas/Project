using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GunListUI : BaseUi {

		[SerializeField]
		private RectTransform_Text_Container m_TitleText;
		public RectTransform_Text_Container TitleText { get { return m_TitleText; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_GunListUI_GunCard_Container m_GunCard;
		public RectTransform_GunListUI_GunCard_Container GunCard { get { return m_GunCard; } }

		[SerializeField]
		private RectTransform_Image_Container m_CurImage;
		public RectTransform_Image_Container CurImage { get { return m_CurImage; } }

		[SerializeField]
		private RectTransform_Image_Container m_WeaponViewport;
		public RectTransform_Image_Container WeaponViewport { get { return m_WeaponViewport; } }

		[SerializeField]
		private RectTransform_Container m_WeaponContent;
		public RectTransform_Container WeaponContent { get { return m_WeaponContent; } }

		[SerializeField]
		private RectTransform_Text_Container m_EmptyTxt;
		public RectTransform_Text_Container EmptyTxt { get { return m_EmptyTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_GotoBtn;
		public RectTransform_Button_Image_Container GotoBtn { get { return m_GotoBtn; } }

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
		public class RectTransform_GunListUI_GunCard_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private GunListUI_GunCard m_GunCard;
			public GunListUI_GunCard GunCard { get { return m_GunCard; } }

			private Queue<GunListUI_GunCard> mCachedInstances;
			public GunListUI_GunCard GetInstance() {
				GunListUI_GunCard instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<GunListUI_GunCard>(m_GunCard);
				}
				Transform t0 = m_GunCard.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(GunListUI_GunCard instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<GunListUI_GunCard>(); }
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
