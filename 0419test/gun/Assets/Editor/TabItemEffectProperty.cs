using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomPropertyDrawer(typeof(TabGroup.TabItemEffect))]
public class TabItemEffectProperty : PropertyDrawer {

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		return 96f;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty(position, label, property);
		EditorGUI.LabelField(new Rect(position.x, position.y, position.width, 16f), label);
		EditorGUI.EndProperty();
		//position.x += 16f;
		//	position.width -= 16f;
		position.y += 16f;
		position.height -= 16f;
		EditorGUI.indentLevel++;
		SerializedProperty pType = property.FindPropertyRelative("m_ItemEffectType");
		SerializedProperty pvl = property.FindPropertyRelative("valueLeft");
		SerializedProperty pvh = property.FindPropertyRelative("valueHighlight");
		SerializedProperty pvr = property.FindPropertyRelative("valueRight");
		Vector4 vl = pvl.vector4Value;
		Vector4 vh = pvh.vector4Value;
		Vector4 vr = pvr.vector4Value;
		EditorGUI.BeginChangeCheck();
		EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, 16f), pType);
		bool reset = EditorGUI.EndChangeCheck();
		Rect rectTarget = new Rect(position.x, position.y + 16f, position.width, 16f);
		Rect rpl = new Rect(position.x, position.y + 32f, position.width, 16f);
		Rect rph = new Rect(position.x, position.y + 48f, position.width, 16f);
		Rect rpr = new Rect(position.x, position.y + 64f, position.width, 16f);
		bool changed = false;
		switch ((TabGroup.eTabItemEffectType)pType.enumValueIndex) {
			case TabGroup.eTabItemEffectType.AnchoredPosition:
				SerializedProperty apt = property.FindPropertyRelative("m_RectTransform");
				EditorGUI.PropertyField(rectTarget, apt);
				EditorGUI.BeginChangeCheck();
				Vector2 apl = EditorGUI.Vector2Field(rpl, "Anchored Position Left", new Vector2(vl.x, vl.y));
				Vector2 aph = EditorGUI.Vector2Field(rph, "anchored position highlight", new Vector2(vh.x, vh.y));
				Vector2 apr = EditorGUI.Vector2Field(rpr, "anchored position right", new Vector2(vr.x, vr.y));
				if (reset) {
					RectTransform rt = apt.objectReferenceValue as RectTransform;
					apl = aph = apr = rt == null ? Vector2.zero : rt.anchoredPosition;
				}
				if (EditorGUI.EndChangeCheck() || reset) {
					vl.x = apl.x; vl.y = apl.y;
					vh.x = aph.x; vh.y = aph.y;
					vr.x = apr.x; vr.y = apr.y;
					changed = true;
				}
				break;
			case TabGroup.eTabItemEffectType.Scale:
				SerializedProperty st = property.FindPropertyRelative("m_RectTransform");
				EditorGUI.PropertyField(rectTarget, st);
				EditorGUI.BeginChangeCheck();
				Vector3 sl = EditorGUI.Vector3Field(rpl, "Scale Left", new Vector3(vl.x, vl.y, vl.z));
				Vector3 sh = EditorGUI.Vector3Field(rph, "Scale Highlight", new Vector3(vh.x, vh.y, vh.z));
				Vector3 sr = EditorGUI.Vector3Field(rpr, "Scale Right", new Vector3(vr.x, vr.y, vr.z));
				if (reset) {
					RectTransform rt = st.objectReferenceValue as RectTransform;
					sl = sh = sr = rt == null ? Vector3.one : rt.localScale;
				}
				if (EditorGUI.EndChangeCheck() || reset) {
					vl.x = sl.x; vl.y = sl.y; vl.z = sl.z;
					vh.x = sh.x; vh.y = sh.y; vh.z = sh.z;
					vr.x = sr.x; vr.y = sr.y; vr.z = sr.z;
					changed = true;
				}
				break;
			case TabGroup.eTabItemEffectType.ActiveOnStart:
			case TabGroup.eTabItemEffectType.ActiveOnEnd:
				SerializedProperty ag = property.FindPropertyRelative("m_GameObject");
				EditorGUI.PropertyField(rectTarget, ag);
				EditorGUI.BeginChangeCheck();
				bool al = EditorGUI.Toggle(rpl, "Active Left", vl.x > 0.5f);
				bool ah = EditorGUI.Toggle(rph, "Active Highlight", vh.x > 0.5f);
				bool ar = EditorGUI.Toggle(rpr, "Active Right", vr.x > 0.5f);
				if (reset) {
					GameObject go = ag.objectReferenceValue as GameObject;
					al = ah = ar = go == null ? true : go.activeSelf;
				}
				if (EditorGUI.EndChangeCheck() || reset) {
					vl.x = al ? 1f : 0f;
					vh.x = ah ? 1f : 0f;
					vr.x = ar ? 1f : 0f;
					changed = true;
				}
				break;
			case TabGroup.eTabItemEffectType.EnableOnStart:
			case TabGroup.eTabItemEffectType.EnableOnEnd:
				SerializedProperty ec = property.FindPropertyRelative("m_Component");
				EditorGUI.PropertyField(rectTarget, ec);
				EditorGUI.BeginChangeCheck();
				bool el = EditorGUI.Toggle(rpl, "Enable Left", vl.x > 0.5f);
				bool eh = EditorGUI.Toggle(rph, "Enable Highlight", vh.x > 0.5f);
				bool er = EditorGUI.Toggle(rpr, "Enable Right", vr.x > 0.5f);
				if (reset) {
					MonoBehaviour mb = ec.objectReferenceValue as MonoBehaviour;
					el = eh = er = mb == null ? true : mb.enabled;
				}
				if (EditorGUI.EndChangeCheck() || reset) {
					vl.x = el ? 1f : 0f;
					vh.x = eh ? 1f : 0f;
					vr.x = er ? 1f : 0f;
					changed = true;
				}
				break;
			case TabGroup.eTabItemEffectType.ColorOnGraphic:
			case TabGroup.eTabItemEffectType.ColorOnCanvasRenderer:
				SerializedProperty cg = property.FindPropertyRelative("m_Graphic");
				EditorGUI.PropertyField(rectTarget, cg);
				EditorGUI.BeginChangeCheck();
				Color cl = EditorGUI.ColorField(rpl, "Color Left", new Color(vl.x, vl.y, vl.z, vl.w));
				Color ch = EditorGUI.ColorField(rph, "Color Highlight", new Color(vh.x, vh.y, vh.z, vh.w));
				Color cr = EditorGUI.ColorField(rpr, "Color Right", new Color(vr.x, vr.y, vr.z, vr.w));
				if (reset) {
					Graphic graphic = cg.objectReferenceValue as Graphic;
					cl = ch = cr = graphic == null ? Color.white :
						(TabGroup.eTabItemEffectType)pType.enumValueIndex == TabGroup.eTabItemEffectType.ColorOnGraphic ?
						graphic.color : graphic.canvasRenderer.GetColor();
				}
				if (EditorGUI.EndChangeCheck() || reset) {
					vl.x = cl.r; vl.y = cl.g; vl.z = cl.b; vl.w = cl.a;
					vh.x = ch.r; vh.y = ch.g; vh.z = ch.b; vh.w = ch.a;
					vr.x = cr.r; vr.y = cr.g; vr.z = cr.b; vr.w = cr.a;
					changed = true;
				}
				break;

		}
		if (changed) {
			pvl.vector4Value = vl;
			pvh.vector4Value = vh;
			pvr.vector4Value = vr;
		}
		EditorGUI.indentLevel--;
	}

}
