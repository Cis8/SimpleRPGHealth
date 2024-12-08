using UnityEngine;
using ElectricDrill.SimpleRpgCore;
using ElectricDrill.SimpleRpgHealth;

namespace ElectricDrill.SimpleRpgCore.Events
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "EntityGainedHealth Game Event", menuName = "Simple RPG/Events/Generated/EntityGainedHealth")]
    public class EntityGainedHealthGameEvent : GameEventGeneric2<EntityHealth, long>
    {
    }
}