using System.Collections;
using PlayerAiming;
using UnityEngine;

namespace GunStuff
{
    public class GunEquipper : MonoBehaviour
    {
        [SerializeField] private Transform lookBase;
        [SerializeField] private Transform upperChest;

        private bool _isGunEquipped;
        private AimBehaviour _aimBehaviour;
        private GunHandPlacement _gunHandPlacement;
        private Animator _animator;
        private DeadZoneLook _deadZoneLook;
        private GunPhysicsData _currentlyEquippedPhysicsData;

        private Transform _equippedGunTransform;

        public bool IsGunEquipped => _isGunEquipped;

        private void Awake()
        {
            _aimBehaviour = GetComponent<AimBehaviour>();
            _gunHandPlacement = GetComponent<GunHandPlacement>();
            _animator = GetComponent<Animator>();
            _deadZoneLook = lookBase.GetComponent<DeadZoneLook>();
        }

        public void UnEquipGun()
        {
            _isGunEquipped = false;

            _animator.SetBool(Constants.IsHoldingTwoHandedGun, false);
            _animator.SetBool(Constants.IsHoldingPistol, false);
            _animator.SetBool(Constants.IsAiming, false);

            _aimBehaviour.UnEquipGun();
            _gunHandPlacement.UnEquipGun();


            _deadZoneLook.UseDeadZone = false;
            _equippedGunTransform.SetParent(null);

            _currentlyEquippedPhysicsData.EnablePhysics();
        }

        public void EquipPistol(GameObject gun)
        {
            EquipGun(gun, Constants.IsHoldingPistol);
        }

        private void EquipGun(GameObject gun, int animName)
        {
            GunPositionData posData = gun.GetComponent<GunPositionData>();
            _currentlyEquippedPhysicsData = gun.GetComponent<GunPhysicsData>();

            if (posData is not null && _currentlyEquippedPhysicsData is not null)
            {
                _isGunEquipped = true;
                _equippedGunTransform = gun.transform;

                _equippedGunTransform.SetParent(transform);
                _animator.SetBool(animName, true);
                _deadZoneLook.UseDeadZone = true;
                _currentlyEquippedPhysicsData.DisablePhysics();

                _gunHandPlacement.EquipGun(posData);
                _aimBehaviour.EquipGun(posData);

                _equippedGunTransform.localPosition = posData.GunLocalPosition;
                _equippedGunTransform.localEulerAngles = posData.GunLocalRotation;
                //StartCoroutine(LerpGunToPosition(1f, posData));
            }
        }

        public void EquipRifle(GameObject gun)
        {
            EquipGun(gun, Constants.IsHoldingTwoHandedGun);
        }

        private IEnumerator LerpGunToPosition(float timeToLerp, GunPositionData posData)
        {
            float timeElapsed = 0f;

            Vector3 currentPos = _equippedGunTransform.localPosition;
            //Vector3 currentRot = _equippedGunTransform.localEulerAngles;

            while (timeElapsed < timeToLerp)
            {
                float t = timeElapsed / timeToLerp;

                _equippedGunTransform.localPosition = Vector3.Lerp(currentPos, posData.GunLocalPosition, t);
                //_equippedGunTransform.localEulerAngles = Vector3.Lerp(currentRot, localEuler, t);

                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;
            }

            _equippedGunTransform.localPosition = posData.GunLocalPosition;
            //_equippedGunTransform.localEulerAngles = localEuler;

            _aimBehaviour.EquipGun(posData);
        }
    }
}