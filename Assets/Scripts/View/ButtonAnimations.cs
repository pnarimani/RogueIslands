using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.View
{
    public class ButtonAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOKill();
            transform.DOScale(1.1f, 0.1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOKill();
            transform.DOScale(1f, 0.1f);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
