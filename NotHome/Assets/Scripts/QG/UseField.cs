using System.Collections.Generic;
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
    
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _initPos = transform.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        if (!_isPlant)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
        if (!_isPlant)
        {
            if (Vector3.Distance(transform.position, GetNearestSlot()) < 75)
            {
                _isPlant = true;
                transform.position = GetNearestSlot();
                ListSlotField.Instance._listIsPlant[_indexPlant] = true;
                ListSlotField.Instance._listPlant[_indexPlant].SetActive(true);
                FieldManager.Instance.StartCo(_indexPlant, _seedTime, int.Parse(gameObject.name[5].ToString()));
            }
            else
            {
                transform.position = _initPos;
            }
        }
    }
    private Vector3 GetNearestSlot()
    {
        List<Transform> slots = ListSlotField.Instance._listPosSlot;
        Vector3 slotNearest = slots[0].position;
        _indexPlant = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            if (Vector3.Distance(transform.position, slots[i].position) < Vector3.Distance(transform.position, slotNearest) && !ListSlotField.Instance._listIsPlant[i])
            {
                slotNearest = slots[i].position;
                _indexPlant = i;
            }
        }
        return slotNearest;
    }

    public void GetPlantFinish()
    {
        Debug.Log("GetPlant");
        transform.position = _initPos;
        _isPlant = false;
        ListSlotField.Instance._listIsPlant[_indexPlant] = false;
        ListSlotField.Instance._listPlant[_indexPlant].SetActive(false);
        FieldManager.Instance._timerText[_indexPlant].text = "";
        ListSlotField.Instance._listPlant[_indexPlant].GetComponent<MeshRenderer>().material = FieldManager.Instance._materialBrown;
        gameObject.GetComponent<Button>().enabled = false;
        print("Caca");
    }
}
