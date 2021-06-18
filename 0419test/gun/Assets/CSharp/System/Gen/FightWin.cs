using System.Collections.Generic;
using UnityEngine;

namespace EZ {

	public partial class FightWin : BaseUi {

		[SerializeField]
		private RectTransform_Container m_ScrollNode;
		public RectTransform_Container ScrollNode { get { return m_ScrollNode; } }

		[SerializeField]
		private RectTransform_FightWin_ResIcon_Container m_ResIcon;
		public RectTransform_FightWin_ResIcon_Container ResIcon { get { return m_ResIcon; } }

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
		public class RectTransform_FightWin_ResIcon_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private FightWin_ResIcon m_ResIcon;
			public FightWin_ResIcon ResIcon { get { return m_ResIcon; } }

			private Queue<FightWin_ResIcon> mCachedInstances;
			public FightWin_ResIcon GetInstance() {
				FightWin_ResIcon instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<FightWin_ResIcon>(m_ResIcon);
				}
				Transform t0 = m_ResIcon.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(FightWin_ResIcon instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<FightWin_ResIcon>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

	}

}
