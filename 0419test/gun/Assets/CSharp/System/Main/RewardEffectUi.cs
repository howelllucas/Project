using System.Collections.Generic;
using UnityEngine;

namespace EZ {

	public partial class RewardEffectUi : BaseUi {

		[SerializeField]
		private RectTransform_RewardEffectUi_RewardItem_Container RewardItem;

		[SerializeField]
		private RectTransform_RewardEffectUi_PowerItem_Container PowerItem;

		[SerializeField]
		private RectTransform_RewardEffectUi_RewardKey_Container RewardKey;

		[System.Serializable]
		private class RectTransform_RewardEffectUi_PowerItem_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private RewardEffectUi_PowerItem m_PowerItem;
			public RewardEffectUi_PowerItem PowerItem { get { return m_PowerItem; } }

			private Queue<RewardEffectUi_PowerItem> mCachedInstances;
			public RewardEffectUi_PowerItem GetInstance() {
				RewardEffectUi_PowerItem instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<RewardEffectUi_PowerItem>(m_PowerItem);
				}
				Transform t0 = m_PowerItem.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(RewardEffectUi_PowerItem instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<RewardEffectUi_PowerItem>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		private class RectTransform_RewardEffectUi_RewardItem_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private RewardEffectUi_RewardItem m_RewardItem;
			public RewardEffectUi_RewardItem RewardItem { get { return m_RewardItem; } }

			private Queue<RewardEffectUi_RewardItem> mCachedInstances;
			public RewardEffectUi_RewardItem GetInstance() {
				RewardEffectUi_RewardItem instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<RewardEffectUi_RewardItem>(m_RewardItem);
				}
				Transform t0 = m_RewardItem.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(RewardEffectUi_RewardItem instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<RewardEffectUi_RewardItem>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

		[System.Serializable]
		private class RectTransform_RewardEffectUi_RewardKey_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private RewardEffectUi_RewardKey m_RewardKey;
			public RewardEffectUi_RewardKey RewardKey { get { return m_RewardKey; } }

			private Queue<RewardEffectUi_RewardKey> mCachedInstances;
			public RewardEffectUi_RewardKey GetInstance() {
				RewardEffectUi_RewardKey instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<RewardEffectUi_RewardKey>(m_RewardKey);
				}
				Transform t0 = m_RewardKey.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(RewardEffectUi_RewardKey instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<RewardEffectUi_RewardKey>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

	}

}
