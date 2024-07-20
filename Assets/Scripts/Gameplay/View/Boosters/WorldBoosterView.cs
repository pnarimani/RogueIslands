using RogueIslands.Gameplay.Boosters;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class WorldBoosterView : MonoBehaviour, IHighlightable, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _highlight;
        [SerializeField] private Transform _rangeVisuals;
        [SerializeField] private GameObject _deletionWarning;

        private WorldBooster _booster;

        public WorldBooster Data => _booster;

        public void Initialize(WorldBooster booster)
        {
            _booster = booster;
            GetComponent<BoosterView>().Initialize(booster);
            _rangeVisuals.transform.localScale = Vector3.one * (booster.Range * 2);
        }

        private void Start()
        {
            var size = transform.GetBounds(null).size;
            _highlight.transform.localScale = Vector3.one * (Mathf.Max(size.x, size.z) * 1.3f);
        }

        public void Highlight(bool highlight)
        {
            _highlight.SetActive(highlight);
        }

        public void ShowRange(bool showRange)
        {
            _rangeVisuals.gameObject.SetActive(showRange);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EffectRangeHighlighter.HighlightWorldBooster(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EffectRangeHighlighter.LowlightAll();
        }

        public void WarnDeletion(bool shouldWarn)
            => _deletionWarning.SetActive(shouldWarn);
    }
}