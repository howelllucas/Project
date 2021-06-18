using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public class FightPlotUI_FightPlotUIItem : MonoBehaviour {

		[SerializeField]
		private RectTransform_Text_Container m_Plottxt;
		public RectTransform_Text_Container Plottxt { get { return m_Plottxt; } }

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
