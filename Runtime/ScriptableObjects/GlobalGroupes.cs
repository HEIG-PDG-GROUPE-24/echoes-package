using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime
{
    
    [Serializable]
    public class GlobalGroupes
    {
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = true)]
        public List<GroupesRow> Groupes = new();

        [Serializable]
        public class GroupesRow
        {
            public string Name = "New Groupe";

            [ListDrawerSettings(ShowFoldout = true, DraggableItems = true)] [ValueDropdown("GetAllNPCs")]
            public List<NPCEchoes> Members = new();

            private IEnumerable GetAllNPCs()
            {
                List<NPCEchoes> npcs = EchoesGlobal.GetAllNPCs();
                return npcs;
            }
        }
    }
}