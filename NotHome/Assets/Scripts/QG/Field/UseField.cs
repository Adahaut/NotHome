using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UseField : NetworkBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 _startPosition;
    private Transform _transform;

    [SerializeField] private Seed _seedPrefab;
    [SerializeField] private GameObject _player;

    private NewFieldManager _fieldManager;

    private void Start()
    {
        GetComponent<Image>().sprite = _seedPrefab._img;
        GetComponentInChildren<TMP_Text>().text = _seedPrefab._name;
        StartCoroutine(FindFieldManager());
        _startPosition = transform.position;
        _transform = transform;
    }

    private IEnumerator FindFieldManager()
    {
        while (NewFieldManager.instance == null)
        {
            yield return null;
        }

        _fieldManager = NewFieldManager.instance;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(isOwned)
        {
            if (Vector3.Distance(_transform.position, GetNearestSlot()) < 75)
            {
                _seedPrefab._isPlanted = true;
                _transform.position = GetNearestSlot();
                CmdAddPlant(_seedPrefab._index, _seedPrefab._id);
                GetComponentInParent<PlayerFieldUI>().UpdateUI();
            }
            else
            {
                _transform.position = _startPosition;
            }
        }
        
    }

    [Command]
    public void CmdAddPlant(int index, int seedId)
    {
        Seed newSeed = Instantiate(NewFieldManager.instance._seedPrefabs[seedId]);
        newSeed.seedId = seedId;
        newSeed.transform.position = NewFieldManager.instance._plantPositons[index].position;

        NetworkServer.Spawn(newSeed.gameObject);
        NewFieldManager.instance._allPlants[index] = newSeed;

        NewFieldManager.instance.RpcAddPlant(newSeed.netId, index);
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
                if (Vector3.Distance(_transform.position, slots[i].position) < Vector3.Distance(_transform.position, slotNearest))
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
