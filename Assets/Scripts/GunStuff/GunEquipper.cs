using Constants;
using Items;
using Items.ItemInterfaces;
using PlayerAiming;
using UnityEngine;

namespace GunStuff
{
    public class GunEquipper : MonoBehaviour
    {
        [SerializeField] private Transform lookBase;

        private bool _isGunEquipped;
        private AimBehaviour _aimBehaviour;
        private GunHandPlacement _gunHandPlacement;
        private Animator _animator;
        private DeadZoneLook _deadZoneLook;
        private Transform _equippedGunTransform;

        public bool IsGunEquipped => _isGunEquipped;

        private void Awake()
        {
            _aimBehaviour = GetComponent<AimBehaviour>();
            _gunHandPlacement = GetComponent<GunHandPlacement>();
            _animator = GetComponent<Animator>();
            _deadZoneLook = lookBase.GetComponent<DeadZoneLook>();
        }

        // public void UnEquipGun()
        // {
        //     _isGunEquipped = false;
        //
        //     _animator.SetBool(AnimatorConstants.IsHoldingTwoHandedGun, false);
        //     _animator.SetBool(AnimatorConstants.IsHoldingPistol, false);
        //     _animator.SetBool(AnimatorConstants.IsAiming, false);
        //
        //     _aimBehaviour.UnEquipGun();
        //     _gunHandPlacement.UnEquipGun();
        //
        //     _deadZoneLook.UseDeadZone = false;
        //     //_equippedGunTransform.SetParent(null);
        //
        //     //_currentlyEquippedPhysicsData.EnablePhysics();
        // }

        public bool EquipPistol(Transform gunTransform, IWeapon gunInfo) => EquipGun(gunTransform, gunInfo, AnimatorConstants.IsHoldingPistol);
        
        public bool EquipRifle(Transform gunTransform, IWeapon gunInfo) => EquipGun(gunTransform, gunInfo,AnimatorConstants.IsHoldingTwoHandedGun);
        

        private bool EquipGun(Transform gun, IWeapon gunInfo, int animName)
        {
            GunPositionData posData = gun.GetComponent<GunPositionData>();
            if (posData is not null)
            {
                _isGunEquipped = true;
                _equippedGunTransform = gun;

                _equippedGunTransform.SetParent(transform);
                _animator.SetBool(animName, true);
                _deadZoneLook.UseDeadZone = true;

                _gunHandPlacement.EquipGun(posData);
                _aimBehaviour.EquipGun(posData, gunInfo);

                EquippedPosition equippedPosition = gunInfo.EquippedPosition;
                _equippedGunTransform.localPosition = equippedPosition.EquippedLocalPosition;
                _equippedGunTransform.localEulerAngles = equippedPosition.EquippedLocalRotation;
                //StartCoroutine(LerpGunToPosition(1f, posData));
                return true;
            }

            return false;
        }

        // private IEnumerator LerpGunToPosition(float timeToLerp, GunPositionData posData)
        // {
        //     float timeElapsed = 0f;
        //
        //     Vector3 currentPos = _equippedGunTransform.localPosition;
        //     //Vector3 currentRot = _equippedGunTransform.localEulerAngles;
        //
        //     while (timeElapsed < timeToLerp)
        //     {
        //         float t = timeElapsed / timeToLerp;
        //
        //         _equippedGunTransform.localPosition = Vector3.Lerp(currentPos, posData.GunLocalPosition, t);
        //         //_equippedGunTransform.localEulerAngles = Vector3.Lerp(currentRot, localEuler, t);
        //
        //         yield return new WaitForEndOfFrame();
        //         timeElapsed += Time.deltaTime;
        //     }
        //
        //     _equippedGunTransform.localPosition = posData.GunLocalPosition;
        //     //_equippedGunTransform.localEulerAngles = localEuler;
        //
        //     _aimBehaviour.EquipGun(posData);
        // }
    }
}