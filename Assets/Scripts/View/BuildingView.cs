using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using RogueIslands.Buildings;
using RogueIslands.View.Feedbacks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.View
{
    public class BuildingView : MonoBehaviour, IBuildingView, IPointerEnterHandler, IPointerExitHandler, IHighlightable
    {
        [SerializeField] private GameObject _synergyRange;
        [SerializeField] private BuildingTriggerFeedback _triggerFeedback;
        [SerializeField] private LabelFeedback _retriggerLabelFeedback;
        [SerializeField] private ParticleSystem _productsParticleSystem;

        public Building Data { get; private set; }

        private bool IsPlacedDown => !Data.Id.IsDefault();

        private void Awake()
        {
            SetLayerRecursively(gameObject, LayerMask.NameToLayer("Building"));
            EffectRangeHighlighter.Register(this);
        }

        private void OnDestroy()
        {
            EffectRangeHighlighter.Remove(this);
        }

        public void Initialize(Building building)
        {
            Data = building;
            _synergyRange.transform.localScale = Vector3.one * (building.Range * 2);

            GetComponent<DescriptionBoxSpawner>().Initialize(building);
        }

        public async void BuildingTriggered(bool isRetrigger)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.2f);
            AnimationScheduler.EnsureExtraTime(1.3f);
            var count = Data.Output + Data.OutputUpgrade;

            await UniTask.WaitForSeconds(wait);

            var ps = PlayParticleSystem(count);

            var task = _triggerFeedback.Play();
            if (isRetrigger)
                task = UniTask.WhenAll(task, _retriggerLabelFeedback.Play());
            await task;

            await UniTask.WaitForSeconds(0.6f);

            await GameUI.Instance.ProductTarget.Attract(ps, () => { GameUI.Instance.ProductBoosted(1); });
        }

        private ParticleSystem PlayParticleSystem(double count)
        {
            var ps = Instantiate(_productsParticleSystem, transform, false);
            var burst = ps.emission.GetBurst(0);
            burst.count = (int)(count);
            ps.emission.SetBurst(0, burst);
            ps.Play();
            return ps;
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsPlacedDown)
                return;
            ShowRange(true);
            EffectRangeHighlighter.Highlight(transform.position, Data.Range);
            var island = GameManager.Instance.State.Clusters.Find(x => x.Buildings.Contains(Data));
            if (island != null)
            {
                var allBuildings = FindObjectsOfType<BuildingView>();
                foreach (var building in island.Buildings)
                {
                    var neighbour = allBuildings.First(b => b.Data == building);
                    neighbour.Highlight(true);
                    neighbour.ShowRange(true);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsPlacedDown)
                return;
            
            ShowRange(false);
            EffectRangeHighlighter.LowlightAll();
        }

        public void Highlight(bool highlight)
        {
            var m = transform.FindRecursive("Cube").GetComponent<MeshRenderer>();
            m.material.EnableKeyword("_EMISSION");
            m.material.SetColor("_EmissionColor", highlight ? new Color(0f, 0.4f, 0f) : Color.black);
        }

        public void ShowRange(bool showRange)
        {
            _synergyRange.SetActive(showRange);
        }
    }
}