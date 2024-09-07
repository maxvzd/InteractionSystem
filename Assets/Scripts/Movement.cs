using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float turnSpeed;
    private float _runModifier;
    
    private Animator _animator;

    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        float verticalInput = Input.GetAxis(Constants.VerticalKey);
        float horizontalInput = Input.GetAxis(Constants.HorizontalKey);
        
        _runModifier += Input.GetAxis("Mouse ScrollWheel");
        _runModifier = Mathf.Clamp(_runModifier, 0.5f, 2f);

        verticalInput *= _runModifier;
        horizontalInput *= _runModifier;
        
        _animator.SetFloat(Constants.Vertical, verticalInput);
        _animator.SetFloat(Constants.Horizontal, horizontalInput);

        if (verticalInput > 0 || verticalInput < 0 || horizontalInput > 0 || horizontalInput < 0)
        {
            var currentPosition = transform.position;
            var targetPosition = target.position;
            Vector3 relativePos = new Vector3(targetPosition.x, currentPosition.y, targetPosition.z) - currentPosition;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }
    }
}
