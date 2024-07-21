using System.Linq;
using Cysharp.Threading.Tasks;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;
using RogueIslands.Gameplay.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class MoneyChangeVisualizer : MonoBehaviour, IBoosterMoneyVisualizer
    {
        [SerializeField] private LabelFeedback _money;
        [SerializeField] private CardTriggerFeedback _cardTriggerFeedback;
        
        public async void Play(int money)
        {
            var state = GameManager.Instance.State;
            _cardTriggerFeedback.Play().Forget();
            Building responsibleBuilding = null;

            if (state.CurrentEvent is BuildingEvent buildingEvent)
                responsibleBuilding = buildingEvent.Building;

            if (responsibleBuilding == null || !responsibleBuilding.IsPlacedDown(state))
                AnimationScheduler.WaitForTotalTime();

            await AnimationScheduler.ScheduleAndWait(1, 0.1F);

            UniTask task;
            if (responsibleBuilding != null)
            {
                if (responsibleBuilding.IsPlacedDown(state))
                {
                    var view = ObjectRegistry.GetBuildings().First(b => b.Data == responsibleBuilding);
                    task = view.BuildingMadeMoney(money);
                }
                else
                {
                    var view = ObjectRegistry.GetBuildingCards().First(b => b.Data == responsibleBuilding);
                    task = view.BuildingMadeMoney(money);
                }
            }
            else
            {
                _money.SetText($"${money}");
                task = _money.Play();
            }

            await UniTask.WaitForSeconds(0.2f);

            GameUI.Instance.MoneyBoosted(money);

            await task;
        }
    }
}