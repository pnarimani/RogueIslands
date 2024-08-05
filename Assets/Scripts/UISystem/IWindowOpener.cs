using Cysharp.Threading.Tasks;

namespace RogueIslands.UISystem
{
    public interface IWindowOpener
    {
        T Open<T>(UILayer layer = default);
        UniTask<T> OpenAsync<T>(UILayer layer = default);
    }
}