using Cysharp.Threading.Tasks;
using RogueIslands.Gameplay.Boosters;
using UnityEngine;
using UnityEngine.Assertions;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterView : MonoBehaviour, IBoosterView
    {
        public IBooster Data { get; private set; }

        public async void Remove()
        {
            await UniTask.WaitForSeconds(AnimationScheduler.GetTotalTime());

            Destroy(gameObject);
            GameUI.Instance.RefreshDate();
            GameUI.Instance.RefreshMoney();
        }

        public IBoosterScoreVisualizer GetScoringVisualizer() => GetComponent<IBoosterScoreVisualizer>();

        public IBoosterScalingVisualizer GetScalingVisualizer() => GetComponent<IBoosterScalingVisualizer>();

        public IBoosterMoneyVisualizer GetMoneyVisualizer() => GetComponent<IBoosterMoneyVisualizer>();

        public IBoosterResetVisualizer GetResetVisualizer() => GetComponent<IBoosterResetVisualizer>();

        public IBoosterRetriggerVisualizer GetRetriggerVisualizer() => GetComponent<BoosterRetriggerVisualizer>();

        public void Initialize(IBooster booster)
        {
            Assert.IsNotNull(booster);

            Data = booster;
            GetComponent<DescriptionBoxSpawner>().Initialize(booster);
        }
    }
}