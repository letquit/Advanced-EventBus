using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 事件总线工具类，用于管理所有事件类型和事件总线实例的初始化与清理
/// </summary>
public static class EventBusUtil
{
    /// <summary>
    /// 获取所有事件类型的只读列表
    /// </summary>
    public static IReadOnlyList<Type> EventTypes { get; set; }

    /// <summary>
    /// 获取所有事件总线类型的只读列表
    /// </summary>
    public static IReadOnlyList<Type> EventBusTypes { get; set; }

#if UNITY_EDITOR
    /// <summary>
    /// 获取当前播放模式状态
    /// </summary>
    public static PlayModeStateChange PlayModeState { get; set; }

    /// <summary>
    /// 编辑器加载时初始化方法，注册播放模式状态变化事件
    /// </summary>
    [InitializeOnLoadMethod]
    public static void InitializeEditor()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChange;
    }

    /// <summary>
    /// 播放模式状态变化回调方法
    /// </summary>
    /// <param name="state">新的播放模式状态</param>
    private static void OnPlayModeStateChange(PlayModeStateChange state)
    {
        PlayModeState = state;
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            // 退出播放模式时清空所有事件总线
            ClearAllBuses();
        }
    }
#endif

    /// <summary>
    /// 运行时初始化方法，在场景加载前执行事件系统初始化
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        EventTypes = PredefinedAssemblyUtil.GetTypes(typeof(IEvent));
        EventBusTypes = InitializeAllBuses();
    }

    /// <summary>
    /// 初始化所有事件总线实例
    /// </summary>
    /// <returns>包含所有事件总线类型的列表</returns>
    private static List<Type> InitializeAllBuses()
    {
        List<Type> eventBusTypes = new List<Type>();

        var typedef = typeof(EventBus<>);
        foreach (var eventType in EventTypes)
        {
            var busType = typedef.MakeGenericType(eventType);
            eventBusTypes.Add(busType);
            Debug.Log($"Initialized EventBus<{eventType.Name}>");
        }

        return eventBusTypes;
    }

    /// <summary>
    /// 清空所有事件总线中的订阅者
    /// </summary>
    public static void ClearAllBuses()
    {
        Debug.Log("Clearing all buses...");
        for (int i = 0; i < EventBusTypes.Count; i++)
        {
            var busType = EventBusTypes[i];
            var clearMethod = busType.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);
            clearMethod.Invoke(null, null);
        }
    }
}