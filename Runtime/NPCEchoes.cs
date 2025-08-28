using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes
{
    public class NPCEchoes : MonoBehaviour
    {

        [InlineEditor] public NPCSo npcData;

        public Dictionary<string,double> OpinionOfPlayer { private set; get; }
        private Dictionary<string, double> personality;
        private Dictionary<string, double> informantsTrust;

        public bool InPlayerInteraction { private set; get; }
        public bool AcceptsInterferenceDuringInteraction {private set; get;}

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
            return informantsTrust[other.npcData.name];
        }

        /**
         * @return an appreciation score of the player between -1 and 1 inclusive
         */
        public double AppreciationOfPlayer()
        {
            double score = 0;
            foreach (var trait in personality.Keys)
            {
                double maxDiff = 10; // max - min
                double diff = Math.Abs(OpinionOfPlayer[trait] - personality[trait]);
                score += Normalize(diff,0,maxDiff) / personality.Count;
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
            // For each contact, give them our opinion of the player
        }

        protected bool ReceiveOpinion(NPCEchoes from)
        {
            if (InPlayerInteraction && !AcceptsInterferenceDuringInteraction) return false;
            //adjust current opinion
            double trustLevel = TrustTowards(from);
            foreach (var trait in OpinionOfPlayer.Keys)
            {
                double average = OpinionOfPlayer[trait] + from.OpinionOfPlayer[trait];
                double diff = OpinionOfPlayer[trait] - average;
                OpinionOfPlayer[trait] += diff * Normalize(trustLevel, 0, 10);
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
    }
}
