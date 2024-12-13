using UnityEngine;
using ElectricDrill.SimpleRpgHealth;

namespace ElectricDrill.SimpleRpgCore.Events
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "EntityHealed Game Event", menuName = "Simple RPG Health/Events/Generated/EntityHealed")]
    public class EntityHealedGameEvent : GameEventGeneric1<ReceivedHealInfo>
    {
    }
}