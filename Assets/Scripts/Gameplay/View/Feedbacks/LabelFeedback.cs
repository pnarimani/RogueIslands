using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RogueIslands.Gameplay.View.Feedbacks
{
    [Serializable]
    public class LabelFeedback : MonoBehaviour
    {
        [SerializeField] private Transform _labelParent;
        [SerializeField] private Transform[] _backgrounds;
        [SerializeField] private CanvasGroup _group;

        private void Awake()
        {
            _group.alpha = 0;
        }

        public async UniTask Play()
        {
            _labelParent.DOComplete();
            _group.DOComplete();
            foreach (var bg in _backgrounds)
            {
                bg.DOComplete();
                bg.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                bg.localScale = Vector3.one;
                bg.DORotateQuaternion(Quaternion.Euler(0, 0, Random.Range(0, 360)), 5)
                    .SetSpeedBased()
                    .SetEase(Ease.Linear)
                    .SetLink(gameObject);
                bg.DOScale(Random.Range(2f, 3f), 1f)
                    .SetSpeedBased()
                    .SetEase(Ease.Linear)
                    .SetLink(gameObject);
            }

            _group.alpha = 0;
            _group.DOFade(1, 0.1f).SetLink(gameObject);

            _labelParent.localScale = Vector3.one * 1.5f;
            _labelParent.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad).SetLink(gameObject);

            _group.DOFade(0, 0.05f).SetDelay(0.5f).SetLink(gameObject);

            await UniTask.WaitForSeconds(0.7f);
        }

        public void SetText(string text)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = text;
        }

        public void Show()
        {
            _group.alpha = 1;
        }
    }
}