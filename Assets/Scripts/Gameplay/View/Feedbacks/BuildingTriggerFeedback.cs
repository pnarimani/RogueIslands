using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Feedbacks
{
    [Serializable]
    public class BuildingTriggerFeedback
    {
        [SerializeField] private Transform _target;

        public async UniTask Play()
        {
            _target.DOPunchRotation(new Vector3(0, 4, 0), 0.5f);
            _target.DOPunchScale(Vector3.one * 0.1f, 0.5f);
            await UniTask.WaitForSeconds(0.5f);
        }
    }
}