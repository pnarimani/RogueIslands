using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.Gameplay.View
{
    public class DescriptionBoxSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Transform _descriptionBoxParent;
        [SerializeField] private DescriptionBox _descriptionBoxPrefab;
        [SerializeField] private bool _showName;
        [SerializeField] private bool _growToBottom = true;
        
        private DescriptionBox _descriptionBoxInstance;

        private IDescribableItem _describableItem;

        public void Initialize(IDescribableItem describableItem)
        {
            _describableItem = describableItem;
        }
        
        private void OnDestroy()
        {
            if (_descriptionBoxInstance != null)
                Destroy(_descriptionBoxInstance.gameObject);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (_describableItem == null)
            {
                Debug.LogError("No describable item set on DescriptionBoxSpawner");
                return;
            }

            if (_descriptionBoxInstance == null)
            {
                _descriptionBoxInstance = Instantiate(
                    _descriptionBoxPrefab,
                    _descriptionBoxParent
                );
                _descriptionBoxInstance.SetDescription(_describableItem.Description.Get(_describableItem));

                if (_showName && _describableItem is INamedItem namedItem)
                    _descriptionBoxInstance.ShowName(namedItem.Name);

                if (_describableItem is IHasAlternateDescriptionTitle alternateDescriptionTitle)
                    _descriptionBoxInstance.ShowName(alternateDescriptionTitle.AlternateTitle);
                
                _descriptionBoxInstance.SetGrowToBottom(_growToBottom);
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