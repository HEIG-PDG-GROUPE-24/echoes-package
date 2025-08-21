using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using TMPro;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEditor.Experimental;

namespace Tests.playMode
{
    public class testCounter
    {
        [Test]
        public void testInitialisation()
        {
            GameObject go = new GameObject();
            GameObject valueDisplayGo = new GameObject();
            
            var valueText = valueDisplayGo.AddComponent<TextMeshProUGUI>();
            var manager = go.AddComponent<Counter>();
            
            manager.countDisplay = valueText;
            manager.UpdateValue();
            
            Assert.AreEqual("0",valueText.text);
        }
        
        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(50)]
        public void testIncrement(int times)
        {
            GameObject go = new GameObject();
            GameObject valueDisplayGo = new GameObject();
            
            var valueText = valueDisplayGo.AddComponent<TextMeshProUGUI>();
            var manager = go.AddComponent<Counter>();
            
            manager.countDisplay = valueText;

            for(int i = 0; i < times; i++)
                manager.on_increment_press();
            
            Assert.AreEqual(times.ToString(), valueText.text);
        }
        

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(50)]
        public void testDecrement(int times)
        {
            GameObject go = new GameObject();
            GameObject valueDisplayGo = new GameObject();
            
            var valueText = valueDisplayGo.AddComponent<TextMeshProUGUI>();
            var manager = go.AddComponent<Counter>();
            manager.countDisplay = valueText;

            for(int i = 0; i < times; i++)
                manager.on_decrement_press();
            
            Assert.AreEqual((-times).ToString(), valueText.text);
        }
    }
}
