using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElectricDrill.SimpleRpgHealth {
    public abstract class DefReductionFn : ScriptableObject
    {
        public abstract double ReducedDef(long piercingStatValue, long piercedStatValue);
    }
}
