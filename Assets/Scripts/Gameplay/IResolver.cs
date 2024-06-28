namespace RogueIslands.Gameplay
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}