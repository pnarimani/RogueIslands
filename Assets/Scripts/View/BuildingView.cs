using System;
using System.Linq;
using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;
using RogueIslands.Buildings;
using RogueIslands.View.Audio;
using RogueIslands.View.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.View
{
    public class BuildingView : MonoBehaviour, IBuildingView, IPointerEnterHandler, IPointerExitHandler, IHighlightable
    {
        [SerializeField] private GameObject _synergyRange;
        [SerializeField] private BuildingTriggerFeedback _triggerFeedback;
        [SerializeField] private LabelFeedback _retriggerLabelFeedback;
        [SerializeField] private LabelFeedback _moneyFeedback;
        [SerializeField] private GameObject _productsParticleSystem;
        [SerializeField] private UIParticleAttractor _attractorPrefab;

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
            
            if(IsPlacedDown)
                StaticResolver.Resolve<IBuildingAudio>().PlayBuildingPlaced();
        }

        public async void BuildingTriggered(bool isRetrigger)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.2f);
            AnimationScheduler.EnsureExtraTime(2.5f);
            var count = Data.Output + Data.OutputUpgrade;

            await UniTask.WaitForSeconds(wait);

            var ps = PlayParticleSystem(count);

            var task = _triggerFeedback.Play();
            if (isRetrigger)
                task = UniTask.WhenAll(task, _retriggerLabelFeedback.Play());
            await task;

            _attractorPrefab.gameObject.SetActive(false);
            var attractor = Instantiate(_attractorPrefab, GameUI.Instance.ProductTarget, false);
            attractor.particleSystem = ps;
            attractor.gameObject.SetActive(true);

            var hitCount = 0;
            attractor.onAttracted.AddListener(() =>
            {
                GameUI.Instance.ProductBoosted(1);
                hitCount++;
            });

            while (hitCount < count)
            {
                var distance = Vector3.Distance(attractor.transform.position, ps.transform.position);
                var t = Mathf.InverseLerp(500, 2000, distance);
                var speed = Mathf.Lerp(1f, 3.5f, t);
                attractor.maxSpeed = speed;
                await UniTask.DelayFrame(1, cancellationToken: destroyCancellationToken);
            }

            Destroy(attractor.gameObject);
            Destroy(ps.transform.parent.gameObject);
        }

        public async UniTask BuildingMadeMoney(int money)
        {
            _moneyFeedback.GetComponentInChildren<TextMeshProUGUI>().text = "$" + money;
            await _moneyFeedback.Play();
        }

        private ParticleSystem PlayParticleSystem(double count)
        {
            var obj = Instantiate(_productsParticleSystem, GameUI.Instance.transform, false);
            obj.GetComponent<PinToWorldObject>().Target = transform;
            var ps = obj.GetComponentInChildren<ParticleSystem>();
            var bursts = new ParticleSystem.Burst[(int)count];
            for (int i = 0; i < bursts.Length; i++)
            {
                bursts[i] = new ParticleSystem.Burst
                {
                    count = 1,
                    cycleCount = 1,
                    probability = 1,
                    time = 0.1f * i,
                };
            }

            ps.emission.SetBursts(bursts, bursts.Length);
            obj.GetComponent<UIParticle>().Play();
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