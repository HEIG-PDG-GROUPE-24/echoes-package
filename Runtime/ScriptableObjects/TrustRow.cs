using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Echoes.Runtime.ScriptableObjects
{
    [Serializable]
    public class TrustRow : ISerializationCallbackReceiver
    {
        public NPC current { private get; set; }
        public float Min { get; set; }
        public float Max { get; set; }

        [ListDrawerSettings(ShowFoldout = true, DraggableItems = true)] [ValueDropdown("GetAllNPCs")]
        public EchoesNpcComponent Contact = null;
        
        [HideInInspector]
        public string contactName;


        [PropertyRange("Min", "Max")] public int TrustLevel;

        [Button(ButtonSizes.Small)]
        public void ResetTrust()
        {
            TrustLevel = Mathf.RoundToInt((Min + Max) / 2);
        }

        private IEnumerable GetAllNPCs()
        {
            List<EchoesNpcComponent> npcs = EchoesGlobal.GetAllNPCs();
            npcs.RemoveAll(npc => npc.npcData.Name == current.Name);
            return npcs;
        }

        public void OnBeforeSerialize()
        {
            if(Contact != null && Contact.npcData != null)
                contactName = Contact.npcData.Name;
        }

        public void OnAfterDeserialize()
        {
            // do nothing
        }

        public void ResolveReference()
        {
            try
            {
                Contact = EchoesGlobal.GetAllNPCs().Find(npc => npc.npcData != null && npc.npcData.Name == contactName);
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("npc {0} could not be resolved", contactName);
            }
        }
    }
}