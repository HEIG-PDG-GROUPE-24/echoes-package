using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trait
{
    public string name;
    public string lowValueLabel;
    public string highValueLabel;
}

[System.Serializable]
public class TraitValue
{
    public string traitName;
    public double value;
}

public class Personality : List<TraitValue>
{
    
}

[System.Serializable]
public record PersonalityData
{
    public TraitValue[] traitValues;
}