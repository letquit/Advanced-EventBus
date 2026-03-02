using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 英雄角色管理类，负责处理英雄的生命值、法力值以及事件系统交互
/// </summary>
public class Hero : MonoBehaviour
{
    private HealthComponent health;
    private ManaComponent mana;

    EventBinding<TestEvent> testEventBinding;
    EventBinding<PlayerEvent> playerEventBinding;

    /// <summary>
    /// 当对象被启用时调用，注册事件监听器
    /// </summary>
    private void OnEnable()
    {
        testEventBinding = new EventBinding<TestEvent>(HandleTestEvent);
        EventBus<TestEvent>.Register(testEventBinding);

        playerEventBinding = new EventBinding<PlayerEvent>(HandlePlayerEvent);
        EventBus<PlayerEvent>.Register(playerEventBinding);
    }

    /// <summary>
    /// 当对象被禁用时调用，注销事件监听器
    /// </summary>
    private void OnDisable()
    {
        EventBus<TestEvent>.Deregister(testEventBinding);
        EventBus<PlayerEvent>.Deregister(playerEventBinding);
    }

    /// <summary>
    /// 初始化组件，在Awake阶段确保健康和法力组件存在
    /// </summary>
    private void Awake()
    {
        health = gameObject.GetComponent<HealthComponent>();
        if (health == null) health = gameObject.AddComponent<HealthComponent>();

        mana = gameObject.GetComponent<ManaComponent>();
        if (mana == null) mana = gameObject.AddComponent<ManaComponent>();
    }

    /// <summary>
    /// 每帧更新逻辑，处理键盘输入并触发相应事件
    /// </summary>
    private void Update()
    {
        // 检测A键按下，触发测试事件
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            EventBus<TestEvent>.Raise(new TestEvent());
        }

        // 检测B键按下，触发玩家状态事件
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            EventBus<PlayerEvent>.Raise(new PlayerEvent
            {
                health = health.GetHealth(),
                mana = mana.GetMana()
            });
        }
    }

    /// <summary>
    /// 处理测试事件的回调方法
    /// </summary>
    private void HandleTestEvent()
    {
        Debug.Log("Test event received!");
    }

    /// <summary>
    /// 处理玩家事件的回调方法
    /// </summary>
    /// <param name="playerEvent">包含玩家状态信息的事件对象</param>
    private void HandlePlayerEvent(PlayerEvent playerEvent)
    {
        Debug.Log($"Player event received! Health: {playerEvent.health}, Mana: {playerEvent.mana}");
    }
}