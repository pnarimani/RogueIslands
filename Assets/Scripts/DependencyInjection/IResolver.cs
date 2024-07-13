namespace RogueIslands.DependencyInjection
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}