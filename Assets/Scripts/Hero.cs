using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hero : MonoBehaviour
{
    private HealthComponent health;
    private ManaComponent mana;

    EventBinding<TestEvent> testEventBinding;
    EventBinding<PlayerEvent> playerEventBinding;

    private void OnEnable()
    {
        testEventBinding = new EventBinding<TestEvent>(HandleTestEvent);
        EventBus<TestEvent>.Register(testEventBinding);
        
        playerEventBinding = new EventBinding<PlayerEvent>(HandlePlayerEvent);
        EventBus<PlayerEvent>.Register(playerEventBinding);
    }

    private void OnDisable()
    {
        EventBus<TestEvent>.Deregister(testEventBinding);
        EventBus<PlayerEvent>.Deregister(playerEventBinding);
    }

    private void Awake()
    {
        health = gameObject.GetComponent<HealthComponent>();
        if (health == null) health = gameObject.AddComponent<HealthComponent>();

        mana = gameObject.GetComponent<ManaComponent>();
        if (mana == null) mana = gameObject.AddComponent<ManaComponent>();
    }
    
    private void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            EventBus<TestEvent>.Raise(new TestEvent());
        }

        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            EventBus<PlayerEvent>.Raise(new PlayerEvent
            {
                health = health.GetHealth(),
                mana = mana.GetMana()
            });
        }
    }

    private void HandleTestEvent()
    {
        Debug.Log("Test event received!");
    }

    private void HandlePlayerEvent(PlayerEvent playerEvent)
    {
        Debug.Log($"Player event received! Health: {playerEvent.health}, Mana: {playerEvent.mana}");
    }
}