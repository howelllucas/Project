using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class GunInfoUI : BaseUi {

		[SerializeField]
		private RectTransform_Text_Container m_TitleText;
		public RectTransform_Text_Container TitleText { get { return m_TitleText; } }

		[SerializeField]
		private RectTransform_Text_Container m_NameTxt;
		public RectTransform_Text_Container NameTxt { get { return m_NameTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_RarityTxt;
		public RectTransform_Text_Container RarityTxt { get { return m_RarityTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_TypeIcon;
		public RectTransform_Image_Container TypeIcon { get { return m_TypeIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_TypeTxt;
		public RectTransform_Text_Container TypeTxt { get { return m_TypeTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_Frame;
		public RectTransform_Image_Container Frame { get { return m_Frame; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_IconBtn;
		public RectTransform_Button_Image_Container IconBtn { get { return m_IconBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_TipsTitle;
		public RectTransform_Text_Container TipsTitle { get { return m_TipsTitle; } }

		[SerializeField]
		private RectTransform_Text_Container m_ChipCount;
		public RectTransform_Text_Container ChipCount { get { return m_ChipCount; } }

		[SerializeField]
		private RectTransform_Text_Container m_LevelTxt;
		public RectTransform_Text_Container LevelTxt { get { return m_LevelTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ResetBtn;
		public RectTransform_Button_Image_Container ResetBtn { get { return m_ResetBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_IconFrame1;
		public RectTransform_Image_Container IconFrame1 { get { return m_IconFrame1; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon1;
		public RectTransform_Image_Container Icon1 { get { return m_Icon1; } }

		[SerializeField]
		private RectTransform_Image_Container m_IconFrame2;
		public RectTransform_Image_Container IconFrame2 { get { return m_IconFrame2; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon2;
		public RectTransform_Image_Container Icon2 { get { return m_Icon2; } }

		[SerializeField]
		private RectTransform_Image_Container m_IconFrame3;
		public RectTransform_Image_Container IconFrame3 { get { return m_IconFrame3; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon3;
		public RectTransform_Image_Container Icon3 { get { return m_Icon3; } }

		[SerializeField]
		private RectTransform_Image_Container m_IconFrame4;
		public RectTransform_Image_Container IconFrame4 { get { return m_IconFrame4; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon4;
		public RectTransform_Image_Container Icon4 { get { return m_Icon4; } }

		[SerializeField]
		private RectTransform_Image_Container m_IconFrame5;
		public RectTransform_Image_Container IconFrame5 { get { return m_IconFrame5; } }

		[SerializeField]
		private RectTransform_Image_Container m_Icon5;
		public RectTransform_Image_Container Icon5 { get { return m_Icon5; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_StarUpBtn;
		public RectTransform_Button_Image_Container StarUpBtn { get { return m_StarUpBtn; } }

		[SerializeField]
		private RectTransform_Container m_RedPoint;
		public RectTransform_Container RedPoint { get { return m_RedPoint; } }

		[SerializeField]
		private RectTransform_Image_Container m_OccupiedFlag;
		public RectTransform_Image_Container OccupiedFlag { get { return m_OccupiedFlag; } }

		[SerializeField]
		private RectTransform_Text_Container m_FirePower;
		public RectTransform_Text_Container FirePower { get { return m_FirePower; } }

		[SerializeField]
		private RectTransform_Text_Container m_NextAtk;
		public RectTransform_Text_Container NextAtk { get { return m_NextAtk; } }

		[SerializeField]
		private RectTransform_Text_Container m_CurAtk;
		public RectTransform_Text_Container CurAtk { get { return m_CurAtk; } }

		[SerializeField]
		private RectTransform_Text_Container m_CurAtkSpeed;
		public RectTransform_Text_Container CurAtkSpeed { get { return m_CurAtkSpeed; } }

		[SerializeField]
		private RectTransform_Text_Container m_NextAtkSpeed;
		public RectTransform_Text_Container NextAtkSpeed { get { return m_NextAtkSpeed; } }

		[SerializeField]
		private RectTransform_Text_Container m_ProductionBonus;
		public RectTransform_Text_Container ProductionBonus { get { return m_ProductionBonus; } }

		[SerializeField]
		private RectTransform_Text_Container m_FuseSkillName;
		public RectTransform_Text_Container FuseSkillName { get { return m_FuseSkillName; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_FuseSkillIcon;
		public RectTransform_Button_Image_Container FuseSkillIcon { get { return m_FuseSkillIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_FuseSkillLevel;
		public RectTransform_Text_Container FuseSkillLevel { get { return m_FuseSkillLevel; } }

		[SerializeField]
		private RectTransform_Text_Container m_CampSkillName;
		public RectTransform_Text_Container CampSkillName { get { return m_CampSkillName; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CampSkillIcon;
		public RectTransform_Button_Image_Container CampSkillIcon { get { return m_CampSkillIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_CampSkillLevel;
		public RectTransform_Text_Container CampSkillLevel { get { return m_CampSkillLevel; } }

		[SerializeField]
		private RectTransform_CardCampDataChangeBubble_Container m_CampDataChangeBubble;
		public RectTransform_CardCampDataChangeBubble_Container CampDataChangeBubble { get { return m_CampDataChangeBubble; } }

		[SerializeField]
		private RectTransform_ButtonEx_Image_Container m_LvUpBtn;
		public RectTransform_ButtonEx_Image_Container LvUpBtn { get { return m_LvUpBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_GoldIcon;
		public RectTransform_Image_Container GoldIcon { get { return m_GoldIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_LvUpCost;
		public RectTransform_Text_Container LvUpCost { get { return m_LvUpCost; } }

		[SerializeField]
		private RectTransform_Container m_OptBtns;
		public RectTransform_Container OptBtns { get { return m_OptBtns; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_RemoveBtn;
		public RectTransform_Button_Image_Container RemoveBtn { get { return m_RemoveBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ReplaceBtn;
		public RectTransform_Button_Image_Container ReplaceBtn { get { return m_ReplaceBtn; } }

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
		public class RectTransform_ButtonEx_Image_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private ButtonEx m_buttonEx;
			public ButtonEx buttonEx { get { return m_buttonEx; } }

			[SerializeField]
			private Image m_image;
			public Image image { get { return m_image; } }

		}

		[System.Serializable]
		public class RectTransform_CardCampDataChangeBubble_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private CardCampDataChangeBubble m_CardCampDataChangeBubble;
			public CardCampDataChangeBubble CardCampDataChangeBubble { get { return m_CardCampDataChangeBubble; } }

			private Queue<CardCampDataChangeBubble> mCachedInstances;
			public CardCampDataChangeBubble GetInstance() {
				CardCampDataChangeBubble instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<CardCampDataChangeBubble>(m_CardCampDataChangeBubble);
				}
				Transform t0 = m_CardCampDataChangeBubble.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(CardCampDataChangeBubble instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<CardCampDataChangeBubble>(); }
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
