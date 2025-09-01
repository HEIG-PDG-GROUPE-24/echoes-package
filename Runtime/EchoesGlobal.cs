using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime
{
    public class EchoesGlobal : MonoBehaviour
    {
        [InlineEditor] public NPCGlobalStatsGeneratorSo GlobalStats;

        public static List<EchoesNpcComponent> GetAllNPCs() => FindObjectsByType<EchoesNpcComponent>(FindObjectsSortMode.None).ToList();
    }
}