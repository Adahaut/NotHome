using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UseField : NetworkBehaviour, IDragHandler, IEndDragHandler
{

    /*
    
    


    public void OnDrag(PointerEventData eventData)
    {
        if (!_seedPrefab._isPlanted)
        {
            _transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("caca");
    }
    */

    private Vector3 _startPosition;
    private Transform _transform;

    [SerializeField] private Seed _seedPrefab;
    [SerializeField] private GameObject _player;

    private void Start()
    {
        GetComponent<Image>().sprite = _seedPrefab._img;
        GetComponentInChildren<TMP_Text>().text = _seedPrefab._name;

        _startPosition = transform.position;
        _transform = transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.position = Input.mousePosition;
    }

    [Client]
    public void OnEndDrag(PointerEventData eventData)
    {
        if(isOwned)
        {
            if (Vector3.Distance(_transform.position, GetNearestSlot()) < 75)
            {
                _seedPrefab._isPlanted = true;
                _transform.position = GetNearestSlot();
                //GameObject.Find("Field_Manager").GetComponent<NewFieldManager>().CmdAddPlant(_seedPrefab._index, _seedPrefab);
                NewFieldManager.instance.CmdAddPlant(_seedPrefab._index, _seedPrefab._id);

            }
            else
            {
                _transform.position = _startPosition;
            }
        }
        
    }
    private Vector3 GetNearestSlot()
    {
        List<Transform> slots = new();
        slots = _player.GetComponentInChildren<PlayerFieldSlot>()._listSlots;
        Vector3 slotNearest = slots[0].position;
        _seedPrefab._index = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].gameObject.activeSelf)
            {
                if (Vector3.Distance(_transform.position, slots[i].position) < Vector3.Distance(_transform.position, slotNearest) && !FieldManager.Instance._plantList[i]._isUsed)
                {
                    slotNearest = slots[i].position;
                    _seedPrefab._index = i;
                }
            }
        }
        return slotNearest;
    }

    //public void GetPlantFinish()
    //{
    //    Debug.Log("GetPlant");
    //    _transform.position = _initPos;
    //    _isPlant = false;
    //    ListSlotField.Instance._listIsPlant[_indexPlant] = false;
    //    ListSlotField.Instance._listPlant[_indexPlant].SetActive(false);
    //    //FieldManager.Instance._timerText[_indexPlant].text = "";
    //    ListSlotField.Instance._listPlant[_indexPlant].GetComponent<MeshRenderer>().material = FieldManager.Instance._materialBrown;
    //    gameObject.GetComponent<Button>().enabled = false;
    //}


    
}
