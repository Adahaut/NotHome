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
        GetComponent<Image>().sprite = _seedPrefab.seedStruct._img;
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
            if (Vector3.Distance(_transform.position, GetNearestSlot()) < 75)
            {
                _seedPrefab.seedStruct._isPlanted = true;
                _transform.position = GetNearestSlot();
                CmdAddPlant(_seedPrefab.seedStruct._index, _seedPrefab.seedStruct._id);
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
        SeedObject newSeedObject = NewFieldManager.instance._seedPrefabs[seedId];
        Seed newSeed = newSeedObject.seedStruct;
        newSeed.seedId = seedId;

        newSeedObject.transform.position = NewFieldManager.instance._plantPositons[index].position;

        NetworkServer.Spawn(newSeedObject.gameObject);
        NewFieldManager.instance._allPlants[index] = newSeed;

        //RpcAddPlant(newSeed.netId, index);
    }

    //[ClientRpc]
    //public void RpcAddPlant(uint seedNetId, int index)
    //{
    //    if (NetworkServer.spawned.TryGetValue(seedNetId, out NetworkIdentity seedIdentity))
    //    {
    //        Seed seed = seedIdentity.GetComponent<Seed>();
    //        seed.StartGrow(NewFieldManager.instance._plantPositons[index], index);
    //        if (!NewFieldManager.instance._allPlants.Contains(seed))
    //        {
    //            NewFieldManager.instance._allPlants[index] = seed;
    //        }
    //        NewFieldManager.instance.t += 1;
    //    }
    //}


    private Vector3 GetNearestSlot()
    {
        List<Transform> slots = new();
        slots = _player.GetComponentInChildren<PlayerFieldSlot>()._listSlots;
        Vector3 slotNearest = slots[0].position;
        _seedPrefab.seedStruct._index = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].gameObject.activeSelf)
            {
                if (Vector3.Distance(_transform.position, slots[i].position) < Vector3.Distance(_transform.position, slotNearest))
                {
                    slotNearest = slots[i].position;
                    _seedPrefab.seedStruct._index = i;
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
