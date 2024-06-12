using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelector : MonoBehaviour
{
    private Transform _hightlight;
    private Transform _selection;
    private RaycastHit _hit;
    private Camera _playerCam;
    private Transform _cameraTransform;
    [SerializeField] private Color _outlineColor;
    [SerializeField] private float _outlineWidth;
    [SerializeField] private float _distance;

    private void Start()
    {
        _playerCam = GetComponentInChildren<Camera>();
        _cameraTransform = _playerCam.transform;
    }

    private void Update()
    {
        if(_hightlight != null)
        {
            _hightlight.gameObject.GetComponent<Outline>().enabled = false;
            _hightlight = null;
        }

        if(!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out _hit, _distance))
        {
            _hightlight = _hit.transform;
            if(_hightlight.CompareTag("Item") || _hightlight.CompareTag("Ladder") || _hightlight.gameObject.layer == LayerMask.NameToLayer("Interectable") || _hightlight.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                if(_hightlight.GetComponent<Outline>() != null)
                {
                    _hightlight.gameObject.GetComponent<Outline>().enabled = true;
                }
                else
                {
                    Outline outline = _hightlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    outline.OutlineColor = _outlineColor;
                    outline.OutlineWidth = _outlineWidth;
                    outline.OutlineMode = Outline.Mode.OutlineVisible;
                }
            }
            else
            {
                _hightlight = null;
            }

        }

    }
}
