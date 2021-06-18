using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EZ {

	public partial class LevelDetail : BaseUi {

		[SerializeField]
		private RectTransform_Image_Container m_bg;
		public RectTransform_Image_Container bg { get { return m_bg; } }

		[SerializeField]
		private RectTransform_Container m_ScrollView;
		public RectTransform_Container ScrollView { get { return m_ScrollView; } }

		[SerializeField]
		private RectTransform_Image_Container m_Viewport;
		public RectTransform_Image_Container Viewport { get { return m_Viewport; } }

		[SerializeField]
		private RectTransform_Container m_Content;
		public RectTransform_Container Content { get { return m_Content; } }

		[SerializeField]
		private RectTransform_LevelDetail_progress_Container m_progress;
		public RectTransform_LevelDetail_progress_Container progress { get { return m_progress; } }

		[SerializeField]
		private RectTransform_LevelDetail_step_Container m_step;
		public RectTransform_LevelDetail_step_Container step { get { return m_step; } }

		[SerializeField]
		private RectTransform_LevelDetail_gun_Container m_gun;
		public RectTransform_LevelDetail_gun_Container gun { get { return m_gun; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_topTip;
		public RectTransform_Button_Image_Container topTip { get { return m_topTip; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_bottomTip;
		public RectTransform_Button_Image_Container bottomTip { get { return m_bottomTip; } }

		[SerializeField]
		private RectTransform_Button_Image_Container m_CloseBtn;
		public RectTransform_Button_Image_Container CloseBtn { get { return m_CloseBtn; } }

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
		public class RectTransform_LevelDetail_gun_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private LevelDetail_gun m_gun;
			public LevelDetail_gun gun { get { return m_gun; } }

			private Queue<LevelDetail_gun> mCachedInstances;
			public LevelDetail_gun GetInstance() {
				LevelDetail_gun instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<LevelDetail_gun>(m_gun);
				}
				Transform t0 = m_gun.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(LevelDetail_gun instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<LevelDetail_gun>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_LevelDetail_progress_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private LevelDetail_progress m_progress;
			public LevelDetail_progress progress { get { return m_progress; } }

			private Queue<LevelDetail_progress> mCachedInstances;
			public LevelDetail_progress GetInstance() {
				LevelDetail_progress instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<LevelDetail_progress>(m_progress);
				}
				Transform t0 = m_progress.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(LevelDetail_progress instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<LevelDetail_progress>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		public class RectTransform_LevelDetail_step_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private LevelDetail_step m_step;
			public LevelDetail_step step { get { return m_step; } }

			private Queue<LevelDetail_step> mCachedInstances;
			public LevelDetail_step GetInstance() {
				LevelDetail_step instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<LevelDetail_step>(m_step);
				}
				Transform t0 = m_step.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(LevelDetail_step instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<LevelDetail_step>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

	}

}
