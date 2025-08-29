using Echoes.Runtime;
using NUnit.Framework;
using UnityEngine;
using Echoes.Runtime.SerializableDataStructs;
using Echoes.Runtime.SaveSystem;
using Echoes.Runtime.ScriptableObjects;

namespace Tests.Play
{
    public class ComponentNpcTests
    {
        private NPCEchoes npc;
        private NPCEchoes npc2;
        private NPCGlobalStatsGeneratorSo globalStats;

        [SetUp]
        public void Setup()
        {
            npc = new GameObject().AddComponent<NPCEchoes>();
            npc.npcData = ScriptableObject.CreateInstance<NPC>();
            npc.npcData.Name = "TestNPCData";

            npc2 = new GameObject().AddComponent<NPCEchoes>();
            npc2.npcData = ScriptableObject.CreateInstance<NPC>();
            npc2.npcData.Name = "TestNPCData2";
            
            globalStats = NPCGlobalStatsGeneratorSo.Instance;
        }

        [Test]
        public void ContactAddedSuccessfully()
        {
            npc.npcData.Contacts.Clear();
            npc.npcData.Contacts.Add(npc2);

            Assert.AreEqual(1, npc.npcData.Contacts.Count);
            Assert.AreEqual("TestNPCData2", npc.npcData.Contacts[0].npcData.Name);
        }

        [Test]
        public void ContactRemovedSuccessfully()
        {
            npc.npcData.Contacts.Add(npc2);
            npc.npcData.Contacts.Remove(npc2);

            Assert.AreEqual(0, npc.npcData.Contacts.Count);
        }
        
        [Test]
        public void TrustRowInitialization()
        {
            npc.npcData.Trusts.Clear();
            var trustRow = new TrustRow
            {
                Contact = npc2,
                current = npc.npcData,
                Min = -100,
                Max = 100,
                TrustLevel = 0
            };
            npc.npcData.Trusts.Add(trustRow);

            Assert.AreEqual(1, npc.npcData.Trusts.Count);
            Assert.AreEqual("TestNPCData2", npc.npcData.Trusts[0].Contact.npcData.Name);
            Assert.AreEqual(0, npc.npcData.Trusts[0].TrustLevel);
        }
        
        [Test]
        public void TraitRowInitialization()
        {
            npc.npcData.Traits.Clear();
            var traitRow = new TraitsRow
            {
                Name = "Bravery",
                Min = 0,
                Max = 100,
                Intensity = 50
            };
            npc.npcData.Traits.Add(traitRow);

            Assert.AreEqual(1, npc.npcData.Traits.Count);
            Assert.AreEqual("Bravery", npc.npcData.Traits[0].Name);
            Assert.AreEqual(50, npc.npcData.Traits[0].Intensity);
        }
        
        [Test]
        public void OpinionRowInitialization()
        {
            npc.npcData.Opinions.Clear();
            var opinionRow = new TraitsRow
            {
                Name = "Trustworthiness",
                Min = 0,
                Max = 100,
                Intensity = 75
            };
            npc.npcData.Opinions.Add(opinionRow);

            Assert.AreEqual(1, npc.npcData.Opinions.Count);
            Assert.AreEqual("Trustworthiness", npc.npcData.Opinions[0].Name);
            Assert.AreEqual(75, npc.npcData.Opinions[0].Intensity);
        }
        
        [Test]
        public void SyncWithGlobalTraitsAddsMissingTraits()
        {
            var globalTrait = globalStats.globalTraits.Traits[0];

            npc.npcData.Traits.Clear();
            npc.npcData.SyncWithGlobalTraits();

            Assert.IsTrue(npc.npcData.Traits.Exists(t => t.Name == globalTrait.Name));
        }
        
        [Test]
        public void SyncWithGlobalTraitsAddsMissingOpinions()
        {
            // Assuming NPCGlobalStatsGeneratorSo.Instance.globalTraits has at least one trait
            var globalTrait = globalStats.globalTraits.Traits[0];

            npc.npcData.Opinions.Clear();
            npc.npcData.SyncWithGlobalTraits();

            Assert.IsTrue(npc.npcData.Opinions.Exists(t => t.Name == globalTrait.Name));
        }
        
