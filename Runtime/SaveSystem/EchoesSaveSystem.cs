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

        internal SerializableNpcData NpcData = new ();


        public void Save()
        {
            var echoesNpcsGo = EchoesGlobal.GetAllNPCs(); // find all npcs

            NpcData.data = new EchoesNpcData[echoesNpcsGo.Count];
            for (int i = 0; i < echoesNpcsGo.Count; i++)
                NpcData.data[i] = new EchoesNpcData(echoesNpcsGo[i]); // extracts data to save from npc

            Write(); // write implementation in children
        }

        public void Load()
        {
            Read(); // read implementation in children

            var echoesNpcsGo = EchoesGlobal.GetAllNPCs(); // find all npcs

            var npcByname = echoesNpcsGo.ToDictionary(npc => npc.name);

            foreach (var npcdata in NpcData.data)
                npcByname[npcdata.name].LoadFromData(npcdata);
        }
    }

}