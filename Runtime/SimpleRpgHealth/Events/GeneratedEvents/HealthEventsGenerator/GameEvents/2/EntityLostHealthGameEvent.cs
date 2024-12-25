using UnityEngine;
using ElectricDrill.SimpleRpgHealth;

namespace ElectricDrill.SimpleRpgCore.Events
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "EntityLostHealth Game Event", menuName = "Simple RPG Health/Events/Generated/EntityLostHealth")]
    public class EntityLostHealthGameEvent : GameEventGeneric2<EntityHealth, long>
    {
    }
}