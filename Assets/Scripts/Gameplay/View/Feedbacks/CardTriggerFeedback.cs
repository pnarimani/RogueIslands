using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RogueIslands.Gameplay.View.Feedbacks
{
    [Serializable]
    public class CardTriggerFeedback
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _duration = 0.5f;
        
        public async UniTask Play()
        {
            _target.DOKill();
            _target.DOPunchScale(Vector3.one * 0.2f, _duration)
                .OnComplete(() => _target.localScale = Vector3.one);
            _target.DOPunchRotation(new Vector3(0, 0, Random.Range(-5, 5)), _duration)
                .OnComplete(() => _target.localEulerAngles = Vector3.zero);
            await UniTask.WaitForSeconds(_duration);
        }
    }
}