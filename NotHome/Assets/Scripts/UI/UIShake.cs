using UnityEngine;

public class UIShake : MonoBehaviour
{
    public Transform _playerTransform;
    public Transform _playerCamera;
    public float _driftAmount = 0.1f;
    public float _positionDriftAmount = 0.1f;
    public float _smoothSpeed = 5f;

    private Vector3 _initialPosition;
    private Vector3 _previousCameraRotation;

    void Start()
    {
        _initialPosition = transform.localPosition;

        _previousCameraRotation = _playerCamera.eulerAngles;
    }

    private Vector3 UIDriftWithCameraRotation()
    {
        Vector3 _cameraRotationDelta = _playerCamera.eulerAngles - _previousCameraRotation;

        if (_cameraRotationDelta.x > 180) _cameraRotationDelta.x -= 360;
        if (_cameraRotationDelta.x < -180) _cameraRotationDelta.x += 360;
        if (_cameraRotationDelta.y > 180) _cameraRotationDelta.y -= 360;
        if (_cameraRotationDelta.y < -180) _cameraRotationDelta.y += 360;

        return  new Vector3(
            -_cameraRotationDelta.y * _driftAmount,
            _cameraRotationDelta.x * _driftAmount,
            0
        );
    }

    void Update()
    {
        Vector3 _driftOffset = UIDriftWithCameraRotation();

        Vector3 _targetPosition = _initialPosition + _driftOffset;

        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, Time.deltaTime * _smoothSpeed);

        _previousCameraRotation = _playerCamera.eulerAngles;
    }
}

