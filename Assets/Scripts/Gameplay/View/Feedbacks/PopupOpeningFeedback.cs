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
            _bg.DOLocalMoveY(-Screen.height,  AnimationScheduler.Scale(0.5f))
                .From()
                .SetEase(Ease.OutBack);
        }
        
        public UniTask PlayClosing()
        {
            var duration = AnimationScheduler.Scale(0.5f);
            _bg.DOLocalMoveY(-Screen.height, duration)
                .SetEase(Ease.InBack);
            return UniTask.WaitForSeconds(duration);
        }
    }
}