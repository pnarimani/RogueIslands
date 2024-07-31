using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.View.Commons;
using RogueIslands.UISystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.Gameplay.View.Descriptions
{
    public class DescriptionBoxSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private static readonly UILayer _overlayLayer = new("OverlayLayer");
        [SerializeField] private Transform _descriptionBoxParent;
        [SerializeField] private DescriptionBox _descriptionBoxPrefab;
        [SerializeField] private bool _enableAfterFirstMouseExit;
        [SerializeField] private DescriptionAlignment _horizontalAlignment = DescriptionAlignment.Center;
        [SerializeField] private DescriptionAlignment _verticalAlignment = DescriptionAlignment.Start;

        private DescriptionBox _descBox;
        private IDescribableItem _describableItem;
        private bool _hasSpawnedManually;
        private bool _isEnabled;
        private IUIRootProvider _uiRootProvider;

        private void OnDestroy()
        {
            if (_descBox != null)
                Destroy(_descBox.gameObject);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!_isEnabled)
                return;

            if (_describableItem == null)
                return;

            if (!_hasSpawnedManually)
                Spawn();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (!_hasSpawnedManually)
                HideManually();

            _isEnabled = true;
        }

        public void Initialize(IDescribableItem describableItem)
        {
            _uiRootProvider = StaticResolver.Resolve<IUIRootProvider>();
            _describableItem = describableItem;

            _isEnabled = !_enableAfterFirstMouseExit;
        }

        public void SpawnManually()
        {
            Spawn();
            _hasSpawnedManually = true;
        }

        private void Spawn()
        {
            if (_descBox == null)
            {
                var root = _uiRootProvider.GetRoot(_overlayLayer);
                _descBox = Instantiate(_descriptionBoxPrefab,
                    _descriptionBoxParent.position,
                    Quaternion.identity,
                    root
                );
                var remoteChild = _descBox.gameObject.AddComponent<RemoteChild>();
                remoteChild.SetParent(_descriptionBoxParent, Vector3.zero);
                _descBox.SetAlignment(_horizontalAlignment, _verticalAlignment);
                _descBox.SetDescription(GetDescriptionText());
                if(_describableItem is BoosterCard b)
                    _descBox.ShowRarity(b.Rarity);
                ShowName();
            }
        }

        private void ShowName()
        {
            if (_describableItem is INamedItem namedItem)
                _descBox.ShowName(namedItem.Name);
            else if (_describableItem is IHasAlternateDescriptionTitle alternateDescriptionTitle)
                _descBox.ShowName(alternateDescriptionTitle.AlternateTitle);
            else
                _descBox.HideName();
        }

        private string GetDescriptionText()
            => DescriptionTextProvider.Get(_describableItem);

        public void HideManually()
        {
            if (_descBox != null)
            {
                Destroy(_descBox.gameObject);
                _descBox = null;
            }

            _hasSpawnedManually = false;
        }
    }
}