using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Feedbacks
{
    [Serializable]
    public class PopupOpeningFeedback
    {
        [SerializeField] private Transform _bg;

        public void PlayOpening()
        {
            _bg.DOLocalMoveY(-Screen.height, 0.5f)
                .From()
                .SetEase(Ease.OutBack);
        }

        public UniTask PlayClosing()
        {
            return _bg.DOLocalMoveY(-Screen.height, 0.5f)
                .SetEase(Ease.InBack)
                .AwaitForComplete();
        }
    }
}