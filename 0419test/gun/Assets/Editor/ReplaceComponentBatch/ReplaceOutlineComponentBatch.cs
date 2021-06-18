using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Game.UI;

namespace Game
{
    public class ReplaceOutlineComponentBatch : ReplaceComponentBatch
    {
        private Type srcType = typeof(UI.OutLineDxx);
        private Type dstType = typeof(UI.OutlineEx);

        [MenuItem("批量脚本替换/Outline替换")]
        public static void OpenOutlineReplaceComponentWindow()
        {
            var window = GetWindow<ReplaceOutlineComponentBatch>("Outline脚本替换");
        }

        protected override void OnGUI()
        {
            EditorGUILayout.LabelField("被替换组件名", srcType.Name);
            EditorGUILayout.LabelField("替换组件名", dstType.Name);
            if (GUILayout.Button("替换"))
            {
                ReplaceComponent(srcType, dstType);
            }
        }

        protected override void DoReplace(Component srcComp, Type dstType)
        {
            OutLineDxx dxx = srcComp as OutLineDxx;
            if (dxx.enabled)
            {
                var dstComp = srcComp.gameObject.GetComponent(dstType);
                if (dstComp == null)
                {
                    dstComp = srcComp.gameObject.AddComponent(dstType);
                    OnReplace(srcComp, dstComp);
                }
            }
            DestroyImmediate(srcComp, true);
        }

        protected override void OnReplace(Component srcComponent, Component dstComponent)
        {
            base.OnReplace(srcComponent, dstComponent);
            OutLineDxx dxx = srcComponent as OutLineDxx;
            OutlineEx ex = dstComponent as OutlineEx;
            if (dxx == null || ex == null)
                return;

            var color = dxx.effectColor;
            color.a = Mathf.Min(1, color.a * 5);
            ex.OutlineColor = color;

            ex.OutlineWidth = 2;//dxx.effectDistance.magnitude / 1.414f;
            ex.ShadowOffset = new Vector2(0, -5);
        }
    }
}