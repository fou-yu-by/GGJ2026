using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public delegate void EventHandler(object sender, EventArgs e);

    private Dictionary<string, EventHandler> handlerDic = new Dictionary<string, EventHandler>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
    }

    public void AddListener(string eventName, EventHandler handler)
    {
        if (!handlerDic.ContainsKey(eventName))
        {
            handlerDic.Add(eventName, handler);
        }
        else
        {
            handlerDic[eventName] += handler;
        }
    }

    public void RemoveListener(string eventName, EventHandler handler)
    {
        if (handlerDic.ContainsKey(eventName))
        {
            handlerDic[eventName] -= handler;
        }
    }

    /// <summary>
    /// 无参数触发
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="sender">触发源</param>
    public void TriggerEvent(string eventName, object sender)
    {
        if (handlerDic.ContainsKey(eventName))
        {
            handlerDic[eventName]?.Invoke(sender, EventArgs.Empty);
        }
    }

    public void TriggerEvent(string eventName, object sender, EventArgs eventArgs)
    {
        if (handlerDic.ContainsKey(eventName))
        {
            handlerDic[eventName]?.Invoke(sender, eventArgs);
        }
    }

    public void Clear()
    {
        handlerDic.Clear();
    }
}

public static class EventManagerExtensions
{
    public static void TriggerEvent(this object sender, string eventName)
    {
        EventManager.Instance.TriggerEvent(eventName, sender);
    }

    public static void TriggerEvent(this object sender, string eventName, EventArgs eventArgs)
    {
        EventManager.Instance.TriggerEvent(eventName, sender, eventArgs);
    }
}