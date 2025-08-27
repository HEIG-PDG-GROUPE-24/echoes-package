using UnityEngine;
using System.IO;

public class JsonEchoesSaveSystem : EchoesSaveSystem
{
    
    public string SaveDirectory{set;get;} = Application.persistentDataPath;
    const string FILENAME = "Echoes.json";
    
    protected internal override void Write()
    {
        File.WriteAllText(
            Path.Combine(SaveDirectory, FILENAME),
            JsonUtility.ToJson(NpcData)
        );
    }

    protected internal override void Read()
    {
        NpcData = JsonUtility.FromJson<SerializableNpcData>(
            File.ReadAllText(Path.Combine(SaveDirectory, FILENAME))
        );
    }
}