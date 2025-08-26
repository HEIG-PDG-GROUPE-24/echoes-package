using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/**
 * System managing saves of runtime evolving data
 */

[System.Serializable]
public record SerializableNpcData
{
    public EchoesNpcData[] data;
}

public abstract class EchoesSaveSystem
{
    protected abstract void Write();
    protected abstract void Read();

    protected SerializableNpcData NpcData;
    
    public void Save()
    {
        EchoesNpc[] echoesNpcsGo = Object.FindObjectsByType<EchoesNpc>(FindObjectsSortMode.None); // find all npcs
        
        NpcData.data = new EchoesNpcData[echoesNpcsGo.Length];
        for(int i = 0; i < echoesNpcsGo.Length; i++)
            NpcData.data[i] = new EchoesNpcData(echoesNpcsGo[i]); // extracts data to save from npc
            
        Write(); // write implementation in children
        
        NpcData = null; // ref no longer necessary
    }

    public void Load()
    {
        Read(); // read implementation in children
        
        EchoesNpc[] echoesNpcsGo = Object.FindObjectsByType<EchoesNpc>(FindObjectsSortMode.None); // find all npcs
        
        var npcByname = echoesNpcsGo.ToDictionary(npc => npc.name);
        
        foreach(var npcdata in NpcData.data)
            npcByname[npcdata.name].LoadFromData(npcdata);
        
        NpcData = null; // ref no longer necessary
    }
}

public class JsonEchoesSaveSystem : EchoesSaveSystem
{
    
    public string SaveDirectory{set;get;} = Application.persistentDataPath;
    const string FILENAME = "Echoes.json";
    
    protected override void Write()
    {
        File.WriteAllText(
            Path.Combine(SaveDirectory, FILENAME),
            JsonUtility.ToJson(NpcData)
            );
    }

    protected override void Read()
    {
        NpcData = JsonUtility.FromJson<SerializableNpcData>(
            File.ReadAllText(Path.Combine(SaveDirectory, FILENAME))
            );
    }
}

public class BinaryEchoesSaveSystem : EchoesSaveSystem
{
    private readonly System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
    public string SaveDirectory{set;get;} = Application.persistentDataPath;
    private const string FILENAME = "Echoes.bin";
    
    protected override void Write()
    {
        using FileStream os = File.Open(Path.Combine(SaveDirectory,FILENAME), FileMode.Create);
        _formatter.Serialize(os, NpcData);
    }

    protected override void Read()
    {
        using FileStream os = File.Open(Path.Combine(SaveDirectory,FILENAME), FileMode.Create);
        NpcData = (SerializableNpcData)_formatter.Deserialize(os);
    }
}