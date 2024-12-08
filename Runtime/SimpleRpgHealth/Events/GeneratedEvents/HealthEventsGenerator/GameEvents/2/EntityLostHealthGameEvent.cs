using UnityEngine;
using ElectricDrill.SimpleRpgCore;
using ElectricDrill.SimpleRpgHealth;

namespace ElectricDrill.SimpleRpgCore.Events
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "EntityLostHealth Game Event", menuName = "Simple RPG/Events/Generated/EntityLostHealth")]
    public class EntityLostHealthGameEvent : GameEventGeneric2<EntityHealth, long>
    {
    }
}