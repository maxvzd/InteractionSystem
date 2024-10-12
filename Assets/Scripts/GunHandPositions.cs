using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandPositions : MonoBehaviour
{
    [SerializeField] private Transform leftHandIkTarget;
    [SerializeField] private Transform rightHandIkTarget;

    public Vector3 LeftHandIkTargetPosition => leftHandIkTarget.position;
    public Quaternion LeftHandIkTargetRotation => leftHandIkTarget.rotation;
    
    public Vector3 RightHandIkTargetPosition => rightHandIkTarget.position;
    public Quaternion RightHandIkTargetRotation => rightHandIkTarget.rotation;
}
