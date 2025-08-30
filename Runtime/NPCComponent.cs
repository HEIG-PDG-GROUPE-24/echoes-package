using System;
using System.Collections.Generic;
using System.Linq;
using Echoes.Runtime.SerializableDataStructs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes.Runtime
{
    public class NPCEchoes : MonoBehaviour
    {
        [InlineEditor] public NPC npcData;

        private Dictionary<string, double> _opinionOfPlayer = new();

        /**
         * @param traitName name of the trait to fetch value for
         * @return value corresponding to this trait
         */
        public double GetOpinionOfPlayer(string traitName)
        {
            return _opinionOfPlayer[traitName];
        }

        public void SetOpinionOfPlayer(string traitName, double opinion)
        {
            if (!IsValidTraitValue(opinion))
                throw new ArgumentOutOfRangeException(nameof(opinion), opinion,
                    "Should be between minimum and maximum inclusive values for traits");

            _opinionOfPlayer[traitName] = opinion;
        }

        public void AddToOpinionOfPlayer(string traitName, double value)
        {
            value += _opinionOfPlayer[traitName];
            
            if (!IsValidTraitValue(value))
                throw new ArgumentOutOfRangeException(nameof(value), value,
                    "Should be between minimum and maximum inclusive values for traits");
            
            _opinionOfPlayer[traitName] = value;
        }

        public List<string> GetPlayerOpinionTraits()
        {
            return _opinionOfPlayer.Keys.ToList();
        }

        public Dictionary<string, double> Personality { private set; get; } = new();
        public Dictionary<string, double> InformantsTrust { private set; get; } = new();
        public HashSet<NPCEchoes> Contacts { private set; get; } = new();
        private Dictionary<string, double> _lastInformantInfluence = new();

        public bool InPlayerInteraction { private set; get; } = false;
        public bool AcceptsInterferenceDuringInteraction { private set; get; } = false;
        private double _appreciationOfPlayerAtStartOfInteraction;

        /**
         * Loads npc state from serializable data
         * @param data serializable data to load
         */
        public void LoadFromData(EchoesNpcData data)
        {
            if (npcData != null && npcData.name != data.name)
                throw new ArgumentException("Name of NPC doesn't match");

            bool dataIsComplete = true;

            _opinionOfPlayer.Clear();
            if (data.opinionOfPlayer != null)
                foreach (var trait in data.opinionOfPlayer)
                    _opinionOfPlayer.Add(trait.traitName, trait.value);
            else dataIsComplete = false;

            Personality.Clear();
            if (data.npcPersonality != null)
                foreach (var trait in data.npcPersonality)
                    Personality.Add(trait.traitName, trait.value);
            else dataIsComplete = false;

            InformantsTrust.Clear();
            if (data.trustLevels != null)
                foreach (var trust in data.trustLevels)
                    InformantsTrust.Add(trust.informantName, trust.level);
            else dataIsComplete = false;

            if (!dataIsComplete)
                throw new ArgumentException("Data is incomplete");
        }

        /**
         * Set npc state to default, IE the state defined in the editor and stored in the SO file.
         * It also combines contacts and group members into a single Contacts Set
         */
        public void LoadFromSo()
        {
            _opinionOfPlayer.Clear();
            foreach (var trait in npcData.Opinions)
                _opinionOfPlayer.Add(trait.Name, trait.Intensity);

            Personality.Clear();
            foreach (var trait in npcData.Traits)
                Personality.Add(trait.Name, trait.Intensity);

            InformantsTrust.Clear();
            foreach (var trust in npcData.Trusts)
                InformantsTrust.Add(trust.Contact.name, trust.TrustLevel);

            Contacts.Clear();
            foreach (var contact in npcData.Contacts)
            {
                Contacts.Add(contact);
            }

            _lastInformantInfluence.Clear();

            foreach (var group in NPCGlobalStatsGeneratorSo.Instance.globalGroupes.Groupes.Where(group =>
                         group.Members.Contains(this)))
            {
                foreach (var member in group.Members.Where(member => member != this)) Contacts.Add(member);
            }
        }

        /**
         * @param x number to normalize
         * @param min minimum expected value of x
         * @param max maximum expected value of x
         * @return value of x normalized between -1.0 and 1.0
         * @throws ArgumentOutOfRangeException
         */
        private double Normalize(double x, double min, double max)
        {
            if (x < min || x > max)
                throw new ArgumentOutOfRangeException(nameof(x), x, "value must be between min and max inclusive");
            return (x - min) / (max - min) * 2 - 1;
        }

        /**
         * @param other
         * @return trust value of this npc towards other
         */
        public double TrustTowards(NPCEchoes other)
        {
            return InformantsTrust[other.npcData.name];
        }

        /**
         * @return an appreciation score of the player between -1 and 1 inclusive
         */
        public double AppreciationOfPlayer()
        {
            double score = 0;
            foreach (var trait in Personality.Keys)
            {
                double maxDiff = NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue -
                                 NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue;
                double diff = Math.Abs(_opinionOfPlayer[trait] - Personality[trait]);
                score -= Normalize(diff, 0, maxDiff) / Personality.Count;
            }

            return score;
        }

        /**
         * Indicates the start of an interaction between the player and this npc. This interaction can be of any nature and any length of time.
         * @param allowInterference whether the npc can receive an opinion from another during the interaction
         * @return true if the interaction started, false if one is already going
         */
        public bool StartPlayerInteraction(bool allowInterference = false)
        {
            if (InPlayerInteraction) return false;
            AcceptsInterferenceDuringInteraction = allowInterference;
            InPlayerInteraction = true;
            _appreciationOfPlayerAtStartOfInteraction = AppreciationOfPlayer();
            return true;
        }

        /**
         * Indicates the end of an interaction between the player and this npc, of any nature or length of time.
         * Calling this method also triggers the propagation of the newly found opinion of that npc
         */
        public void EndPlayerInteraction()
        {
            double appreciationDiff = AppreciationOfPlayer() - _appreciationOfPlayerAtStartOfInteraction;
            foreach (var informant in _lastInformantInfluence.Keys.ToList())
            {
                InformantsTrust[informant] += (appreciationDiff * _lastInformantInfluence[informant]) *
                                              (NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue -
                                               NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue);
            }

            foreach (var contact in Contacts)
            {
                contact.ReceiveOpinion(this);
            }
        }

        /**
         * Combines this npc's current opinion of the player with the other's based on the trust value toward that informant.
         * @param from npc giving their opinion
         * @return whether the npc received the opinion.
         */
        protected bool ReceiveOpinion(NPCEchoes from)
        {
            if (InPlayerInteraction && !AcceptsInterferenceDuringInteraction) return false;
            if (!InformantsTrust.ContainsKey(from.npcData.name))
                InformantsTrust.Add(from.npcData.name,
                    (NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue +
                     NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue) / 2);

            double startingPlayerAppreciation = AppreciationOfPlayer();

            //adjust current opinion
            double trustLevel = TrustTowards(from);
            foreach (var trait in _opinionOfPlayer.Keys.ToList())
            {
                double average = (_opinionOfPlayer[trait] + from._opinionOfPlayer[trait]) / 2;
                double diff = average - _opinionOfPlayer[trait];
                _opinionOfPlayer[trait] += diff * ((Normalize(trustLevel,
                    NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue,
                    NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue) + 1) / 2);
            }

            _lastInformantInfluence[from.npcData.name] = AppreciationOfPlayer() - startingPlayerAppreciation;

            return true;
        }

        public override string ToString() => npcData != null ? npcData.Name : "No NPC Data";
        
        private Boolean IsValidTraitValue(double value)
        {
            return value <= NPCGlobalStatsGeneratorSo.Instance.globalTraits.maxValue ||
                   value >= NPCGlobalStatsGeneratorSo.Instance.globalTraits.minValue;
        }
    }
}