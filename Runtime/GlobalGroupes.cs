using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime
{
    [Serializable]
    class GlobalGroupes
    {
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = true)]
        public List<GroupesRow> Groupes = new();

        [Serializable]
        public class GroupesRow
        {
            public string Name = "New Groupe";

            [ValueDropdown("TreeViewOfInts", ExpandAllMenuItems = true)]
            public List<int> Members = new List<int>();

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
}