using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CharUI_skill : BaseUi {

		[SerializeField]
		private RectTransform_Image_Container m_skillbg1;
		public RectTransform_Image_Container skillbg1 { get { return m_skillbg1; } }

		[SerializeField]
		private RectTransform_Image_Container m_Newskill1;
		public RectTransform_Image_Container Newskill1 { get { return m_Newskill1; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Skillicon1;
		public RectTransform_Button_Image_Container Skillicon1 { get { return m_Skillicon1; } }

		[SerializeField]
		private RectTransform_Image_Container m_Skilllvbg1;
		public RectTransform_Image_Container Skilllvbg1 { get { return m_Skilllvbg1; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skilllvtxt1;
		public RectTransform_Text_Container Skilllvtxt1 { get { return m_Skilllvtxt1; } }

		[SerializeField]
		private RectTransform_Text_Button_Container m_Closetxt1;
		public RectTransform_Text_Button_Container Closetxt1 { get { return m_Closetxt1; } }

		[SerializeField]
		private RectTransform_Text_Container m_Openlvtxt1;
		public RectTransform_Text_Container Openlvtxt1 { get { return m_Openlvtxt1; } }

		[SerializeField]
		private RectTransform_Container m_SkillUp1;
		public RectTransform_Container SkillUp1 { get { return m_SkillUp1; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewIMG1;
		public RectTransform_Image_Container NewIMG1 { get { return m_NewIMG1; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skillnametxt1;
		public RectTransform_Text_Container Skillnametxt1 { get { return m_Skillnametxt1; } }

		[SerializeField]
		private RectTransform_Image_Container m_skillbg2;
		public RectTransform_Image_Container skillbg2 { get { return m_skillbg2; } }

		[SerializeField]
		private RectTransform_Image_Container m_Newskill2;
		public RectTransform_Image_Container Newskill2 { get { return m_Newskill2; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Skillicon2;
		public RectTransform_Button_Image_Container Skillicon2 { get { return m_Skillicon2; } }

		[SerializeField]
		private RectTransform_Image_Container m_Skilllvbg2;
		public RectTransform_Image_Container Skilllvbg2 { get { return m_Skilllvbg2; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skilllvtxt2;
		public RectTransform_Text_Container Skilllvtxt2 { get { return m_Skilllvtxt2; } }

		[SerializeField]
		private RectTransform_Text_Button_Container m_Closetxt2;
		public RectTransform_Text_Button_Container Closetxt2 { get { return m_Closetxt2; } }

		[SerializeField]
		private RectTransform_Text_Container m_Openlvtxt2;
		public RectTransform_Text_Container Openlvtxt2 { get { return m_Openlvtxt2; } }

		[SerializeField]
		private RectTransform_Container m_SkillUp2;
		public RectTransform_Container SkillUp2 { get { return m_SkillUp2; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewIMG2;
		public RectTransform_Image_Container NewIMG2 { get { return m_NewIMG2; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skillnametxt2;
		public RectTransform_Text_Container Skillnametxt2 { get { return m_Skillnametxt2; } }

		[SerializeField]
		private RectTransform_Image_Container m_skillbg3;
		public RectTransform_Image_Container skillbg3 { get { return m_skillbg3; } }

		[SerializeField]
		private RectTransform_Image_Container m_Newskill3;
		public RectTransform_Image_Container Newskill3 { get { return m_Newskill3; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Skillicon3;
		public RectTransform_Button_Image_Container Skillicon3 { get { return m_Skillicon3; } }

		[SerializeField]
		private RectTransform_Image_Container m_Skilllvbg3;
		public RectTransform_Image_Container Skilllvbg3 { get { return m_Skilllvbg3; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skilllvtxt3;
		public RectTransform_Text_Container Skilllvtxt3 { get { return m_Skilllvtxt3; } }

		[SerializeField]
		private RectTransform_Text_Button_Container m_Closetxt3;
		public RectTransform_Text_Button_Container Closetxt3 { get { return m_Closetxt3; } }

		[SerializeField]
		private RectTransform_Text_Container m_Openlvtxt3;
		public RectTransform_Text_Container Openlvtxt3 { get { return m_Openlvtxt3; } }

		[SerializeField]
		private RectTransform_Container m_SkillUp3;
		public RectTransform_Container SkillUp3 { get { return m_SkillUp3; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewIMG3;
		public RectTransform_Image_Container NewIMG3 { get { return m_NewIMG3; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skillnametxt3;
		public RectTransform_Text_Container Skillnametxt3 { get { return m_Skillnametxt3; } }

		[SerializeField]
		private RectTransform_Image_Container m_skillbg4;
		public RectTransform_Image_Container skillbg4 { get { return m_skillbg4; } }

		[SerializeField]
		private RectTransform_Image_Container m_Newskill4;
		public RectTransform_Image_Container Newskill4 { get { return m_Newskill4; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Skillicon4;
		public RectTransform_Button_Image_Container Skillicon4 { get { return m_Skillicon4; } }

		[SerializeField]
		private RectTransform_Image_Container m_Skilllvbg4;
		public RectTransform_Image_Container Skilllvbg4 { get { return m_Skilllvbg4; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skilllvtxt4;
		public RectTransform_Text_Container Skilllvtxt4 { get { return m_Skilllvtxt4; } }

		[SerializeField]
		private RectTransform_Text_Button_Container m_Closetxt4;
		public RectTransform_Text_Button_Container Closetxt4 { get { return m_Closetxt4; } }

		[SerializeField]
		private RectTransform_Text_Container m_Openlvtxt4;
		public RectTransform_Text_Container Openlvtxt4 { get { return m_Openlvtxt4; } }

		[SerializeField]
		private RectTransform_Container m_SkillUp4;
		public RectTransform_Container SkillUp4 { get { return m_SkillUp4; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewIMG4;
		public RectTransform_Image_Container NewIMG4 { get { return m_NewIMG4; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skillnametxt4;
		public RectTransform_Text_Container Skillnametxt4 { get { return m_Skillnametxt4; } }

		[SerializeField]
		private RectTransform_Image_Container m_skillbg5;
		public RectTransform_Image_Container skillbg5 { get { return m_skillbg5; } }

		[SerializeField]
		private RectTransform_Image_Container m_Newskill5;
		public RectTransform_Image_Container Newskill5 { get { return m_Newskill5; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Skillicon5;
		public RectTransform_Button_Image_Container Skillicon5 { get { return m_Skillicon5; } }

		[SerializeField]
		private RectTransform_Image_Container m_Skilllvbg5;
		public RectTransform_Image_Container Skilllvbg5 { get { return m_Skilllvbg5; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skilllvtxt5;
		public RectTransform_Text_Container Skilllvtxt5 { get { return m_Skilllvtxt5; } }

		[SerializeField]
		private RectTransform_Text_Button_Container m_Closetxt5;
		public RectTransform_Text_Button_Container Closetxt5 { get { return m_Closetxt5; } }

		[SerializeField]
		private RectTransform_Text_Container m_Openlvtxt5;
		public RectTransform_Text_Container Openlvtxt5 { get { return m_Openlvtxt5; } }

		[SerializeField]
		private RectTransform_Container m_SkillUp5;
		public RectTransform_Container SkillUp5 { get { return m_SkillUp5; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewIMG5;
		public RectTransform_Image_Container NewIMG5 { get { return m_NewIMG5; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skillnametxt5;
		public RectTransform_Text_Container Skillnametxt5 { get { return m_Skillnametxt5; } }

		[SerializeField]
		private RectTransform_Image_Container m_skillbg6;
		public RectTransform_Image_Container skillbg6 { get { return m_skillbg6; } }

		[SerializeField]
		private RectTransform_Image_Container m_Newskill6;
		public RectTransform_Image_Container Newskill6 { get { return m_Newskill6; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Skillicon6;
		public RectTransform_Button_Image_Container Skillicon6 { get { return m_Skillicon6; } }

		[SerializeField]
		private RectTransform_Image_Container m_Skilllvbg6;
		public RectTransform_Image_Container Skilllvbg6 { get { return m_Skilllvbg6; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skilllvtxt6;
		public RectTransform_Text_Container Skilllvtxt6 { get { return m_Skilllvtxt6; } }

		[SerializeField]
		private RectTransform_Text_Button_Container m_Closetxt6;
		public RectTransform_Text_Button_Container Closetxt6 { get { return m_Closetxt6; } }

		[SerializeField]
		private RectTransform_Text_Container m_Openlvtxt6;
		public RectTransform_Text_Container Openlvtxt6 { get { return m_Openlvtxt6; } }

		[SerializeField]
		private RectTransform_Container m_SkillUp6;
		public RectTransform_Container SkillUp6 { get { return m_SkillUp6; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewIMG6;
		public RectTransform_Image_Container NewIMG6 { get { return m_NewIMG6; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skillnametxt6;
		public RectTransform_Text_Container Skillnametxt6 { get { return m_Skillnametxt6; } }

		[SerializeField]
		private RectTransform_Image_Container m_skillbg7;
		public RectTransform_Image_Container skillbg7 { get { return m_skillbg7; } }

		[SerializeField]
		private RectTransform_Image_Container m_Newskill7;
		public RectTransform_Image_Container Newskill7 { get { return m_Newskill7; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Skillicon7;
		public RectTransform_Button_Image_Container Skillicon7 { get { return m_Skillicon7; } }

		[SerializeField]
		private RectTransform_Image_Container m_Skilllvbg7;
		public RectTransform_Image_Container Skilllvbg7 { get { return m_Skilllvbg7; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skilllvtxt7;
		public RectTransform_Text_Container Skilllvtxt7 { get { return m_Skilllvtxt7; } }

		[SerializeField]
		private RectTransform_Text_Button_Container m_Closetxt7;
		public RectTransform_Text_Button_Container Closetxt7 { get { return m_Closetxt7; } }

		[SerializeField]
		private RectTransform_Text_Container m_Openlvtxt7;
		public RectTransform_Text_Container Openlvtxt7 { get { return m_Openlvtxt7; } }

		[SerializeField]
		private RectTransform_Container m_SkillUp7;
		public RectTransform_Container SkillUp7 { get { return m_SkillUp7; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewIMG7;
		public RectTransform_Image_Container NewIMG7 { get { return m_NewIMG7; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skillnametxt7;
		public RectTransform_Text_Container Skillnametxt7 { get { return m_Skillnametxt7; } }

		[SerializeField]
		private RectTransform_Image_Container m_skillbg8;
		public RectTransform_Image_Container skillbg8 { get { return m_skillbg8; } }

		[SerializeField]
		private RectTransform_Image_Container m_Newskill8;
		public RectTransform_Image_Container Newskill8 { get { return m_Newskill8; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Skillicon8;
		public RectTransform_Button_Image_Container Skillicon8 { get { return m_Skillicon8; } }

		[SerializeField]
		private RectTransform_Image_Container m_Skilllvbg8;
		public RectTransform_Image_Container Skilllvbg8 { get { return m_Skilllvbg8; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skilllvtxt8;
		public RectTransform_Text_Container Skilllvtxt8 { get { return m_Skilllvtxt8; } }

		[SerializeField]
		private RectTransform_Text_Button_Container m_Closetxt8;
		public RectTransform_Text_Button_Container Closetxt8 { get { return m_Closetxt8; } }

		[SerializeField]
		private RectTransform_Text_Container m_Openlvtxt8;
		public RectTransform_Text_Container Openlvtxt8 { get { return m_Openlvtxt8; } }

		[SerializeField]
		private RectTransform_Container m_SkillUp8;
		public RectTransform_Container SkillUp8 { get { return m_SkillUp8; } }

		[SerializeField]
		private RectTransform_Image_Container m_NewIMG8;
		public RectTransform_Image_Container NewIMG8 { get { return m_NewIMG8; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skillnametxt8;
		public RectTransform_Text_Container Skillnametxt8 { get { return m_Skillnametxt8; } }

		[SerializeField]
		private RectTransform_Image_Container m_MainRoleAdapter;
		public RectTransform_Image_Container MainRoleAdapter { get { return m_MainRoleAdapter; } }

		[SerializeField]
		private RectTransform_Image_Container m_DesBg;
		public RectTransform_Image_Container DesBg { get { return m_DesBg; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Skill_S_Icon;
		public RectTransform_Button_Image_Container Skill_S_Icon { get { return m_Skill_S_Icon; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skill_S_Name;
		public RectTransform_Text_Container Skill_S_Name { get { return m_Skill_S_Name; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skill_S_Level;
		public RectTransform_Text_Container Skill_S_Level { get { return m_Skill_S_Level; } }

		[SerializeField]
		private RectTransform_Container m_SkillUp;
		public RectTransform_Container SkillUp { get { return m_SkillUp; } }

		[SerializeField]
		private RectTransform_Text_Container m_Skill_S_Des;
		public RectTransform_Text_Container Skill_S_Des { get { return m_Skill_S_Des; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_UpBtn;
		public RectTransform_Button_Image_Container UpBtn { get { return m_UpBtn; } }

		[SerializeField]
		private RectTransform_Image_Container m_CmIcon;
		public RectTransform_Image_Container CmIcon { get { return m_CmIcon; } }

		[SerializeField]
		private RectTransform_Text_Container m_CmNum;
		public RectTransform_Text_Container CmNum { get { return m_CmNum; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Tableft;
		public RectTransform_Button_Image_Container Tableft { get { return m_Tableft; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Tabright;
		public RectTransform_Button_Image_Container Tabright { get { return m_Tabright; } }

		[SerializeField]
		private RectTransform_Image_Container m_Tableft_on;
		public RectTransform_Image_Container Tableft_on { get { return m_Tableft_on; } }

		[SerializeField]
		private RectTransform_Image_Container m_Tabright_on;
		public RectTransform_Image_Container Tabright_on { get { return m_Tabright_on; } }

		[SerializeField]
		private RectTransform_Text_Container m_Tableftname;
		public RectTransform_Text_Container Tableftname { get { return m_Tableftname; } }

		[SerializeField]
		private RectTransform_Text_Container m_Tabrightname;
		public RectTransform_Text_Container Tabrightname { get { return m_Tabrightname; } }

		[SerializeField]
		private RectTransform_Image_Container m_Lockimg;
		public RectTransform_Image_Container Lockimg { get { return m_Lockimg; } }

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
		public class RectTransform_Text_Button_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private Text m_text;
			public Text text { get { return m_text; } }

			[SerializeField]
			private Button m_button;
			public Button button { get { return m_button; } }

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
