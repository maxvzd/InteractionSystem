using UnityEngine;

namespace PlayerAiming
{
    public class GunPositionData : MonoBehaviour
    {
        [SerializeField] private Transform leftHandIkTarget;
        [SerializeField] private Transform rightHandIkTarget;
        [SerializeField] private Transform gunFulcrum;
        [SerializeField] private Transform rearSight;
        [SerializeField] private Vector3 aimPosition;

        [SerializeField] private Vector3 gunLocalPosition;
        [SerializeField] private Vector3 gunLocalRotation;

        public Transform GunFulcrum => gunFulcrum;
        public Transform RearSight => rearSight;
        public Vector3 AimPosition => aimPosition;
        public Vector3 GunLocalPosition => gunLocalPosition;
        public Vector3 GunLocalRotation => gunLocalRotation;
        public Transform RightHandIkTarget => rightHandIkTarget;
        public Transform LeftHandIkTarget => leftHandIkTarget;
    }
}
