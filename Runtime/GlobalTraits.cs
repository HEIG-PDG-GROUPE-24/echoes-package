using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes
{
    [Serializable]
    class GlobalTraits
    {
        [OnValueChanged(nameof(ClampAllIntensities))]
        public float minValue;

        [OnValueChanged(nameof(ClampAllIntensities))]
        public float maxValue = 10f;

        [ListDrawerSettings(ShowFoldout = true, DraggableItems = true)]
        public List<TraitsRow> Traits = new();

        private void ClampAllIntensities()
        {
            if (minValue > maxValue) (minValue, maxValue) = (maxValue, minValue);
            foreach (var t in Traits)
                t.Intensity = Mathf.Clamp(t.Intensity, minValue, maxValue);
        }

        [Serializable]
        public class TraitsRow
        {
            public string TraitName;

            [PropertyRange("@$root.minValue", "@$root.maxValue")] [InlineButton("@Intensity = $root.minValue", "Reset")]
            public float Intensity;

            public TraitsRow()
            {
            }

            public TraitsRow(string name, float intensity = 0f)
            {
                TraitName = name;
                Intensity = intensity;
            }
        }
    }
}