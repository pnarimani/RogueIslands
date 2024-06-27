namespace RogueIslands.UISystem
{
    public interface IWindowRegistry
    {
        string GetKey<T>() where T : IWindow;
    }
}