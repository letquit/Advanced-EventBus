
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private static int maxHealth =100;
    
    private int health = maxHealth;
    
    
    public int GetHealth()
    {
        return health;
    }
}
