using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    // Update is called once per frame
    private void Update()
    {
        transform.LookAt(target);
    }
}
