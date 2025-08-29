using Echoes.Runtime;
using NUnit.Framework;
using UnityEngine;

using Echoes.Runtime.SerializableDataStructs;
using Echoes.Runtime.SaveSystem;

namespace Tests.Play
{
    public class SaveSystemTests
    {
        private TraitValue[] RandomPersonalityData(int size = 5)
        {
            TraitValue[] personalityData = new TraitValue[size];
            for (int i = 0; i < size; i++)
            {
                personalityData[i] = new TraitValue();
                personalityData[i].traitName = "trait name " + i;
                personalityData[i].value = Random.Range(-1.0f, 1.0f);
            }
            
            return personalityData;
        }

        private TrustLevel[] RandomInformantsTrustLevels(int size = 5)
        {
            TrustLevel[] trustLevels = new TrustLevel[size];
            for (int i = 0; i < size; i++)
            {
                trustLevels[i] = new TrustLevel();
                trustLevels[i].informantName = "Name " + i;
                trustLevels[i].level = Random.Range(-1.0f, 1.0f);
            }
            
            return trustLevels;
        }
        
        private SerializableNpcData GenerateComplexTestData(int size = 5)
        {
            var npcs = new SerializableNpcData
            {
                data = new EchoesNpcData[size]
            };
            
            for (int i = 0; i < size; i++)
            {
                npcs.data[i] = new EchoesNpcData(new NPCEchoes(),false);
                npcs.data[i].name = "Name " + i;
                npcs.data[i].trustLevels = RandomInformantsTrustLevels();
                npcs.data[i].npcPersonality = RandomPersonalityData();
                npcs.data[i].opinionOfPlayer = RandomPersonalityData();
            }
            
            return npcs;
        }

        private SerializableNpcData GenerateSimpleTestData()
        {
            const string nom = "Simple test";  
            var npcsData = new SerializableNpcData
            {
                data = new EchoesNpcData[1]
            };
            npcsData.data[0] = new EchoesNpcData(new NPCEchoes(),false)
            {
                name = nom
            };
            
            return npcsData;
        }
        
        [Test]
        public void SimpleJsonWriteRead()
        {
            EchoesSaveSystem saveSystem = new JsonEchoesSaveSystem();
            
            var testData = GenerateSimpleTestData();
            
            saveSystem.NpcData = testData;
            saveSystem.Write();
            saveSystem.NpcData = null; // makes sure to nuke all data
            saveSystem.Read();
            
            Debug.Log(((JsonEchoesSaveSystem)saveSystem).SaveDirectory);
            Assert.AreEqual(testData.data[0].name,saveSystem.NpcData?.data[0].name);
        }
        
        [Test]
        public void SimpleBinaryWriteRead()
        {
            EchoesSaveSystem saveSystem = new BinaryEchoesSaveSystem();
            
            var testData = GenerateSimpleTestData();
            
            saveSystem.NpcData = testData;
            saveSystem.Write();
            saveSystem.NpcData = null; // makes sure to nuke all data
            saveSystem.Read();
            
            Debug.Log(((BinaryEchoesSaveSystem)saveSystem).SaveDirectory);
            Assert.AreEqual(testData.data[0].name,saveSystem.NpcData?.data[0].name);
        }

        private void AssertTrustLevels(TrustLevel[] expectedTrustLevels, TrustLevel[] actualTrustLevels)
        {
            for (int i = 0; i < expectedTrustLevels.Length; i++)
            {
                var expected = expectedTrustLevels[i];
                var actual = actualTrustLevels[i];
                
                Assert.AreEqual(expected.informantName, actual.informantName);
                Assert.AreEqual(expected.level, actual.level,0.01);
            }
        }
        
        private void AssertPersonalityData(TraitValue[] expectedPerso, TraitValue[] actualPerso)
        {
            for (int i = 0; i < expectedPerso.Length; i++)
            {
                var expected = expectedPerso[i];
                var actual = actualPerso[i];
                
                Assert.AreEqual(expected.traitName, actual.traitName);
                Assert.AreEqual(expected.value, actual.value,0.01);
            }
        }

        private void AssertNestedEquals(SerializableNpcData expected, SerializableNpcData actual)
        {
            for (int i = 0; i < expected.data.Length; i++)
            {
                Assert.AreEqual(expected.data[i].name, actual.data[i].name);
                AssertTrustLevels(expected.data[i].trustLevels, actual.data[i].trustLevels);
                AssertPersonalityData(expected.data[i].npcPersonality, actual.data[i].npcPersonality);
                AssertPersonalityData(expected.data[i].opinionOfPlayer, actual.data[i].opinionOfPlayer);
            }
        }

        [Test]
        public void ComplexJsonWriteRead()
        {
            EchoesSaveSystem saveSystem = new JsonEchoesSaveSystem();
            
            var testData = GenerateComplexTestData();
            saveSystem.NpcData = testData;
            saveSystem.Write();
            saveSystem.NpcData = null;
            saveSystem.Read();
            
            Debug.Log(((JsonEchoesSaveSystem)saveSystem).SaveDirectory);
            AssertNestedEquals(testData,saveSystem.NpcData);
        }
        
        [Test]
        public void ComplexBinaryWriteRead()
        {
            EchoesSaveSystem saveSystem = new BinaryEchoesSaveSystem();
            
            var testData = GenerateComplexTestData();
            saveSystem.NpcData = testData;
            saveSystem.Write();
            saveSystem.NpcData = null;
            saveSystem.Read();
            
            Debug.Log(((BinaryEchoesSaveSystem)saveSystem).SaveDirectory);
            AssertNestedEquals(testData,saveSystem.NpcData);
        }
    }
}