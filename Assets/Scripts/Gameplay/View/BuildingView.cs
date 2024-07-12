using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.View.Feedbacks;
using RogueIslands.View.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.Gameplay.View
{
    public class BuildingView : MonoBehaviour, IBuildingView, IPointerEnterHandler, IPointerExitHandler, IHighlightable
    {
        [SerializeField] private GameObject _synergyRange;
        [SerializeField] private GameObject _highlight;
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
        }

        public void Initialize(Building building)
        {
            Data = building;
            _synergyRange.transform.localScale = Vector3.one * (building.Range * 2);
            var vector3 = transform.GetCollisionBounds().size;
            vector3.y = vector3.z;
            vector3.z = 1;
            _highlight.transform.localScale = vector3 * 1.5f;

            GetComponent<DescriptionBoxSpawner>().Initialize(building);

            if (IsPlacedDown)
                StaticResolver.Resolve<IBuildingAudio>().PlayBuildingPlaced();
        }

        public async void BuildingTriggered(bool isRetrigger)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.2f);
            var extraTime = SettingsPopup.SettingsParticles ? 2f : 0.2f;
            AnimationScheduler.EnsureExtraTime(extraTime);
            var count = Data.Output + Data.OutputUpgrade;

            await UniTask.WaitForSeconds(wait);

            ParticleSystem ps = null;
            if (SettingsPopup.SettingsParticles)
            {
                ps = PlayParticleSystem(count);
            }

            var task = _triggerFeedback.Play();
            if (isRetrigger)
                task = UniTask.WhenAll(task, _retriggerLabelFeedback.Play());
            await task;

            if (ps != null)
            {
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
            else
            {
                GameUI.Instance.ProductBoosted(count);
            }
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
            EffectRangeHighlighter.HighlightBuilding(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!IsPlacedDown)
                return;

            EffectRangeHighlighter.LowlightAll();
        }

        public void Highlight(bool highlight) => _highlight.SetActive(highlight);

        public void ShowRange(bool showRange) => _synergyRange.SetActive(showRange);

        public void ShowValidPlacement(bool isValidPlacement)
        {
            foreach (var component in GetComponentsInChildren<MeshRenderer>())
            {
                var mat = component.material;
                var c = mat.color;
                c.a = isValidPlacement ? 1 : 0.5f;
                mat.color = c;
                component.material = mat;
            }
        }
    }
}