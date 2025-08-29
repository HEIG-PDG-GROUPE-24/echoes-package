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
        
        [InlineEditor]
        public NPC npcData;

        public Dictionary<string,double> OpinionOfPlayer { private set; get; } =  new Dictionary<string, double>();
        public Dictionary<string, double> Personality {private set; get;} = new Dictionary<string, double>();
        public Dictionary<string, double> InformantsTrust {private set; get; } = new Dictionary<string, double>();
        public HashSet<NPCEchoes> Contacts { private set; get; } = new HashSet<NPCEchoes>();

        public bool InPlayerInteraction { private set; get; }
        public bool AcceptsInterferenceDuringInteraction {private set; get;}
        
        /**
         * Loads npc state from serializable data
         * @param data serializable data to load
         */
        public void LoadFromData(EchoesNpcData data)
        {
            if(npcData != null && npcData.name != data.name)
                throw new ArgumentException("Name of NPC doesn't match");

            bool dataIsComplete = true; 
            
            OpinionOfPlayer = new Dictionary<string, double>();
            if (data.opinionOfPlayer != null)
                foreach (var trait in data.opinionOfPlayer)
                    OpinionOfPlayer.Add(trait.traitName, trait.value);
            else dataIsComplete = false;

            Personality = new Dictionary<string, double>();
            if(data.npcPersonality != null)
                foreach (var trait in data.npcPersonality)
                    Personality.Add(trait.traitName, trait.value);
            else dataIsComplete = false;
            
            InformantsTrust =  new Dictionary<string, double>();
            if(data.trustLevels != null)
                foreach (var trust in data.trustLevels)
                    InformantsTrust.Add(trust.informantName,trust.level);
            else dataIsComplete = false;

            if (!dataIsComplete)
                throw new ArgumentException("Data is incomplete");
        }

        /**
         * Set npc state to default, IE the state defined in the editor and stored in the SO file.
         */
        public void LoadFromSo()
        {
            OpinionOfPlayer = new Dictionary<string, double>();
            foreach (var trait in npcData.Opinions)
                OpinionOfPlayer.Add(trait.Name, trait.Intensity);

            Personality = new Dictionary<string, double>();
            foreach (var trait in npcData.Traits)
                Personality.Add(trait.Name, trait.Intensity);
            
            InformantsTrust =  new Dictionary<string, double>();
            foreach (var trust in npcData.Trusts)
                InformantsTrust.Add(trust.Contact.name,trust.TrustLevel);
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
            if (x < min || x > max) throw new ArgumentOutOfRangeException(nameof(x),x,"value must be between min and max inclusive");
            return (x - min) / (max - min) * 2 - 1;
        }
        
        public double OpinionOfPlayerRegarding(string traitName)
        {
            return OpinionOfPlayer[traitName];
        }

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
                double maxDiff = 1; // max - min
                double diff = Math.Abs(OpinionOfPlayer[trait] - Personality[trait]);
                score -= Normalize(diff,0,maxDiff) / Personality.Count;
            }
            return score;
        }

        public bool StartPlayerInteraction(bool allowInterference = false)
        {
            if (InPlayerInteraction) return false;
            AcceptsInterferenceDuringInteraction = allowInterference;
            InPlayerInteraction = true;
            return true;
        }

        public void EndPlayerInteraction()
        {
            foreach (var contact in Contacts)
            {
                contact.ReceiveOpinion(this);
            }
        }

        protected bool ReceiveOpinion(NPCEchoes from)
        {
            if (InPlayerInteraction && !AcceptsInterferenceDuringInteraction) return false;
            //adjust current opinion
            double trustLevel = TrustTowards(from);
            foreach (var trait in OpinionOfPlayer.Keys.ToList())
            {
                double average = (OpinionOfPlayer[trait] + from.OpinionOfPlayer[trait]) / 2;
                double diff = average - OpinionOfPlayer[trait];
                OpinionOfPlayer[trait] += diff * ((Normalize(trustLevel, 0, 10)+1)/2);
            }
            
            return true;
        }
        
        
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        public override string ToString() => npcData != null ? npcData.Name : "No NPC Data";
    }
}
