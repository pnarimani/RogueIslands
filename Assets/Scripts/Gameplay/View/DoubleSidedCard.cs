using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class DoubleSidedCard : MonoBehaviour
    {
        [SerializeField] private RectTransform _front, _back;

        private void Update()
        {
            _back.gameObject.SetActive(_back.rotation.eulerAngles.y is > 90 and < 270);
            _front.gameObject.SetActive(_back.rotation.eulerAngles.y is < 90 or > 270);
        }
    }
}
