using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRecoil : MonoBehaviour
{
    private Vector3 _currentRotation, _targetRotation, _targetPosition, _currentPosition, _initialGunPosition;
    public Transform _camTransform;

    [SerializeField] private float _recoilX;
    [SerializeField] private float _recoilY;
    [SerializeField] private float _recoilZ;

    [SerializeField] private float _kickBackZ;

    public float _snappiness, _returnAmout;

    private void Start()
    {
        _initialGunPosition = transform.localPosition;
    }

    private void Update()
    {
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, Time.deltaTime * _returnAmout);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, Time.fixedDeltaTime * _snappiness);
        transform.localRotation = Quaternion.Euler(_currentRotation);
        Back();
    }

    public void Recoil()
    {
        _targetPosition -= new Vector3(0, 0, _kickBackZ);
        _targetRotation += new Vector3(_recoilX, Random.Range(-_recoilY, _recoilY), Random.Range(-_recoilZ, _recoilZ));
    }

    private void Back()
    {
        _targetPosition = Vector3.Lerp(_targetPosition, _initialGunPosition, Time.deltaTime * _returnAmout);
        _currentPosition = Vector3.Lerp(_currentPosition, _targetPosition, Time.fixedDeltaTime * _snappiness);
        transform.localPosition = _currentPosition;
    }
}
