using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.View
{
    public class DescriptionBoxSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Transform _descriptionBoxParent;
        [SerializeField] private DescriptionBox _descriptionBoxPrefab;
        
        private DescriptionBox _descriptionBoxInstance;
        
        private IDescribableItem _describableItem;

        public void Initialize(IDescribableItem describableItem)
        {
            _describableItem = describableItem;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (_descriptionBoxInstance == null)
            {
                _descriptionBoxInstance = Instantiate(_descriptionBoxPrefab, _descriptionBoxParent);
                _descriptionBoxInstance.SetDescription(_describableItem.Description.Get(_describableItem));
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (_descriptionBoxInstance != null)
            {
                Destroy(_descriptionBoxInstance.gameObject);
                _descriptionBoxInstance = null;
            }
        }
    }
}