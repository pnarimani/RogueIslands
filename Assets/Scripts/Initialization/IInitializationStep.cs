using Cysharp.Threading.Tasks;

namespace RogueIslands.Initialization
{
    public interface IInitializationStep
    {
        UniTask Initialize();
    }
}