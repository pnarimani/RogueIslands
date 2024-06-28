using Unity.Cinemachine;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    [AddComponentMenu("Cinemachine/Procedural/Rotation Control/SetYRotationToHorizontalValue")]
    [SaveDuringPlay]
    [DisallowMultipleComponent]
    [CameraPipeline(CinemachineCore.Stage.Aim)]
    [RequiredTarget(RequiredTargetAttribute.RequiredTargets.LookAt)]
    public class SetYRotationToHorizontalValue : CinemachineComponentBase
    {
        private CinemachineOrbitalFollow _follow;
        public override bool IsValid => enabled && LookAtTarget != null;
        public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Aim;

        private void Awake()
        {
            _follow = GetComponent<CinemachineOrbitalFollow>();
        }

        public override void MutateCameraState(ref CameraState curState, float deltaTime)
        {
            if (IsValid && curState.HasLookAt())
            {
                var self = transform.localRotation.eulerAngles;
                curState.RawOrientation = Quaternion.Euler(self.x, _follow.HorizontalAxis.Value, self.z);
            }
        }
    }
}