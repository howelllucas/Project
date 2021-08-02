using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ui_Type
{
    bot,
    mid,
    top,
    system
}
public class UIMgr : BaseManager<UIMgr>
{
    public Dictionary<string, BasePanel> UiDic = new Dictionary<string, BasePanel>();

    GameObject bot;
    GameObject mid;
    GameObject top;
    GameObject system;

    public UIMgr()
    {
        GameObject canvas;
        GameObject eventSystem;
        canvas =ResMgr.GetInstance().LoadRes<GameObject>("Canvas");
        GameObject.DontDestroyOnLoad(canvas);

        bot = GameObject.Find("bot");
        mid = GameObject.Find("mid");
        top = GameObject.Find("top");
        system = GameObject.Find("system");

        eventSystem=ResMgr.GetInstance().LoadRes<GameObject>("EventSystem");
        GameObject.DontDestroyOnLoad(eventSystem);
    }
    public void ShowPanel(string name, Ui_Type type)
    {
        ResMgr.GetInstance().LoadAsync<GameObject>(name, (obj) =>
        {
            GameObject fatherGo=bot;
            switch (type)
            {
                case Ui_Type.mid:
                    fatherGo = mid;
                    break;
                case Ui_Type.top:
                    fatherGo = top;
                    break;
                case Ui_Type.system:
                    fatherGo = system;
                    break;
                default:
                    break;
            }
            obj.transform.SetParent(fatherGo.transform);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            UiDic.Add(name, obj.GetComponent<BasePanel>());
        });
    }

    public void HidePanel(string name)
    {

    }
}
