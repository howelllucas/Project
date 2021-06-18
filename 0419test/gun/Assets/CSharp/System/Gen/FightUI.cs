using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class FightUI : BaseUi {

		[SerializeField]
		private RectTransform_Container m_DynamicNode;
		public RectTransform_Container DynamicNode { get { return m_DynamicNode; } }

		[SerializeField]
		private RectTransform_Container m_BossHpNode;
		public RectTransform_Container BossHpNode { get { return m_BossHpNode; } }

		[SerializeField]
		private RectTransform_Container m_NpcProgress;
		public RectTransform_Container NpcProgress { get { return m_NpcProgress; } }

		[SerializeField]
		private RectTransform_FightUI_Itemtime_Container m_Itemtime;
		public RectTransform_FightUI_Itemtime_Container Itemtime { get { return m_Itemtime; } }

		[SerializeField]
		private RectTransform_Container m_RightTop;
		public RectTransform_Container RightTop { get { return m_RightTop; } }

		[SerializeField]
		private RectTransform_Container m_LeftTop;
		public RectTransform_Container LeftTop { get { return m_LeftTop; } }

		[SerializeField]
		private RectTransform_Container m_LeftBottom;
		public RectTransform_Container LeftBottom { get { return m_LeftBottom; } }

		[SerializeField]
		private RectTransform_Container m_RightBottom;
		public RectTransform_Container RightBottom { get { return m_RightBottom; } }

		[SerializeField]
		private RectTransform_Container m_GunPowerProgress;
		public RectTransform_Container GunPowerProgress { get { return m_GunPowerProgress; } }

		[SerializeField]
		private RectTransform_Image_Container m_PowerBg;
		public RectTransform_Image_Container PowerBg { get { return m_PowerBg; } }

		[SerializeField]
		private RectTransform_Image_Container m_Itemicon;
		public RectTransform_Image_Container Itemicon { get { return m_Itemicon; } }

		[SerializeField]
		private RectTransform_Image_Container m_progress_bg;
		public RectTransform_Image_Container progress_bg { get { return m_progress_bg; } }

		[SerializeField]
		private RectTransform_Image_Container m_content;
		public RectTransform_Image_Container content { get { return m_content; } }

		[SerializeField]
		private RectTransform_Container m_ArrorNode;
		public RectTransform_Container ArrorNode { get { return m_ArrorNode; } }

		[SerializeField]
		private RectTransform_Image_Container m_ArrowBg;
		public RectTransform_Image_Container ArrowBg { get { return m_ArrowBg; } }

		[SerializeField]
		private RectTransform_Container m_RotateNode;
		public RectTransform_Container RotateNode { get { return m_RotateNode; } }

		[SerializeField]
		private RectTransform_Text_Container m_Distance;
		public RectTransform_Text_Container Distance { get { return m_Distance; } }

		[SerializeField]
		private RectTransform_Image_Container m_ArrowPointBg;
		public RectTransform_Image_Container ArrowPointBg { get { return m_ArrowPointBg; } }

		[SerializeField]
		private RectTransform_Image_Container m_battery;
		public RectTransform_Image_Container battery { get { return m_battery; } }

		[SerializeField]
		private RectTransform_Image_Container m_capture;
		public RectTransform_Image_Container capture { get { return m_capture; } }

		[SerializeField]
		private RectTransform_Image_Container m_monster;
		public RectTransform_Image_Container monster { get { return m_monster; } }

		[SerializeField]
		private RectTransform_Image_Container m_property;
		public RectTransform_Image_Container property { get { return m_property; } }

		[SerializeField]
		private RectTransform_Image_Container m_copter;
		public RectTransform_Image_Container copter { get { return m_copter; } }

		[SerializeField]
		private RectTransform_Container m_RotateNodePoint;
		public RectTransform_Container RotateNodePoint { get { return m_RotateNodePoint; } }

		[SerializeField]
		private RectTransform_Text_Container m_DistancePoint;
		public RectTransform_Text_Container DistancePoint { get { return m_DistancePoint; } }

		[SerializeField]
		private RectTransform_Text_Container m_cointxt;
		public RectTransform_Text_Container cointxt { get { return m_cointxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_countdownname;
		public RectTransform_Text_Container countdownname { get { return m_countdownname; } }

		[SerializeField]
		private RectTransform_Text_Container m_countdowntime;
		public RectTransform_Text_Container countdowntime { get { return m_countdowntime; } }

		[SerializeField]
		private RectTransform_Text_Container m_wavetxt;
		public RectTransform_Text_Container wavetxt { get { return m_wavetxt; } }

		[SerializeField]
		private RectTransform_Text_Container m_ChipCount;
		public RectTransform_Text_Container ChipCount { get { return m_ChipCount; } }

		[SerializeField]
		private RectTransform_Image_Container m_DataNode;
		public RectTransform_Image_Container DataNode { get { return m_DataNode; } }

		[SerializeField]
		private RectTransform_Text_Container m_Atk;
		public RectTransform_Text_Container Atk { get { return m_Atk; } }

		[SerializeField]
		private RectTransform_Text_Container m_AtkTime;
		public RectTransform_Text_Container AtkTime { get { return m_AtkTime; } }

		[SerializeField]
		private RectTransform_Text_Container m_PassParam;
		public RectTransform_Text_Container PassParam { get { return m_PassParam; } }

		[SerializeField]
		private RectTransform_Container m_PetNameNode;
		public RectTransform_Container PetNameNode { get { return m_PetNameNode; } }

		[SerializeField]
		private RectTransform_Container m_StaticNode;
		public RectTransform_Container StaticNode { get { return m_StaticNode; } }

		[SerializeField]
		private RectTransform_Image_Container m_ChipIcon;
		public RectTransform_Image_Container ChipIcon { get { return m_ChipIcon; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_pause;
		public RectTransform_Button_Image_Container pause { get { return m_pause; } }

		[SerializeField]
		private RectTransform_FightUI_HeartItem_Container m_HeartItem;
		public RectTransform_FightUI_HeartItem_Container HeartItem { get { return m_HeartItem; } }

		[SerializeField]
		private RectTransform_Container m_TaskIconNode;
		public RectTransform_Container TaskIconNode { get { return m_TaskIconNode; } }

		[SerializeField]
		private RectTransform_Image_Container m_battery1;
		public RectTransform_Image_Container battery1 { get { return m_battery1; } }

		[SerializeField]
		private RectTransform_Image_Container m_capture1;
		public RectTransform_Image_Container capture1 { get { return m_capture1; } }

		[SerializeField]
		private RectTransform_Image_Container m_monster1;
		public RectTransform_Image_Container monster1 { get { return m_monster1; } }

		[SerializeField]
		private RectTransform_Image_Container m_property1;
		public RectTransform_Image_Container property1 { get { return m_property1; } }

		[SerializeField]
		private RectTransform_Image_Container m_copter1;
		public RectTransform_Image_Container copter1 { get { return m_copter1; } }

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
		public class RectTransform_FightUI_HeartItem_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private FightUI_HeartItem m_HeartItem;
			public FightUI_HeartItem HeartItem { get { return m_HeartItem; } }

			private Queue<FightUI_HeartItem> mCachedInstances;
			public FightUI_HeartItem GetInstance() {
				FightUI_HeartItem instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<FightUI_HeartItem>(m_HeartItem);
				}
				Transform t0 = m_HeartItem.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(FightUI_HeartItem instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<FightUI_HeartItem>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_FightUI_Itemtime_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private FightUI_Itemtime m_Itemtime;
			public FightUI_Itemtime Itemtime { get { return m_Itemtime; } }

			private Queue<FightUI_Itemtime> mCachedInstances;
			public FightUI_Itemtime GetInstance() {
				FightUI_Itemtime instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<FightUI_Itemtime>(m_Itemtime);
				}
				Transform t0 = m_Itemtime.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(FightUI_Itemtime instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<FightUI_Itemtime>(); }
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
