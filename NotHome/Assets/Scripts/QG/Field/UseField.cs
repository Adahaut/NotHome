using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UseField : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 _initPos;
    private int _indexPlant;
    private bool _isPlant;
    public float _seedTime;
    public static UseField Instance;
    private Transform _transform;

    [SerializeField] private GameObject player;
    public Sprite s;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        _transform = transform;
        _initPos = transform.position;
        GetComponent<Image>().sprite = s;

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isPlant)
        {
            _transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isPlant)
        {
            if (Vector3.Distance(_transform.position, GetNearestSlot()) < 75)
            {
                _isPlant = true;
                _transform.position = GetNearestSlot();
                Plant p = new Plant("lol", s);
                FieldManager.Instance._plantList[_indexPlant]._isUsed = true;
                FieldManager.Instance._plantList[_indexPlant] = p;
                player.GetComponentInChildren<PlayerFieldSlot>()._listSlots[_indexPlant].gameObject.GetComponent<Image>().sprite
                    = FieldManager.Instance._plantList[_indexPlant]._img;
                //ListSlotField.Instance._listPlant[_indexPlant].SetActive(true);
                //FieldManager.Instance.StartCo(_indexPlant, _seedTime, int.Parse(gameObject.name[5].ToString()));
            }
            else
            {
                _transform.position = _initPos;
            }
        }
    }
    private Vector3 GetNearestSlot()
    {
        //List<Transform> slots = ListSlotField.Instance._listPosSlot;
        List<Transform> slots = new();
        slots = player.GetComponentInChildren<PlayerFieldSlot>()._listSlots;
        Vector3 slotNearest = slots[0].position;
        _indexPlant = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].gameObject.activeSelf)
            {
                if (Vector3.Distance(_transform.position, slots[i].position) < Vector3.Distance(_transform.position, slotNearest) && !FieldManager.Instance._plantList[i]._isUsed)
                {
                    slotNearest = slots[i].position;
                    _indexPlant = i;
                    Debug.Log("entering IFFFFFFFFFF");
                }
            }
        }
        return slotNearest;
    }

    public void GetPlantFinish()
    {
        Debug.Log("GetPlant");
        _transform.position = _initPos;
        _isPlant = false;
        ListSlotField.Instance._listIsPlant[_indexPlant] = false;
        ListSlotField.Instance._listPlant[_indexPlant].SetActive(false);
        //FieldManager.Instance._timerText[_indexPlant].text = "";
        ListSlotField.Instance._listPlant[_indexPlant].GetComponent<MeshRenderer>().material = FieldManager.Instance._materialBrown;
        gameObject.GetComponent<Button>().enabled = false;
    }
}
