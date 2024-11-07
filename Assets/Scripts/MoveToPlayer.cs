using System;
using System.Collections;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private const float PLAYER_STANDING_HEIGHT = 1.75f;
    private const float PLAYER_CROUCHING_HEIGHT = PLAYER_STANDING_HEIGHT - PlayerCrouchBehaviour.CROUCH_DISTANCE;
    private float _playerHeight = PLAYER_STANDING_HEIGHT;
    private IEnumerator _playerHeightLerpRoutine;
    
    private void Start()
    {
        PlayerCrouchBehaviour crouchBehaviour = playerTransform.GetComponent<PlayerCrouchBehaviour>();
        crouchBehaviour.PlayerCrouched += OnPlayerCrouched;
        crouchBehaviour.PlayerUnCrouched += OnPlayerUnCrouched;
    }

    private void OnPlayerUnCrouched(object sender, EventArgs e)
    {
        if (_playerHeightLerpRoutine is not null)
        {
            StopCoroutine(_playerHeightLerpRoutine);
        }

        _playerHeightLerpRoutine = LerpPlayerHeight(PLAYER_STANDING_HEIGHT, 0.2f);
        StartCoroutine(_playerHeightLerpRoutine);
    }

    private void OnPlayerCrouched(object sender, EventArgs e)
    {
        if (_playerHeightLerpRoutine is not null)
        {
            StopCoroutine(_playerHeightLerpRoutine);
        }

        _playerHeightLerpRoutine = LerpPlayerHeight(PLAYER_CROUCHING_HEIGHT, 0.2f);
        StartCoroutine(_playerHeightLerpRoutine);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = playerTransform.position + Vector3.up * _playerHeight;
    }

    private IEnumerator LerpPlayerHeight(float heightToLerpTo, float lerpTime)
    {
        float timeElapsed = 0f;

        float currentHeight = _playerHeight;

        while (timeElapsed < lerpTime)
        {
            float t = timeElapsed / lerpTime;

            _playerHeight = Mathf.Lerp(currentHeight, heightToLerpTo, t);

            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
        }

        _playerHeight = heightToLerpTo;
    }
}
