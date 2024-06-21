using Cysharp.Threading.Tasks;
using RogueIslands.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.View
{
    public class BuildingView : MonoBehaviour, IBuildingView
    {
        [SerializeField] private GameObject _synergyRange;
        [SerializeField] private BuildingTriggerFeedback _triggerFeedback;
        [SerializeField] private LabelFeedback _retriggerLabelFeedback;
        [SerializeField] private ParticleSystem _productsParticleSystem;
        
        public Building Data { get; private set; }

        private void Awake()
        {
            SetLayerRecursively(gameObject, LayerMask.NameToLayer("Building"));
        }

        public void Initialize(Building building)
        {
            Data = building;
            _synergyRange.transform.localScale = Vector3.one * (building.Range * 2);
        }

        public void ShowSynergyRange(bool show)
            => _synergyRange.SetActive(show);

        public void HighlightConnection(bool isEnabled)
        {
            var m = transform.FindRecursive("Cube").GetComponent<MeshRenderer>();
            m.material.EnableKeyword("_EMISSION");
            m.material.SetColor("_EmissionColor", isEnabled ? new Color(0f, 0.4f, 0f) : Color.black);
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
            
            await GameUI.Instance.ProductTarget.Attract(ps, () =>
            {
                GameUI.Instance.ProductBoosted(1);
            });
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
    }
}