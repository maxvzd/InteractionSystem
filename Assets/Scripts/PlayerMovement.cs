using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
    }
}
