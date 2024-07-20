using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
using UnityEngine;

namespace RogueIslands.Gameplay.View.DeckBuilding
{
    public class ConsumableTargetSelection : MonoBehaviour
    {
        [SerializeField] private string _targetConsumable;
        [SerializeField] private List<BuildingSlot> Slots;

        public bool CanHandle(Consumable consumable)
            => consumable.Name == _targetConsumable;

        public IReadOnlyList<BuildingSlot> GetSlots() => Slots;

        public async UniTask PlaySubmitAnimation(Consumable consumable, List<Building> selectedBuildings)
        {
            var hasAnyAnimations = Slots.Any(s => s.ShouldFlipOnSubmit);

            if (!hasAnyAnimations)
            {
                PerformConversion(selectedBuildings, consumable);
                return;
            }

            var duration = 0.5f;

            foreach (var slot in Slots)
            {
                if (slot.ShouldFlipOnSubmit)
                {
                    slot.Transform.DORotate(new Vector3(0, 180, 0), duration)
                        .SetEase(Ease.InOutCubic)
                        .SetLoops(2, LoopType.Yoyo);
                }
            }

            await UniTask.WaitForSeconds(duration);

            PerformConversion(selectedBuildings, consumable);
            var buildingsState = GameManager.Instance.State.Buildings;
            foreach (var slot in Slots)
            {
                var view = slot.Transform.GetComponentInChildren<BuildingCardView>();
                if (view != null)
                    view.Initialize(buildingsState.Deck.Find(x => x.Id == view.Data.Id));
            }

            await UniTask.WaitForSeconds(duration);
        }

        private void PerformConversion(IReadOnlyList<Building> buildings, Consumable consumable)
        {
            StaticResolver.Resolve<DeckBuildingController>().ExecuteConsumable(consumable, buildings);
        }
    }
}