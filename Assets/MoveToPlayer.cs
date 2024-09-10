using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    // Update is called once per frame
    private void Update()
    {
        transform.position = playerTransform.position;
    }
}
