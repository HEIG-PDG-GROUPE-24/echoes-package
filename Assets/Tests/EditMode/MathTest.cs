using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEditor.Experimental;

namespace Tests.EditMode
{
    public class MathTest
    {
        [Test]
        public void TestAdd()
        {
            Assert.AreEqual(ComplicatedMath.Add(1,2),3);
        }
        
        [Test]
        public void TestMultiply()
        {
            Assert.AreEqual(ComplicatedMath.Multiply(1,2),2);
        }
    }
}