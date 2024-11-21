using System.Collections.Generic;
using Constants;
using GunStuff.PlayerAiming;
using Items.ItemInterfaces;
using PlayerAiming;
using UnityEngine;

namespace GunStuff
{
    public class GunEquipper : MonoBehaviour
    {
        [SerializeField] private Transform lookBase;
        [SerializeField] private Transform recoilScriptTransform;

        private PlayerGunPosition _playerGunPosition;
        private IKHandPlacement _ikHandPlacement;
        private Animator _animator;
        private DeadZoneLook _deadZoneLook;
        private Transform _equippedGunTransform;
        private IList<PlayerRecoil> _playerRecoilScripts;
        private PlayerGunPosition _playerAiming;

        private void Awake()
        {
            _playerGunPosition = GetComponent<PlayerGunPosition>();
            _ikHandPlacement = GetComponent<IKHandPlacement>();
            _animator = GetComponent<Animator>();
            _deadZoneLook = lookBase.GetComponent<DeadZoneLook>();
            _playerRecoilScripts = lookBase.GetComponentsInChildren<PlayerRecoil>();
            _playerAiming = GetComponent<PlayerGunPosition>();
        }

        public void UnEquipGun()
        {
            LayerManager.ChangeLayerOfItem(_equippedGunTransform, LayerMask.NameToLayer(LayerConstants.LAYER_GUN), TagConstants.InteractableTag);

            _animator.SetBool(AnimatorConstants.IsHoldingTwoHandedGun, false);
            _animator.SetBool(AnimatorConstants.IsHoldingPistol, false);
            _animator.SetBool(AnimatorConstants.IsAiming, false);

            _playerGunPosition.UnEquipGun();
            _ikHandPlacement.TakeHandsOffGun();

            IGun gun = _equippedGunTransform.GetComponent<IGun>();
            if (gun is not null)
            {
                gun.GunFired -= gun.RecoilBehaviour.AddRecoil;
                gun.CurrentAimAtTarget = null;
                foreach (PlayerRecoil recoilScript in _playerRecoilScripts)
                {
                    gun.GunFired -= recoilScript.AddRecoil;
                }
                
                _playerAiming.playerAiming.RemoveListener(gun.RecoilBehaviour.PlayerIsAiming);
                _playerAiming.playerNotAiming.RemoveListener(gun.RecoilBehaviour.PlayerIsNotAiming);
            }

            _deadZoneLook.UseDeadZone = false;
            _equippedGunTransform = null;
        }

        public bool EquipPistol(Transform gunTransform, IGun gunInfo) => EquipGun(gunTransform, gunInfo, AnimatorConstants.IsHoldingPistol);

        public bool EquipRifle(Transform gunTransform, IGun gunInfo) => EquipGun(gunTransform, gunInfo, AnimatorConstants.IsHoldingTwoHandedGun);


        private bool EquipGun(Transform gunTransform, IGun gunInfo, int animName)
        {
            _equippedGunTransform = gunTransform;

            _equippedGunTransform.SetParent(transform);
            _animator.SetBool(animName, true);
            _deadZoneLook.UseDeadZone = true;
            
            _ikHandPlacement.EquipGun(gunInfo.Components);
            _playerGunPosition.EquipGun(gunInfo);
            
            GunRecoil recoilScript = gunInfo.RecoilBehaviour;
            gunInfo.GunFired += recoilScript.AddRecoil;
            gunInfo.CurrentAimAtTarget = recoilScriptTransform;
            foreach (PlayerRecoil playerRecoilScript in _playerRecoilScripts)
            {
                gunInfo.GunFired += playerRecoilScript.AddRecoil;
            }

            _playerAiming.playerAiming.AddListener(recoilScript.PlayerIsAiming);
            _playerAiming.playerNotAiming.AddListener(recoilScript.PlayerIsNotAiming);

            LayerManager.ChangeLayerOfItem(gunTransform, LayerMask.NameToLayer(LayerConstants.LAYER_PLAYER), TagConstants.PlayerTag);
            return true;
        }
    }
}