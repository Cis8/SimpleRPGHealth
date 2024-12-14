using System;
using ElectricDrill.SimpleRpgCore.Stats;
using ElectricDrill.SimpleRpgCore.Utils;
using UnityEngine;

namespace ElectricDrill.SimpleRpgHealth
{
    [Serializable]
    public class LifestealStatConfig
    {
        [SerializeField] private Stat lifestealStat;
        [SerializeField] private DmgStateSelector dmgStateSelector;
        [SerializeField] private Source lifestealSource;
        public Stat LifestealStat => lifestealStat;
        public DmgStateSelector DmgStateSelector => dmgStateSelector;
        public Source LifestealSource => lifestealSource;
    }

    [CreateAssetMenu(fileName = "New Lifesteal Config", menuName = "Simple RPG Health/Lifesteal Config")]
    public class LifestealConfig : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<DmgType, LifestealStatConfig> lifestealMappings;
        public SerializableDictionary<DmgType, LifestealStatConfig> LifestealMappings => lifestealMappings;
    }
    
    [Serializable]
    public enum DmgStateSelector
    {
        /// <summary>
        /// The raw damage before any defensive reductions are applied.
        /// </summary>
        Raw,

        /// <summary>
        /// Damage after being reduced by defensive stats.
        /// </summary>
        DefReduced,

        /// <summary>
        /// Damage after reductions from defensive stats and barrier absorption.
        /// </summary>
        DefBarrierReduced,

        /// <summary>
        /// The net damage actually lost from the target's HP.
        /// </summary>
        Net
    }
}