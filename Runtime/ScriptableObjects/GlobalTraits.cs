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
            DraggableItems = true
        )]
        [OnCollectionChanged(nameof(UpdateAllNPCs))]
        public List<GlobalTraitsRow> Traits = new();

        private void ClampAllIntensities()
        {
            if (minValue > maxValue) (minValue, maxValue) = (maxValue, minValue);
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