using System;
using System.Linq;

namespace Echoes.Runtime.SerializableDataStructs
{

    [Serializable]
    public record EchoesNpcData
    {
        public string name;

        public TraitValue[] opinionOfPlayer;
        public TraitValue[] npcPersonality;

        public TrustLevel[] trustLevels;

        public EchoesNpcData(NPCEchoes npc, bool enforceCompleteData = true)
        {
            if (enforceCompleteData && (
                    npc.OpinionOfPlayer == null ||
                    npc.Personality == null ||
                    npc.InformantsTrust == null
                ))
                throw new ArgumentException("Npc is incomplete, objects are missing", nameof(npc));
            
            
            if (npc.OpinionOfPlayer != null)
            {
                opinionOfPlayer = new TraitValue[npc.OpinionOfPlayer.Count];
                int i = 0;
                foreach (var trait in npc.OpinionOfPlayer.Keys)
                {
                    opinionOfPlayer[i++] = new TraitValue
                    {
                        traitName = trait,
                        value = npc.OpinionOfPlayer[trait]
                    };
                }
            }
            
            if (npc.Personality != null)
            {
                npcPersonality = new TraitValue[npc.Personality.Count];
                int i = 0;
                foreach (var trait in npc.Personality.Keys)
                {
                    npcPersonality[i++] = new TraitValue
                    {
                        traitName = trait,
                        value = npc.Personality[trait]
                    };
                }
            }
            
            if (npc.InformantsTrust != null)
            {
                trustLevels = new TrustLevel[npc.InformantsTrust.Count];
                int i = 0;
                foreach (var informantName in npc.InformantsTrust.Keys)
                {
                    trustLevels[i++] = new TrustLevel
                    {
                        informantName = informantName,
                        level = npc.InformantsTrust[informantName]
                    };
                }
            }
        }

    }
}
    