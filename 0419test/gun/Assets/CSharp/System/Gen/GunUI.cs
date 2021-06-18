using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GunUI : BaseUi {

		[SerializeField]
		private RectTransform_Text_Container m_GunCount;
		public RectTransform_Text_Container GunCount { get { return m_GunCount; } }

		[SerializeField]
		private RectTransform_Text_Container m_Title;
		public RectTransform_Text_Container Title { get { return m_Title; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ChipBtn;
		public RectTransform_Button_Image_Container ChipBtn { get { return m_ChipBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_ScrollView;
		public RectTransform_Image_Container ScrollView { get { return m_ScrollView; } }

		[SerializeField]
		private RectTransform_Container m_BattleCards;
		public RectTransform_Container BattleCards { get { return m_BattleCards; } }

		[SerializeField]
		private RectTransform_Container m_ChooseCards;
		public RectTransform_Container ChooseCards { get { return m_ChooseCards; } }

		[SerializeField]
		private RectTransform_GunUI_GunCard_Container m_GunCard;
		public RectTransform_GunUI_GunCard_Container GunCard { get { return m_GunCard; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_SubGunBtn1;
		public RectTransform_Button_Image_Container SubGunBtn1 { get { return m_SubGunBtn1; } }

		[SerializeField]
		private RectTransform_Image_Container m_SubGunLockImg1;
		public RectTransform_Image_Container SubGunLockImg1 { get { return m_SubGunLockImg1; } }

		[SerializeField]
		private RectTransform_Container m_SubGunRedPoint1;
		public RectTransform_Container SubGunRedPoint1 { get { return m_SubGunRedPoint1; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_SubGunBtn2;
		public RectTransform_Button_Image_Container SubGunBtn2 { get { return m_SubGunBtn2; } }

		[SerializeField]
		private RectTransform_Image_Container m_SubGunLockImg2;
		public RectTransform_Image_Container SubGunLockImg2 { get { return m_SubGunLockImg2; } }

		[SerializeField]
		private RectTransform_Container m_SubGunRedPoint2;
		public RectTransform_Container SubGunRedPoint2 { get { return m_SubGunRedPoint2; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_SubGunBtn3;
		public RectTransform_Button_Image_Container SubGunBtn3 { get { return m_SubGunBtn3; } }

		[SerializeField]
		private RectTransform_Image_Container m_SubGunLockImg3;
		public RectTransform_Image_Container SubGunLockImg3 { get { return m_SubGunLockImg3; } }

		[SerializeField]
		private RectTransform_Container m_SubGunRedPoint3;
		public RectTransform_Container SubGunRedPoint3 { get { return m_SubGunRedPoint3; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_SubGunBtn4;
		public RectTransform_Button_Image_Container SubGunBtn4 { get { return m_SubGunBtn4; } }

		[SerializeField]
		private RectTransform_Image_Container m_SubGunLockImg4;
		public RectTransform_Image_Container SubGunLockImg4 { get { return m_SubGunLockImg4; } }

		[SerializeField]
		private RectTransform_Container m_SubGunRedPoint4;
		public RectTransform_Container SubGunRedPoint4 { get { return m_SubGunRedPoint4; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_MainGunSkillBtn;
		public RectTransform_Button_Image_Container MainGunSkillBtn { get { return m_MainGunSkillBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_SubGunSkillBtn1;
		public RectTransform_Button_Image_Container SubGunSkillBtn1 { get { return m_SubGunSkillBtn1; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_SubGunSkillBtn2;
		public RectTransform_Button_Image_Container SubGunSkillBtn2 { get { return m_SubGunSkillBtn2; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_SubGunSkillBtn3;
		public RectTransform_Button_Image_Container SubGunSkillBtn3 { get { return m_SubGunSkillBtn3; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_SubGunSkillBtn4;
		public RectTransform_Button_Image_Container SubGunSkillBtn4 { get { return m_SubGunSkillBtn4; } }

		[SerializeField]
		private RectTransform_GunUI_GunSkill_Container m_GunSkill;
		public RectTransform_GunUI_GunSkill_Container GunSkill { get { return m_GunSkill; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_MainGunBtn;
		public RectTransform_Button_Image_Container MainGunBtn { get { return m_MainGunBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_Bg;
		public RectTransform_Image_Container Bg { get { return m_Bg; } }

		[SerializeField]
		private RectTransform_Container m_MainRoleAdapter;
		public RectTransform_Container MainRoleAdapter { get { return m_MainRoleAdapter; } }

		[SerializeField]
		private Transform_Container m_MainRoleNode;
		public Transform_Container MainRoleNode { get { return m_MainRoleNode; } }

		[SerializeField]
		private RectTransform_Text_Container m_FirePower;
		public RectTransform_Text_Container FirePower { get { return m_FirePower; } }

		[SerializeField]
		private RectTransform_Text_Container m_NextAtk;
		public RectTransform_Text_Container NextAtk { get { return m_NextAtk; } }

		[SerializeField]
		private RectTransform_Container m_HeroCards;
		public RectTransform_Container HeroCards { get { return m_HeroCards; } }

		[SerializeField]
		private RectTransform_Container m_LockCards;
		public RectTransform_Container LockCards { get { return m_LockCards; } }

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
		public class RectTransform_GunUI_GunCard_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private GunUI_GunCard m_GunCard;
			public GunUI_GunCard GunCard { get { return m_GunCard; } }

			private Queue<GunUI_GunCard> mCachedInstances;
			public GunUI_GunCard GetInstance() {
				GunUI_GunCard instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<GunUI_GunCard>(m_GunCard);
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
		public class RectTransform_GunUI_GunSkill_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private GunUI_GunSkill m_GunSkill;
			public GunUI_GunSkill GunSkill { get { return m_GunSkill; } }

			private Queue<GunUI_GunSkill> mCachedInstances;
			public GunUI_GunSkill GetInstance() {
				GunUI_GunSkill instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<GunUI_GunSkill>(m_GunSkill);
				}
				Transform t0 = m_GunSkill.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(GunUI_GunSkill instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<GunUI_GunSkill>(); }
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

		[System.Serializable]
		public class Transform_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private Transform m_transform;
			public Transform transform { get { return m_transform; } }

		}

	}

}
