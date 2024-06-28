namespace RogueIslands.DependencyInjection
{
    public interface IModule
    {
        void Load(IContainerBuilder builder);
    }
}