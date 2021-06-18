using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class ExpeditionUI : BaseUi {

		[SerializeField]
		private RectTransform_Image_Container m_Bg;
		public RectTransform_Image_Container Bg { get { return m_Bg; } }

		[SerializeField]
		private RectTransform_Image_Container m_RotateNode;
		public RectTransform_Image_Container RotateNode { get { return m_RotateNode; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_StartBtn;
		public RectTransform_Button_Image_Container StartBtn { get { return m_StartBtn; } }

		[SerializeField]
		private RectTransform_InputField_Image_Container m_InputFieldCmp;
		public RectTransform_InputField_Image_Container InputFieldCmp { get { return m_InputFieldCmp; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ChpaterBtn;
		public RectTransform_Button_Image_Container ChpaterBtn { get { return m_ChpaterBtn; } }

		[SerializeField]
		private RectTransform_Container m_IdleRewardNode;
		public RectTransform_Container IdleRewardNode { get { return m_IdleRewardNode; } }

		[SerializeField]
		private RectTransform_Image_Container m_IdleProgressImg;
		public RectTransform_Image_Container IdleProgressImg { get { return m_IdleProgressImg; } }

		[SerializeField]
		private RectTransform_Text_Container m_IdleRewardTxt;
		public RectTransform_Text_Container IdleRewardTxt { get { return m_IdleRewardTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_IdleRewardBtn;
		public RectTransform_Button_Image_Container IdleRewardBtn { get { return m_IdleRewardBtn; } }

		[SerializeField]
		private RectTransform_Container m_QuickIdleNode;
		public RectTransform_Container QuickIdleNode { get { return m_QuickIdleNode; } }

		[SerializeField]
		private RectTransform_Text_Container m_IdleRateTxt;
		public RectTransform_Text_Container IdleRateTxt { get { return m_IdleRateTxt; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_QuickIdleBtn;
		public RectTransform_Button_Image_Container QuickIdleBtn { get { return m_QuickIdleBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_ChangeWeaponBtn;
		public RectTransform_Button_Image_Container ChangeWeaponBtn { get { return m_ChangeWeaponBtn; } }

		[SerializeField]
		private RectTransform_Container m_LevelList;
		public RectTransform_Container LevelList { get { return m_LevelList; } }

		[SerializeField]
		private RectTransform_ExpeditionUI_LevelItem_Container m_LevelItem;
		public RectTransform_ExpeditionUI_LevelItem_Container LevelItem { get { return m_LevelItem; } }

		[SerializeField]
		private RectTransform_ExpeditionUI_LevelItem_Container m_LevelItem_Boss;
		public RectTransform_ExpeditionUI_LevelItem_Container LevelItem_Boss { get { return m_LevelItem_Boss; } }

		[SerializeField]
		private RectTransform_Container m_LevelRoot5;
		public RectTransform_Container LevelRoot5 { get { return m_LevelRoot5; } }

		[SerializeField]
		private RectTransform_Container m_LevelRoot3;
		public RectTransform_Container LevelRoot3 { get { return m_LevelRoot3; } }

		[SerializeField]
		private RectTransform_Container m_LevelRoot1;
		public RectTransform_Container LevelRoot1 { get { return m_LevelRoot1; } }

		[SerializeField]
		private RectTransform_Container m_LevelRoot7;
		public RectTransform_Container LevelRoot7 { get { return m_LevelRoot7; } }

		[SerializeField]
		private RectTransform_Container m_LevelRoot9;
		public RectTransform_Container LevelRoot9 { get { return m_LevelRoot9; } }

		[SerializeField]
		private RectTransform_Container m_LevelRoot4;
		public RectTransform_Container LevelRoot4 { get { return m_LevelRoot4; } }

		[SerializeField]
		private RectTransform_Container m_LevelRoot2;
		public RectTransform_Container LevelRoot2 { get { return m_LevelRoot2; } }

		[SerializeField]
		private RectTransform_Container m_LevelRoot6;
		public RectTransform_Container LevelRoot6 { get { return m_LevelRoot6; } }

		[SerializeField]
		private RectTransform_Container m_LevelRoot8;
		public RectTransform_Container LevelRoot8 { get { return m_LevelRoot8; } }

		[SerializeField]
		private RectTransform_Container m_LevelRoot10;
		public RectTransform_Container LevelRoot10 { get { return m_LevelRoot10; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_StarBtn;
		public RectTransform_Button_Image_Container StarBtn { get { return m_StarBtn; } }

		[SerializeField]
		private RectTransform_Text_Container m_StarTxt;
		public RectTransform_Text_Container StarTxt { get { return m_StarTxt; } }

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
		public class RectTransform_ExpeditionUI_LevelItem_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private ExpeditionUI_LevelItem m_ExpeditionUI_LevelItem;
			public ExpeditionUI_LevelItem ExpeditionUI_LevelItem { get { return m_ExpeditionUI_LevelItem; } }

			private Queue<ExpeditionUI_LevelItem> mCachedInstances;
			public ExpeditionUI_LevelItem GetInstance() {
				ExpeditionUI_LevelItem instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<ExpeditionUI_LevelItem>(m_ExpeditionUI_LevelItem);
				}
				Transform t0 = m_ExpeditionUI_LevelItem.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(ExpeditionUI_LevelItem instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<ExpeditionUI_LevelItem>(); }
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
		public class RectTransform_InputField_Image_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private InputField m_inputField;
			public InputField inputField { get { return m_inputField; } }

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
