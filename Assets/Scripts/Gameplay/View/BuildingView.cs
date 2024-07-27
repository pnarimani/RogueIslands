using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.View.Commons;
using RogueIslands.Gameplay.View.Feedbacks;
using RogueIslands.UISystem;
using RogueIslands.View.Audio;
using TMPro;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class BuildingView : MonoBehaviour, IBuildingView, IHighlightable
    {
        [SerializeField] private GameObject _synergyRange;
        [SerializeField] private GameObject _highlight;
        [SerializeField] private BuildingTriggerFeedback _triggerFeedback;
        [SerializeField] private LabelFeedback _retriggerLabelFeedback;
        [SerializeField] private LabelFeedback _moneyFeedback;
        [SerializeField] private LabelFeedback _outputFeedback;
        [SerializeField] private LabelFeedback _bonusContainer;
        [SerializeField] private GameObject _invalidPlacement;

        public static int TriggerCount;

        private GameObject _invalidPlacementInstance;
        private readonly List<LabelFeedback> _dryRunLabels = new();

        public Building Data { get; private set; }

        private bool IsPlacedDown => !Data.Id.IsDefault();

        public bool IsPreview { get; set; }

        private void Awake()
        {
            SetLayerRecursively(gameObject, LayerMask.NameToLayer("Building"));
        }

        public async void BuildingTriggered(int score)
        {
            await ShowScoringFeedback(score, _outputFeedback);
        }

        public async void BonusTriggered(int score)
        {
            await ShowScoringFeedback(score, _bonusContainer);
        }

        public void ShowDryRunTrigger(Dictionary<int, int> triggerAndCount)
        {
            foreach (var (trigger, count) in triggerAndCount)
            {
                ShowDryRun(trigger * count, _outputFeedback);
            }
        }

        public void ShowDryRunBonus(Dictionary<int, int> bonusAndCount)
        {
            foreach (var (bonus, count) in bonusAndCount)
            {
                ShowDryRun(bonus * count, _bonusContainer);
            }
        }

        public void HideAllDryRunLabels()
        {
            foreach (var label in _dryRunLabels)
                Destroy(label.gameObject);

            _dryRunLabels.Clear();
        }

        private async UniTask ShowScoringFeedback(int count, LabelFeedback feedbackSource)
        {
            await AnimationScheduler.ScheduleAndWait(1f, 0.1f);
            var overlay = GetOverlayRoot();
            var feedback = Instantiate(feedbackSource, overlay);
            feedback.SetText($"+{count}");
            feedback.gameObject.AddComponent<RemoteChild>().SetParent(feedbackSource.transform, Vector3.zero);
            GameUI.Instance.ProductBoosted(count);

            var scoringAudio = StaticResolver.Resolve<IScoringAudio>();
            scoringAudio.PlayScoreSound(TriggerCount / 12f);
            TriggerCount++;

            await UniTask.WhenAll(_triggerFeedback.Play(), feedback.Play());
            Destroy(feedback.gameObject);
        }

        public void Highlight(bool highlight)
        {
            _highlight.SetActive(highlight);
        }

        public void ShowRange(bool showRange)
        {
            _synergyRange.SetActive(showRange);
        }

        public void Initialize(Building building)
        {
            if (building.Id.IsDefault())
                throw new InvalidOperationException();

            Data = building;
            _synergyRange.transform.localScale = Vector3.one * (building.Range * 2);
            var vector3 = transform.GetBounds(GetMeshRenderers()).size;
            vector3.y = vector3.z;
            vector3.z = 1;
            _highlight.transform.localScale = vector3 * 1.5f;

            GetComponent<DescriptionBoxSpawner>().Initialize(building);

            if (IsPlacedDown)
                StaticResolver.Resolve<IBuildingAudio>().PlayBuildingPlaced();
        }

        public async UniTask BuildingMadeMoney(int money)
        {
            _moneyFeedback.GetComponentInChildren<TextMeshProUGUI>().text = "$" + money;
            await _moneyFeedback.Play();
        }

        private static void SetLayerRecursively(GameObject obj, int newLayer)
        {
            if (obj == null)
                return;

            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
            {
                if (child == null)
                    continue;

                SetLayerRecursively(child.gameObject, newLayer);
            }
        }

        public void ShowValidPlacement(bool isValidPlacement)
        {
            if (!isValidPlacement && _invalidPlacementInstance == null)
            {
                var overlay = GetOverlayRoot();
                _invalidPlacementInstance = Instantiate(_invalidPlacement, overlay);
                _invalidPlacementInstance.gameObject.AddComponent<RemoteChild>()
                    .SetParent(_invalidPlacement.transform, Vector3.zero);
                _invalidPlacementInstance.SetActive(true);
            }
            else if(isValidPlacement && _invalidPlacementInstance != null)
            {
                Destroy(_invalidPlacementInstance);
            }
        }

        public IEnumerable<MeshRenderer> GetMeshRenderers()
        {
            return GetComponentsInChildren<MeshRenderer>()
                .Where(m => m.gameObject != _synergyRange && m.gameObject != _highlight);
        }

        private void ShowDryRun(double bonus, LabelFeedback source)
        {
            var instance = Instantiate(source, GetOverlayRoot());
            instance.gameObject.AddComponent<RemoteChild>().SetParent(source.transform, Vector3.zero);
            instance.Show();
            instance.SetText($"+{bonus:0.#}");
            _dryRunLabels.Add(instance);
        }

        private static Transform GetOverlayRoot()
        {
            return StaticResolver.Resolve<IUIRootProvider>().GetRoot(new UILayer("OverlayLayer"));
        }
    }
}