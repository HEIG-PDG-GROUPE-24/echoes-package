using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime.ScriptableObjects
{
    [Serializable]
    public class GlobalTraitsRow
    {
        [Required]
        [OnValueChanged(nameof(UpdateAllNPCs))]
        public string Name;

        [HideInInspector] public float Min;
        [HideInInspector] public float Max;

        [PropertyRange("@Min", "@Max")]
        [InlineButton("@Intensity = Min", "Reset")]
        public float Intensity;

        public GlobalTraitsRow(string name, int intensity)
        {
            Name = name;
            Intensity = intensity;
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