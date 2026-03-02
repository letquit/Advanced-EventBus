using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 事件总线泛型类，用于管理特定类型事件的订阅、发布和处理
/// </summary>
/// <typeparam name="T">事件类型，必须实现IEvent接口</typeparam>
public static class EventBus<T> where T : IEvent
{
    private static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

    /// <summary>
    /// 注册事件绑定
    /// </summary>
    /// <param name="binding">要注册的事件绑定对象</param>
    public static void Register(EventBinding<T> binding) => bindings.Add(binding);

    /// <summary>
    /// 注销事件绑定
    /// </summary>
    /// <param name="binding">要注销的事件绑定对象</param>
    public static void Deregister(EventBinding<T> binding) => bindings.Remove(binding);

    /// <summary>
    /// 触发事件，通知所有已注册的监听器
    /// </summary>
    /// <param name="@event">要触发的事件实例</param>
    public static void Raise(T @event)
    {
        // 遍历所有已注册的绑定并执行相应的事件回调方法
        foreach (var binding in bindings)
        {
            binding.OnEvent.Invoke(@event);
            binding.OnEventNoArgs.Invoke();
        }
    }

    /// <summary>
    /// 清空所有事件绑定
    /// </summary>
    private static void Clear()
    {
        Debug.Log($"Clearing {typeof(T).Name} bindings");
        bindings.Clear();
    }
}