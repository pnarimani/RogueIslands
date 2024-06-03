using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace RogueIslands.View
{
    public class BuildingView : MonoBehaviour, IBuildingView
    {
        [SerializeField] private GameObject _synergyRange;
        [SerializeField] private MMF_Player _triggerFeedback, _retriggerFeedback;
        
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
            var m = transform.Find("Cube").GetComponent<MeshRenderer>();
            m.material.EnableKeyword("_EMISSION");
            m.material.SetColor("_EmissionColor", isEnabled ? new Color(0f, 0.4f, 0f) : Color.black);
        }

        public async void BuildingTriggered(bool isRetrigger)
        {
            var delay = AnimationScheduler.AllocateTime(_triggerFeedback.TotalDuration);
            await UniTask.WaitForSeconds(delay);
            _triggerFeedback.PlayFeedbacks();
            if (isRetrigger)
                _retriggerFeedback.PlayFeedbacks();
        }
    }
}