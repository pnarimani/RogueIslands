using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public interface IHighlightable
    {
        // ReSharper disable once InconsistentNaming
        Transform transform { get; }
        void Highlight(bool highlight);
        void ShowRange(bool showRange);
    }
}