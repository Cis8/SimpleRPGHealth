using UnityEngine;
using ElectricDrill.SimpleRpgCore;
using ElectricDrill.SimpleRpgHealth;

namespace ElectricDrill.SimpleRpgCore.Events
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "PreDmg Game Event", menuName = "Simple RPG/Events/Generated/PreDmg")]
    public class PreDmgGameEvent : GameEventGeneric2<PreDmgInfo, EntityCore>
    {
    }
}