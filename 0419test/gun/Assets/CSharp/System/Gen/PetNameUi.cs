using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class PetNameUi : BaseAttrUi {

		[SerializeField]
		private RectTransform_Container m_FollowNode;
		public RectTransform_Container FollowNode { get { return m_FollowNode; } }

		[SerializeField]
		private RectTransform_Text_Container m_NameText;
		public RectTransform_Text_Container NameText { get { return m_NameText; } }

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
