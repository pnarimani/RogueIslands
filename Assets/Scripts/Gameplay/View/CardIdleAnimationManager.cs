using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class CardIdleAnimationManager : MonoBehaviour
    {
        private void Update()
        {
            for (var i = 0; i < CardIdleAnimation.Instances.Count; i++)
            {
                var cardIdleAnimation = CardIdleAnimation.Instances[i];
                cardIdleAnimation.transform.localPosition = GetPositionAnimation(i);
                cardIdleAnimation.transform.localRotation = GetRotationAnimation(i);
            }
        }
        
        private Vector2 GetPositionAnimation(int index)
            => new(0, Mathf.Sin(Time.time * 2 + index * 0.5f) * 4);

        private Quaternion GetRotationAnimation(int i)
        {
            return Quaternion.Euler(0, 0, Mathf.Sin(Time.time * 1 + Mathf.Pow(i, 3) * 0.5f) * 1);
        }
    }
}