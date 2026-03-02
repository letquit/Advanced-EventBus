
using UnityEngine;

public class ManaComponent : MonoBehaviour
{
    private static int maxMana = 100;
    private int mana = maxMana;
    
    public int GetMana()
    {
        return mana;
    }
}
