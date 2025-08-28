using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime
{
    [Serializable]
    public class TrustRow
    {
        public NPC current { private get; set; }
        public float Min { get; set; }
        public float Max { get; set; }

        [ListDrawerSettings(ShowFoldout = true, DraggableItems = true)] [ValueDropdown("GetAllNPCs")]
        public NPCEchoes Contact = new();


        [PropertyRange("Min", "Max")] public int TrustLevel;

        [Button(ButtonSizes.Small)]
        public void ResetTrust()
        {
            TrustLevel = Mathf.RoundToInt((Min + Max) / 2);
        }

        private IEnumerable GetAllNPCs()
        {
            List<NPCEchoes> npcs = EchoesGlobal.GetAllNPCs();
            npcs.RemoveAll(npc => npc.npcData.Name == current.Name);
            return npcs;
        }
    }
}