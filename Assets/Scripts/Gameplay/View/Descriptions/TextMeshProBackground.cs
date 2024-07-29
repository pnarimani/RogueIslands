using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace RogueIslands.Gameplay.View.Descriptions
{
    public class TextMeshProBackground : MonoBehaviour
    {
        [SerializeField] private RectOffset _padding;
        [SerializeField] private Image _backgroundPrefab;
        private readonly List<Image> _images = new();
        private TextMeshProUGUI _text;

        public RectOffset Padding
        {
            get => _padding;
            set => _padding = value;
        }

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(TextChanged);
        }

        private void OnDestroy()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(TextChanged);
            foreach (var image in _images)
                Destroy(image.gameObject);
        }

        private void TextChanged(Object obj)
        {
            if (obj == _text)
                ManualUpdate().Forget();
        }

        public async UniTaskVoid ManualUpdate()
        {
            await UniTask.DelayFrame(1);

            int usedImages = 0;
            var hasBegun = false;
            var color = Color.white;
            var yMin = float.MaxValue;
            var yMax = float.MinValue;
            var xMin = float.MaxValue;
            var xMax = float.MinValue;

            for (var i = 0; i < _text.textInfo.characterCount; i++)
            {
                var currChar = _text.textInfo.characterInfo[i];
                if ((currChar.style & FontStyles.Highlight) == 0)
                {
                    if (hasBegun)
                    {
                        hasBegun = false;
                        
                        var localPosition = new Vector2(xMin, yMin);
                        var size = new Vector2(xMax - xMin, yMax - yMin);

                        var img = GetBackgroundImage(usedImages++);
                        img.color = color;

                        localPosition.x -= Padding.left;
                        localPosition.y -= Padding.bottom;
                        img.rectTransform.position = _text.transform.TransformPoint(localPosition);
                        img.rectTransform.sizeDelta = size + new Vector2(Padding.horizontal, Padding.vertical);
                        img.pixelsPerUnitMultiplier = img.sprite.rect.height / img.rectTransform.sizeDelta.y;
                        img.gameObject.SetActive(true);
                        
                        yMin = float.MaxValue;
                        yMax = float.MinValue;
                        xMin = float.MaxValue;
                        xMax = float.MinValue;
                    }
                    
                    continue;
                }

                hasBegun = true;

                color = currChar.highlightState.color;
                color.a = 1f;
                
                yMin = Mathf.Min(yMin, currChar.bottomLeft.y);
                yMax = Mathf.Max(yMax, currChar.topLeft.y);
                xMin = Mathf.Min(xMin, currChar.bottomLeft.x);
                xMax = Mathf.Max(xMax, currChar.bottomRight.x);
            }
            
            for (var i = usedImages; i < _images.Count; i++)
                _images[i].gameObject.SetActive(false);
        }

        private Image GetBackgroundImage(int bIndex)
        {
            Image img;
            if (bIndex < _images.Count)
            {
                img = _images[bIndex];
            }
            else
            {
                img = Instantiate(_backgroundPrefab, transform.parent, false);
                img.transform.SetSiblingIndex(transform.GetSiblingIndex());
                _images.Add(img);
            }

            return img;
        }
    }
}