using UnityEngine;

namespace Echoes
{
    [CreateAssetMenu(fileName = "TrustLevelSo", menuName = "Scriptable Objects/TrustLevelSo")]
    public class TrustLevelSo : ScriptableObject
    {
        public int level;
        public string description;
    }
}
