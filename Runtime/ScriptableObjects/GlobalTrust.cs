using System;
using Sirenix.OdinInspector;

namespace Echoes.Runtime.ScriptableObjects
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