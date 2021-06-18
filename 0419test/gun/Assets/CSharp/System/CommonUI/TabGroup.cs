using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour {

	public enum eTabItemEffectType {
		AnchoredPosition, Scale,
		ActiveOnStart, ActiveOnEnd, EnableOnStart, EnableOnEnd,
		ColorOnGraphic, ColorOnCanvasRenderer
	}

	public interface IEffectTargetContainer {
		eTabItemEffectType itemEffectType { get; }
		GameObject gameObject { get; }
		RectTransform rectTransform { get; }
		Graphic graphic { get; }
		MonoBehaviour component { get; }
	}

	[System.Serializable]
	public class TabItem {
		public Button button;
		public TabItemEffect[] itemEffects;
	}

	[System.Serializable]
	public class TabItemEffect : IEffectTargetContainer {
		[SerializeField]
		private eTabItemEffectType m_ItemEffectType;
		[SerializeField]
		private GameObject m_GameObject;
		[SerializeField]
		private RectTransform m_RectTransform;
		[SerializeField]
		private Graphic m_Graphic;
		[SerializeField]
		private MonoBehaviour m_Component;
		public Vector4 valueLeft;
		public Vector4 valueHighlight;
		public Vector4 valueRight;
		public eTabItemEffectType itemEffectType { get { return m_ItemEffectType; } }
		public GameObject gameObject { get { return m_GameObject; } }
		public RectTransform rectTransform { get { return m_RectTransform; } }
		public Graphic graphic { get { return m_Graphic; } }
		public MonoBehaviour component { get { return m_Component; } }
	}

    public class TabGroupEvent : UnityEvent<int> { }
    [SerializeField]
    private TabGroupEvent m_OnIndexChanged = new TabGroupEvent();
    public TabGroupEvent OnIndexChanged { get { return m_OnIndexChanged; } }

    [SerializeField]
	private TabItem[] m_TabItems;
	[SerializeField]
	private float m_SwitchDuration = 0.3f;

	private bool mStarted = false;
	private int mSelectedIndex = -1;

	private List<TweenItem> mTweenings = new List<TweenItem>();

	public void SetIndex(int index) {
		if (index < 0 || index >= m_TabItems.Length) { return; }
		if (mStarted) { FlushSelected(mSelectedIndex, index); }
		mSelectedIndex = index;
	}

	void Start() {
		for (int i = 0, imax = m_TabItems.Length; i < imax; i++) {
			int index = i;
			TabItem tab = m_TabItems[i];
			if (tab == null || tab.button == null) { continue; }
			tab.button.onClick.AddListener(() => { OnTabClick(index); });
		}
		mStarted = true;
		FlushSelected(-1, mSelectedIndex);
	}

	void Update() {
		float deltaTime = Time.unscaledDeltaTime;
		for (int i = mTweenings.Count - 1; i >= 0; i--) {
			TweenItem tween = mTweenings[i];
			tween.timer += deltaTime;
			float t = Mathf.Clamp01(tween.timer * tween.duration);
			bool done = t >= 1f;
			float f = Mathf.Sin(t * 0.5f * Mathf.PI);
			ApplyEffectValue(tween, Vector4.LerpUnclamped(tween.valueFrom, tween.valueTo, f), t);
			if (done) {
				mTweenings.RemoveAt(i);
				TweenItem.Cache(tween);
			}
		}
	}

	private void FlushSelected(int prev, int index) {
		int len = m_TabItems.Length;
		if (index < 0 || index >= len) { return; }
		for (int i = 0; i < len; i++) {
			TabItem tab = m_TabItems[i];
			if (tab == null || tab.itemEffects == null) { continue; }
			int s1 = i == index ? 0 : (i < index ? -1 : 1);
			bool anim = true;
			bool flush = true;
			if (prev < 0) {
				anim = false;
			} else {
				int s0 = i == prev ? 0 : (i < prev ? -1 : 1);
				flush = s0 != s1;
			}
			if (!flush) { continue; }
			foreach (TabItemEffect effect in tab.itemEffects) {
				Vector4 value = s1 == 0 ? effect.valueHighlight : (s1 < 0 ? effect.valueLeft : effect.valueRight); ;
				bool noAnim = false;
				switch (effect.itemEffectType) {
					case eTabItemEffectType.ActiveOnStart:
						if (effect.gameObject != null) {
							effect.gameObject.SetActive(value.x > 0.5f);
						}
						noAnim = true;
						break;
					case eTabItemEffectType.EnableOnStart:
						if (effect.component != null) {
							effect.component.enabled = value.x > 0.5f;
						}
						noAnim = true;
						break;
				}
				if (noAnim) { continue; }
				if (!anim) {
					ApplyEffectValue(effect, value, 1f);
					continue;
				}
				TweenItem tween = TweenItem.Get();
				tween.itemEffectType = effect.itemEffectType;
				tween.timer = 0f;
				tween.duration = 1f / m_SwitchDuration;
				tween.valueTo = value;
				switch (effect.itemEffectType) {
					case eTabItemEffectType.AnchoredPosition:
						tween.rectTransform = effect.rectTransform;
						if (tween.rectTransform != null) {
							Vector2 pos = tween.rectTransform.anchoredPosition;
							tween.valueFrom = new Vector4(pos.x, pos.y, 0f, 0f);
						}
						break;
					case eTabItemEffectType.Scale:
						tween.rectTransform = effect.rectTransform;
						if (tween.rectTransform != null) {
							Vector3 scale = tween.rectTransform.localScale;
							tween.valueFrom = new Vector4(scale.x, scale.y, scale.z, 0f);
						}
						break;
					case eTabItemEffectType.ActiveOnEnd:
						tween.gameObject = effect.gameObject;
						break;
					case eTabItemEffectType.EnableOnEnd:
						tween.component = effect.component;
						break;
					case eTabItemEffectType.ColorOnGraphic:
						tween.graphic = effect.graphic;
						if (tween.graphic != null) {
							Color color = tween.graphic.color;
							tween.valueFrom = new Vector4(color.r, color.g, color.b, color.a);
						}
						break;
					case eTabItemEffectType.ColorOnCanvasRenderer:
						tween.graphic = effect.graphic;
						if (tween.graphic != null) {
							Color color = tween.graphic.canvasRenderer.GetColor();
							tween.valueFrom = new Vector4(color.r, color.g, color.b, color.a);
						}
						break;
				}
				for (int j = mTweenings.Count - 1; j >= 0; j--) {
					TweenItem ti = mTweenings[j];
					if (ti.itemEffectType != tween.itemEffectType) { continue; }
					if (tween.rectTransform != null && tween.rectTransform != ti.rectTransform) { continue; }
					if (tween.gameObject != null && tween.gameObject != ti.gameObject) { continue; }
					if (tween.component != null && tween.component != ti.component) { continue; }
					if (tween.graphic != null && tween.graphic != ti.graphic) { continue; }
					mTweenings.RemoveAt(j);
				}
				mTweenings.Add(tween);
			}
		}
	}

	private void ApplyEffectValue(IEffectTargetContainer target, Vector4 value, float t) {
		switch (target.itemEffectType) {
			case eTabItemEffectType.AnchoredPosition:
				if (target.rectTransform != null) {
					target.rectTransform.anchoredPosition = new Vector2(value.x, value.y);
				}
				break;
			case eTabItemEffectType.Scale:
				if (target.rectTransform != null) {
					target.rectTransform.localScale = new Vector3(value.x, value.y, value.z);
				}
				break;
			case eTabItemEffectType.ActiveOnEnd:
				if (target.gameObject != null && t >= 1f) {
					target.gameObject.SetActive(value.x > 0.5f);
				}
				break;
			case eTabItemEffectType.EnableOnEnd:
				if (target.component != null && t >= 1f) {
					target.component.enabled = value.x > 0.5f;
				}
				break;
			case eTabItemEffectType.ColorOnGraphic:
				if (target.graphic != null) {
					target.graphic.color = new Color(value.x, value.y, value.z, value.w);
				}
				break;
			case eTabItemEffectType.ColorOnCanvasRenderer:
				if (target.graphic != null) {
					target.graphic.canvasRenderer.SetColor(new Color(value.x, value.y, value.z, value.w));
				}
				break;
		}
	}

	private void OnTabClick(int index) {
		FlushSelected(mSelectedIndex, index);
		mSelectedIndex = index;
        m_OnIndexChanged.Invoke(index);
    }

	private class TweenItem : IEffectTargetContainer {
		public eTabItemEffectType itemEffectType { get; set; }
		public float timer;
		public float duration;
		public GameObject gameObject { get; set; }
		public RectTransform rectTransform { get; set; }
		public Graphic graphic { get; set; }
		public MonoBehaviour component { get; set; }
		public Vector4 valueFrom;
		public Vector4 valueTo;

		private static Queue<TweenItem> cached_instances = new Queue<TweenItem>();
		public static TweenItem Get() {
			TweenItem item = null;
			if (cached_instances.Count > 0) { item = cached_instances.Dequeue(); }
			if (item == null) { item = new TweenItem(); }
			return item;
		}
		public static void Cache(TweenItem item) {
			if (item == null) { return; }
			item.gameObject = null;
			item.rectTransform = null;
			item.graphic = null;
			item.component = null;
			cached_instances.Enqueue(item);
		}
	}

}
