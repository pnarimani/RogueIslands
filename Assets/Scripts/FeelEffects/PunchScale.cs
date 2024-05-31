using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace RogueIslands.FeelEffects
{
    [FeedbackPath("Transform/Punch Scale")]
    public class PunchScale : MMF_Feedback
    {
        [MMFInspectorGroup("Scale", true, 12, true)]
        [Tooltip("the object to animate")]
        public Transform AnimateScaleTarget;

        public float Punch = 0.2f;
        public float Duration = 0.2f;
        
        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            AnimateScaleTarget.DOComplete();
            AnimateScaleTarget.DOPunchScale(Vector3.one * Punch, Duration);
        }

        public override float FeedbackDuration => Duration;
    }
}