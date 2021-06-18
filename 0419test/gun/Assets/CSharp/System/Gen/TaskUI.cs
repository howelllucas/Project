using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class TaskUI : BaseUi {

		[SerializeField]
		private RectTransform_Image_Container m_Root;
		public RectTransform_Image_Container Root { get { return m_Root; } }

		[SerializeField]
		private RectTransform_Image_Container m_ProgressRoot;
		public RectTransform_Image_Container ProgressRoot { get { return m_ProgressRoot; } }

		[SerializeField]
		private RectTransform_Image_Container m_TaskProgress;
		public RectTransform_Image_Container TaskProgress { get { return m_TaskProgress; } }

		[SerializeField]
		private RectTransform_Text_Container m_TaskCount;
		public RectTransform_Text_Container TaskCount { get { return m_TaskCount; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CampMapBtn;
		public RectTransform_Button_Image_Container CampMapBtn { get { return m_CampMapBtn; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_Playerbg;
		public RectTransform_Button_Image_Container Playerbg { get { return m_Playerbg; } }

		[SerializeField]
		private RectTransform_Text_Container m_Leveltxt;
		public RectTransform_Text_Container Leveltxt { get { return m_Leveltxt; } }

		[SerializeField]
		private RectTransform_Container m_TaskRoot;
		public RectTransform_Container TaskRoot { get { return m_TaskRoot; } }

		[SerializeField]
		private RectTransform_TaskUI_TaskItemUi_Container m_TaskItemUi;
		public RectTransform_TaskUI_TaskItemUi_Container TaskItemUi { get { return m_TaskItemUi; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_NextCampBtn;
		public RectTransform_Button_Image_Container NextCampBtn { get { return m_NextCampBtn; } }

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
		public class RectTransform_TaskUI_TaskItemUi_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private TaskUI_TaskItemUi m_TaskItemUi;
			public TaskUI_TaskItemUi TaskItemUi { get { return m_TaskItemUi; } }

			private Queue<TaskUI_TaskItemUi> mCachedInstances;
			public TaskUI_TaskItemUi GetInstance() {
				TaskUI_TaskItemUi instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<TaskUI_TaskItemUi>(m_TaskItemUi);
				}
				Transform t0 = m_TaskItemUi.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(TaskUI_TaskItemUi instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<TaskUI_TaskItemUi>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

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
