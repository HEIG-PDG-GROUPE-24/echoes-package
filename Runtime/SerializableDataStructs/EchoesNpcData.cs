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

            npcPersonality = new TraitValue[npc.GetPersonalityTraits().Count];
            i = 0;
            foreach (var trait in npc.GetPersonalityTraits())
            {
                npcPersonality[i++] = new TraitValue
                {
                    traitName = trait,
                    value = npc.GetPersonality(trait)
                };
            }

            trustLevels = new TrustLevel[npc.GetInformants().Count];
            i = 0;
            foreach (var informantName in npc.GetInformants())
            {
                trustLevels[i++] = new TrustLevel
                {
                    informantName = informantName,
                    level = npc.GetTrustTowards(informantName)
                };
            }
        }
    }
}