        [Test]
        public void SyncWithGlobalTraitsDoesNotDuplicateExistingTraits()
        {
            var globalTrait = globalStats.globalTraits.Traits[0];

            npc.npcData.Traits.Clear();
            npc.npcData.Traits.Add(new TraitsRow { Name = globalTrait.Name, Intensity = 50, Min = 0, Max = 100 });
            npc.npcData.SyncWithGlobalTraits();

            int count = npc.npcData.Traits.FindAll(t => t.Name == globalTrait.Name).Count;
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void SyncWithGlobalTraitsDoesNotDuplicateExistingOpinions()
        {
            var globalTrait = globalStats.globalTraits.Traits[0];

            npc.npcData.Opinions.Clear();
            npc.npcData.Opinions.Add(new TraitsRow { Name = globalTrait.Name, Intensity = 50, Min = 0, Max = 100 });
            npc.npcData.SyncWithGlobalTraits();

            int count = npc.npcData.Opinions.FindAll(t => t.Name == globalTrait.Name).Count;
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void ResetTrustSetsTrustLevelToMidpoint()
        {
            var trustRow = new TrustRow
            {
                current = npc.npcData,
                Min = -100,
                Max = 100,
                TrustLevel = 50
            };
            trustRow.ResetTrust();

            Assert.AreEqual(0, trustRow.TrustLevel);
        }
        
        [Test]
        public void ResetTraitSetsIntensityToMidpoint()
        {
            var traitRow = new TraitsRow
            {
                Name = "Bravery",
                Min = 0,
                Max = 100,
                Intensity = 80
            };
            traitRow.Intensity = (traitRow.Min + traitRow.Max) / 2;

            Assert.AreEqual(50, traitRow.Intensity);
        }
        
        [Test]
        public void ResetOpinionSetsIntensityToMidpoint()
        {
            var opinionRow = new TraitsRow
            {
                Name = "Trustworthiness",
                Min = 0,
                Max = 100,
                Intensity = 90
            };
            opinionRow.Intensity = (opinionRow.Min + opinionRow.Max) / 2;

            Assert.AreEqual(50, opinionRow.Intensity);
        }
        
        [Test]
        public void MultipleNPCsMaintainSeparateData()
        {
            npc.npcData.Traits.Clear();
            npc2.npcData.Traits.Clear();

            var traitRow1 = new TraitsRow
            {
                Name = "Bravery",
                Min = 0,
                Max = 100,
                Intensity = 70
            };
            npc.npcData.Traits.Add(traitRow1);

            var traitRow2 = new TraitsRow
            {
                Name = "Cautiousness",
                Min = 0,
                Max = 100,
                Intensity = 30
            };
            npc2.npcData.Traits.Add(traitRow2);

            Assert.AreEqual(1, npc.npcData.Traits.Count);
            Assert.AreEqual("Bravery", npc.npcData.Traits[0].Name);
            Assert.AreEqual(70, npc.npcData.Traits[0].Intensity);

            Assert.AreEqual(1, npc2.npcData.Traits.Count);
            Assert.AreEqual("Cautiousness", npc2.npcData.Traits[0].Name);
            Assert.AreEqual(30, npc2.npcData.Traits[0].Intensity);
        }
        
        [Test]
        public void TrustedContactCanHaveDifferentLevels()
        {
            npc.npcData.Trusts.Clear();
            var trustRow1 = new TrustRow
            {
                Contact = npc2,
                current = npc.npcData,
                Min = -100,
                Max = 100,
                TrustLevel = 20
            };
            npc.npcData.Trusts.Add(trustRow1);

            npc2.npcData.Trusts.Clear();
            var trustRow2 = new TrustRow
            {
                Contact = npc,
                current = npc2.npcData,
                Min = -100,
                Max = 100,
                TrustLevel = 80
            };
            npc2.npcData.Trusts.Add(trustRow2);

            Assert.AreEqual(1, npc.npcData.Trusts.Count);
            Assert.AreEqual(20, npc.npcData.Trusts[0].TrustLevel);

            Assert.AreEqual(1, npc2.npcData.Trusts.Count);
            Assert.AreEqual(80, npc2.npcData.Trusts[0].TrustLevel);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var obj in GameObject.FindObjectsOfType<NPCEchoes>())
            {
                Object.DestroyImmediate(obj.gameObject);
            }
        }
    }
}