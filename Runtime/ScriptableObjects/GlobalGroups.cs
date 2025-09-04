using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Echoes.Runtime.ScriptableObjects
{
    
    [Serializable]
    public class GlobalGroups
    {
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = true)]
        public List<GroupsRow> Groups = new();

        [Serializable]
        public class GroupsRow : ISerializationCallbackReceiver
        {
            public string Name = "New Group";

            [ListDrawerSettings(ShowFoldout = true, DraggableItems = true)] [ValueDropdown("GetAllNPCs")]
            public List<EchoesNpcComponent> Members = new();
            
            [SerializeField][HideInInspector]
            public List<string> MemberNames = new();

            public void ResolveReferences()
            {
                Members.Clear();
                foreach (var name in MemberNames)
                {
                    try
                    {
                        Members.Add(EchoesGlobal.GetAllNPCs().Find(npc => npc.name == name));
                    }
                    catch (Exception e)
                    {
                        Debug.LogErrorFormat("group {0} could not resolve npc {1}",Name,name);
                    }
                }
            }

            private IEnumerable GetAllNPCs()
            {
                return EchoesGlobal.GetAllNPCs();
            }

            public void OnBeforeSerialize()
            {
                MemberNames.Clear();
                Members
                    .Where(member => member != null && member.npcData != null)
                    .Where(member => !MemberNames.Contains(member.npcData.name))
                    .ForEach(member =>MemberNames.Add(member.npcData.Name));
            }

            public void OnAfterDeserialize()
            {
                // do nothing
            }
        }
    }
}