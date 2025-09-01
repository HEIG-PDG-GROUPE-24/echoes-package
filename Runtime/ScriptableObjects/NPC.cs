using System;
using System.Collections;
using System.Collections.Generic;
using Echoes.Runtime.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime
{
    [CreateAssetMenu(fileName = "NPCSo", menuName = "Scriptable Objects/Echoes - NPC")]
    public class NPC : ScriptableObject
    {
        
        public string Name;

        [TabGroup("Infos", "Contacts", SdfIconType.ImageAlt, TextColor = "#99E3D4")]
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = true)] [ValueDropdown("GetAllNPCs")]
        public List<EchoesNpcComponent> Contacts = new();


        [TabGroup("Infos", "Trust", SdfIconType.Shield, TextColor = "#F7D6E0")]
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = true, OnBeginListElementGUI = nameof(OnTrustRowAdded))]
        public List<TrustRow> Trusts = new();

        [TabGroup("Infos", "Traits", SdfIconType.Stars, TextColor = "#F2B5D4")]
        [ListDrawerSettings(
            ShowItemCount = true,
            DraggableItems = false,
            HideAddButton = true,
            HideRemoveButton = true,
            OnBeginListElementGUI = nameof(OnTraitRowAdded))]
        public List<TraitsRow> Traits = new();

        [TabGroup("Infos", "Opinion", SdfIconType.Question, TextColor = "#FFE156")]
        [ListDrawerSettings(
            ShowItemCount = true,
            DraggableItems = false,
            HideAddButton = true,
            HideRemoveButton = true,
            OnBeginListElementGUI = nameof(OnOpinionRowAdded))]
        public List<TraitsRow> Opinions = new();
        
        private IEnumerable GetAllNPCs()
        {
            List<EchoesNpcComponent> npcs = EchoesGlobal.GetAllNPCs();
            npcs.RemoveAll(npc => npc.npcData.Name == Name);
            return npcs;
        }
        
        private void OnTrustRowAdded(int index)
        {
            if (index < 0 || index >= Trusts.Count)
                return;

            var row = Trusts[index];
            row.current = this; ;
            row.Min = NPCGlobalStatsGeneratorSo.Instance.globalTrust.minValue;
            row.Max = NPCGlobalStatsGeneratorSo.Instance.globalTrust.maxValue;
            
            if (row.TrustLevel < row.Min || row.TrustLevel > row.Max)
                row.ResetTrust();
            
        }
        
        private void OnTraitRowAdded(int index)
        {
            if (index < 0 || index >= Traits.Count)
                return;

            var row = Traits[index];
            var min = NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue;
            var max = NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue;
            row.Min = min;
            row.Max = max;

            if (row.Intensity < row.Min || row.Intensity > row.Max)
                row.Intensity = min;

        }
        
        private void OnOpinionRowAdded(int index)
        {
            if (index < 0 || index >= Opinions.Count)
                return;

            var row = Opinions[index];
            var min = NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue;
            var max = NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue;
            row.Min = min;
            row.Max = max;

            if (row.Intensity < row.Min || row.Intensity > row.Max)
                row.Intensity = min;

        }

        public void SyncWithGlobalTraits()
        {
            var olderTraits = new List<TraitsRow>(Traits);
            var olderOpinions = new List<TraitsRow>(Opinions);

            Traits.Clear();
            Opinions.Clear();
            List<GlobalTraitsRow> newer = NPCGlobalStatsGeneratorSo.Instance.globalTraits.Traits;
            var min = NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue;

            foreach (var t in newer)
            {
                var oldTrait = olderTraits.Find(x => x.Name == t.Name);
                var oldOpinion = olderOpinions.Find(x => x.Name == t.Name);

                Traits.Add(new TraitsRow(t.Name, oldTrait != null ? oldTrait.Intensity : min));
                Opinions.Add(new TraitsRow(t.Name, oldOpinion != null ? oldOpinion.Intensity : min));
            }
        }

    }
}