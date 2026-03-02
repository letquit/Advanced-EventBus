using System;

/// <summary>
/// 定义事件绑定接口，提供带参数和不带参数的事件处理方法
/// </summary>
/// <typeparam name="T">事件类型，必须实现IEvent接口</typeparam>
internal interface IEventBinding<T>
{
    /// <summary>
    /// 获取或设置带事件参数的事件处理方法
    /// </summary>
    public Action<T> OnEvent { get; set; }

    /// <summary>
    /// 获取或设置不带参数的事件处理方法
    /// </summary>
    public Action OnEventNoArgs { get; set; }
}

/// <summary>
/// 事件绑定实现类，支持带参数和不带参数的事件处理
/// </summary>
/// <typeparam name="T">事件类型，必须实现IEvent接口</typeparam>
public class EventBinding<T> : IEventBinding<T> where T : IEvent
{
    /// <summary>
    /// 存储带事件参数的事件处理方法，默认为空操作
    /// </summary>
    private Action<T> onEvent = _ => { };

    /// <summary>
    /// 存储不带参数的事件处理方法，默认为空操作
    /// </summary>
    private Action onEventNoArgs = () => { };

    /// <summary>
    /// 显式接口实现：获取或设置带事件参数的事件处理方法
    /// </summary>
    Action<T> IEventBinding<T>.OnEvent
    {
        get => onEvent;
        set => onEvent = value;
    }

    /// <summary>
    /// 显式接口实现：获取或设置不带参数的事件处理方法
    /// </summary>
    Action IEventBinding<T>.OnEventNoArgs
    {
        get => onEventNoArgs;
        set => onEventNoArgs = value;
    }

    /// <summary>
    /// 使用带事件参数的处理方法初始化EventBinding实例
    /// </summary>
    /// <param name="onEvent">带事件参数的事件处理方法</param>
    public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;

    /// <summary>
    /// 使用不带参数的处理方法初始化EventBinding实例
    /// </summary>
    /// <param name="onEventNoArgs">不带参数的事件处理方法</param>
    public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;

    /// <summary>
    /// 向不带参数的事件处理方法集合中添加新的处理方法
    /// </summary>
    /// <param name="onEvent">要添加的不带参数的事件处理方法</param>
    public void Add(Action onEvent) => onEventNoArgs += onEvent;

    /// <summary>
    /// 从不带参数的事件处理方法集合中移除指定的处理方法
    /// </summary>
    /// <param name="onEvent">要移除的不带参数的事件处理方法</param>
    public void Remove(Action onEvent) => onEventNoArgs -= onEvent;

    /// <summary>
    /// 向带事件参数的事件处理方法集合中添加新的处理方法
    /// </summary>
    /// <param name="onEvent">要添加的带事件参数的事件处理方法</param>
    public void Add(Action<T> onEvent) => this.onEvent += onEvent;

    /// <summary>
    /// 从带事件参数的事件处理方法集合中移除指定的处理方法
    /// </summary>
    /// <param name="onEvent">要移除的带事件参数的事件处理方法</param>
    public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;
}