using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Echoes.Runtime.ScriptableObjects
{
    
    [Serializable]
    public class GlobalGroups
    {
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = true)]
        public List<GroupsRow> Groups = new();

        [Serializable]
        public class GroupsRow
        {
            public string Name = "New Group";

            [ListDrawerSettings(ShowFoldout = true, DraggableItems = true)] [ValueDropdown("GetAllNPCs")]
            public List<EchoesNpcComponent> Members = new();

            private IEnumerable GetAllNPCs()
            {
                return EchoesGlobal.GetAllNPCs();
            }
        }
    }
}