namespace RogueIslands
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}