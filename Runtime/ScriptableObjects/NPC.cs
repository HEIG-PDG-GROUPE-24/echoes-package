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
        private string Name;

        [TabGroup("Infos", "Contacts", SdfIconType.ImageAlt, TextColor = "#99E3D4")]
        [ValueDropdown("TreeViewOfInts", ExpandAllMenuItems = true)]
        public List<int> Contacts = new();

        [TabGroup("Infos", "Trust", SdfIconType.Shield, TextColor = "#F7D6E0")]
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = true)]
        public List<TrustRow> Trusts = new();

        [TabGroup("Infos", "Traits", SdfIconType.Stars, TextColor = "#F2B5D4")]
        [ListDrawerSettings(
            ShowItemCount = true,
            DraggableItems = false,
            HideAddButton = true,
            HideRemoveButton = true)]
        public List<TraitsRow> Traits = new();

        [TabGroup("Infos", "Opinion", SdfIconType.Question, TextColor = "#FFE156")]
        [ListDrawerSettings(
            ShowItemCount = true,
            DraggableItems = false,
            HideAddButton = true,
            HideRemoveButton = true)]
        public List<TraitsRow> Opinions = new();
    }
}