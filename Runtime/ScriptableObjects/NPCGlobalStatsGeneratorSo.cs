using System;
using System.Collections.Generic;
using System.Linq;
using Echoes.Runtime.ScriptableObjects;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace Echoes.Runtime
{
    [CreateAssetMenu(fileName = "GlobalStats", menuName = "Echoes/Global Stats")]
    public class GlobalStats : SerializedScriptableObject
    { 
        
        [TabGroup("Infos", "Traits", SdfIconType.ImageAlt, TextColor = "#99E3D4")]
        [InlineProperty, HideLabel]
        [SerializeField]
        public GlobalTraits globalTraits = new GlobalTraits();

        [TabGroup("Infos", "Groupes", SdfIconType.People, TextColor = "#F7D6E0")]
        [InlineProperty, HideLabel]
        [SerializeField]
        public GlobalGroups globalGroups = new GlobalGroups();

        [TabGroup("Infos", "Trust", SdfIconType.Shield, TextColor = "#F2B5D4")]
        [InlineProperty, HideLabel]
        [SerializeField]
        public GlobalTrust globalTrust = new GlobalTrust();
        
        [TabGroup("Infos","Distance", SdfIconType.PinMap, TextColor = "#FFE156")]
        [InlineProperty, HideLabel]
        [OdinSerialize]
        public GlobalDistance globalDistance = new GlobalDistance();
        
        private List<Rumor> _rumors = new List<Rumor>();
        
        /**
         * @param rumor to add to the update list
         */
        public void AddRumor(Rumor rumor)
        {
            OnRumorAdded.ForEach(action => action(rumor));
            _rumors.Add(rumor);
        }

        /**
         * @param distance to add to the distance ran of each rumor
         * When the rumor ran the necessary distance, it propagates player opinion and is removed from the update list
         */
        public void UpdateRumors(double distance)
        {
            foreach (var rumor in _rumors.ToList().Where(rumor => rumor.Update(distance)))
            {
                OnRumorRemoved.ForEach(action => action(rumor));
                _rumors.Remove(rumor);
            }
        }

        public HashSet<Action<Rumor>> OnRumorAdded = new();
        public HashSet<Action<Rumor>> OnRumorRemoved = new();
        
        private static GlobalStats _instance;

        public static GlobalStats Instance
        {
            get
            {
                if (_instance == null)
                {
#if UNITY_EDITOR
                    // In Editor, find the asset automatically
                    var guids = UnityEditor.AssetDatabase.FindAssets("t:GlobalStats");
                    if (guids.Length > 0)
                    {
                        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                        _instance = UnityEditor.AssetDatabase.LoadAssetAtPath<GlobalStats>(path);
                    }
#endif
                    if (_instance == null)
                    {
                        Debug.LogError("No GlobalStats asset found in project!");
                    }
                }
                return _instance;
            }
        }
    }
}