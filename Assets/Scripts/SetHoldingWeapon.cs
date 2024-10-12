using UnityEngine;

//TEMPORARY
public class SetHoldingWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool(Constants.IsHoldingTwoHandedGun, true);
    }
}
