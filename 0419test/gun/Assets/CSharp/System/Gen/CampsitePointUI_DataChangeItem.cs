using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class CampsitePointUI_DataChangeItem : MonoBehaviour {

		[SerializeField]
		private RectTransform_Text_Container m_LastValTxt;
		public RectTransform_Text_Container LastValTxt { get { return m_LastValTxt; } }

		[SerializeField]
		private RectTransform_Image_Container m_UpFlag;
		public RectTransform_Image_Container UpFlag { get { return m_UpFlag; } }

		[SerializeField]
		private RectTransform_Image_Container m_DownFlag;
		public RectTransform_Image_Container DownFlag { get { return m_DownFlag; } }

		[SerializeField]
		private RectTransform_Text_Container m_CurValTxt;
		public RectTransform_Text_Container CurValTxt { get { return m_CurValTxt; } }

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
