using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public record EchoesNpcData
{
    public string name;
    
    public PersonalityData opinionOfPlayer;
    public PersonalityData npcPersonality;
    
    public InformantsTrustLevels trustLevels;

    public EchoesNpcData(EchoesNpc npc)
    {
        
    }
}