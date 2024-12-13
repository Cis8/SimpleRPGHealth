using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElectricDrill.SimpleRpgHealth {
    [CreateAssetMenu(fileName = "New Source", menuName = "Simple RPG Health/Source")]
    public class Source : ScriptableObject
    {
        public override string ToString() {
            return name;
        }
    }
}
