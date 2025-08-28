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

            [ListDrawerSettings(Expanded = true, DraggableItems = true)] [ValueDropdown("GetAllNPCs")]
            public List<NPC> Members = new();

            private IEnumerable GetAllNPCs()
            {
                // You can fetch from a central registry, a Resources folder, or a static database
                return null;
            }
        }
    }
}