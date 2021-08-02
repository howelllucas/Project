using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePanel :MonoBehaviour
{
    //用字典存储所有UI控件,值为控件上的脚本，比如button，有两个image和button，所以用list存储
    public Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>(); 
    
    void Awake()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<Slider>();
        FindChildrenControl<ScrollRect>();
    }
    //从字典里取脚本的方法
    protected T GetControl<T>(string name) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(name))
        {
            for (int i = 0; i < controlDic[name].Count; i++)
            {
                if (controlDic[name][i] is T)
                {
                    return controlDic[name][i] as T;
                }
            }
            
        }
        
            return null;
        
    }
    //把找到的脚本放到字典里
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();
        string name;
        for (int i = 0; i < controls.Length; i++)
        {
            name = controls[i].gameObject.name;
            if (controlDic.ContainsKey(name))
            {
                controlDic[name].Add(controls[i]);
            }
            else
            {
                controlDic.Add(name, new List<UIBehaviour>() { controls[i] });
            }
        }
    }

    public virtual void showPanel()
    {

    }
    public virtual void hidePanel()
    {

    }
}
