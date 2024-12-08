using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElectricDrill.SimpleRpgHealth {
    [CreateAssetMenu(fileName = "New Dmg Source", menuName = "Simple RPG/Dmg Source")]
    public class Source : ScriptableObject
    {
        public override string ToString() {
            return name;
        }
    }
}
