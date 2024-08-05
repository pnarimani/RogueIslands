using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.View.Commons;
using RogueIslands.Gameplay.View.Descriptions;
using RogueIslands.Gameplay.View.Feedbacks;
using RogueIslands.UISystem;
using RogueIslands.View.Audio;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class BuildingView : MonoBehaviour, IBuildingView, IHighlightable
    {
        public static int TriggerCount;
        [SerializeField] private GameObject _synergyRange;
        [SerializeField] private GameObject _highlight;
        [SerializeField] private BuildingTriggerFeedback _triggerFeedback;
        [SerializeField] private LabelFeedback _retriggerLabelFeedback;
        [SerializeField] private LabelFeedback _moneyFeedback;
        [SerializeField] private LabelFeedback _outputFeedback;
        [SerializeField] private LabelFeedback _bonusContainer;
        [SerializeField] private GameObject _invalidPlacement;
        [SerializeField] private GameObject _modelParent;
        [SerializeField] private GameObject[] _layoutPrefabs;

        private readonly List<LabelFeedback> _dryRunLabels = new();

        private GameObject _invalidPlacementInstance;
        private GameObject _layoutInstance;

        public Building Data { get; private set; }

        private bool IsPlacedDown => !Data.Id.IsDefault();

        public bool IsPreview { get; set; }

        public void Initialize(Building building)
        {
            if (building.Id.IsDefault())
                throw new InvalidOperationException();

            Data = building;
            _synergyRange.transform.localScale = Vector3.one * (building.Range * 2);

            GetComponent<DescriptionBoxSpawner>().Initialize(building);

            if (IsPlacedDown)
                StaticResolver.Resolve<IBuildingAudio>().PlayBuildingPlaced();

            for (var i = 0; i < _layoutPrefabs.Length; i++)
                if ((int)building.Size == i)
                {
                    _layoutInstance = Instantiate(_layoutPrefabs[i], transform, false);
                    var mesh = _layoutInstance.GetComponentInChildren<MeshRenderer>();
                    mesh.material.color = building.Color.Color;
                }

            SetLayerRecursively(gameObject, LayerMask.NameToLayer("Building"));
            foreach (var col in _modelParent.GetComponentsInChildren<Collider>(true))
                Destroy(col);
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
                ShowDryRun(trigger * count, _outputFeedback);
        }

        public void ShowDryRunBonus(Dictionary<int, int> bonusAndCount)
        {
            foreach (var (bonus, count) in bonusAndCount)
                ShowDryRun(bonus * count, _bonusContainer);
        }

        public void HideAllDryRunLabels()
        {
            foreach (var label in _dryRunLabels)
                Destroy(label.gameObject);

            _dryRunLabels.Clear();
        }

        public void Highlight(bool highlight)
        {
            // _highlight.SetActive(highlight);
        }

        public void ShowRange(bool showRange)
        {
            _synergyRange.SetActive(showRange);
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
            scoringAudio.PlayScoreSound(Mathf.Clamp01(TriggerCount / 12f));
            TriggerCount++;

            await UniTask.WhenAll(_triggerFeedback.Play(), feedback.Play());
            Destroy(feedback.gameObject);
        }

        public void ShowLayout(bool show)
        {
            foreach (var rend in _layoutInstance.GetComponentsInChildren<MeshRenderer>(true))
                rend.gameObject.SetActive(show);
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
            else if (isValidPlacement && _invalidPlacementInstance != null)
            {
                Destroy(_invalidPlacementInstance);
            }
        }

        private void ShowDryRun(double bonus, LabelFeedback source)
        {
            var instance = Instantiate(source, GetOverlayRoot());
            instance.gameObject.AddComponent<RemoteChild>().SetParent(source.transform, Vector3.zero);
            instance.Show();
            instance.SetText($"+{bonus:0.#}");
            _dryRunLabels.Add(instance);
        }

        private static Transform GetOverlayRoot() =>
            StaticResolver.Resolve<IUIRootProvider>().GetRoot(new UILayer("OverlayLayer"));

        private void OnDrawGizmosSelected()
        {
            foreach (var bounds in GetAllBounds().Bounds)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }

        public CompositeBounds GetAllBounds()
        {
            if (Data == null)
                return new CompositeBounds(Vector3.zero, Array.Empty<Bounds>());

            var colliders = _layoutInstance.GetComponentsInChildren<BoxCollider>(true);
            var bounds = new Bounds[colliders.Length];
            for (var i = 0; i < colliders.Length; i++)
            {
                var col = colliders[i];
                var colliderBounds = new Bounds(col.transform.position + col.center, col.size);
                bounds[i] = colliderBounds;
            }

            return new CompositeBounds(transform.position, bounds);
        }
    }
}