using UnityEngine;

namespace GunStuff
{
    public class GunPositionData : MonoBehaviour
    {
        [SerializeField] private Transform leftHandIkTarget;
        [SerializeField] private Transform rightHandIkTarget;
        [SerializeField] private Transform rearSight;
        [SerializeField] private Transform actualGun;
        [SerializeField] private Vector3 aimPosition;

        public Transform GunFulcrum => transform;
        public Transform RearSight => rearSight;
        public Transform ActualGun => actualGun;
        public Vector3 AimPosition => aimPosition;
        public Transform RightHandIkTarget => rightHandIkTarget;
        public Transform LeftHandIkTarget => leftHandIkTarget;
    }
}
