using UnityEngine;

namespace EZ {

	public partial class NGhandUI : MonoBehaviour {

		[SerializeField]
		private RectTransform_Container AdaptNode;

		[SerializeField]
		private RectTransform_Container BtnNode;

		[System.Serializable]
		private class RectTransform_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

		}

	}

}
