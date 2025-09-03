using Echoes.Runtime;
using Echoes.Runtime.ScriptableObjects;
using NUnit.Framework;
using UnityEngine;
using Debug = UnityEngine.Debug;
namespace Tests.Play
{
    public class ComponentNpcTests
    {
        private EchoesNpcComponent _echoesNpc;
        private EchoesNpcComponent npc2;
        private GlobalStats globalStats;

        [SetUp]
        public void Setup()
        {
            _echoesNpc = new GameObject().AddComponent<EchoesNpcComponent>();
            _echoesNpc.npcData = ScriptableObject.CreateInstance<NPC>();
            _echoesNpc.npcData.Name = "TestNPCData";

            npc2 = new GameObject().AddComponent<EchoesNpcComponent>();
            npc2.npcData = ScriptableObject.CreateInstance<NPC>();
            npc2.npcData.Name = "TestNPCData2";
            
            globalStats = GlobalStats.Instance;
            
            globalStats.globalTraits.Traits.Clear();
            globalStats.globalTraits.Traits.Add(new GlobalTraitsRow("test"));
        }

        [Test]
        public void ContactAddedSuccessfully()
        {
            _echoesNpc.npcData.Contacts.Clear();
            _echoesNpc.npcData.Contacts.Add(npc2);

            Assert.AreEqual(1, _echoesNpc.npcData.Contacts.Count);
            Assert.AreEqual("TestNPCData2", _echoesNpc.npcData.Contacts[0].npcData.Name);
        }

        [Test]
        public void ContactRemovedSuccessfully()
        {
            _echoesNpc.npcData.Contacts.Add(npc2);
            _echoesNpc.npcData.Contacts.Remove(npc2);

            Assert.AreEqual(0, _echoesNpc.npcData.Contacts.Count);
        }
        
        [Test]
        public void TrustRowInitialization()
        {
            _echoesNpc.npcData.Trusts.Clear();
            var trustRow = new TrustRow
            {
                Contact = npc2,
                current = _echoesNpc.npcData,
                Min = -100,
                Max = 100,
                TrustLevel = 0
            };
            _echoesNpc.npcData.Trusts.Add(trustRow);

            Assert.AreEqual(1, _echoesNpc.npcData.Trusts.Count);
            Assert.AreEqual("TestNPCData2", _echoesNpc.npcData.Trusts[0].Contact.npcData.Name);
            Assert.AreEqual(0, _echoesNpc.npcData.Trusts[0].TrustLevel);
        }
        
        [Test]
        public void TraitRowInitialization()
        {
            _echoesNpc.npcData.Traits.Clear();
            var traitRow = new TraitsRow
            {
                Name = "Bravery",
                Min = 0,
                Max = 100,
                Intensity = 50
            };
            _echoesNpc.npcData.Traits.Add(traitRow);

            Assert.AreEqual(1, _echoesNpc.npcData.Traits.Count);
            Assert.AreEqual("Bravery", _echoesNpc.npcData.Traits[0].Name);
            Assert.AreEqual(50, _echoesNpc.npcData.Traits[0].Intensity);
        }
        
        [Test]
        public void OpinionRowInitialization()
        {
            _echoesNpc.npcData.Opinions.Clear();
            var opinionRow = new TraitsRow
            {
                Name = "Trustworthiness",
                Min = 0,
                Max = 100,
                Intensity = 75
            };
            _echoesNpc.npcData.Opinions.Add(opinionRow);

            Assert.AreEqual(1, _echoesNpc.npcData.Opinions.Count);
            Assert.AreEqual("Trustworthiness", _echoesNpc.npcData.Opinions[0].Name);
            Assert.AreEqual(75, _echoesNpc.npcData.Opinions[0].Intensity);
        }
        
        [Test]
        public void SyncWithGlobalTraitsAddsMissingTraits()
        {
            var globalTrait = globalStats.globalTraits.Traits[0];

            _echoesNpc.npcData.Traits.Clear();
            _echoesNpc.npcData.SyncWithGlobalTraits();

            Assert.IsTrue(_echoesNpc.npcData.Traits.Exists(t => t.Name == globalTrait.Name));
        }
        
