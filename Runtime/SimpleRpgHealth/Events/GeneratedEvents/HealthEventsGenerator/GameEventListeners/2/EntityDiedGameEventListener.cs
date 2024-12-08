using UnityEngine;
using ElectricDrill.SimpleRpgCore;
using ElectricDrill.SimpleRpgCore.Events;
using ElectricDrill.SimpleRpgHealth;

namespace ElectricDrill.SimpleRpgCore.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityDiedGameEventListener : GameEventListenerGeneric2<EntityHealth, TakenDmgInfo>
    {
    }
}