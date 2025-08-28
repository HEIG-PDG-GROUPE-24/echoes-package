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
