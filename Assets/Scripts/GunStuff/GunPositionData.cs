using UnityEngine;

namespace GunStuff
{
    public class GunPositionData : MonoBehaviour
    {
        [SerializeField] private Transform leftHandIkTarget;
        [SerializeField] private Transform rightHandIkTarget;
        [SerializeField] private Transform rearSight;
        [SerializeField] private Transform frontSight;
        [SerializeField] private Transform gunFulcrum;
        [SerializeField] private Transform gunMesh;
        [SerializeField] private Transform muzzle;
        [SerializeField] private Vector3 aimPosition;

        public Vector3 MuzzlePosition => muzzle.position;
        public Transform GunFulcrum => gunFulcrum;
        public Transform RearSight => rearSight;
        public Transform FrontSight => frontSight;
        public Transform GunMesh => gunMesh;
        public Vector3 AimPosition => aimPosition;
        public Transform RightHandIkTarget => rightHandIkTarget;
        public Transform LeftHandIkTarget => leftHandIkTarget;
    }
}
