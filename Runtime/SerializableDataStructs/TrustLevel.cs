namespace Echoes.Runtime.SerializableDataStructs
{
    [System.Serializable]
    public record TrustLevel
    {
        public string informantName;
        public double level;
    }
}