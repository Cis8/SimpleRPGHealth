using UnityEngine;

namespace ElectricDrill.SimpleRpgHealth
{
    [CreateAssetMenu(fileName = "Destroy On Death Strategy", menuName = "Simple RPG Health/Death strategies/Destroy")]
    public class DestroyOnDeathStrategy : OnDeathStrategy
    {
        public override void Die(EntityHealth entityHealth) {
            Destroy(entityHealth.gameObject);
        }
    }
}