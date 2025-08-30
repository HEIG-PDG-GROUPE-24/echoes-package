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
        private NPCEchoes npc1;
        private NPCEchoes npc2;

        [SetUp]
        public void SetUp()
        {
            goNpc1 = new GameObject();
            goNpc2 = new GameObject();

            npc1 = goNpc1.AddComponent<NPCEchoes>();
            npc2 = goNpc2.AddComponent<NPCEchoes>();

            npc1.npcData = ScriptableObject.CreateInstance<NPC>();
            npc2.npcData = ScriptableObject.CreateInstance<NPC>();

            npc1.npcData.name = "npc1";
            npc2.npcData.name = "npc2";

            npc1.Personality["test"] =
                (NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue -
                 NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue) * 0.5 +
                NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue;
            npc2.Personality["test"] =
                (NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue -
                 NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue) * 0.7 +
                NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue;

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

            npc1.Contacts.Add(npc2);
            npc2.Contacts.Add(npc1);
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void OpinionReception()
        {
            npc2.InformantsTrust["npc1"] = NPCGlobalStatsGeneratorSo.Instance.globalTrust.maxValue;
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
            npc2.InformantsTrust["npc1"] = NPCGlobalStatsGeneratorSo.Instance.globalTrust.minValue;
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
            EchoesNpcData npcData = new EchoesNpcData(new NPCEchoes());
            NPCEchoes npcObject = new NPCEchoes();

            npcData.npcPersonality = new TraitValue[1];
            npcData.npcPersonality[0] = new TraitValue();
            npcData.npcPersonality[0].traitName = "Trait";
            npcData.npcPersonality[0].value = 5;

            npcObject.LoadFromData(npcData);

            AssertEqual(npcData, npcObject);
        }

        private void AssertEqual(EchoesNpcData data, NPCEchoes npc)
        {
            foreach (var trait in data.opinionOfPlayer)
                Assert.AreEqual(trait.value, npc.GetOpinionOfPlayer(trait.traitName));

            foreach (var trait in data.npcPersonality)
                Assert.AreEqual(trait.value, npc.Personality[trait.traitName]);

            foreach (var trust in data.trustLevels)
                Assert.AreEqual(trust.level, npc.InformantsTrust[trust.informantName]);
        }
    }
}