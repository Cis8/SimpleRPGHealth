using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElectricDrill.SimpleRpgHealth {
    [CreateAssetMenu(fileName = "PercentualDefReductionFn", menuName = "Simple RPG Health/Def Reduction Functions/Percentual Def Reduction")]
    public class PercentualDefReductionFn : DefReductionFn
    {
        public override double ReducedDef(long piercingStatValue, long piercedStatValue) {
            var reducedDefStat = piercedStatValue - piercedStatValue * piercingStatValue / 100.0d;
            return reducedDefStat < 0 ? 0 : reducedDefStat;
        }
    }
}
