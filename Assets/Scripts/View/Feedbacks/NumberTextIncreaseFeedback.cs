using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RogueIslands.View.Feedbacks
{
    [Serializable]
    public class NumberTextIncreaseFeedback
    {
        [SerializeField] private Transform _root;

        public void Play()
        {
            _root.DOKill();
            _root.DOPunchScale(Vector3.one * 0.2f, 0.1f)
                .OnComplete(() => _root.localScale = Vector3.one);
            _root.DOPunchRotation(new Vector3(0, 0, Random.Range(-5, 5)), 0.1f)
                .OnComplete(() => _root.localEulerAngles = Vector3.zero);
        }
    }
}