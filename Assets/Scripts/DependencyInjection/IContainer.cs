namespace RogueIslands.DependencyInjection
{
    public interface IContainer
    {
        T Resolve<T>();
    }
}