using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime
{
    [Serializable]
    public class TrustRow
    {
        [ValueDropdown("GetNames")] public string ContactName;

        [Range(0, 10)] public int TrustLevel;

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
        
        private static IEnumerable<string> GetNames()
        {
            return new List<string>() { "Alice", "Bob", "Charlie" };
        }
    }
}