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

        public static int TriggerCount;

        private LabelFeedback _bonusInstance;

        public Building Data { get; private set; }

        private bool IsPlacedDown => !Data.Id.IsDefault();
        
        public bool IsPreview { get; set; }

        private void Awake()
        {
            SetLayerRecursively(gameObject, LayerMask.NameToLayer("Building"));
        }

        public async void BuildingTriggered(int count)
        {
            await ShowScoringFeedback(count, _outputFeedback);
        }

        public async void BonusTriggered(int count)
        {
            await ShowScoringFeedback(count, _bonusContainer);
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
            scoringAudio.PlayScoreSound(Mathf.Clamp(TriggerCount, 0, scoringAudio.ClipCount - 1));
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
            foreach (var component in GetMeshRenderers())
            {
                var mat = component.material;
                var c = mat.color;
                c.a = isValidPlacement ? 1 : 0.5f;
                mat.color = c;
                component.material = mat;
            }
        }

        public IEnumerable<MeshRenderer> GetMeshRenderers()
        {
            return GetComponentsInChildren<MeshRenderer>()
                .Where(m => m.gameObject != _synergyRange && m.gameObject != _highlight);
        }

        public void ShowBonus(double bonus)
        {
            if (_bonusInstance == null)
            {
                var feedbackSource = GameManager.Instance.State.HasSensitive() ? _outputFeedback : _bonusContainer;
                _bonusInstance = Instantiate(feedbackSource, GetOverlayRoot());
                _bonusInstance.gameObject.AddComponent<RemoteChild>().SetParent(feedbackSource.transform, Vector3.zero);
                _bonusInstance.Show();
            }

            _bonusInstance.SetText($"+{bonus:0.#}");
        }

        private static Transform GetOverlayRoot()
        {
            return StaticResolver.Resolve<IUIRootProvider>().GetRoot(new UILayer("OverlayLayer"));
        }

        public void HideBonus()
        {
            if (_bonusInstance != null)
                Destroy(_bonusInstance.gameObject);
        }
    }
}