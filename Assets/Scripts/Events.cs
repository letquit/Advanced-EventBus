/// <summary>
/// 定义事件接口，作为所有事件类型的基接口
/// </summary>
public interface IEvent
{
}

/// <summary>
/// 测试事件结构体，实现IEvent接口
/// </summary>
public struct TestEvent : IEvent
{
}

/// <summary>
/// 玩家事件类，实现IEvent接口，用于表示玩家相关的事件数据
/// </summary>
public class PlayerEvent : IEvent
{
    /// <summary>
    /// 玩家当前生命值
    /// </summary>
    public int health;

    /// <summary>
    /// 玩家当前法力值
    /// </summary>
    public int mana;
}