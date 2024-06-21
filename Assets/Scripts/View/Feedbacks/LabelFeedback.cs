using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace RogueIslands.View.Feedbacks
{
    [Serializable]
    public class LabelFeedback
    {
        [SerializeField] private Transform _labelParent;
        [SerializeField] private CanvasGroup _group;
        
        public async UniTask Play()
        {
            _group.alpha = 1;
            _labelParent.localScale = Vector3.zero;
            _labelParent.DOScale(2, 1f);
            _group.DOFade(0, 0.5f);
            await UniTask.WaitForSeconds(1);
        }
    }
}