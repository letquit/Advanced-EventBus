using UnityEngine;

/// <summary>
/// 健康组件类，用于管理游戏对象的生命值
/// </summary>
public class HealthComponent : MonoBehaviour
{
    /// <summary>
    /// 最大生命值，所有实例共享的静态变量
    /// </summary>
    private static int maxHealth = 100;

    /// <summary>
    /// 当前生命值
    /// </summary>
    private int health = maxHealth;


    /// <summary>
    /// 获取当前生命值
    /// </summary>
    /// <returns>当前生命值</returns>
    public int GetHealth()
    {
        return health;
    }
}