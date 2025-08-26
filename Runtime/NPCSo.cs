using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

namespace Echoes
{
    [CreateAssetMenu(fileName = "NPCSo", menuName = "Scriptable Objects/NPCSo")]
    public class NPCSo : ScriptableObject
    {
        [SerializeField, ShowInInspector]
        private string name;
        
        [TabGroup("Infos", "Contacts", SdfIconType.ImageAlt, TextColor = "#99E3D4")]
        [ValueDropdown("TreeViewOfInts", ExpandAllMenuItems = true)]
        public List<int> IntTreview = new List<int>() { 1, 2, 7 };

        [TabGroup("Infos", "Trust", SdfIconType.Shield, TextColor = "#F7D6E0")]
        [ListDrawerSettings(Expanded = true, DraggableItems = true)]
        public List<TrustRow> Contacts = new List<TrustRow>()
        {
            new TrustRow("Alice", 5),
            new TrustRow("Bob", 3)
        };
        
        [Serializable]
        public class TrustRow
        {
            [ValueDropdown("GetNames")]
            public string ContactName;

            [Range(0, 10)]
            public int TrustLevel;

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
            [ReadOnly]
            public string TraitName;
            
            [Range(0, 10)]
            public int Intensity;

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
        public List<TraitsRow> Traits = new List<TraitsRow>()
        {
            new TraitsRow("Brave", 7),
            new TraitsRow("Cautious", 4)
        };
        
        [TabGroup("Infos", "Opinion", SdfIconType.Question, TextColor = "#FFE156")]
        public bool top;
        
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