        [Test]
        public void SyncWithGlobalTraitsAddsMissingOpinions()
        {
            // Assuming GlobalStats.Instance.globalTraits has at least one trait
            var globalTrait = globalStats.globalTraits.Traits[0];

            _echoesNpc.npcData.Opinions.Clear();
            _echoesNpc.npcData.SyncWithGlobalTraits();

            Assert.IsTrue(_echoesNpc.npcData.Opinions.Exists(t => t.Name == globalTrait.Name));
        }
        
        [Test]
        public void SyncWithGlobalTraitsDoesNotDuplicateExistingTraits()
        {
            var globalTrait = globalStats.globalTraits.Traits[0];

            _echoesNpc.npcData.Traits.Clear();
            _echoesNpc.npcData.Traits.Add(new TraitsRow { Name = globalTrait.Name, Intensity = 50, Min = 0, Max = 100 });
            _echoesNpc.npcData.SyncWithGlobalTraits();
            
            _echoesNpc.npcData.Traits.ForEach(t => Debug.Log(t.Name));
            int count = _echoesNpc.npcData.Traits.FindAll(t => t.Name == globalTrait.Name).Count;
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void SyncWithGlobalTraitsDoesNotDuplicateExistingOpinions()
        {
            var globalTrait = globalStats.globalTraits.Traits[0];

            _echoesNpc.npcData.Opinions.Clear();
            _echoesNpc.npcData.Opinions.Add(new TraitsRow { Name = globalTrait.Name, Intensity = 50, Min = 0, Max = 100 });
            _echoesNpc.npcData.SyncWithGlobalTraits();

            int count = _echoesNpc.npcData.Opinions.FindAll(t => t.Name == globalTrait.Name).Count;
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void ResetTrustSetsTrustLevelToMidpoint()
        {
            var trustRow = new TrustRow
            {
                current = _echoesNpc.npcData,
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
            _echoesNpc.npcData.Traits.Clear();
            npc2.npcData.Traits.Clear();

            var traitRow1 = new TraitsRow
            {
                Name = "Bravery",
                Min = 0,
                Max = 100,
                Intensity = 70
            };
            _echoesNpc.npcData.Traits.Add(traitRow1);

            var traitRow2 = new TraitsRow
            {
                Name = "Cautiousness",
                Min = 0,
                Max = 100,
                Intensity = 30
            };
            npc2.npcData.Traits.Add(traitRow2);

            Assert.AreEqual(1, _echoesNpc.npcData.Traits.Count);
            Assert.AreEqual("Bravery", _echoesNpc.npcData.Traits[0].Name);
            Assert.AreEqual(70, _echoesNpc.npcData.Traits[0].Intensity);

            Assert.AreEqual(1, npc2.npcData.Traits.Count);
            Assert.AreEqual("Cautiousness", npc2.npcData.Traits[0].Name);
            Assert.AreEqual(30, npc2.npcData.Traits[0].Intensity);
        }
        
        [Test]
        public void TrustedContactCanHaveDifferentLevels()
        {
            _echoesNpc.npcData.Trusts.Clear();
            var trustRow1 = new TrustRow
            {
                Contact = npc2,
                current = _echoesNpc.npcData,
                Min = -100,
                Max = 100,
                TrustLevel = 20
            };
            _echoesNpc.npcData.Trusts.Add(trustRow1);

            npc2.npcData.Trusts.Clear();
            var trustRow2 = new TrustRow
            {
                Contact = _echoesNpc,
                current = npc2.npcData,
                Min = -100,
                Max = 100,
                TrustLevel = 80
            };
            npc2.npcData.Trusts.Add(trustRow2);

            Assert.AreEqual(1, _echoesNpc.npcData.Trusts.Count);
            Assert.AreEqual(20, _echoesNpc.npcData.Trusts[0].TrustLevel);

            Assert.AreEqual(1, npc2.npcData.Trusts.Count);
            Assert.AreEqual(80, npc2.npcData.Trusts[0].TrustLevel);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var obj in Object.FindObjectsByType<EchoesNpcComponent>(FindObjectsSortMode.None))
            {
                Object.Destroy(obj.gameObject);
            }
        }
    }
}