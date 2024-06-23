using System.Linq;
using Cysharp.Threading.Tasks;
using RogueIslands.Boosters.Actions;
using RogueIslands.Buildings;
using RogueIslands.GameEvents;
using RogueIslands.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.View.Boosters
{
    public class MoneyChangeVisualizer : BoosterActionVisualizer<ChangeMoneyAction>
    {
        [SerializeField] private LabelFeedback _money;

        protected override async UniTask OnAfterBoosterExecuted(GameState state, ChangeMoneyAction action,
            BoosterView booster)
        {
            Building responsibleBuilding = null;

            if (state.CurrentEvent is BuildingEvent buildingEvent)
                responsibleBuilding = buildingEvent.Building;

            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.2f);
            await UniTask.WaitForSeconds(wait);

            UniTask task;
            if (responsibleBuilding != null)
            {
                if (responsibleBuilding.Id.IsDefault())
                {
                    var view = FindObjectsOfType<BuildingCardView>().First(b => b.Data == responsibleBuilding);
                    task = view.BuildingMadeMoney(action.Change);
                }
                else
                {
                    var view = FindObjectsOfType<BuildingView>().First(b => b.Data == responsibleBuilding);
                    task = view.BuildingMadeMoney(action.Change);
                }
            }
            else
            {
                _money.SetText("$" + action.Change);
                task = _money.Play();
            }

            await UniTask.WaitForSeconds(0.2f);

            GameUI.Instance.MoneyBoosted(action.Change);

            await task;
        }
    }
}