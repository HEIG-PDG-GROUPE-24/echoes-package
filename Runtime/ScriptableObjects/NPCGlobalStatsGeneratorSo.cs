using Echoes.Runtime.ScriptableObjects;
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
        public GlobalTraits globalTraits = new GlobalTraits();

        [TabGroup("Infos", "Groupes", SdfIconType.People, TextColor = "#F7D6E0")]
        [InlineProperty, HideLabel]
        [SerializeField]
        public GlobalGroupes globalGroupes = new GlobalGroupes();

        [TabGroup("Infos", "Trust", SdfIconType.Shield, TextColor = "#F2B5D4")]
        [InlineProperty, HideLabel]
        [SerializeField]
        public GlobalTrust globalTrust = new GlobalTrust();
        
        private static NPCGlobalStatsGeneratorSo _instance;

        public static NPCGlobalStatsGeneratorSo Instance
        {
            get
            {
                if (_instance == null)
                {
#if UNITY_EDITOR
                    // In Editor, find the asset automatically
                    var guids = UnityEditor.AssetDatabase.FindAssets("t:NPCGlobalStatsGeneratorSo");
                    if (guids.Length > 0)
                    {
                        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                        _instance = UnityEditor.AssetDatabase.LoadAssetAtPath<NPCGlobalStatsGeneratorSo>(path);
                    }
#endif
                    if (_instance == null)
                    {
                        Debug.LogError("No NPCGlobalStatsGeneratorSo asset found in project!");
                    }
                }
                return _instance;
            }
        }
    }
}