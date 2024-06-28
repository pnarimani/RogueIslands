namespace RogueIslands.UISystem
{
    public interface IWindowOpener
    {
        T Open<T>(UILayer layer = default);
    }
}