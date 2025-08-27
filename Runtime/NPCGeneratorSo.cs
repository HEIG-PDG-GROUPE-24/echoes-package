using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime
{
    [CreateAssetMenu(fileName = "NPCSo", menuName = "Scriptable Objects/Echoes - NPC")]
    public class NPCGeneratorSo : ScriptableObject
    {
        private string Name;

        [TabGroup("Infos", "Contacts", SdfIconType.ImageAlt, TextColor = "#99E3D4")]
        [ValueDropdown("TreeViewOfInts", ExpandAllMenuItems = true)]
        public List<int> IntTreview = new List<int>() { 1, 2, 7 };

        [TabGroup("Infos", "Trust", SdfIconType.Shield, TextColor = "#F7D6E0")]
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = true)]
        public List<TrustRow> Contacts = new()
        {
            new TrustRow("Alice", 5),
            new TrustRow("Bob", 3)
        };

        [Serializable]
        public class TrustRow
        {
            [ValueDropdown("GetNames")] public string ContactName;

            [Range(0, 10)] public int TrustLevel;

            [Button(ButtonSizes.Small)]
            public void ResetTrust()
            {
                TrustLevel = 5; // default value
            }

            public TrustRow(string name, int trust)
            {
                ContactName = name;
                TrustLevel = trust;
            }

            // Example dropdown values
            private static IEnumerable<string> GetNames()
            {
                return new List<string>() { "Alice", "Bob", "Charlie" };
            }
        }

        [Serializable]
        public class TraitsRow
        {
            [ReadOnly] public string TraitName;

            [Range(0, 10)] public int Intensity;

            [Button(ButtonSizes.Small)]
            public void ResetTrait()
            {
                Intensity = 5; // default value
            }

            public TraitsRow(string name, int intensity)
            {
                TraitName = name;
                Intensity = intensity;
            }
        }

        [TabGroup("Infos", "Traits", SdfIconType.Stars, TextColor = "#F2B5D4")]
        [ListDrawerSettings(
            ShowItemCount = true,
            DraggableItems = false,
            HideAddButton = true,
            HideRemoveButton = true)]
        public List<TraitsRow> Traits = new List<TraitsRow>()
        {
            new TraitsRow("Brave", 7),
            new TraitsRow("Cautious", 4)
        };

        [TabGroup("Infos", "Opinion", SdfIconType.Question, TextColor = "#FFE156")]
        [ListDrawerSettings(
            ShowItemCount = true,
            DraggableItems = false,
            HideAddButton = true,
            HideRemoveButton = true)]
        public List<TraitsRow> Opinions = new List<TraitsRow>()
        {
            new TraitsRow("Brave", 8),
            new TraitsRow("Cautious", 2)
        };

        private IEnumerable TreeViewOfInts()
        {
            return new ValueDropdownList<int>()
            {
                { "One", 1 },
                { "Two", 2 },
                { "Three", 3 },
                { "Four", 4 },
                { "Five", 5 },
                { "Six", 6 },
                { "Seven", 7 },
                { "Eight", 8 },
                { "Nine", 9 },
                { "Ten", 10 }
            };
        }
    }
}