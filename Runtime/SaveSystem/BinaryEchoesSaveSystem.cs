using System.IO;
using UnityEngine;

namespace Echoes.Runtime.SaveSystem
{

    public class BinaryEchoesSaveSystem : EchoesSaveSystem
    {
        private readonly System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _formatter =
            new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

        public string SaveDirectory { set; get; } = Application.persistentDataPath;
        private const string FILENAME = "Echoes.bin";

        protected internal override void Write()
        {
            using FileStream os = File.Open(Path.Combine(SaveDirectory, FILENAME), FileMode.Create);
            _formatter.Serialize(os, NpcData);
        }

        protected internal override void Read()
        {
            using FileStream os = File.Open(Path.Combine(SaveDirectory, FILENAME), FileMode.Open);
            NpcData = (SerializableNpcData)_formatter.Deserialize(os);
        }
    }

}