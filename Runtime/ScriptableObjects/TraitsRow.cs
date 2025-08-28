using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime.ScriptableObjects
{
    [Serializable]
    public class TraitsRow
    {
        public string Name;

        [HideInInspector] public float Min;
        [HideInInspector] public float Max;

        [PropertyRange("@Min", "@Max")]
        [InlineButton("@Intensity = Min", "Reset")]
        public float Intensity;

        public TraitsRow(string name, int intensity)
        {
            Name = name;
            Intensity = intensity;
        }
    }
}