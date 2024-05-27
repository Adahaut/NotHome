using UnityEngine;

public class UIShake : MonoBehaviour
{
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

        print("camera rotation : " + _cameraRotationDelta);

        if (_cameraRotationDelta.x <= 3f && _cameraRotationDelta.y <= 3f)
        {
            print("0");
            return Vector3.zero;
        }
            

        return  new Vector3(
            -_cameraRotationDelta.y * _driftAmount,
            _cameraRotationDelta.x * _driftAmount,
            0
        );
    }

    private bool DistanceBetweenPositionAndTargetPos(Vector3 _position, Vector3 _targetPosition)
    {
        return _targetPosition.x < _position.x + 0.5f && _targetPosition.x > _position.x - 0.5f
            && _targetPosition.y < _position.y + 0.5f && _targetPosition.y > _position.y - 0.5f
            && _targetPosition.z < _position.z + 0.5f && _targetPosition.z > _position.z - 0.5f;
    }

    void Update()
    {
        Vector3 _driftOffset = UIDriftWithCameraRotation();
        print("drift offset : " + _driftOffset);

        Vector3 _targetPosition =  _initialPosition + _driftOffset;
        print("target position : " + _targetPosition);

        transform.localPosition = DistanceBetweenPositionAndTargetPos(transform.localPosition, _targetPosition) ? 
            Vector3.zero : Vector3.Lerp(transform.localPosition, _targetPosition, Time.deltaTime * _smoothSpeed);

        _previousCameraRotation = _playerCamera.eulerAngles;
    }
}

