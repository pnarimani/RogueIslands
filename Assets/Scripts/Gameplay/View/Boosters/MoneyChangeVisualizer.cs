using System.Collections.Generic;
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

        private readonly List<LabelFeedback> _dryRunLabels = new();

        public async void Play(int money)
        {
            var state = GameManager.Instance.State;
            Building responsibleBuilding = null;

            if (state.CurrentEvent is BuildingEvent buildingEvent)
                responsibleBuilding = buildingEvent.Building;

            if (responsibleBuilding == null || !responsibleBuilding.IsPlacedDown(state))
                AnimationScheduler.WaitForTotalTime();

            await AnimationScheduler.ScheduleAndWait(1, 0.1F);

            _cardTriggerFeedback.Play().Forget();

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

        public void ShowDryRunMoney(Dictionary<int, int> moneyAndCount)
        {
            foreach (var (money, count) in moneyAndCount)
            {
                var label = Instantiate(_money, _money.transform.parent, true);
                label.SetText(count > 1 ? $"${money} x {count}" : $"${money}");
                label.Show();
                _dryRunLabels.Add(label);
            }
        }

        public void ShowDryRunProbability()
        {
            var label = Instantiate(_money, _money.transform.parent, true);
            label.SetText("???");
            label.Show();
            _dryRunLabels.Add(label);
        }

        public void HideDryRun()
        {
            foreach (var label in _dryRunLabels) 
                Destroy(label.gameObject);
            
            _dryRunLabels.Clear();
        }
    }
}