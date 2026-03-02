using UnityEngine;

/// <summary>
/// 魔法值组件类，用于管理游戏对象的魔法值系统
/// </summary>
public class ManaComponent : MonoBehaviour
{
    /// <summary>
    /// 最大魔法值，静态变量，所有实例共享
    /// </summary>
    private static int maxMana = 100;

    /// <summary>
    /// 当前魔法值，初始化为最大魔法值
    /// </summary>
    private int mana = maxMana;

    /// <summary>
    /// 获取当前魔法值
    /// </summary>
    /// <returns>当前魔法值</returns>
    public int GetMana()
    {
        return mana;
    }
}