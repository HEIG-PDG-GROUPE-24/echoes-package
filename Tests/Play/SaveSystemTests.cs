using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tests.Play
{
    public class SaveSystemTests
    {
        private PersonalityData RandomPersonalityData(int size = 5)
        {
            PersonalityData personalityData = new PersonalityData();
            personalityData.traitValues = new TraitValue[size];
            for (int i = 0; i < size; i++)
            {
                personalityData.traitValues[i] = new TraitValue();
                personalityData.traitValues[i].traitName = "trait name " + i;
                personalityData.traitValues[i].value = Random.Range(-1.0f, 1.0f);
            }
            
            return personalityData;
        }

        private InformantsTrustLevels RandomInformantsTrustLevels(int size = 5)
        {
            InformantsTrustLevels trustLevels = new InformantsTrustLevels();
            trustLevels.trustLevels =  new TrustLevel[size];
            for (int i = 0; i < size; i++)
            {
                trustLevels.trustLevels[i] = new TrustLevel();
                trustLevels.trustLevels[i].informantName = "Name " + i;
                trustLevels.trustLevels[i].level = Random.Range(-1.0f, 1.0f);
            }
            
            return trustLevels;
        }
        
        private SerializableNpcData GenerateComplexTestData(int size = 5)
        {
            var data = new SerializableNpcData
            {
                data = new EchoesNpcData[size]
            };
            
            for (int i = 0; i < size; i++)
            {
                data.data[i] = new EchoesNpcData(new EchoesNpc());
                data.data[i].name = "Name " + i;
                data.data[i].trustLevels = RandomInformantsTrustLevels();
                data.data[i].npcPersonality = RandomPersonalityData();
                data.data[i].opinionOfPlayer = RandomPersonalityData();
            }
            
            return data;
        }

        private SerializableNpcData GenerateSimpleTestData()
        {
            const string nom = "Simple test";  
            var npcsData = new SerializableNpcData
            {
                data = new EchoesNpcData[1]
            };
            npcsData.data[0] = new EchoesNpcData(new EchoesNpc())
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
        
        private void AssertPersonalityData(PersonalityData expectedPerso, PersonalityData actualPerso)
        {
            for (int i = 0; i < expectedPerso.traitValues.Length; i++)
            {
                var expected = expectedPerso.traitValues[i];
                var actual = actualPerso.traitValues[i];
                
                Assert.AreEqual(expected.traitName, actual.traitName);
                Assert.AreEqual(expected.value, actual.value,0.01);
            }
        }

        private void AssertNestedEquals(SerializableNpcData expected, SerializableNpcData actual)
        {
            for (int i = 0; i < expected.data.Length; i++)
            {
                Assert.AreEqual(expected.data[i].name, actual.data[i].name);
                AssertTrustLevels(expected.data[i].trustLevels.trustLevels, actual.data[i].trustLevels.trustLevels);
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