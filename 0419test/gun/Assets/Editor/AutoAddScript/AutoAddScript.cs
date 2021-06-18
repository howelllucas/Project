using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using PT3Localization;
using Game.UI;

[CustomEditor(typeof(RectTransform))]
public class AutoAddScript : DecoratorEditor
{
    public AutoAddScript() : base("RectTransformEditor") { }
    public void OnEnable()
    {
        //这里的target是在Editor中的变量，也就是当前对象
        RectTransform rectTransform = target as RectTransform;


        //--------------Text  begin-----------------
        Text text = rectTransform.GetComponent<Text>();
        TextAppend textAppend = rectTransform.GetComponent<TextAppend>();
        if (text != null && textAppend == null)
        {
            rectTransform.gameObject.AddComponent<TextAppend>();
        }
        else if(text == null && textAppend != null)
        {
            Object.DestroyImmediate(textAppend);
        }

        //var outline = rectTransform.GetComponent<Outline>();
        //if (outline != null)
        //    Object.DestroyImmediate(outline);

        //OutLineDxx outlineDxx = rectTransform.GetComponent<OutLineDxx>();
        //if (text != null && outlineDxx == null)
        //{
        //    rectTransform.gameObject.AddComponent<OutLineDxx>();
        //}
        //else if (text == null && outlineDxx != null)
        //{
        //    Object.DestroyImmediate(outlineDxx);
        //}
        //--------------Text  end-----------------
    }
}
