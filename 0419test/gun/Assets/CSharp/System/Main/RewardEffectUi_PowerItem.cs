using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class RewardEffectUi_PowerItem : MonoBehaviour {

		[SerializeField]
		private RectTransform_Text_Container m_GradeText;
		public RectTransform_Text_Container GradeText { get { return m_GradeText; } }

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
