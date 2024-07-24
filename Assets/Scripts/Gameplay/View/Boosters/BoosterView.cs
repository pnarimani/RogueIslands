using Cysharp.Threading.Tasks;
using RogueIslands.Gameplay.Boosters;
using UnityEngine;
using UnityEngine.Assertions;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterView : MonoBehaviour, IBoosterView
    {
        public IBooster Data { get; private set; }

        private void Awake()
        {
        }

        public async void Remove()
        {
            await UniTask.WaitForSeconds(AnimationScheduler.GetTotalTime());

            Destroy(gameObject);
            GameUI.Instance.RefreshDate();
            GameUI.Instance.RefreshMoney();
        }

        public IBoosterScoreVisualizer GetScoringVisualizer()
        {
            return GetComponent<IBoosterScoreVisualizer>();
        }

        public void ShowRetriggerEffect()
        {
            GetComponent<BoosterRetriggerVisualizer>().Play().Forget();
        }

        public IBoosterScalingVisualizer GetScalingVisualizer()
        {
            return GetComponent<IBoosterScalingVisualizer>();
        }

        public IBoosterMoneyVisualizer GetMoneyVisualizer()
        {
            return GetComponent<IBoosterMoneyVisualizer>();
        }

        public void BoosterReset()
        {
            GetComponent<BoosterResetVisualizer>().Play().Forget();
        }

        public void Initialize(IBooster booster)
        {
            Assert.IsNotNull(booster);

            Data = booster;
            GetComponent<DescriptionBoxSpawner>().Initialize(booster);
        }
    }
}