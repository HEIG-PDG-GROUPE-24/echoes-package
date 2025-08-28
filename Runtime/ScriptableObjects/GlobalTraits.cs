using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime.ScriptableObjects
{
    [Serializable]
    public class GlobalTraits
    {
        [Required] [OnValueChanged(nameof(ClampAllIntensities))]
        public float minValue = 1f;

        [Required] [OnValueChanged(nameof(ClampAllIntensities))]
        public float maxValue = 10f;

        [ListDrawerSettings(
            ShowFoldout = true,
            DraggableItems = true,
            OnBeginListElementGUI = nameof(OnRowAdded)
        )]
        [OnCollectionChanged(nameof(UpdateAllNPCs))]
        public List<GlobalTraitsRow> Traits = new();

        private void ClampAllIntensities()
        {
            if (minValue > maxValue) (minValue, maxValue) = (maxValue, minValue);
            foreach (var t in Traits)
            {
                t.Min = minValue;
                t.Max = maxValue;
                t.Intensity = Mathf.Clamp(t.Intensity, minValue, maxValue);
            }
        }

        private void OnRowAdded(int index)
        {
            if (index < 0 || index >= Traits.Count)
                return;

            var row = Traits[index];
            row.Min = minValue;
            row.Max = maxValue;
            row.Intensity = Mathf.Clamp(row.Intensity, minValue, maxValue);
        }

        private void UpdateAllNPCs()
        {
            var allNpcs = EchoesGlobal.GetAllNPCs();
            foreach (var npc in allNpcs)
            {
                npc.npcData.SyncWithGlobalTraits();
            }
        }
    }
}