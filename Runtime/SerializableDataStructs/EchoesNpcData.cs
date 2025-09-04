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

        public EchoesNpcData(EchoesNpcComponent echoesNpc)
        {
            opinionOfPlayer = new TraitValue[echoesNpc.GetPlayerOpinionTraits().Count];
            int i = 0;
            foreach (var trait in echoesNpc.GetPlayerOpinionTraits())
            {
                opinionOfPlayer[i++] = new TraitValue
                {
                    traitName = trait,
                    value = echoesNpc.GetOpinionOfPlayer(trait)
                };
            }

            npcPersonality = new TraitValue[echoesNpc.GetPersonalityTraits().Count];
            i = 0;
            foreach (var trait in echoesNpc.GetPersonalityTraits())
            {
                npcPersonality[i++] = new TraitValue
                {
                    traitName = trait,
                    value = echoesNpc.GetPersonality(trait)
                };
            }

            trustLevels = new TrustLevel[echoesNpc.GetInformants().Count];
            i = 0;
            foreach (var informantName in echoesNpc.GetInformants())
            {
                trustLevels[i++] = new TrustLevel
                {
                    informantName = informantName,
                    level = echoesNpc.GetTrustTowards(informantName)
                };
            }

            name = echoesNpc.name;
        }
    }
}