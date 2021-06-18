using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GunStarUpUI : BaseUi {

		[SerializeField]
		private RectTransform_Text_Container m_NameTxt;
		public RectTransform_Text_Container NameTxt { get { return m_NameTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_CurStarBack1;
		public RectTransform_Image_Container CurStarBack1 { get { return m_CurStarBack1; } }

		[SerializeField]
		private RectTransform_Image_Container m_CurStar1;
		public RectTransform_Image_Container CurStar1 { get { return m_CurStar1; } }

		[SerializeField]
		private RectTransform_Image_Container m_CurStarBack2;
		public RectTransform_Image_Container CurStarBack2 { get { return m_CurStarBack2; } }

		[SerializeField]
		private RectTransform_Image_Container m_CurStar2;
		public RectTransform_Image_Container CurStar2 { get { return m_CurStar2; } }

		[SerializeField]
		private RectTransform_Image_Container m_CurStarBack3;
		public RectTransform_Image_Container CurStarBack3 { get { return m_CurStarBack3; } }

		[SerializeField]
		private RectTransform_Image_Container m_CurStar3;
		public RectTransform_Image_Container CurStar3 { get { return m_CurStar3; } }

		[SerializeField]
		private RectTransform_Image_Container m_CurStarBack4;
		public RectTransform_Image_Container CurStarBack4 { get { return m_CurStarBack4; } }

		[SerializeField]
		private RectTransform_Image_Container m_CurStar4;
		public RectTransform_Image_Container CurStar4 { get { return m_CurStar4; } }

		[SerializeField]
		private RectTransform_Image_Container m_CurStarBack5;
		public RectTransform_Image_Container CurStarBack5 { get { return m_CurStarBack5; } }

		[SerializeField]
		private RectTransform_Image_Container m_CurStar5;
		public RectTransform_Image_Container CurStar5 { get { return m_CurStar5; } }

		[SerializeField]
		private RectTransform_Image_Container m_UpStarBack1;
		public RectTransform_Image_Container UpStarBack1 { get { return m_UpStarBack1; } }

		[SerializeField]
		private RectTransform_Image_Container m_UpStar1;
		public RectTransform_Image_Container UpStar1 { get { return m_UpStar1; } }

		[SerializeField]
		private RectTransform_Image_Container m_UpStarBack2;
		public RectTransform_Image_Container UpStarBack2 { get { return m_UpStarBack2; } }

		[SerializeField]
		private RectTransform_Image_Container m_UpStar2;
		public RectTransform_Image_Container UpStar2 { get { return m_UpStar2; } }

		[SerializeField]
		private RectTransform_Image_Container m_UpStarBack3;
		public RectTransform_Image_Container UpStarBack3 { get { return m_UpStarBack3; } }

		[SerializeField]
		private RectTransform_Image_Container m_UpStar3;
		public RectTransform_Image_Container UpStar3 { get { return m_UpStar3; } }

		[SerializeField]
		private RectTransform_Image_Container m_UpStarBack4;
		public RectTransform_Image_Container UpStarBack4 { get { return m_UpStarBack4; } }

		[SerializeField]
		private RectTransform_Image_Container m_UpStar4;
		public RectTransform_Image_Container UpStar4 { get { return m_UpStar4; } }

		[SerializeField]
		private RectTransform_Image_Container m_UpStarBack5;
		public RectTransform_Image_Container UpStarBack5 { get { return m_UpStarBack5; } }

		[SerializeField]
		private RectTransform_Image_Container m_UpStar5;
		public RectTransform_Image_Container UpStar5 { get { return m_UpStar5; } }

		[SerializeField]
		private RectTransform_Text_Container m_CurLevel;
		public RectTransform_Text_Container CurLevel { get { return m_CurLevel; } }

		[SerializeField]
		private RectTransform_Text_Container m_NextLevel;
		public RectTransform_Text_Container NextLevel { get { return m_NextLevel; } }

		[SerializeField]
		private RectTransform_Text_Container m_CurDPS;
		public RectTransform_Text_Container CurDPS { get { return m_CurDPS; } }

		[SerializeField]
		private RectTransform_Text_Container m_NextDPS;
		public RectTransform_Text_Container NextDPS { get { return m_NextDPS; } }

		[SerializeField]
		private RectTransform_Text_Container m_CurAtk;
		public RectTransform_Text_Container CurAtk { get { return m_CurAtk; } }

		[SerializeField]
		private RectTransform_Text_Container m_NextAtk;
		public RectTransform_Text_Container NextAtk { get { return m_NextAtk; } }

		[SerializeField]
		private RectTransform_Text_Container m_SkillName;
		public RectTransform_Text_Container SkillName { get { return m_SkillName; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_SkillIcon;
		public RectTransform_Button_Image_Container SkillIcon { get { return m_SkillIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_SkillLevel;
		public RectTransform_Text_Container SkillLevel { get { return m_SkillLevel; } }

		[SerializeField]
		private RectTransform_Container m_ChipRoot;
		public RectTransform_Container ChipRoot { get { return m_ChipRoot; } }

		[SerializeField]
		private RectTransform_GunStarUpUI_GunCard_Container m_GunCard;
		public RectTransform_GunStarUpUI_GunCard_Container GunCard { get { return m_GunCard; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_StarUpBtn;
		public RectTransform_Button_Image_Container StarUpBtn { get { return m_StarUpBtn; } }

		[SerializeField]
		private RectTransform_Container m_CardsRoot;
		public RectTransform_Container CardsRoot { get { return m_CardsRoot; } }

		[SerializeField]
		private RectTransform_Image_Container m_WeaponViewport;
		public RectTransform_Image_Container WeaponViewport { get { return m_WeaponViewport; } }

		[SerializeField]
		private RectTransform_Container m_WeaponContent;
		public RectTransform_Container WeaponContent { get { return m_WeaponContent; } }

		[SerializeField]
		private RectTransform_GunStarUpUI_ChooseChip_Container m_ChooseChip;
		public RectTransform_GunStarUpUI_ChooseChip_Container ChooseChip { get { return m_ChooseChip; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseChooseBtn;
		public RectTransform_Button_Image_Container CloseChooseBtn { get { return m_CloseChooseBtn; } }

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
		public class RectTransform_GunStarUpUI_ChooseChip_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private GunStarUpUI_ChooseChip m_ChooseChip;
			public GunStarUpUI_ChooseChip ChooseChip { get { return m_ChooseChip; } }

			private Queue<GunStarUpUI_ChooseChip> mCachedInstances;
			public GunStarUpUI_ChooseChip GetInstance() {
				GunStarUpUI_ChooseChip instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<GunStarUpUI_ChooseChip>(m_ChooseChip);
				}
				Transform t0 = m_ChooseChip.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(GunStarUpUI_ChooseChip instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<GunStarUpUI_ChooseChip>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_GunStarUpUI_GunCard_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private GunStarUpUI_GunCard m_GunCard;
			public GunStarUpUI_GunCard GunCard { get { return m_GunCard; } }

			private Queue<GunStarUpUI_GunCard> mCachedInstances;
			public GunStarUpUI_GunCard GetInstance() {
				GunStarUpUI_GunCard instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<GunStarUpUI_GunCard>(m_GunCard);
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
			public bool CacheInstance(GunStarUpUI_GunCard instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<GunStarUpUI_GunCard>(); }
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
