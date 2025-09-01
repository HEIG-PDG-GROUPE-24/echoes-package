using Echoes.Runtime.SerializableDataStructs;
using Echoes.Runtime;
using Echoes.Runtime.ScriptableObjects;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Play
{
    public class NpcBehaviourTest
    {
        private GameObject _goNpc1;
        private GameObject _goNpc2;
        private EchoesNpcComponent _npc1;
        private EchoesNpcComponent _npc2;
        private const double Precision = 0.0001; 

        [SetUp]
        public void SetUp()
        {
            _goNpc1 = new GameObject();
            _goNpc2 = new GameObject();

            _npc1 = _goNpc1.AddComponent<EchoesNpcComponent>();
            _npc2 = _goNpc2.AddComponent<EchoesNpcComponent>();

            _npc1.npcData = ScriptableObject.CreateInstance<NPC>();
            _npc2.npcData = ScriptableObject.CreateInstance<NPC>();

            _npc1.npcData.name = "npc1";
            _npc2.npcData.name = "npc2";

            _npc1.SetPersonality("test",
                (GlobalStats.Instance.globalTraits.maxValue -
                 GlobalStats.Instance.globalTraits.minValue) * 0.5 +
                GlobalStats.Instance.globalTraits.minValue
            );
            _npc2.SetPersonality("test",
                (GlobalStats.Instance.globalTraits.maxValue -
                 GlobalStats.Instance.globalTraits.minValue) * 0.7 +
                GlobalStats.Instance.globalTraits.minValue
            );

            _npc1.SetOpinionOfPlayer("test",
                (GlobalStats.Instance.globalTraits.maxValue -
                 GlobalStats.Instance.globalTraits.minValue) * 0.3 +
                GlobalStats.Instance.globalTraits.minValue
            );
            _npc2.SetOpinionOfPlayer("test",
                (GlobalStats.Instance.globalTraits.maxValue -
                 GlobalStats.Instance.globalTraits.minValue) * 0.7 +
                GlobalStats.Instance.globalTraits.minValue
            );

            _npc1.AddContact(_npc2);
            _npc2.AddContact(_npc1);
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void OpinionReception()
        {
            _npc2.SetTrustTowards("npc1", GlobalStats.Instance.globalTrust.maxValue);
            _npc1.EndPlayerInteraction();
            Assert.AreEqual(
                (GlobalStats.Instance.globalTraits.maxValue -
                 GlobalStats.Instance.globalTraits.minValue) * 0.5 +
                GlobalStats.Instance.globalTraits.minValue,
                _npc2.GetOpinionOfPlayer("test"), Precision
            );
        }

        [Test]
        public void OpinionReceptionNoTrust()
        {
            _npc2.SetTrustTowards("npc1", GlobalStats.Instance.globalTrust.minValue);
            _npc1.EndPlayerInteraction();

            Assert.AreEqual(
                (GlobalStats.Instance.globalTraits.maxValue -
                 GlobalStats.Instance.globalTraits.minValue) * 0.7 +
                GlobalStats.Instance.globalTraits.minValue,
                _npc2.GetOpinionOfPlayer("test"), Precision
            );
        }

        [Test]
        public void PlayerScore1()
        {
            Assert.AreEqual(_npc1.AppreciationOfPlayer(), 0.6, Precision);
        }

        [Test]
        public void PlayerScore2()
        {
            Assert.AreEqual(_npc2.AppreciationOfPlayer(), 1, Precision);
        }

        [Test]
        public void InteractionState()
        {
            Assert.False(_npc1.InPlayerInteraction);

            _npc1.StartPlayerInteraction();
            Assert.True(_npc1.InPlayerInteraction);

            _npc1.EndPlayerInteraction();
            Assert.False(_npc2.InPlayerInteraction);
        }

        [Test]
        public void InteractionRefuseOpinion()
        {
            _npc1.StartPlayerInteraction();
            Assert.False(_npc1.ReceiveOpinion(_npc2));
        }

        [Test]
        public void AdjustingTrust()
        {
            _npc2.SetTrustTowards("npc1", GlobalStats.Instance.globalTrust.maxValue);

            // npc1 has an interaction with player and transmits their opinion to npc2
            Debug.LogFormat("{0} player score before receiving opinion : {1}", nameof(_npc2),
                _npc2.AppreciationOfPlayer());
            _npc1.EndPlayerInteraction();
            Debug.LogFormat("{0} player score after receiving opinion : {1}", nameof(_npc2),
                _npc2.AppreciationOfPlayer());

            // npc2 has an interaction with the player that modifies it's opinion
            Debug.LogFormat("{0} trust towards {1} before interaction : {2}", nameof(_npc2), nameof(_npc1),
                _npc2.GetTrustTowards(_npc1));
            Debug.LogFormat("{0} player score before interaction : {1}", nameof(_npc2), _npc2.AppreciationOfPlayer());
            _npc2.StartPlayerInteraction();
            _npc2.AddToOpinionOfPlayer("test",
                0.2 * (GlobalStats.Instance.globalTraits.maxValue -
                       GlobalStats.Instance.globalTraits.minValue)
            );
            _npc2.EndPlayerInteraction();

            // verify npc2 adjusted their trust towards npc1 accordingly
            Debug.LogFormat("{0} player score after interaction : {1}", nameof(_npc2), _npc2.AppreciationOfPlayer());
            Debug.LogFormat("{0} trust towards {1} after interaction : {2}", nameof(_npc2), nameof(_npc1),
                _npc2.GetTrustTowards(_npc1)
            );

            Assert.AreEqual(
                GlobalStats.Instance.globalTrust.maxValue +
                (GlobalStats.Instance.globalTrust.maxValue -
                 GlobalStats.Instance.globalTrust.minValue) * (-0.4 * +0.4),
                _npc2.GetTrustTowards(_npc1),
                Precision
            );
        }

        [Test]
        public void ObjectToData()
        {
            EchoesNpcData npcData = new EchoesNpcData(_npc1);
            AssertEqual(npcData, _npc1);
        }

        [Test]
        public void DataToObject()
        {
            EchoesNpcData npcData = new EchoesNpcData(new GameObject().AddComponent<EchoesNpcComponent>());
            EchoesNpcComponent echoesNpcObject = new GameObject().AddComponent<EchoesNpcComponent>();

            npcData.npcPersonality = new TraitValue[1];
            npcData.npcPersonality[0] = new TraitValue
            {
                traitName = "Trait",
                value = 5
            };

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