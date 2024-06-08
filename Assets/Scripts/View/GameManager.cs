﻿using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RogueIslands.Boosters;
using RogueIslands.Particles;
using RogueIslands.View.Shop;
using UnityEngine;

namespace RogueIslands.View
{
    public class GameManager : Singleton<GameManager>, IGameView
    {
        [SerializeField] private ShopScreen _shopPrefab;
        
        public GameState State { get; private set; }
        public bool IsPlaying { get; private set; }

        private void Start()
        {
            GameUI.Instance.PlayClicked += OnPlayClicked;

            State = GameFactory.NewGame();
            
            ShowBuildingsInHand();
            
            GameUI.Instance.RefreshAll();
        }

        private async void OnPlayClicked()
        {
            if (!State.CanPlay() || IsPlaying)
                return;

            IsPlaying = true;
            
            AnimationScheduler.ResetTime();
            
            State.Play(this);

            var timer = 0f;
            while (timer < AnimationScheduler.GetExtraTime())
            {
                await UniTask.DelayFrame(1);
                timer += Time.deltaTime;
            }

            var particleTargets = FindObjectsOfType<ParticleSystemTarget>();
            while (particleTargets.Any(t => t.IsTrackingParticles()))
            {
                await UniTask.DelayFrame(1);
            }
            
            GameUI.Instance.RefreshScores();

            State.ProcessScore(this);

            IsPlaying = false;
        }

        public void ShowGameWinScreen()
        {
            
        }

        public async void ShowWeekWin()
        {
            await UniTask.WaitForSeconds(1);
            Instantiate(_shopPrefab);
        }

        public void DestroyBuildings()
        {
            foreach (var building in FindObjectsOfType<BuildingView>())
                Destroy(building.gameObject);
        }

        public void AddBooster(Booster instance)
        {
            GameUI.Instance.ShowBoosterCard(instance);
            GameUI.Instance.RefreshDate();
            GameUI.Instance.RefreshMoneyAndEnergy();
        }

        public void RemoveBooster(Booster booster)
        {
            GameUI.Instance.RemoveBoosterCard(booster);
            GameUI.Instance.RefreshDate();
            GameUI.Instance.RefreshMoneyAndEnergy();
        }

        public void ShowBuildingsInHand()
        {
            foreach (var v in FindObjectsOfType<BuildingCardView>()) 
                Destroy(v.gameObject);

            foreach (var b in State.BuildingsInHand)
                GameUI.Instance.ShowBuildingCard(b);
        }

        public IGameUI GetUI() => GameUI.Instance;
        
        public void SpawnBuilding(Building data)
        {
            var building = Instantiate(Resources.Load<BuildingView>(data.PrefabAddress), data.Position, data.Rotation);
            building.transform.DOMoveY(1, 0.3f)
                .From()
                .SetRelative(true)
                .SetEase(Ease.OutBounce);
            building.SetData(data);
        }

        public void ShowLoseScreen()
        {
        }

        public IBuildingView GetBuilding(Building building)
            => FindObjectsOfType<BuildingView>().First(b => b.Data == building);

        public IBoosterView GetBooster(Booster booster) 
            => GameUI.Instance.GetBoosterCard(booster);

        public async void HighlightIsland(Island island)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.4f);
            await UniTask.WaitForSeconds(wait);

            foreach (var building in island)
            {
                foreach (var view in FindObjectsOfType<BuildingView>())
                {
                    if (view.Data != building)
                        continue;

                    view.transform.DOLocalMoveY(0.2f, 0.2f)
                        .SetEase(Ease.OutBack);
                    break;
                }
            }
        }

        public async void LowlightIsland(Island island)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.4f);
            await UniTask.WaitForSeconds(wait);

            foreach (var building in island)
            {
                foreach (var view in FindObjectsOfType<BuildingView>())
                {
                    if (view.Data != building)
                        continue;

                    view.transform.DOLocalMoveY(-0.2f, 0.2f)
                        .SetEase(Ease.OutBounce);
                    break;
                }
            }
        }
    }
}