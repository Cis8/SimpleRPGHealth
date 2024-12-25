using UnityEngine;
using ElectricDrill.SimpleRpgHealth;

namespace ElectricDrill.SimpleRpgCore.Events
{
    /// <summary>
    /// The entity whose Max HP changed, the new Max HP, and the old Max HP
    /// </summary>
    public class EntityMaxHealthChangedGameEventListener : GameEventListenerGeneric3<EntityHealth, long, long>
    {
    }
}