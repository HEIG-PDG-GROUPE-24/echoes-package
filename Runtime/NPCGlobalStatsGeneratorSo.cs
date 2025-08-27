using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime
{
    [CreateAssetMenu(fileName = "NPCGlobalStatsSo", menuName = "Scriptable Objects/Echoes - Global Stats")]
    public class NPCGlobalStatsGeneratorSo : ScriptableObject
    {
        [TabGroup("Infos", "Traits", SdfIconType.ImageAlt, TextColor = "#99E3D4")]
        [InlineProperty, HideLabel]
        [SerializeField]
        private GlobalTraits globalTraits = new GlobalTraits();

        [TabGroup("Infos", "Groupes", SdfIconType.People, TextColor = "#F7D6E0")]
        [InlineProperty, HideLabel]
        [SerializeField]
        private GlobalGroupes globalGroupes = new GlobalGroupes();

        [TabGroup("Infos", "Trust", SdfIconType.Shield, TextColor = "#F2B5D4")]
        [InlineProperty, HideLabel]
        [SerializeField]
        private GlobalTrust globalTrust = new GlobalTrust();

    }
}