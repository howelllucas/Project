using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class MainUi : BaseUi {

		[SerializeField]
		private RectTransform_Image_Container Bg;

		[SerializeField]
		private RectTransform_Container MainRoleAdapter;

		[SerializeField]
		private RectTransform_Container Bgeffect;

		[SerializeField]
		private Transform_Container MainRoleNode;

		[SerializeField]
		private Transform_Container UI_fire_1;

		[SerializeField]
		private RectTransform_Container FireAdapter;

		[SerializeField]
		private RectTransform_Button_Image_Container Head;

		[SerializeField]
		private RectTransform_Text_Container LevelTxt;

		[SerializeField]
		private RectTransform_Image_Container Exp;

		[SerializeField]
		private RectTransform_Button_Image_Container Config;

		[SerializeField]
		private RectTransform_Button_Image_Container Achi;

		[SerializeField]
		private RectTransform_Image_Container exp;

		[SerializeField]
		private RectTransform_Text_Container level;

		[SerializeField]
		private RectTransform_Button_Image_Container SevenDay;

		[SerializeField]
		private RectTransform_Image_Container icon;

		[SerializeField]
		private RectTransform_Image_Container SevenDayBg;

		[SerializeField]
		private RectTransform_Button_Container FirstCharge;

		[SerializeField]
		private RectTransform_Image_Container fTip;

		[SerializeField]
		private RectTransform_Button_Container TimeGift1;

		[SerializeField]
		private RectTransform_Image_Container TimeIcon1;

		[SerializeField]
		private RectTransform_Text_Container TimeTip1;

		[SerializeField]
		private RectTransform_Button_Container TimeGift2;

		[SerializeField]
		private RectTransform_Image_Container TimeIcon2;

		[SerializeField]
		private RectTransform_Text_Container TimeTip2;

		[SerializeField]
		private RectTransform_Button_Image_Container levelshow;

		[SerializeField]
		private RectTransform_Image_Container levelBg;

		[SerializeField]
		private RectTransform_Image_Container SceneImg;

		[SerializeField]
		private RectTransform_Image_Container Reward;

		[SerializeField]
		private RectTransform_Image_Container levelContent;

		[SerializeField]
		private RectTransform_Image_Container pass1;

		[SerializeField]
		private RectTransform_Image_Container pass2;

		[SerializeField]
		private RectTransform_Image_Container pass3;

		[SerializeField]
		private RectTransform_Image_Container passProgress;

		[SerializeField]
		private RectTransform_Image_Container rewardBox;

		[SerializeField]
		private RectTransform_Image_Container boxIcon;

		[SerializeField]
		private RectTransform_Container BranchLevel;

		[SerializeField]
		private RectTransform_Image_Container GetMDTLight;

		[SerializeField]
		private RectTransform_Button_Image_Container GetMDT;

		[SerializeField]
		private RectTransform_Button_Image_Container Btnstart;

		[SerializeField]
		private RectTransform_InputField_Image_Container InputFieldCmp;

		[SerializeField]
		private RectTransform_Button_Image_Container Btnstartbranch;

		[SerializeField]
		private RectTransform_Button_Image_Container BtnRandompass;

		[SerializeField]
		private RectTransform_Image_Container newNumBg;

		[SerializeField]
		private RectTransform_Text_Container newNum;

		[SerializeField]
		private RectTransform_Image_Container msg;

		[SerializeField]
		private RectTransform_Image_Container wifi1;

		[SerializeField]
		private RectTransform_Image_Container wifi2;

		[SerializeField]
		private RectTransform_Image_Container wifi3;

		[SerializeField]
		private RectTransform_Container Olcoinbg;

		[SerializeField]
		private RectTransform_Button_Image_Container OlcoinBtn;

		[SerializeField]
		private RectTransform_Button_Image_Container DebugBtn;

		[SerializeField]
		private RectTransform_Container LevelDetailAward;

		[SerializeField]
		private RectTransform_Button_Image_Container LDAbgbtn;

		[SerializeField]
		private RectTransform_Image_Container LDAGunShadow;

		[SerializeField]
		private RectTransform_Image_Container LDAGunDown;

		[SerializeField]
		private RectTransform_Image_Container LDAGunIcon;

		[SerializeField]
		private RectTransform_Image_Container LDASubWpIcon;

		[SerializeField]
		private RectTransform_Image_Container LDAPetIcon;

		[SerializeField]
		private RectTransform_Image_Container LDAGunEffect;

		[SerializeField]
		private RectTransform_Text_Container LDAGunName;

		[SerializeField]
		private RectTransform_Image_Container LDAAwardIcon;

		[SerializeField]
		private RectTransform_Image_Container LDARedPoint;

		[SerializeField]
		private RectTransform_Container NextDay;

		[SerializeField]
		private RectTransform_Button_Image_Container NDbgbtn;

		[SerializeField]
		private RectTransform_Image_Container NDGunShadow;

		[SerializeField]
		private RectTransform_Image_Container NDGunDown;

		[SerializeField]
		private RectTransform_Image_Container NDGunEffect;

		[SerializeField]
		private RectTransform_Image_Container NDGunIcon;

		[SerializeField]
		private RectTransform_Image_Container NDSubWepIcon;

		[SerializeField]
		private RectTransform_Image_Container NDPetIcon;

		[SerializeField]
		private RectTransform_Text_Container NDGunName;

		[SerializeField]
		private RectTransform_Image_Container NDAwardIcon;

		[System.Serializable]
		private class RectTransform_Button_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private Button m_button;
			public Button button { get { return m_button; } }

		}

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
		private class RectTransform_InputField_Image_Container {

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

		[System.Serializable]
		private class Transform_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private Transform m_transform;
			public Transform transform { get { return m_transform; } }

		}

	}

}
