using UnityEngine;
using ElectricDrill.SimpleRpgHealth;

namespace ElectricDrill.SimpleRpgCore.Events
{
    /// <summary>
    /// The entity whose Max HP changed, the new Max HP, and the old Max HP
    /// </summary>
    [CreateAssetMenu(fileName = "EntityMaxHealthChanged Game Event", menuName = "Simple RPG Health/Events/Generated/EntityMaxHealthChanged")]
    public class EntityMaxHealthChangedGameEvent : GameEventGeneric3<EntityHealth, long, long>
    {
    }
}