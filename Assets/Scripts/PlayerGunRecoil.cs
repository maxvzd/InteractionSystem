using System.Collections;
using UnityEngine;

public class PlayerGunRecoil : MonoBehaviour
{
    [SerializeField] private float recoveryTime;
    [SerializeField] private float recoilSpeed;

    private IEnumerator _recoilLerpRoutine;
    private bool _isRecoiling;

    private float _recoveryTimeElapsed;
    private Vector3 _positionAtEndOfRecoil;
    private Vector3 _originalLocalPosition;

    private void Start()
    {
        _originalLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (_isRecoiling) return;
        
        if (_recoveryTimeElapsed < recoveryTime)
        {
            float t = _recoveryTimeElapsed / recoveryTime; 
            transform.localPosition = Vector3.Lerp(_positionAtEndOfRecoil, _originalLocalPosition, t);
            _recoveryTimeElapsed += Time.deltaTime;
        }
    }

    public void AddRecoil(object sender, GunFiredEventArgs eventArgs)
    {
        if (_recoilLerpRoutine is not null)
        {
            StopCoroutine(_recoilLerpRoutine);
        }

        _recoilLerpRoutine = AddRecoilCoRoutine(recoilSpeed, eventArgs.Recoil);
        StartCoroutine(_recoilLerpRoutine);
    }

    private IEnumerator AddRecoilCoRoutine(float lerpTime, float recoilAmount)
    {
        float timeElapsed = 0f;
        Vector3 startingPosition = transform.localPosition;
        Vector3 targetPosition = startingPosition + Vector3.up * recoilAmount;
        _isRecoiling = true;
        while (timeElapsed < lerpTime)
        {
            float t = timeElapsed / lerpTime;
            
            transform.localPosition = Vector3.Lerp(startingPosition, targetPosition, t);
            
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
        }

        _isRecoiling = false;
        transform.localPosition = targetPosition;
        _positionAtEndOfRecoil = targetPosition;
        _recoveryTimeElapsed = 0f;
    }
}
