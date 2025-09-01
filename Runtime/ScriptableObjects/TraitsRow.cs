using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime.ScriptableObjects
{
    [Serializable]
    public class TraitsRow
    {
        [ReadOnly] public string Name;

        [PropertyRange("@Min", "@Max")] [InlineButton("@Intensity = (Min + Max) / 2", "Reset")]
        public float Intensity;

        public TraitsRow()
        {
        }

        public TraitsRow(string name, float intensity)
        {
            Name = name;
            Intensity = intensity;
        }

        [HideInInspector] public float Min;
        [HideInInspector] public float Max;
    }
}