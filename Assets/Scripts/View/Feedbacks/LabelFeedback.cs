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
            _labelParent.DOComplete();
            _group.DOComplete();
            
            _group.alpha = 0;
            _group.DOFade(1, 0.1f);
            
            _labelParent.localScale = Vector3.one * 1.5f;
            _labelParent.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad);
            
            _group.DOFade(0, 0.05f).SetDelay(0.5f);
            
            await UniTask.WaitForSeconds(0.7f);
        }
    }
}