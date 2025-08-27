using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public record EchoesNpcData
{
    public string name;
    
    public TraitValue[] opinionOfPlayer;
    public TraitValue[] npcPersonality;
    
    public TrustLevel[] trustLevels;

    public EchoesNpcData(EchoesNpc npc)
    {
        
    }
}