using Sirenix.OdinInspector;
using UnityEngine;

namespace Echoes
{
    public class NPCEchoes : MonoBehaviour
    {

        [InlineEditor] public NPCSo npcData;

        public Dictionary<string,double> OpinionOfplayer { private set; get; }
        private Dictionary<string, double> personality;
        private Dictionary<string, double> informtantsTrust;

        public bool InPlayerInteraction { private set; get; }
        public bool AcceptsInterferenceDuringInteraction {private set; get;}
        
        public double OpinionOfPlayerRegarding(string traitName)
        {
            return OpinionOfPlayer[traitName];
        }

        public double TrustTowards(NPCEchoes other)
        {
            return informantsTrust[other.npcData.Name];
        }

        public double AppreciationOfPlayer()
        {
            // calculate a score based on current opinion and personality
        }

        public bool StartPlayerInteraction(bool allowInterference = false)
        {
            if InPlayerInteraction return false;
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
