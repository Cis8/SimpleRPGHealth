using UnityEngine;
using ElectricDrill.SimpleRpgHealth;
using ElectricDrill.SimpleRpgCore;

namespace ElectricDrill.SimpleRpgCore.Events
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "PreHeal Game Event", menuName = "Simple RPG Health/Events/Generated/PreHeal")]
    public class PreHealGameEvent : GameEventGeneric2<PreHealInfo, EntityCore>
    {
    }
}