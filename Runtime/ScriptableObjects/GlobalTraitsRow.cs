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
        

        public GlobalTraitsRow(string name)
        {
            Name = name;
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