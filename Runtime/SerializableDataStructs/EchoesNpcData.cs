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

        public EchoesNpcData(NPCEchoes npc)
        {
            opinionOfPlayer = new TraitValue[npc.GetPlayerOpinionTraits().Count];
            int i = 0;
            foreach (var trait in npc.GetPlayerOpinionTraits())
            {
                opinionOfPlayer[i++] = new TraitValue
                {
                    traitName = trait,
                    value = npc.GetOpinionOfPlayer(trait)
                };
            }

            npcPersonality = new TraitValue[npc.Personality.Count];
            i = 0;
            foreach (var trait in npc.Personality.Keys)
            {
                npcPersonality[i++] = new TraitValue
                {
                    traitName = trait,
                    value = npc.Personality[trait]
                };
            }

            trustLevels = new TrustLevel[npc.InformantsTrust.Count];
            i = 0;
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