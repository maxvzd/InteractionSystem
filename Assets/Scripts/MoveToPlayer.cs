using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    // Update is called once per frame
    private void Update()
    {
        //1.8 for player height
        transform.position = playerTransform.position + Vector3.up * 1.8f;
    }
}
