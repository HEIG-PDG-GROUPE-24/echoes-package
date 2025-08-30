using System;
using Echoes.Runtime.SerializableDataStructs;
using Echoes.Runtime;
using NUnit.Framework;
using UnityEditor.Build.Content;
using UnityEngine;

namespace Tests.Play
{
    public class NpcBehaviourTest
    {
        private GameObject goNpc1;
        private GameObject goNpc2;
        private EchoesNpcComponent npc1;
        private EchoesNpcComponent npc2;

        [SetUp]
        public void SetUp()
        {
            goNpc1 = new GameObject();
            goNpc2 = new GameObject();

            npc1 = goNpc1.AddComponent<EchoesNpcComponent>();
            npc2 = goNpc2.AddComponent<EchoesNpcComponent>();

            npc1.npcData = ScriptableObject.CreateInstance<NPC>();
            npc2.npcData = ScriptableObject.CreateInstance<NPC>();

            npc1.npcData.name = "npc1";
            npc2.npcData.name = "npc2";

            npc1.SetPersonality("test",
                (NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue -
                 NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue) * 0.5 +
                NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue
            );
            npc2.SetPersonality("test",
                (NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue -
                 NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue) * 0.7 +
                NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue
            );

            npc1.SetOpinionOfPlayer("test",
                (NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue -
                 NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue) * 0.3 +
                NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue
            );
            npc2.SetOpinionOfPlayer("test",
                (NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue -
                 NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue) * 0.7 +
                NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue
            );
            
            npc1.AddContact(npc2);
            npc2.AddContact(npc1);
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void OpinionReception()
        {
            npc2.SetTrustTowards("npc1", NPCGlobalStatsGeneratorSo.Instance.globalTrust.maxValue);
            npc1.EndPlayerInteraction();
            Assert.AreEqual(
                (NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue -
                 NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue) * 0.5 +
                NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue,
                npc2.GetOpinionOfPlayer("test"), 0.0001
            );
        }

        [Test]
        public void OpinionReceptionNoTrust()
        {
            npc2.SetTrustTowards("npc1", NPCGlobalStatsGeneratorSo.Instance.globalTrust.minValue);
            npc1.EndPlayerInteraction();

            Assert.AreEqual(
                (NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue -
                 NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue) * 0.7 +
                NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue,
                npc2.GetOpinionOfPlayer("test"), 0.0001
            );
        }

        [Test]
        public void PlayerScore1()
        {
            Assert.AreEqual(npc1.AppreciationOfPlayer(), 0.6, 0.0001);
        }

        [Test]
        public void PlayerScore2()
        {
            Assert.AreEqual(npc2.AppreciationOfPlayer(), 1, 0.0001);
        }

        [Test]
        public void ObjectToData()
        {
            EchoesNpcData npcData = new EchoesNpcData(npc1);
            AssertEqual(npcData, npc1);
        }

        [Test]
        public void DataToObject()
        {
            EchoesNpcData npcData = new EchoesNpcData(new EchoesNpcComponent());
            EchoesNpcComponent echoesNpcObject = new EchoesNpcComponent();

            npcData.npcPersonality = new TraitValue[1];
            npcData.npcPersonality[0] = new TraitValue();
            npcData.npcPersonality[0].traitName = "Trait";
            npcData.npcPersonality[0].value = 5;

            echoesNpcObject.LoadFromData(npcData);

            AssertEqual(npcData, echoesNpcObject);
        }

        private void AssertEqual(EchoesNpcData data, EchoesNpcComponent echoesNpc)
        {
            foreach (var trait in data.opinionOfPlayer)
                Assert.AreEqual(trait.value, echoesNpc.GetOpinionOfPlayer(trait.traitName));

            foreach (var trait in data.npcPersonality)
                Assert.AreEqual(trait.value, echoesNpc.GetPersonality(trait.traitName));

            foreach (var trust in data.trustLevels)
                Assert.AreEqual(trust.level, echoesNpc.GetTrustTowards(trust.informantName));
        }
    }
}