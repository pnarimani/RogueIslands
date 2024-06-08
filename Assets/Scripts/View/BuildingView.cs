using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using RogueIslands.Particles;
using UnityEngine;

namespace RogueIslands.View
{
    public class BuildingView : MonoBehaviour, IBuildingView
    {
        [SerializeField] private GameObject _synergyRange;
        [SerializeField] private MMF_Player _triggerFeedback, _retriggerFeedback;
        [SerializeField] private ParticleSystem _productsParticleSystem;
        
        public Building Data { get; private set; }

        public void SetData(Building building)
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
            AnimationScheduler.AllocateTime(Mathf.Max(0.4f, _triggerFeedback.TotalDuration));
            AnimationScheduler.EnsureExtraTime(1.3f);
            var count = Data.Output + Data.OutputUpgrade;
            
            await UniTask.WaitForSeconds(wait);

            PlayParticleSystem(count);
            _triggerFeedback.PlayFeedbacks();
            if (isRetrigger)
                _retriggerFeedback.PlayFeedbacks();
            
            await UniTask.WaitForSeconds(0.6f);
            
            await GameUI.Instance.ProductTarget.Attract(_productsParticleSystem, () =>
            {
                GameUI.Instance.ProductBoosted(1);
            });
        }

        private void PlayParticleSystem(double count)
        {
            var burst = _productsParticleSystem.emission.GetBurst(0);
            burst.count = (int)(count);
            _productsParticleSystem.emission.SetBurst(0, burst);
            _productsParticleSystem.Play();
        }
    }
}