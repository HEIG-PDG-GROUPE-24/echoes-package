using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime
{
    [Serializable]
    public class GlobalTrust
    {
        [OnValueChanged(nameof(ClampBetweenValues))]
        public float minValue = 1f;
        
        [OnValueChanged(nameof(ClampBetweenValues))]
        public float maxValue = 10f;
        
        private void ClampBetweenValues()
        {
            if (minValue > maxValue) (minValue, maxValue) = (maxValue, minValue);
        }
    }
}