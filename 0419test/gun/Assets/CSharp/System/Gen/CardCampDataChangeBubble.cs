using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CardCampDataChangeBubble : MonoBehaviour {

		[SerializeField]
		private RectTransform_Text_Container m_RewardFactorBefore;
		public RectTransform_Text_Container RewardFactorBefore { get { return m_RewardFactorBefore; } }

		[SerializeField]
		private RectTransform_Text_Container m_RewardFactorCur;
		public RectTransform_Text_Container RewardFactorCur { get { return m_RewardFactorCur; } }

		[SerializeField]
		private RectTransform_Container m_AutoDataChange;
		public RectTransform_Container AutoDataChange { get { return m_AutoDataChange; } }

		[SerializeField]
		private RectTransform_Text_Container m_AutoValBefore;
		public RectTransform_Text_Container AutoValBefore { get { return m_AutoValBefore; } }

		[SerializeField]
		private RectTransform_Text_Container m_AutoValCur;
		public RectTransform_Text_Container AutoValCur { get { return m_AutoValCur; } }

		[SerializeField]
		private RectTransform_Container m_AutoData;
		public RectTransform_Container AutoData { get { return m_AutoData; } }

		[SerializeField]
		private RectTransform_Text_Container m_AutoVal;
		public RectTransform_Text_Container AutoVal { get { return m_AutoVal; } }

		[SerializeField]
		private RectTransform_Text_Container m_AutoLvTip;
		public RectTransform_Text_Container AutoLvTip { get { return m_AutoLvTip; } }

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
