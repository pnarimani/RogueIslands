namespace RogueIslands.Serialization
{
    public interface ICloner
    {
        T Clone<T>(T data);
        void CloneTo<T>(T source, T target) where T : class;
    }
}