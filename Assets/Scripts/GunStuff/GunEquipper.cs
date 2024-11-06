using Constants;
using Items;
using Items.ItemInterfaces;
using Items.Weapons;
using PlayerAiming;
using UnityEngine;

namespace GunStuff
{
    public class GunEquipper : MonoBehaviour
    {
        [SerializeField] private Transform lookBase;
        [SerializeField] private Transform recoilScriptTransform;

        private AimBehaviour _aimBehaviour;
        private GunHandPlacement _gunHandPlacement;
        private Animator _animator;
        private DeadZoneLook _deadZoneLook;
        private Transform _equippedGunTransform;
        private PlayerGunRecoil _recoil;

        private void Awake()
        {
            _aimBehaviour = GetComponent<AimBehaviour>();
            _gunHandPlacement = GetComponent<GunHandPlacement>();
            _animator = GetComponent<Animator>();
            _deadZoneLook = lookBase.GetComponent<DeadZoneLook>();
            _recoil = recoilScriptTransform.GetComponent<PlayerGunRecoil>();
        }

        public void UnEquipGun()
        {
            LayerManager.ChangeLayerOfItem(_equippedGunTransform, LayerMask.NameToLayer(LayerConstants.LAYER_GUN), TagConstants.InteractableTag);
            
            _animator.SetBool(AnimatorConstants.IsHoldingTwoHandedGun, false);
            _animator.SetBool(AnimatorConstants.IsHoldingPistol, false);
            _animator.SetBool(AnimatorConstants.IsAiming, false);
        
            _aimBehaviour.UnEquipGun();
            _gunHandPlacement.UnEquipGun();

            IGun gun = _equippedGunTransform.GetComponent<IGun>();
            if (gun is not null)
            {
                gun.GunFired -= _recoil.AddRecoil;
                gun.CurrentAimAtTarget = null;
            }

            _deadZoneLook.UseDeadZone = false;
            _equippedGunTransform = null;
        }

        public bool EquipPistol(Transform gunTransform, IGun gunInfo) => EquipGun(gunTransform, gunInfo, AnimatorConstants.IsHoldingPistol);
        
        public bool EquipRifle(Transform gunTransform, IGun gunInfo) => EquipGun(gunTransform, gunInfo,AnimatorConstants.IsHoldingTwoHandedGun);
        

        private bool EquipGun(Transform gun, IGun gunInfo, int animName)
        {
            GunPositionData posData = gun.GetComponent<GunPositionData>();
            if (posData is null) return false;
            
            _equippedGunTransform = gun;

            _equippedGunTransform.SetParent(transform);
            _animator.SetBool(animName, true);
            _deadZoneLook.UseDeadZone = true;

            _gunHandPlacement.EquipGun(posData);
            _aimBehaviour.EquipGun(posData, gunInfo);

            EquippedPosition equippedPosition = gunInfo.EquippedPosition;
            _equippedGunTransform.localPosition = equippedPosition.EquippedLocalPosition;
            _equippedGunTransform.localEulerAngles = equippedPosition.EquippedLocalRotation;

            gunInfo.GunFired += _recoil.AddRecoil;
            gunInfo.CurrentAimAtTarget = recoilScriptTransform;
            
            
            LayerManager.ChangeLayerOfItem(gun, LayerMask.NameToLayer(LayerConstants.LAYER_PLAYER), TagConstants.PlayerTag);
            return true;

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