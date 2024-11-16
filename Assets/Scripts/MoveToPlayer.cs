using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private const float PLAYER_STANDING_HEIGHT = 1.75f;
    private const float PLAYER_CROUCHING_HEIGHT = PLAYER_STANDING_HEIGHT - PlayerCrouchBehaviour.CROUCH_DISTANCE;
    
    
    private void Update()
    {
        transform.position = playerTransform.position + Vector3.up * PLAYER_STANDING_HEIGHT;
    }
}
