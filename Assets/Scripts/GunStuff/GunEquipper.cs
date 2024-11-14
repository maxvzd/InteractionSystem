﻿using System.Collections.Generic;
using Constants;
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
        private GunHandPlacement _gunHandPlacement;
        private Animator _animator;
        private DeadZoneLook _deadZoneLook;
        private Transform _equippedGunTransform;
        private IList<PlayerRecoil> _playerRecoilScripts;

        private void Awake()
        {
            _playerGunPosition = GetComponent<PlayerGunPosition>();
            _gunHandPlacement = GetComponent<GunHandPlacement>();
            _animator = GetComponent<Animator>();
            _deadZoneLook = lookBase.GetComponent<DeadZoneLook>();
            _playerRecoilScripts = lookBase.GetComponentsInChildren<PlayerRecoil>();
        }

        public void UnEquipGun()
        {
            LayerManager.ChangeLayerOfItem(_equippedGunTransform, LayerMask.NameToLayer(LayerConstants.LAYER_GUN), TagConstants.InteractableTag);

            _animator.SetBool(AnimatorConstants.IsHoldingTwoHandedGun, false);
            _animator.SetBool(AnimatorConstants.IsHoldingPistol, false);
            _animator.SetBool(AnimatorConstants.IsAiming, false);

            _playerGunPosition.UnEquipGun();
            _gunHandPlacement.UnEquipGun();

            IGun gun = _equippedGunTransform.GetComponent<IGun>();
            if (gun is not null)
            {
                gun.GunFired -= gun.RecoilBehaviour.AddRecoil;
                gun.CurrentAimAtTarget = null;
                foreach (PlayerRecoil recoilScript in _playerRecoilScripts)
                {
                    gun.GunFired -= recoilScript.AddRecoil;
                }
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
            
            _gunHandPlacement.EquipGun(gunInfo.Components);
            _playerGunPosition.EquipGun(gunInfo);
            
            GunRecoil recoilScript = gunInfo.RecoilBehaviour;
            gunInfo.GunFired += recoilScript.AddRecoil;
            gunInfo.CurrentAimAtTarget = recoilScriptTransform;
            foreach (PlayerRecoil playerRecoilScript in _playerRecoilScripts)
            {
                gunInfo.GunFired += playerRecoilScript.AddRecoil;
            }

            LayerManager.ChangeLayerOfItem(gunTransform, LayerMask.NameToLayer(LayerConstants.LAYER_PLAYER), TagConstants.PlayerTag);
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