using UnityEngine;
using UnityEngine.Serialization;

namespace Echoes.Runtime.SerializableDataStructs
{

    [System.Serializable]
    public record EchoesNpcData
    {
        public string name;

        public TraitValue[] opinionOfPlayer;
        public TraitValue[] npcPersonality;

        public TrustLevel[] trustLevels;

        public EchoesNpcData (NPCEchoes npc)
        {

        }
    }

}