using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Echoes.Runtime.SerializableDataStructs;

[assembly: InternalsVisibleTo("PlaymodeTests")]

namespace Echoes.Runtime.SaveSystem
{
    [System.Serializable]
    public record SerializableNpcData
    {
        public EchoesNpcData[] data;
    }

    public abstract class EchoesSaveSystem
    {
        protected internal abstract void Write();
        protected internal abstract void Read();

        internal SerializableNpcData NpcData;


        public void Save()
        {
            EchoesNpcComponent[] echoesNpcsGo = Object.FindObjectsByType<EchoesNpcComponent>(FindObjectsSortMode.None); // find all npcs

            NpcData.data = new EchoesNpcData[echoesNpcsGo.Length];
            for (int i = 0; i < echoesNpcsGo.Length; i++)
                NpcData.data[i] = new EchoesNpcData(echoesNpcsGo[i]); // extracts data to save from npc

            Write(); // write implementation in children

            NpcData = null; // ref no longer necessary
        }

        public void Load()
        {
            Read(); // read implementation in children

            EchoesNpcComponent[] echoesNpcsGo = Object.FindObjectsByType<EchoesNpcComponent>(FindObjectsSortMode.None); // find all npcs

            var npcByname = echoesNpcsGo.ToDictionary(npc => npc.name);

            foreach (var npcdata in NpcData.data)
                npcByname[npcdata.name].LoadFromData(npcdata);

            NpcData = null; // ref no longer necessary
        }
    }

}