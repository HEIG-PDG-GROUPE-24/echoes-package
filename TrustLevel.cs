using UnityEngine;

[System.Serializable]
public record TrustLevel
{
    public string informantName;
    public double level;
}

[System.Serializable]
public record InformantsTrustLevels
{
    public TrustLevel[] trustLevels;
}