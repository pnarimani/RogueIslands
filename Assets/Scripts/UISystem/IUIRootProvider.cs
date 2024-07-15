using UnityEngine;

namespace RogueIslands.UISystem
{
    public interface IUIRootProvider
    {
        Transform GetRoot(UILayer layer);
    }
}