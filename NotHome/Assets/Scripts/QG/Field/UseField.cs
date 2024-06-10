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

    [SerializeField] private SeedObject _seedPrefab;
    [SerializeField] private GameObject _player;

    private NewFieldManager _fieldManager;

    private void Start()
    {
        _player = transform.root.gameObject;
        //GetComponent<Image>().sprite = _seedPrefab.seedStruct._img;
        GetComponentInChildren<TMP_Text>().text = _seedPrefab.seedStruct._name;
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
            Transform slot = GetNearestSlot();
            if (Vector3.Distance(_transform.position, slot.position) < 75 && !slot.GetComponent<PlayerFieldSlot>()._containSeed)
            {
                _transform.position = slot.position;

                slot.GetComponent<PlayerFieldSlot>().StartGrowing(_seedPrefab.seedStruct._growingTime, 
                    _seedPrefab.seedStruct._name,
                    _seedPrefab.fruitImage,
                    _seedPrefab.seedImage);

                CmdAddPlant(_seedPrefab.seedStruct._index, _seedPrefab.seedStruct._id);
                PlayerFieldUI.UpdateAllUIs();
                this.gameObject.SetActive(false);
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
        Vector3 plantPosition = NewFieldManager.instance._plantPositons[index].position;

        GameObject newSeedObject = Instantiate(NewFieldManager.instance._seedPrefabs[seedId], plantPosition, Quaternion.identity);

        newSeedObject.GetComponent<SeedObject>().StartGrow(plantPosition);

        Seed newSeed = newSeedObject.GetComponent<SeedObject>().seedStruct;
        newSeed.seedId = seedId;
        newSeedObject.transform.position = plantPosition;

        NetworkServer.Spawn(newSeedObject);
        NewFieldManager.instance._allPlants[index] = newSeed;
        NewFieldManager.instance.AddPlant(newSeedObject, index);
    }


    private Transform GetNearestSlot()
    {
        List<Transform> slots = new();
        slots = _player.GetComponentInChildren<FieldSlotsLists>()._listSlots;
        Transform slotNearest = slots[0];
        _seedPrefab.seedStruct._index = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].gameObject.activeSelf)
            {
                if (Vector3.Distance(_transform.position, slots[i].position) < Vector3.Distance(_transform.position, slotNearest.position))
                {
                    slotNearest = slots[i];
                    _seedPrefab.seedStruct._index = i;
                }
            }
        }
        return slotNearest;
    }


}
