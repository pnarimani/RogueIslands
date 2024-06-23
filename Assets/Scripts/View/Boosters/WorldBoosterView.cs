using RogueIslands.Boosters;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RogueIslands.View.Boosters
{
    public class WorldBoosterView : MonoBehaviour, IHighlightable, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Transform _rangeVisuals;
        [SerializeField] private GameObject _deletionWarning;
        
        private WorldBooster _booster;

        private void Awake()
        {
            EffectRangeHighlighter.Register(this);
        }

        private void OnDestroy()
        {
            EffectRangeHighlighter.Remove(this);
        }

        public void Initialize(WorldBooster booster)
        {
            _booster = booster;
            GetComponent<BoosterView>().Initialize(booster);
            _rangeVisuals.transform.localScale = Vector3.one * (booster.Range * 2);
        }

        public void Highlight(bool highlight)
        {
        }

        public void ShowRange(bool showRange)
        {
            _rangeVisuals.gameObject.SetActive(showRange);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _rangeVisuals.gameObject.SetActive(true);
            EffectRangeHighlighter.Highlight(transform.position, _booster.Range, gameObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _rangeVisuals.gameObject.SetActive(false);
            EffectRangeHighlighter.LowlightAll();
        }

        public void WarnDeletion(bool shouldWarn) 
            => _deletionWarning.SetActive(shouldWarn);
    }
}