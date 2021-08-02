using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCenter : BaseManager<EventCenter>
{
    //用字典来存储
    public Dictionary<string, UnityAction<object>> eventDic = new Dictionary<string, UnityAction<object>>();

    //注册事件
    public void AddEventListener(string name,UnityAction<object> action)
    {
        if (eventDic.ContainsKey(name))//有这个事件
        {
            eventDic[name] += action;
        }
        else//没有这个事件
        {
            eventDic.Add(name, action);
        }
    }
    //移除事件
    public void removeEventListener(string name,UnityAction<object> action)
    {
        if (eventDic.ContainsKey(name))
        {
            eventDic[name] -= action;
        }
    }

    //触发事件
    public void EventTrigger(string name, object info)
    {
        if (eventDic.ContainsKey(name))//有这个事件
        {
            eventDic[name].Invoke(info);
        }
        
    }
    //移除这个事件中心，主要用在切换场景时
    public void Clear()
    {
        eventDic.Clear();
    }
}
