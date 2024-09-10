using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMouseInput : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private float maxVerticalAngle;
    
    private float _rotationY;
    private float _rotationX;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _rotationY += Input.GetAxis(Constants.MouseX) * Time.deltaTime * sensitivity;
        _rotationX += Input.GetAxis(Constants.MouseY) * Time.deltaTime * sensitivity;
        _rotationX = Mathf.Clamp(_rotationX, -maxVerticalAngle, maxVerticalAngle);

        transform.eulerAngles = new Vector3(-_rotationX, _rotationY);
    }
}
