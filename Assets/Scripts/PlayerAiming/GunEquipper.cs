using UnityEngine;

namespace PlayerAiming
{
    public class GunEquipper : MonoBehaviour
    {
        //Temp
        [SerializeField] private GameObject gunToEquip;

        [SerializeField] private Transform lookBase;
    
        private bool _isGunEquipped;
        private AimBehaviour _aimBehaviour;
        private GunHandPlacement _gunHandPlacement;
        private Animator _animator;
        private DeadZoneLook _deadZoneLook;
        public bool IsGunEquipped => _isGunEquipped;

        private void Awake()
        {
            _aimBehaviour = GetComponent<AimBehaviour>();
            _gunHandPlacement = GetComponent<GunHandPlacement>();
            _animator = GetComponent<Animator>();
            _deadZoneLook = lookBase.GetComponent<DeadZoneLook>();
        }

        private void Start()
        {
            EquipGun(gunToEquip);
        }

        private void Update()
        {
            //Temporary
            if (Input.GetButtonDown(Constants.SwapWeaponKey))
            {
                if (_isGunEquipped)
                {
                    UnEquipGun();
                }
                else
                {
                    EquipGun(gunToEquip);
                }
            }
        }

        private void UnEquipGun()
        {
            _isGunEquipped = false;
            _aimBehaviour.UnEquipGun();
            _gunHandPlacement.UnEquipGun();
            _animator.SetBool(Constants.IsHoldingTwoHandedGun, false);
            _deadZoneLook.UseDeadZone = false;
            gunToEquip.transform.SetParent(null);
        }

        private void EquipGun(GameObject gun)
        {
            GunPositionData posData = gun.GetComponent<GunPositionData>();

            if (posData is not null)
            {
                _isGunEquipped = true;
            
                _animator.SetBool(Constants.IsHoldingTwoHandedGun, true);
                _aimBehaviour.EquipGun(posData);
                _gunHandPlacement.EquipGun(posData);
                _deadZoneLook.UseDeadZone = true;
                gunToEquip.transform.SetParent(transform);
                gunToEquip.transform.localPosition = posData.GunLocalPosition;
                gunToEquip.transform.localEulerAngles = posData.GunLocalRotation;
            }
        }
    }
}