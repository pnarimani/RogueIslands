using UnityEngine;

namespace Flexalon
{
    [ExecuteAlways]
    public class CurveCenter : MonoBehaviour
    {
        [SerializeField] private int _offset;
        
        private FlexalonCurveLayout _curveLayout;
        
        void OnEnable()
        {
            _curveLayout = GetComponent<FlexalonCurveLayout>();
        }

        void Update()
        {
            if (_curveLayout)
            {
                var count = transform.childCount + _offset;
                
                if (count % 2 == 0)
                {
                    // If even, leave a gap in the middle.
                    _curveLayout.StartAt = _curveLayout.CurveLength / 2 - (count / 2 - 0.5f) * _curveLayout.Spacing;
                }
                else
                {
                    // If odd, center the middle child.
                    _curveLayout.StartAt = _curveLayout.CurveLength / 2 - (count / 2) * _curveLayout.Spacing;
                }
            }
        }
    }
}