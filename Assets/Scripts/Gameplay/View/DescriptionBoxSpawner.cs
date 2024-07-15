using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.View.Commons;
using RogueIslands.UISystem;
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

        private static readonly UILayer _descriptionBoxLayer = new("DescriptionBoxLayer");

        private DescriptionBox _descBox;
        private IDescribableItem _describableItem;
        private IUIRootProvider _uiRootProvider;

        public void Initialize(IDescribableItem describableItem)
        {
            _uiRootProvider = StaticResolver.Resolve<IUIRootProvider>();
            _describableItem = describableItem;
        }

        private void OnDestroy()
        {
            if (_descBox != null)
                Destroy(_descBox.gameObject);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (_describableItem == null)
            {
                Debug.LogError("No describable item set on DescriptionBoxSpawner");
                return;
            }

            if (_descBox == null)
            {
                var root = _uiRootProvider.GetRoot(_descriptionBoxLayer);
                _descBox = Instantiate(_descriptionBoxPrefab,
                    _descriptionBoxParent.position,
                    Quaternion.identity,
                    root
                );
                var remoteChild = _descBox.gameObject.AddComponent<RemoteChild>();
                remoteChild.SetParent(_descriptionBoxParent, Vector3.zero);
                _descBox.SetGrowToBottom(_growToBottom);
                _descBox.SetDescription(GetDescriptionText());
                ShowName();
            }
        }

        private void ShowName()
        {
            if (_showName && _describableItem is INamedItem namedItem)
                _descBox.ShowName(namedItem.Name);

            if (_describableItem is IHasAlternateDescriptionTitle alternateDescriptionTitle)
                _descBox.ShowName(alternateDescriptionTitle.AlternateTitle);
        }

        private string GetDescriptionText()
        {
            return _describableItem.Description.Get(_describableItem);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (_descBox != null)
            {
                Destroy(_descBox.gameObject);
                _descBox = null;
            }
        }
    }
}