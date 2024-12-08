using UnityEngine;

namespace ElectricDrill.SimpleRpgHealth
{
    public abstract class OnDeathStrategy : ScriptableObject
    {
        public abstract void Die(EntityHealth entityHealth);
    }
}