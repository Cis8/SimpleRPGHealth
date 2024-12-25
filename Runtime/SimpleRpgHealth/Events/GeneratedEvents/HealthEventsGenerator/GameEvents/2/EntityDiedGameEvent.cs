using UnityEngine;
using ElectricDrill.SimpleRpgHealth;

namespace ElectricDrill.SimpleRpgCore.Events
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "EntityDied Game Event", menuName = "Simple RPG Health/Events/Generated/EntityDied")]
    public class EntityDiedGameEvent : GameEventGeneric2<EntityHealth, TakenDmgInfo>
    {
    }
}