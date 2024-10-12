using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    // Update is called once per frame
    private void Update()
    {
        Vector3 scrubbedY = playerTransform.position;
        scrubbedY.y = transform.position.y;
        transform.position = scrubbedY;
    }
}
