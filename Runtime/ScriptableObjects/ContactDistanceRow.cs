using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Echoes.Runtime.ScriptableObjects
{
    [Serializable]
    public class ContactDistanceRow
    {
        [ReadOnly] public string Name;

        [PropertyRange("@Min", "@Max")] [InlineButton("@Distance = (Min + Max) / 2", "Reset")]
        [OnValueChanged("onValueChange")]
        public double Distance;

        public void onValueChange()
        {
            globalDistance.SetContactDistance(npcName,Name,Distance);
        } 

        [HideInInspector] public string npcName;
        [HideInInspector] public GlobalDistance globalDistance;
        [HideInInspector] public double Min;
        [HideInInspector] public double Max;
    }
}