using System.Collections.Generic;
using UnityEngine;

namespace EZ {

	public partial class FightPlotUI : BaseUi {

		[SerializeField]
		private RectTransform_FightPlotUI_FightPlotUIItem_Container FightPlotUIItem;

		[System.Serializable]
		private class RectTransform_FightPlotUI_FightPlotUIItem_Container {

			[SerializeField]
			private GameObject m_GameObject;
			public GameObject gameObject { get { return m_GameObject; } }

			[SerializeField]
			private RectTransform m_rectTransform;
			public RectTransform rectTransform { get { return m_rectTransform; } }

			[SerializeField]
			private FightPlotUI_FightPlotUIItem m_FightPlotUIItem;
			public FightPlotUI_FightPlotUIItem FightPlotUIItem { get { return m_FightPlotUIItem; } }

			private Queue<FightPlotUI_FightPlotUIItem> mCachedInstances;
			public FightPlotUI_FightPlotUIItem GetInstance() {
				FightPlotUI_FightPlotUIItem instance = null;
				if (mCachedInstances != null) {
					while ((instance == null || instance.Equals(null)) && mCachedInstances.Count > 0) {
						instance = mCachedInstances.Dequeue();
					}
				}
				if (instance == null || instance.Equals(null)) {
					instance = Instantiate<FightPlotUI_FightPlotUIItem>(m_FightPlotUIItem);
				}
				Transform t0 = m_FightPlotUIItem.transform;
				Transform t1 = instance.transform;
				t1.SetParent(t0.parent);
				t1.localPosition = t0.localPosition;
				t1.localRotation = t0.localRotation;
				t1.localScale = t0.localScale;
				t1.SetSiblingIndex(t0.GetSiblingIndex() + 1);
				return instance;
			}
			public bool CacheInstance(FightPlotUI_FightPlotUIItem instance) {
				if (instance == null || instance.Equals(null)) { return false; }
				if (mCachedInstances == null) { mCachedInstances = new Queue<FightPlotUI_FightPlotUIItem>(); }
				if (mCachedInstances.Contains(instance)) { return false; }
				instance.gameObject.SetActive(false);
				mCachedInstances.Enqueue(instance);
				return true;
			}

		}

	}

}
