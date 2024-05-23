using System.Linq;
using UnityEngine;

public class CheckVisibility : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    private Camera _playerCam;

    private void Start()
    {
        _playerCam = gameObject.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (IsVisible())
        {
            print("visible");
        }
        else
        {
            print("not visible");
        }
    }

    private bool IsVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_playerCam);
        bool isInFrustum = planes.All(plane => plane.GetDistanceToPoint(_target.transform.position) >= 0);

        if (!isInFrustum)
        {
            return false;
        }

        // Check if there is a direct line of sight
        Vector3 directionToTarget = _target.transform.position - _playerCam.transform.position;
        if (Physics.Raycast(_playerCam.transform.position, directionToTarget, out RaycastHit hit))
        {
            if (hit.transform.gameObject == _target)
            {
                return true;
            }
        }

        return false;
    }
}